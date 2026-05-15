
namespace Trainer.Tests;

[TestFixture]
public class TopicModelTests
{
    [Test]
    public void NewTopic_ShouldInitializeEmptyQuestionsList()
    {
        var topic = new Topic();

        Assert.That(topic.Questions, Is.Not.Null);
        Assert.That(topic.Questions.Count, Is.EqualTo(0));
    }

    [Test]
    public void TopicName_CanBeUpdated()
    {
        var topic = new Topic { Name = "OldName" };
        topic.Name = "NewName";

        Assert.That(topic.Name, Is.EqualTo("NewName"));
    }
}