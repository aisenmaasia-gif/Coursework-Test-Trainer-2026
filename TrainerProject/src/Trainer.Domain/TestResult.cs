namespace Trainer.Domain;

public class TestResult
{
    public string TestTitle { get; set; } = string.Empty;
    public DateTime DateFinished { get; set; }
    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }
    public double ScorePercentage => (double)CorrectAnswers / TotalQuestions * 100;
}