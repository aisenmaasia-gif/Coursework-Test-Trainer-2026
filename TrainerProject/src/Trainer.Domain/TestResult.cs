namespace Trainer.Domain;

public class TestResult
{
    public string TestTitle { get; set; } = string.Empty;
    public DateTime DateFinished { get; set; }
    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }
    public double ScorePercentage => (double)CorrectAnswers / TotalQuestions * 100;

    public Guid Id { get; set; } = Guid.NewGuid();
    public string StudentName { get; set; } = string.Empty;
    public string TopicName { get; set; } = string.Empty;
    public double Score { get; set; }
    public DateTime DateTime { get; set; } = DateTime.Now;
}