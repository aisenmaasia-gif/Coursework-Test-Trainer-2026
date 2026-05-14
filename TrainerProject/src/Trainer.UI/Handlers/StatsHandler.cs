using Trainer.BusinessLogic;
using Trainer.DataAccess;

namespace Trainer.UI.Handlers;

public class StatsHandler
{
    private readonly StatisticsService _stats;
    private readonly DataContext _context;

    public StatsHandler(StatisticsService stats, DataContext context)
    {
        _stats = stats;
        _context = context;
    }

    public void ShowHistory()
    {
        UIHelpers.PrintColored("\n=== Історія проходжень ===", ConsoleColor.Cyan);
        var history = _context.History.GetAll();
        if (!history.Any()) Console.WriteLine("Історія порожня.");
        foreach (var h in history) Console.WriteLine($"{h.DateTime:G} | {h.TopicName} | {h.Score} балів");
        UIHelpers.Wait();
    }

    public void ShowStats()
    {
        UIHelpers.PrintColored("\n=== Статистика за темами ===", ConsoleColor.Cyan);
        var report = _stats.GetAverageScoresByTopic();
        if (!report.Any()) Console.WriteLine("Статистика відсутня.");
        foreach (var r in report) Console.WriteLine($"Тема: {r.Key,-15} | Сер. бал: {r.Value:F2}");
        UIHelpers.Wait();
    }
}