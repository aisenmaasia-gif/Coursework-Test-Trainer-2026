using NUnit.Framework;
using Trainer.Domain;

namespace Trainer.Tests;

[TestFixture]
public class MultipleChoiceQuestionTests
{
    [Test]
    public void CheckAnswer_AllCorrectAnswersProvided_ReturnsTrue()
    {
        var q = new MultipleChoiceQuestion { CorrectAnswers = new List<string> { "A", "B" } };
        var userChoice = new List<string> { "A", "B" };
        Assert.That(q.CheckAnswer(userChoice), Is.True);
    }

    [Test]
    public void CheckAnswer_PartialCorrectAnswers_ReturnsFalse()
    {
        var q = new MultipleChoiceQuestion { CorrectAnswers = new List<string> { "A", "B" } };
        var userChoice = new List<string> { "A" };
        Assert.That(q.CheckAnswer(userChoice), Is.False);
    }

    [Test]
    public void CheckAnswer_ExtraWrongAnswer_ReturnsFalse()
    {
        var q = new MultipleChoiceQuestion { CorrectAnswers = new List<string> { "A", "B" } };
        var userChoice = new List<string> { "A", "B", "C" };
        Assert.That(q.CheckAnswer(userChoice), Is.False);
    }

    [Test]
    public void CheckAnswer_WrongTypeInput_ReturnsFalse()
    {
        var q = new MultipleChoiceQuestion { CorrectAnswers = new List<string> { "A" } };
        Assert.That(q.CheckAnswer("NotAList"), Is.False);
    }
}