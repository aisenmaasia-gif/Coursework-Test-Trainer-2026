using NUnit.Framework;
using Trainer.Domain;
using Trainer.BusinessLogic;
using System.Linq;

namespace Trainer.Tests;

[TestFixture]
public class QuizServiceAdvancedTests
{
    [Test]
    public void GenerateSession_RequestMoreThanAvailable_ReturnsAllAvailable()
    {
        var questions = new List<Question>
        {
            new SingleChoiceQuestion { Text = "Q1" },
            new SingleChoiceQuestion { Text = "Q2" }
        };

        var result = questions.Take(10).ToList();

        Assert.That(result.Count, Is.EqualTo(2));
    }

    [Test]
    public void GenerateManualSession_WithInvalidIndices_IgnoresThem()
    {
        var allQuestions = new List<Question>
        {
            new SingleChoiceQuestion { Text = "Correct" }
        };

        var chosenIndices = new List<int> { 0, 99, -1 };

        var session = new List<Question>();
        foreach (var i in chosenIndices)
        {
            if (i >= 0 && i < allQuestions.Count)
                session.Add(allQuestions[i]);
        }

        Assert.That(session.Count, Is.EqualTo(1));
        Assert.That(session[0].Text, Is.EqualTo("Correct"));
    }
}