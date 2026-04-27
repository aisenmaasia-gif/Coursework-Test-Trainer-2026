using System.Linq;

namespace Trainer.Domain;

public class MultipleChoiceQuestion : Question
{
    public List<string> Options { get; set; } = new();
    public List<int> CorrectOptionIndices { get; set; } = new();
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