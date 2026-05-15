
[TestFixture]
public class ShuffleTests
{
    [Test]
    public void Shuffle_DoesNotLoseData()
    {
        var q = new SingleChoiceQuestion
        {
            Options = new List<string> { "1", "2", "3", "4", "5" }
        };

        q.Shuffle();

        Assert.That(q.Options.Count, Is.EqualTo(5));
        Assert.That(q.Options, Contains.Item("1"));
        Assert.That(q.Options, Contains.Item("5"));
    }
}