namespace Trainer.Domain;

public class SingleChoiceQuestion : Question
{
    public List<string> Options { get; set; } = new();
    public string CorrectAnswer { get; set; } = string.Empty;

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
        if (answer == null) return false;

        string userAns = answer.ToString()?.Trim() ?? string.Empty;
        string correctAns = CorrectAnswer?.Trim() ?? string.Empty;

        return string.Equals(userAns, correctAns, StringComparison.OrdinalIgnoreCase);
    }
}