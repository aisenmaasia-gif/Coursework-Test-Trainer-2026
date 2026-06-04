using Trainer.Domain;
using Trainer.DataAccess;

namespace Trainer.BusinessLogic;

public class TopicService
{
    private readonly DataContext _context;

    public TopicService(DataContext context)
    {
        _context = context;
    }

    public List<Topic> GetTopics() => _context.Topics.GetAll().ToList();

    public void AddTopic(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception("Назва теми не може бути порожньою.");

        var topics = GetTopics();
        if (topics.Any(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            throw new Exception($"Тема з назвою '{name}' вже існує.");

        topics.Add(new Topic { Name = name });
        _context.Topics.SaveAll(topics);
    }

    public void DeleteTopic(string name)
    {
        var topics = GetTopics();
        var topic = topics.FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (topic == null)
            throw new Exception($"Тему '{name}' не знайдено.");

        topics.Remove(topic);
        _context.Topics.SaveAll(topics);
    }

    public void UpdateTopicName(string oldName, string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new Exception("Нова назва не може бути порожньою.");

        var topics = GetTopics();
        var topic = topics.FirstOrDefault(t => t.Name.Equals(oldName, StringComparison.OrdinalIgnoreCase));

        if (topic == null)
            throw new Exception($"Тему '{oldName}' не знайдено.");

        if (topics.Any(t => t.Name.Equals(newName, StringComparison.OrdinalIgnoreCase)))
            throw new Exception($"Тема з назвою '{newName}' вже існує.");

        topic.Name = newName;
        _context.Topics.SaveAll(topics);
    }

    public void AddQuestionToTopic(string topicName, Question question)
    {
        var topics = GetTopics();
        var topic = topics.FirstOrDefault(t => t.Name.Equals(topicName, StringComparison.OrdinalIgnoreCase));

        if (topic == null) throw new Exception("Тему не знайдено.");
        if (string.IsNullOrWhiteSpace(question.Text)) throw new Exception("Текст питання не може бути порожнім.");

        if (question.Points <= 0)
        {
            var config = _context.Settings.GetAll().FirstOrDefault() ?? new AppConfig();
            question.Points = config.PointsPerQuestion;
        }

        topic.Questions.Add(question);
        _context.Topics.SaveAll(topics);
    }

    public void DeleteQuestion(string topicName, int index)
    {
        var topics = GetTopics();
        var topic = topics.FirstOrDefault(t => t.Name.Equals(topicName, StringComparison.OrdinalIgnoreCase));

        if (topic == null) throw new Exception("Тему не знайдено.");
        if (index < 0 || index >= topic.Questions.Count) throw new Exception("Невірний номер запитання.");

        topic.Questions.RemoveAt(index);
        _context.Topics.SaveAll(topics);
    }

    public void UpdateQuestion(string topicName, int index, Question updated)
    {
        var topics = GetTopics();
        var topic = topics.FirstOrDefault(t => t.Name.Equals(topicName, StringComparison.OrdinalIgnoreCase));

        if (topic == null) throw new Exception("Тему не знайдено.");
        if (index < 0 || index >= topic.Questions.Count) throw new Exception("Невірний номер запитання.");

        topic.Questions[index] = updated;
        _context.Topics.SaveAll(topics);
    }
}