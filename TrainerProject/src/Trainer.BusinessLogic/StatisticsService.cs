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
        Dictionary<string, List<double>> topicScores = new Dictionary<string, List<double>>();

        foreach (var record in _context.History.GetAll())
        {
            if (!topicScores.ContainsKey(record.TopicName))
            {
                topicScores[record.TopicName] = new List<double>();
            }

            topicScores[record.TopicName].Add(record.Score);
        }

        Dictionary<string, double> averageScores = new Dictionary<string, double>();

        foreach (var pair in topicScores)
        {
            string topicName = pair.Key;
            List<double> scores = pair.Value;

            double sum = 0;
            foreach (double score in scores)
            {
                sum += score;
            }

            double average = scores.Count > 0 ? sum / scores.Count : 0.0;

            averageScores[topicName] = average;
        }

        return averageScores;
    }

    public int GetTotalTestsPassed() => _context.History.GetAll().Count();
}