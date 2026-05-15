using Trainer.BusinessLogic;
using Trainer.Domain;

namespace Trainer.UI.Handlers;

public class QuizHandler
{
    private readonly QuizService _quiz;
    private readonly TopicService _topics;

    public QuizHandler(QuizService quiz, TopicService topics)
    {
        _quiz = quiz;
        _topics = topics;
    }

    public void StartQuiz(string[] args)
    {
        try
        {
            if (args.Length < 2)
            {
                UIHelpers.PrintColored("Використання: start <НазваТеми>", ConsoleColor.Red);
                return;
            }

            string topicName = string.Join(" ", args.Skip(1));
            var topic = _topics.GetTopics().FirstOrDefault(t => t.Name.Equals(topicName, StringComparison.OrdinalIgnoreCase));

            if (topic == null) throw new Exception($"Тему '{topicName}' не знайдено.");
            if (topic.Questions.Count == 0) throw new Exception("У темі немає запитань.");

            UIHelpers.PrintColored($"\n--- Тема: {topic.Name} ---", ConsoleColor.Cyan);
            Console.WriteLine("1. Випадковий вибір\n2. Ручний вибір");

            int mode = UIHelpers.GetSafeInt("Оберіть режим", 1, 2);
            List<Question> session;

            if (mode == 2)
            {
                session = ManualSelection(topic);
            }
            else
            {
                int count = UIHelpers.GetSafeInt("Кількість питань", 1, topic.Questions.Count);
                session = _quiz.GenerateSession(topic.Name, count);
            }

            if (session != null && session.Any())
            {
                ExecuteQuiz(session, topic.Name);
            }
        }
        catch (Exception ex)
        {
            UIHelpers.PrintColored($"\nПомилка тесту: {ex.Message}", ConsoleColor.Red);
            UIHelpers.Wait();
        }
    }

    private List<Question> ManualSelection(Topic topic)
    {
        Console.WriteLine("\nДоступні питання:");
        for (int i = 0; i < topic.Questions.Count; i++)
            Console.WriteLine($"{i + 1}. {topic.Questions[i].Text}");

        List<int> chosenIndices = UIHelpers.GetSafeIntList("Введіть номери через кому", topic.Questions.Count);
        return _quiz.GenerateManualSession(topic.Name, chosenIndices);
    }

    private void ExecuteQuiz(List<Question> questions, string topicName)
    {
        var sessionResults = new List<(Question Q, object UserAns)>();

        try
        {
            foreach (var q in questions)
            {
                Console.Clear();
                UIHelpers.PrintColored($"--- {q.Text} ---", ConsoleColor.Yellow);

                if (q is SingleChoiceQuestion scq)
                {
                    for (int i = 0; i < scq.Options.Count; i++) Console.WriteLine($"{i + 1}. {scq.Options[i]}");
                    int choice = UIHelpers.GetSafeInt("Ваш вибір", 1, scq.Options.Count);
                    sessionResults.Add((q, scq.Options[choice - 1]));
                }
                else if (q is MultipleChoiceQuestion mcq)
                {
                    for (int i = 0; i < mcq.Options.Count; i++) Console.WriteLine($"{i + 1}. {mcq.Options[i]}");
                    var indices = UIHelpers.GetSafeIntList("Ваші відповіді через кому", mcq.Options.Count);
                    var selectedStrings = indices.Select(id => mcq.Options[id]).ToList();
                    sessionResults.Add((q, selectedStrings));
                }
                else if (q is OpenEndedQuestion)
                {
                    sessionResults.Add((q, UIHelpers.Prompt("Ваша відповідь")));
                }
            }

            double score = _quiz.CalculateResult(sessionResults);
            _quiz.SaveResult("User", topicName, score);

            Console.Clear();
            UIHelpers.PrintColored("=== РЕЗУЛЬТАТ ===", ConsoleColor.Cyan);
            UIHelpers.PrintColored($"Ви набрали: {score} балів.", ConsoleColor.Green);

            ShowErrorReview(sessionResults);
        }
        catch (Exception ex)
        {
            UIHelpers.PrintColored($"Помилка під час виконання: {ex.Message}", ConsoleColor.Red);
        }
        finally
        {
            UIHelpers.Wait();
        }
    }

    private void ShowErrorReview(List<(Question Q, object UserAns)> results)
    {
        Console.Write("\nБажаєте переглянути помилки? (y/n): ");
        if (Console.ReadLine()?.ToLower() != "y") return;

        foreach (var item in results)
        {
            if (!item.Q.CheckAnswer(item.UserAns))
            {
                UIHelpers.PrintColored($"[X] {item.Q.Text}", ConsoleColor.Red);
                string userResponse = item.UserAns is List<string> list ? string.Join(", ", list) : item.UserAns.ToString();
                Console.WriteLine($"Ваша відповідь була: {userResponse}");
            }
        }
    }
}