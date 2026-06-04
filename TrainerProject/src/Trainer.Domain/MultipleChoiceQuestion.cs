namespace Trainer.Domain;

public class MultipleChoiceQuestion : Question
{
    public List<string> Options { get; set; } = new();
    public List<string> CorrectAnswers { get; set; } = new();
    public override void Shuffle()
    {
        var random = new Random();
        int n = Options.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            string value = Options[k];
            Options[k] = Options[n];
            Options[n] = value;
        }
    }

    public override bool CheckAnswer(object answer)
    {
        if (answer is List<string> userAnswers)
        {
            HashSet<string> userSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (string item in userAnswers)
            {
                userSet.Add(item.Trim());
            }

            HashSet<string> correctSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (string item in CorrectAnswers)
            {
                correctSet.Add(item.Trim());
            }

            return userSet.SetEquals(correctSet);
        }
        return false;
    }
}