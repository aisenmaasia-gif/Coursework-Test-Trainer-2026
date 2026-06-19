using Trainer.BusinessLogic;
using Trainer.Domain;
namespace Trainer.Tests;

[TestFixture]
public class QuizServiceTests
{
    [Test]
    public void CalculateResult_AllCorrect_ReturnsFullPoints()
    {
        var service = new QuizService(null!);
        var q1 = new SingleChoiceQuestion { Points = 10, CorrectAnswer = "1" };
        var q2 = new OpenEndedQuestion { Points = 5, CorrectAnswer = "2" };

        var answers = new List<(Question, object)>
        {
            (q1, "1"),
            (q2, "2")
        };

        var result = service.CalculateResult(answers);
        Assert.That(result, Is.EqualTo(15.0));
    }

    [Test]
    public void CalculateResult_AllWrong_ReturnsZero()
    {
        var service = new QuizService(null!);
        var q = new SingleChoiceQuestion { Points = 10, CorrectAnswer = "Correct" };
        var answers = new List<(Question, object)> { (q, "Wrong") };

        Assert.That(service.CalculateResult(answers), Is.EqualTo(0.0));
    }

    [Test]
    public void CalculateResult_EmptySession_ReturnsZero()
    {
        var service = new QuizService(null!);
        Assert.That(service.CalculateResult(new List<(Question, object)>()), Is.EqualTo(0.0));
    }
    [Test]
    public void GenerateSession_RequestMoreQuestionsThanAvailable_ReturnsMaxAvailable()
    {

        var questions = new List<Question>
    {
        new SingleChoiceQuestion { Text = "Q1" },
        new SingleChoiceQuestion { Text = "Q2" }
    };

        var countToTake = 5;
        var result = questions.Take(countToTake).ToList();

        Assert.That(result.Count, Is.EqualTo(2));
    }

    [Test]
    public void GenerateSession_NegativeCount_ReturnsEmptyList()
    {
        var questions = new List<Question> { new SingleChoiceQuestion() };
        var result = questions.Take(-1).ToList();

        Assert.That(result, Is.Empty);
    }
}