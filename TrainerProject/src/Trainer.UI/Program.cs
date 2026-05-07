using Trainer.DataAccess;
using Trainer.BusinessLogic;
using Trainer.Domain;

var context = new DataContext();
var quizService = new QuizService(context);
var topicService = new TopicService(context);
var statsService = new StatisticsService(context);

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.Title = "Тренажер для підготовки до тестів v1.0";

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("=== Вітаємо у тренажері для підготовки до іспитів ===");
Console.ResetColor();

Console.Write("Введіть ваше ім'я: ");
string userName = Console.ReadLine() ?? "Гість";

while (true)
{
    Console.Clear();
    Console.WriteLine($"Користувач: {userName}");
    Console.WriteLine("--------------------------------");
    Console.WriteLine("1. Почати тренування");
    Console.WriteLine("2. Керування темами (Admin)");
    Console.WriteLine("3. Переглянути статистику");
    Console.WriteLine("4. Вихід");
    Console.Write("\nОберіть дію: ");

    string choice = Console.ReadLine() ?? "";

    try
    {
        switch (choice)
        {
            case "1": StartTraining(userName, quizService, topicService); break;
            case "2": AdminMenu(topicService); break;
            case "3": ShowStatistics(statsService); break;
            case "4": return;
            default: Console.WriteLine("Невірний вибір. Натисніть будь-яку клавішу..."); Console.ReadKey(); break;
        }
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\nПомилка: {ex.Message}");
        Console.ResetColor();
        Console.ReadKey();
    }
}

void StartTraining(string userName, QuizService quiz, TopicService topics)
{
    var allTopics = topics.GetTopics();
    if (!allTopics.Any()) 
    {
        Console.WriteLine("Список тем порожній. Спочатку додайте тему в Admin-меню.");
        Console.ReadKey();
        return;
    }

    Console.WriteLine("\nОберіть тему (введіть номер):");
    for (int i = 0; i < allTopics.Count; i++)
        Console.WriteLine($"{i + 1}. {allTopics[i].Name}");

    if (!int.TryParse(Console.ReadLine(), out int topicIndex) || topicIndex < 1 || topicIndex > allTopics.Count)
    {
        Console.WriteLine("Невірний вибір.");
        Console.ReadKey();
        return;
    }

    var selectedTopic = allTopics[topicIndex - 1];

    Console.Write("Скільки питань підготувати? ");
    int.TryParse(Console.ReadLine(), out int count);

    var sessionQuestions = quiz.GenerateSession(selectedTopic.Name, count);
    var sessionAnswers = new List<(Question, object)>();

    foreach (var q in sessionQuestions)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Питання: {q.Text}");
        Console.ResetColor();

        if (q is SingleChoiceQuestion scq)
        {
            for (int i = 0; i < scq.Options.Count; i++)
                Console.WriteLine($"{i + 1}. {scq.Options[i]}");
            
            Console.Write("\nВаша відповідь (номер): ");
            if (int.TryParse(Console.ReadLine(), out int ansIdx) && ansIdx > 0 && ansIdx <= scq.Options.Count)
                sessionAnswers.Add((q, scq.Options[ansIdx - 1]));
        }
        else if (q is OpenEndedQuestion)
        {
            Console.Write("Введіть вашу відповідь: ");
            sessionAnswers.Add((q, Console.ReadLine() ?? ""));
        }
    }

    double score = quiz.CalculateResult(sessionAnswers);
    quiz.SaveResult(userName, selectedTopic.Name, score);

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"\nТест завершено! Ви набрали: {score} балів.");
    Console.ResetColor();
    Console.WriteLine("Натисніть будь-яку клавішу...");
    Console.ReadKey();
}

void AdminMenu(TopicService topics)
{
    Console.Clear();
    Console.WriteLine("=== Адміністрування ===");
    Console.WriteLine("1. Створити нову тему");
    Console.WriteLine("2. Додати питання до теми");
    Console.WriteLine("3. Назад");
    
    string subChoice = Console.ReadLine() ?? "";
    if (subChoice == "1")
    {
        Console.Write("Введіть назву теми: ");
        topics.AddTopic(Console.ReadLine() ?? "Без назви");
        Console.WriteLine("Тему створено.");
    }
    else if (subChoice == "2")
    {
        var all = topics.GetTopics();
        for (int i = 0; i < all.Count; i++) Console.WriteLine($"{i+1}. {all[i].Name}");
        
        Console.Write("Оберіть номер теми: ");
        int idx = int.Parse(Console.ReadLine() ?? "1") - 1;

        Console.Write("Введіть текст питання: ");
        string text = Console.ReadLine() ?? "";

        var q = new OpenEndedQuestion { Text = text, Points = 10 };
        topics.AddQuestionToTopic(all[idx].Name, q);
        Console.WriteLine("Питання додано.");
    }
    Console.ReadKey();
}

void ShowStatistics(StatisticsService stats)
{
    Console.Clear();
    Console.WriteLine("=== Статистика успішності ===");
    var scores = stats.GetAverageScoresByTopic();
    
    if (!scores.Any()) Console.WriteLine("Історія порожня.");
    
    foreach (var entry in scores)
    {
        Console.WriteLine($"Тема: {entry.Key} | Сер. бал: {entry.Value:F2}");
    }
    
    Console.WriteLine("\nНатисніть будь-яку клавішу...");
    Console.ReadKey();
}