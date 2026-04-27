namespace Trainer.Domain;

public class Test
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<Question> Questions { get; set; } = new();

    public void AddQuestion(Question question)
    {
        Questions.Add(question);
    }
}