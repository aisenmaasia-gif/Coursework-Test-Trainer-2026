namespace Trainer.Domain;

public class MultipleChoiceQuestion : Question
{
    public List<string> Options { get; set; } = new();
    public List<string> CorrectAnswers { get; set; } = new();

    public override void Shuffle()
    {
        var random = new Random();
        Options = Options.OrderBy(x => random.Next()).ToList();
    }

    public override bool CheckAnswer(object answer)
    {
        if (answer is List<string> userAnswers)
        {
            HashSet<string> userSet = CreateTrimmedSet(userAnswers);
            HashSet<string> correctSet = CreateTrimmedSet(CorrectAnswers);

            return userSet.SetEquals(correctSet);
        }
        return false;
    }

    private HashSet<string> CreateTrimmedSet(List<string> sourceList)
    {
        HashSet<string> resultSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (string item in sourceList)
        {
            resultSet.Add(item.Trim());
        }
        return resultSet;
    }
}