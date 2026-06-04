namespace Trainer.Domain;

public class AppConfig
{
    public double PointsPerQuestion { get; set; } = 10.0;
    public int SessionDurationMinutes { get; set; } = 20;
    public bool ShowCorrectImmediately { get; set; } = true;
    public string LastUsedLanguage { get; set; } = "uk-UA";
}