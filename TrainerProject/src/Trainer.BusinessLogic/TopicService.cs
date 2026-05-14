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

        topics.Add(new Topic {Name = name});
        _context.Topics.SaveAll(topics);
    }

    public void AddQuestionToTopic(string topicName, Question question)
    {
        var topics = _context.Topics.GetAll().ToList();
        var topic = topics.FirstOrDefault(t => t.Name == topicName);

        if(topic == null) throw new Exception("Тему не знайдено");

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
}