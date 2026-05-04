using System.Linq;

namespace Trainer.Domain;

public class MultipleChoiceQuestion : Question
{
    public List<string> Options { get; set; } = new();
    public List<int> CorrectOptionIndices { get; set; } = new();

     public override void Shuffle()
    {
        var random = new Random();
        
        Options = Options.OrderBy(x => random.Next()).ToList();
    }

    public override bool CheckAnswer(object answer)
    {
        if (answer is List<int> userChoices)
        {
            if (userChoices.Count != CorrectOptionIndices.Count) 
                return false;

            return userChoices.All(choice => CorrectOptionIndices.Contains(choice));
        }
        return false;
    }
}