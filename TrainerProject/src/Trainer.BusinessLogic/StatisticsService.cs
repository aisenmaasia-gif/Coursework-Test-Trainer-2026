using Trainer.Domain;
using Trainer.DataAccess;

namespace Trainer.BusinessLogic;

public class StatisticsService
{
    private readonly DataContext _context;

    public StatisticsService(DataContext context)
    {
        _context = context;
    }

    public Dictionary<string, double> GetAverageScoresByTopic()
    {
        return _context.History.GetAll()
            .GroupBy(r => r.TopicName)
            .ToDictionary(
                g => g.Key,
                g => g.Average(r => r.Score)
            );
    }

    public int GetTotalTestsPassed() => _context.History.GetAll().Count();
}