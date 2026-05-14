namespace Trainer.Domain;

public class SingleChoiceQuestion : Question
{
    public List<string> Options { get; set; } = new();
     public string CorrectAnswer { get; set; } = string.Empty;
    public int CorrectOptionIndex { get; set; }

     public override void Shuffle()
    {
        var random = new Random();
        
        Options = Options.OrderBy(x => random.Next()).ToList();
    }


    public override bool CheckAnswer(object answer)
    {
        if (answer is int selectedIndex)
        {
            return selectedIndex == CorrectOptionIndex;
        }
        return false;
    }
}