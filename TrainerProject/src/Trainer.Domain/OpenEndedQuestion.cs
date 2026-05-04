namespace Trainer.Domain;

public class OpenEndedQuestion : Question
{
    public List<string> AcceptableAnswers { get; set; } = new();

     public override void Shuffle()
    {
       
    }


    public override bool CheckAnswer(object answer)
    {
        if (answer is string userAnswer)
        {
            string cleanAnswer = userAnswer.Trim();
            
            return AcceptableAnswers.Any(a => string.Equals(a.Trim(), cleanAnswer, StringComparison.OrdinalIgnoreCase));
        }
        return false;
    }
}