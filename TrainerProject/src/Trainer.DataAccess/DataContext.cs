
using Trainer.Domain; 

namespace Trainer.DataAccess;

public class DataContext
{
    public IRepository<Topic> Topics { get; }
    public IRepository<TestResult> History { get; }
    public IRepository<AppConfig> Settings { get; }

    public DataContext()
    {
        Topics = new JsonRepository<Topic>("topics.json");
        History = new JsonRepository<TestResult>("history.json");
        Settings = new JsonRepository<AppConfig>("config.json");
    }
}