namespace Trainer.Domain;

public class AppConfig
{
    public int DefaultSessionDurationMinutes { get; set; } = 30;
    public bool ShowCorrectAnswersImmediately { get; set; } = true;
    public string LastUsedLanguage { get; set; } = "uk-UA";
}