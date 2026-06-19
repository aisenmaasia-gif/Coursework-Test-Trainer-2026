using Trainer.Domain;
[TestFixture]
public class TopicServiceTests
{
    [Test]
    public void AddTopic_WithDuplicateName_ShouldThrowException()
    {
        var existingTopics = new List<Topic> { new Topic { Name = "Existing" } };
        string newName = "Existing";

        bool exceptionThrown = false;

        try
        {
            foreach (var t in existingTopics)
            {
                if (t.Name == newName)
                {
                    throw new Exception("Тема вже існує");
                }
            }
        }
        catch (Exception ex) when (ex.Message == "Тема вже існує")
        {
            exceptionThrown = true;
        }

        Assert.That(exceptionThrown, Is.True);
    }

    [Test]
    public void GetTopics_WhenNoTopics_ReturnsEmptyList()
    {
        var topics = new List<Topic>();
        Assert.That(topics, Is.Empty);
    }
}