using Trainer.BusinessLogic;
using Trainer.Domain;

namespace Trainer.UI.Handlers;

public class QuizHandler
{
    private readonly QuizService _quizService;
    private readonly TopicService _topicService;

    public QuizHandler(QuizService quizService, TopicService topicService)
    {
        _quizService = quizService;
        _topicService = topicService;
    }

    public void StartQuiz(string[] args)
    {
        if (args.Length < 2)
        {
            UIHelpers.PrintColored("Використання: start <НазваТеми>", ConsoleColor.Red);
            return;
        }

        string topicName = string.Join(" ", args.Skip(1));
        var topic = _topicService.GetTopics().FirstOrDefault(t => t.Name.Equals(topicName, StringComparison.OrdinalIgnoreCase));

        if (topic == null)
        {
            UIHelpers.PrintColored($"Тему '{topicName}' не знайдено.", ConsoleColor.Red);
            return;
        }

        UIHelpers.PrintColored($"\n--- Тема: {topic.Name} ({topic.Questions.Count} питань) ---", ConsoleColor.Cyan);
        Console.WriteLine("1. Випадкові питання (вказати кількість)");
        Console.WriteLine("2. Вибрати питання вручну (вказати номери)");

        string mode = UIHelpers.Prompt("Оберіть режим");
        List<Question> sessionQuestions = new List<Question>();

        if (mode == "2")
        {
            for (int i = 0; i < topic.Questions.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {topic.Questions[i].Text}");
            }

            string input = UIHelpers.Prompt("Введіть номери питань через кому (напр. 1, 3, 5)");
            List<int> chosenIndices = new List<int>();
            string[] parts = input.Split(',');

            foreach (string p in parts)
            {
                if (int.TryParse(p.Trim(), out int idx))
                {
                    chosenIndices.Add(idx - 1);
                }
            }
            sessionQuestions = _quizService.GenerateManualSession(topicName, chosenIndices);
        }
        else
        {
            Console.Write("Скільки питань підготувати? ");
            if (int.TryParse(Console.ReadLine(), out int count))
            {
                sessionQuestions = _quizService.GenerateSession(topicName, count);
            }
        }

        if (sessionQuestions.Count > 0)
        {
            ExecuteQuiz(sessionQuestions, topic.Name);
        }
        else
        {
            UIHelpers.PrintColored("Питання не обрані.", ConsoleColor.Yellow);
        }
    }

    private void ExecuteQuiz(List<Question> questions, string topicName)
    {
        var sessionResults = new List<(Question Q, object UserAns)>();

        foreach (var q in questions)
        {
            Console.Clear();
            UIHelpers.PrintColored($"Питання: {q.Text}", ConsoleColor.Yellow);

            if (q is SingleChoiceQuestion scq)
            {
                for (int i = 0; i < scq.Options.Count; i++) Console.WriteLine($"{i + 1}. {scq.Options[i]}");
                int.TryParse(UIHelpers.Prompt("Ваш вибір (номер)"), out int ans);
                sessionResults.Add((q, ans > 0 && ans <= scq.Options.Count ? scq.Options[ans - 1] : ""));
            }
            else if (q is MultipleChoiceQuestion mcq)
            {
                for (int i = 0; i < mcq.Options.Count; i++) Console.WriteLine($"{i + 1}. {mcq.Options[i]}");
                string input = UIHelpers.Prompt("Ваші відповіді (номери через кому)");

                List<string> selected = new List<string>();
                string[] parts = input.Split(',');
                foreach (var p in parts)
                {
                    if (int.TryParse(p.Trim(), out int idx) && idx > 0 && idx <= mcq.Options.Count)
                        selected.Add(mcq.Options[idx - 1]);
                }
                sessionResults.Add((q, selected));
            }
            else if (q is OpenEndedQuestion)
            {
                sessionResults.Add((q, UIHelpers.Prompt("Ваша відповідь")));
            }
        }

        double score = _quizService.CalculateResult(sessionResults);
        _quizService.SaveResult("User", topicName, score);
        UIHelpers.PrintColored($"\nТест завершено! Ваш бал: {score}", ConsoleColor.Green);

        ShowErrorReview(sessionResults);
        UIHelpers.Wait();
    }

    private void ShowErrorReview(List<(Question Q, object UserAns)> results)
    {
        Console.Write("\nПереглянути помилки? (y/n): ");
        if (Console.ReadLine()?.ToLower() != "y") return;

        foreach (var item in results)
        {
            if (!item.Q.CheckAnswer(item.UserAns))
            {
                UIHelpers.PrintColored($"[X] {item.Q.Text}", ConsoleColor.Red);
                string userResponse = item.UserAns is List<string> list ? string.Join(", ", list) : item.UserAns.ToString();
                Console.WriteLine($"Ваша відповідь: {userResponse}");
            }
        }
    }
}