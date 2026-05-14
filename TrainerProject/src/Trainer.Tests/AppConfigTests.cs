using NUnit.Framework;
using Trainer.Domain;

namespace Trainer.Tests;

[TestFixture]
public class AppConfigTests
{
    [Test]
    public void DefaultConfig_HasCorrectDefaultValues()
    {
        var config = new AppConfig();

        Assert.Multiple(() =>
        {

            Assert.That(config.PointsPerQuestion, Is.EqualTo(10.0));
            Assert.That(config.ShowCorrectImmediately, Is.True);
        });
    }
}