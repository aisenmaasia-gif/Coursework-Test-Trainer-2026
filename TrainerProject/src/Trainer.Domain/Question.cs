namespace Trainer.Domain;

public abstract class Question
{
    public string Text { get; set; } = string.Empty;
    public Difficulty Level { get; set; }
    public double Points { get; set; }
    
    public abstract bool CheckAnswer(object answer);
}