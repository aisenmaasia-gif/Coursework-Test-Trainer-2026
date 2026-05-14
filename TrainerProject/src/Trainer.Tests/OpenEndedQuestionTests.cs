using NUnit.Framework;
using Trainer.Domain;

namespace Trainer.Tests;

[TestFixture]
public class OpenEndedQuestionTests
{
    [Test]
    public void CheckAnswer_SameTextDifferentCase_ReturnsTrue()
    {
        var q = new OpenEndedQuestion { CorrectAnswer = "Україна" };
        Assert.That(q.CheckAnswer("уКрАїНа"), Is.True);
    }

    [Test]
    public void CheckAnswer_WithLeadingSpaces_ReturnsTrue()
    {
        var q = new OpenEndedQuestion { CorrectAnswer = "Київ" };
        Assert.That(q.CheckAnswer("  Київ  "), Is.True);
    }

    [Test]
    public void CheckAnswer_CompletelyWrong_ReturnsFalse()
    {
        var q = new OpenEndedQuestion { CorrectAnswer = "Так" };
        Assert.That(q.CheckAnswer("Ні"), Is.False);
    }
}