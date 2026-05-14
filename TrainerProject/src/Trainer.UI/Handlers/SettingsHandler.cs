using Trainer.DataAccess;
using Trainer.Domain;

namespace Trainer.UI.Handlers;

public class SettingsHandler
{
    private readonly DataContext _context;

    public SettingsHandler(DataContext context) { _context = context; }

    public void ShowSettings()
    {
        var config = _context.Settings.GetAll().FirstOrDefault() ?? new AppConfig();
        
        UIHelpers.PrintColored("\n=== Налаштування ===", ConsoleColor.Magenta);
        Console.WriteLine($"1. Режим показу відповідей: {(config.ShowCorrectImmediately ? "Миттєво" : "В кінці")}");
        
        string choice = UIHelpers.Prompt("\nЗмінити режим показу? (y/n)");
        if (choice.ToLower() == "y")
        {
            config.ShowCorrectImmediately = !config.ShowCorrectImmediately;
            _context.Settings.SaveAll(new List<AppConfig> { config });
            UIHelpers.PrintColored("Налаштування збережено!", ConsoleColor.Green);
        }
        UIHelpers.Wait();
    }
}