using Trainer.Domain;
using Trainer.DataAccess;

namespace Trainer.BusinessLogic;

public delegate void QuizCompletedHandler(TestResult result);

public class QuizService
{
    private readonly DataContext _context;
    private readonly Random _random = new();

    public event QuizCompletedHandler? OnQuizCompleted;

    public QuizService(DataContext context)
    {
        _context = context;
    }

    public List<Question> GenerateSession(string topicName, int questionCount, bool shuffle = true)
    {
        var topic = _context.Topics.GetAll().FirstOrDefault(t => t.Name == topicName);

        if (topic == null || !topic.Questions.Any())
            throw new Exception("Тема не знайдена або вона порожня.");

        var questions = topic.Questions.AsEnumerable();

        if (shuffle)
        {
            questions = questions.OrderBy(q => _random.Next());

            foreach (var q in questions) q.Shuffle();
        }

        return questions.Take(questionCount).ToList();

    }

    public double CalculateResult(List<(Question Question, object UserAnswer)> sessionAnswers)
    {
        double totalPoints = 0;
        foreach (var item in sessionAnswers)
        {
            if (item.Question.CheckAnswer(item.UserAnswer))
            {
                totalPoints += item.Question.Points;
            }
        }
        return totalPoints;
    }

    public void SaveResult(string studentName, string topicName, double score)
    {
        var newResult = new TestResult
        {
            StudentName = studentName,
            TopicName = topicName,
            Score = score,
            DateTime = DateTime.Now
        };

        var results = _context.History.GetAll().ToList();
        results.Add(newResult);
        _context.History.SaveAll(results);
        OnQuizCompleted?.Invoke(newResult);
    }
    public List<Question> GenerateManualSession(string topicName, List<int> selectedIndices)
    {
        var topic = _context.Topics.GetAll()
            .FirstOrDefault(t => t.Name.Equals(topicName, StringComparison.OrdinalIgnoreCase));

        if (topic == null) throw new Exception("Тему не знайдено.");

        List<Question> customList = new List<Question>();

        foreach (int index in selectedIndices)
        {
            if (index >= 0 && index < topic.Questions.Count)
            {
                customList.Add(topic.Questions[index]);
            }
        }

        return customList;
    }
}