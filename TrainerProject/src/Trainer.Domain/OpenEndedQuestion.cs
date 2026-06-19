namespace Trainer.Domain;

public class OpenEndedQuestion : Question
{
    public List<string> AcceptableAnswers { get; set; } = new();

    public string CorrectAnswer
    {
        get => AcceptableAnswers.FirstOrDefault() ?? string.Empty;
        set
        {
            AcceptableAnswers.Clear();
            if (!string.IsNullOrEmpty(value)) AcceptableAnswers.Add(value);
        }
    }

    public override bool CheckAnswer(object answer)
    {
        if (answer is string userAnswer)
        {
            string cleanUserAnswer = userAnswer.Trim();

            foreach (string acceptableAnswer in AcceptableAnswers)
            {
                if (string.Equals(acceptableAnswer.Trim(), cleanUserAnswer, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
        }

        return false;
    }
}