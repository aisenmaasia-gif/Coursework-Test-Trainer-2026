using System.Text.Json.Serialization; 

namespace Trainer.Domain;

[JsonDerivedType(typeof(SingleChoiceQuestion), typeDiscriminator: "single")]
[JsonDerivedType(typeof(MultipleChoiceQuestion), typeDiscriminator: "multiple")]
[JsonDerivedType(typeof(OpenEndedQuestion), typeDiscriminator: "open")]
public abstract class Question
{
    public string Text { get; set; } = string.Empty;
    public double Points { get; set; }
    public abstract void Shuffle(); 
    public abstract bool CheckAnswer(object answer);
}