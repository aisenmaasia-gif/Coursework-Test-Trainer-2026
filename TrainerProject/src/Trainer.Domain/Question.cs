using System.Text.Json.Serialization; 

namespace Trainer.Domain;

[JsonDerivedType(typeof(SingleChoiceQuestion), typeDiscriminator: "single")]
[JsonDerivedType(typeof(MultipleChoiceQuestion), typeDiscriminator: "multiple")]
[JsonDerivedType(typeof(OpenEndedQuestion), typeDiscriminator: "open")]
public abstract class Question
{
    public string Text { get; set; } = string.Empty;
    public Difficulty Level { get; set; }
    public double Points { get; set; }

    public abstract bool CheckAnswer(object answer);
}