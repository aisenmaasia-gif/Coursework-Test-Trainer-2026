
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
            if (userAnswers.Count != CorrectAnswers.Count) 
                return false;

            return userAnswers.All(ua => 
                CorrectAnswers.Any(ca => string.Equals(ua.Trim(), ca.Trim(), StringComparison.OrdinalIgnoreCase))
            );
        }
        return false;
    }
}