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
        if (args.Length < 2) { UIHelpers.PrintColored("Використання: start <НазваТеми>", ConsoleColor.Red); return; }
        string topicName = string.Join(" ", args.Skip(1));

        Console.Write("Кількість питань (за замовчуванням 5): ");
        int.TryParse(Console.ReadLine(), out int count);
        if (count <= 0) count = 5;

        var questions = _quizService.GenerateSession(topicName, count);
        var sessionResults = new List<(Question Q, object UserAns)>();

        foreach (var q in questions)
        {
            Console.Clear();
            UIHelpers.PrintColored($"--- {q.Text} ---", ConsoleColor.Yellow);

            if (q is SingleChoiceQuestion scq)
            {
                for (int i = 0; i < scq.Options.Count; i++) Console.WriteLine($"{i + 1}. {scq.Options[i]}");
                int.TryParse(UIHelpers.Prompt("Ваша відповідь (номер)"), out int ans);
                sessionResults.Add((q, ans > 0 && ans <= scq.Options.Count ? scq.Options[ans - 1] : ""));
            }
            else if (q is MultipleChoiceQuestion mcq)
{
    for (int i = 0; i < mcq.Options.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {mcq.Options[i]}");
    }

    string input = UIHelpers.Prompt("Ваші відповіді (номери через кому, напр. 1, 3)");

    List<string> selectedVariants = new List<string>();

    string[] parts = input.Split(',');

    foreach (string part in parts)
    {
        string cleanPart = part.Trim();

        if (string.IsNullOrEmpty(cleanPart))
        {
            continue;
        }

        if (int.TryParse(cleanPart, out int index))
        {
            if (index > 0 && index <= mcq.Options.Count)
            {
                selectedVariants.Add(mcq.Options[index - 1]);
            }
        }
    }

    sessionResults.Add((q, selectedVariants));
}
        }

        double score = _quizService.CalculateResult(sessionResults);
        _quizService.SaveResult("Користувач", topicName, score);
        UIHelpers.PrintColored($"\nВаш результат: {score} балів.", ConsoleColor.Green);

        ShowErrorReview(sessionResults);
    }

    private void ShowErrorReview(List<(Question Q, object UserAns)> results)
    {
        Console.Write("\nБажаєте переглянути помилки? (y/n): ");
        if (Console.ReadLine()?.ToLower() != "y") return;

        Console.WriteLine("\n--- Аналіз помилок ---");
        foreach (var item in results)
        {
            if (!item.Q.CheckAnswer(item.UserAns))
            {
                UIHelpers.PrintColored($"[X] {item.Q.Text}", ConsoleColor.Red);
                Console.WriteLine($"Ваша відповідь: {(item.UserAns is List<string> l ? string.Join(", ", l) : item.UserAns)}");
            }
        }
        UIHelpers.Wait();
    }
}