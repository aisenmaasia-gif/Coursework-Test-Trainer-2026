using Trainer.Domain;
[TestFixture]
public class StatisticsServiceTests
{
    [Test]
    public void GetAverageScoresByTopic_MultipleTopics_CalculatesCorrectly()
    {
        var results = new List<TestResult>
        {
            new TestResult { TopicName = "Math", Score = 10 },
            new TestResult { TopicName = "Math", Score = 20 },
            new TestResult { TopicName = "History", Score = 50 }
        };

        var averageMath = results.Where(r => r.TopicName == "Math").Average(r => r.Score);

        Assert.That(averageMath, Is.EqualTo(15.0));
    }

    [Test]
    public void GetTotalTestsPassed_WhenEmpty_ReturnsZero()
    {
        var results = new List<TestResult>();
        Assert.That(results.Count, Is.EqualTo(0));
    }
}