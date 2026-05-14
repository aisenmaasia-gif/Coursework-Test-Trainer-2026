using Trainer.DataAccess;
using Trainer.BusinessLogic;
using Trainer.Domain;

var context = new DataContext();
var topicService = new TopicService(context);
var quizService = new QuizService(context);
var statsService = new StatisticsService(context);

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.Clear();
Console.WriteLine("=== Trainer CLI 2026 | Введіть 'help' для списку команд ===");

while (true)
{
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.Write("\n> ");
    string input = Console.ReadLine()?.Trim() ?? "";
    Console.ResetColor();

    if (string.IsNullOrEmpty(input)) continue;

    string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    string command = parts[0].ToLower();

    try
    {
        switch (command)
        {
            case "help": ShowHelp(); break;
            case "exit": return;

            case "list": ShowTopicsList(topicService); break;
            case "start": StartQuiz(parts, quizService, topicService); break;
            case "history": ShowHistory(context); break;
            case "stats": ShowFullStats(statsService); break;

            case "create-topic": CreateTopic(topicService); break;
            case "add-question": AddQuestion(parts, topicService); break;
            case "delete-topic": DeleteTopic(parts, topicService); break;

            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Невідома команда: '{command}'. Введіть 'help' для довідки.");
                Console.ResetColor();
                break;
        }
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Помилка: {ex.Message}");
        Console.ResetColor();
    }
}

void ShowHelp()
{
    Console.WriteLine("\nДоступні команди:");
    Console.WriteLine("  list                  - Показати всі теми");
    Console.WriteLine("  start <TopicName>     - Почати тест за назвою теми");
    Console.WriteLine("  history               - Історія проходжень");
    Console.WriteLine("  stats                 - Статистика успішності");
    Console.WriteLine("\nАдміністрування:");
    Console.WriteLine("  create-topic          - Створити нову тему");
    Console.WriteLine("  add-question <Topic>  - Додати питання до теми");
    Console.WriteLine("  delete-topic <Topic>  - Видалити тему");
    Console.WriteLine("  exit / help           - Вихід / Довідка");
}

void ShowTopicsList(TopicService service)
{
    var topics = service.GetTopics();
    if (!topics.Any()) { Console.WriteLine("Теми відсутні."); return; }
    
    Console.WriteLine("\nДоступні теми:");
    foreach (var t in topics) 
        Console.WriteLine($"- {t.Name} ({t.Questions.Count} питань)");
}

void StartQuiz(string[] args, QuizService quiz, TopicService topics)
{
    if (args.Length < 2) { Console.WriteLine("Використання: start <НазваТеми>"); return; }
    string topicName = string.Join(" ", args.Skip(1));

    Console.Write("Кількість питань: ");
    int.TryParse(Console.ReadLine(), out int count);
    if (count <= 0) count = 5;

    var questions = quiz.GenerateSession(topicName, count);
    var answers = new List<(Question, object)>();

    foreach (var q in questions)
    {
        Console.WriteLine($"\n--- {q.Text} ---");

        if (q is SingleChoiceQuestion scq)
        {
            for (int i = 0; i < scq.Options.Count; i++) Console.WriteLine($"{i + 1}. {scq.Options[i]}");
            Console.Write("Ваш вибір (номер): ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= scq.Options.Count)
                answers.Add((q, scq.Options[choice - 1]));
        }
        else if (q is MultipleChoiceQuestion mcq)
        {
            for (int i = 0; i < mcq.Options.Count; i++) Console.WriteLine($"{i + 1}. {mcq.Options[i]}");
            Console.Write("Ваші відповіді (номери через кому, напр. 1,4): ");
            
            string input = Console.ReadLine() ?? "";
            var selectedStrings = input.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => int.TryParse(s, out int idx) ? idx : -1)
                .Where(idx => idx > 0 && idx <= mcq.Options.Count)
                .Select(idx => mcq.Options[idx - 1])
                .ToList();

            answers.Add((q, selectedStrings));
        }
        else if (q is OpenEndedQuestion)
        {
            Console.Write("Ваша відповідь: ");
            string userResponse = Console.ReadLine()?.Trim() ?? "";
            answers.Add((q, userResponse));
        }
    }

    double res = quiz.CalculateResult(answers);
    quiz.SaveResult("User", topicName, res);
    
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"\nВаш результат: {res} балів.");
    Console.ResetColor();
}

void ShowHistory(DataContext context)
{
    var history = context.History.GetAll();
    if (!history.Any()) { Console.WriteLine("Історія порожня."); return; }
    
    Console.WriteLine("\n=== Історія проходжень ===");
    foreach (var h in history) 
        Console.WriteLine($"{h.DateTime:G} | {h.TopicName} | {h.Score}б.");
}

void ShowFullStats(StatisticsService stats)
{
    var avg = stats.GetAverageScoresByTopic();
    if (!avg.Any()) { Console.WriteLine("Статистика порожня."); return; }
    
    Console.WriteLine("\n=== Статистика успішності за темами ===");
    foreach (var item in avg)
        Console.WriteLine($"{item.Key,-20} : {item.Value,6:F2} сер. бал");
}

void CreateTopic(TopicService service)
{
    Console.Write("Введіть назву нової теми: ");
    string name = Console.ReadLine() ?? "";
    service.AddTopic(name);
    Console.WriteLine($"Тема '{name}' створена.");
}

void AddQuestion(string[] args, TopicService service)
{
    if (args.Length < 2) { Console.WriteLine("Використання: add-question <НазваТеми>"); return; }
    string topicName = string.Join(" ", args.Skip(1)); 

    Console.WriteLine("\nОберіть тип запитання:");
    Console.WriteLine(" 1 - Один варіант (Single Choice)");
    Console.WriteLine(" 2 - Мультивідповідь (Multiple Choice)");
    Console.WriteLine(" 3 - Відкрите питання (Open Ended)");
    Console.Write("> ");
    string type = Console.ReadLine() ?? "1";

    Console.Write("Текст запитання: ");
    string text = Console.ReadLine() ?? "";
    Console.Write("Кількість балів за правильну відповідь: ");
    double.TryParse(Console.ReadLine(), out double points);
    if (points <= 0) points = 10;

    if (type == "1") 
    {
        Console.Write("Варіанти (через кому, напр. 2, 4, 6): ");
        var opts = Console.ReadLine()?.Split(',').Select(s => s.Trim()).ToList() ?? new();
        
        Console.Write("Правильна відповідь (текст): ");
        string correct = Console.ReadLine() ?? "";
        
        service.AddQuestionToTopic(topicName, new SingleChoiceQuestion 
        { 
            Text = text, Options = opts, CorrectAnswer = correct, Points = points 
        });
    }
    else if (type == "2")
    {
        Console.Write("Варіанти (через кому): ");
        var opts = Console.ReadLine()?.Split(',').Select(s => s.Trim()).ToList() ?? new();
        
        Console.Write("УСІ правильні відповіді (через кому): ");
        var corrects = Console.ReadLine()?.Split(',').Select(s => s.Trim()).ToList() ?? new();
        
        service.AddQuestionToTopic(topicName, new MultipleChoiceQuestion 
        { 
            Text = text, Options = opts, CorrectAnswers = corrects, Points = points 
        });
    }
    else 
    {
        Console.Write("Правильна відповідь (текст): ");
        string correct = Console.ReadLine() ?? "";
        
        service.AddQuestionToTopic(topicName, new OpenEndedQuestion 
        { 
            Text = text, CorrectAnswer = correct, Points = points 
        });
    }

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Питання успішно додано!");
    Console.ResetColor();
}

void DeleteTopic(string[] args, TopicService service)
{
    if (args.Length < 2) 
    { 
        Console.WriteLine("Помилка! Використання: delete-topic <НазваТеми>"); 
        return; 
    }

    string topicName = string.Join(" ", args.Skip(1));

    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write($"Ви впевнені, що хочете видалити тему '{topicName}' та ВСІ питання в ній? (y/n): ");
    Console.ResetColor();

    string confirm = Console.ReadLine()?.ToLower() ?? "";

    if (confirm == "y" || confirm == "н") 
    {
        service.DeleteTopic(topicName);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Успішно: Тему '{topicName}' було видалено.");
        Console.ResetColor();
    }
    else
    {
        Console.WriteLine("Видалення скасовано.");
    }
}