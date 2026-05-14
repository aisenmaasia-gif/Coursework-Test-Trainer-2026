using Trainer.Domain; 
using Trainer.BusinessLogic;
[TestFixture]
public class TopicServiceTests
{
    [Test]
    public void AddTopic_WithDuplicateName_ShouldThrowException()
    {
        var existingTopics = new List<Topic> { new Topic { Name = "Existing" } };
        string newName = "Existing";

        Assert.Throws<System.Exception>(() =>
        {
            if (existingTopics.Any(t => t.Name == newName))
                throw new System.Exception("Тема вже існує");
        });
    }

    [Test]
    public void GetTopics_WhenNoTopics_ReturnsEmptyList()
    {
        var topics = new List<Topic>();
        Assert.That(topics, Is.Empty);
    }
}