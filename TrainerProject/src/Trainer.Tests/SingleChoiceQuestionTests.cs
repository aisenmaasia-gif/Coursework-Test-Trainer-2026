
using Trainer.Domain;

namespace Trainer.Tests;

[TestFixture]
public class SingleChoiceQuestionTests
{
    [Test]
    public void CheckAnswer_CorrectString_ReturnsTrue()
    {
        var q = new SingleChoiceQuestion { CorrectAnswer = "Київ" };
        Assert.That(q.CheckAnswer("Київ"), Is.True);
    }

    [Test]
    public void CheckAnswer_WrongString_ReturnsFalse()
    {
        var q = new SingleChoiceQuestion { CorrectAnswer = "Київ" };
        Assert.That(q.CheckAnswer("Львів"), Is.False);
    }

    [Test]
    public void CheckAnswer_NullInput_ReturnsFalse()
    {
        var q = new SingleChoiceQuestion { CorrectAnswer = "Київ" };
        Assert.That(q.CheckAnswer(null), Is.False);
    }
}