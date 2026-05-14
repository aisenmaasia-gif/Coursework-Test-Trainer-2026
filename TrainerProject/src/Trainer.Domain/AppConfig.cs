namespace Trainer.Domain;

public class AppConfig
{
    public double PointsPerQuestion { get; set; } = 10.0;
    public int DefaultSessionDurationMinutes { get; set; } = 30;
    public bool ShowCorrectAnswersImmediately { get; set; } = true;
    public string LastUsedLanguage { get; set; } = "uk-UA";
    public bool ShowCorrectImmediately { get; set; } = true;
    public int SessionDurationMinutes { get; set; } = 20;
    public double DefaultPoints { get; set; } = 10.0;

}