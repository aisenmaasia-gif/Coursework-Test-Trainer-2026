namespace Trainer.Domain;

public class Topic
{
    public string Name { get; set; } = string.Empty;
    
    public List<Test> Tests { get; set; } = new();

     public List<Question> Questions { get; set; } = new();
}