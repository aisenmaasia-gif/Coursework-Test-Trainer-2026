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

    public void AddTopic(string name)
    {
        var topics = _context.Topics.GetAll().ToList();
        if (topics.Any(t => t.Name == name)) throw new Exception("Тема вже існує");

        topics.Add(new Topic { Name = name });
        _context.Topics.SaveAll(topics);
    }

    public void AddQuestionToTopic(string topicName, Question question)
    {
        var topics = _context.Topics.GetAll().ToList();
        var topic = topics.FirstOrDefault(t => t.Name == topicName);

        if (topic == null) throw new Exception("Тему не знайдено");

        topic.Questions.Add(question);
        _context.Topics.SaveAll(topics);
    }

    public List<Topic> GetTopics() => _context.Topics.GetAll().ToList();

    public void DeleteTopic(string name)
    {
        var topics = _context.Topics.GetAll().ToList();

        var topicToRemove = topics.FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (topicToRemove == null)
        {
            throw new Exception($"Тему з назвою '{name}' не знайдено.");
        }

        topics.Remove(topicToRemove);

        _context.Topics.SaveAll(topics);
    }

    public void UpdateTopicName(string oldName, string newName)
    {
        var topics = _context.Topics.GetAll().ToList();
        var topic = topics.FirstOrDefault(t => t.Name == oldName);

        if (topic != null)
        {
            topic.Name = newName;
            _context.Topics.SaveAll(topics);
        }
    }

    public void DeleteQuestion(string topicName, int questionIndex)
    {
        var topics = _context.Topics.GetAll().ToList();
        var topic = topics.FirstOrDefault(t => t.Name == topicName);

        if (topic != null && questionIndex >= 0 && questionIndex < topic.Questions.Count)
        {
            topic.Questions.RemoveAt(questionIndex);
            _context.Topics.SaveAll(topics);
        }
    }

    public void UpdateQuestion(string topicName, int index, Question updated)
    {
        var topics = _context.Topics.GetAll().ToList();
        var topic = topics.FirstOrDefault(t => t.Name == topicName);

        if (topic != null && index >= 0 && index < topic.Questions.Count)
        {
            topic.Questions[index] = updated;
            _context.Topics.SaveAll(topics);
        }
    }
    public void NewTopicName(string oldName, string newName)
    {
        var topics = _context.Topics.GetAll().ToList();

        var topicToEdit = topics.FirstOrDefault(t => t.Name.Equals(oldName, StringComparison.OrdinalIgnoreCase));

        if (topicToEdit == null)
        {
            throw new Exception($"Тему '{oldName}' не знайдено.");
        }

        bool nameExists = topics.Any(t => t.Name.Equals(newName, StringComparison.OrdinalIgnoreCase));
        if (nameExists)
        {
            throw new Exception($"Тема з назвою '{newName}' вже існує.");
        }

        topicToEdit.Name = newName;

        _context.Topics.SaveAll(topics);
    }
}