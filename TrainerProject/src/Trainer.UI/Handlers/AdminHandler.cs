using Trainer.BusinessLogic;
using Trainer.Domain;

namespace Trainer.UI.Handlers;

public class AdminHandler
{
    private readonly TopicService _service;

    public AdminHandler(TopicService service) { _service = service; }

    public void ListTopics()
    {
        var topics = _service.GetTopics();
        if (!topics.Any()) { Console.WriteLine("Список порожній."); return; }
        foreach (var t in topics) Console.WriteLine($"- {t.Name} ({t.Questions.Count} питань)");
    }

    public void CreateTopic()
    {
        string name = UIHelpers.Prompt("Назва нової теми");
        _service.AddTopic(name);
        UIHelpers.PrintColored("Тему створено успішно.", ConsoleColor.Green);
    }

    public void DeleteTopic(string[] args)
    {
        if (args.Length < 2) { UIHelpers.PrintColored("Вкажіть назву теми!", ConsoleColor.Red); return; }
        string name = string.Join(" ", args.Skip(1));
        _service.DeleteTopic(name);
        UIHelpers.PrintColored($"Тему '{name}' видалено.", ConsoleColor.Yellow);
    }

    public void AddQuestion(string[] args)
    {
        if (args.Length < 2) { Console.WriteLine("Використання: add-question <TopicName>"); return; }
        string tName = string.Join(" ", args.Skip(1));

        Console.WriteLine("Тип: 1-Один варіант, 2-Мультивідповідь, 3-Відкрите");
        string type = UIHelpers.Prompt("Оберіть тип");
        string text = UIHelpers.Prompt("Текст питання");

        if (type == "1")
        {
            var opts = UIHelpers.Prompt("Варіанти через кому").Split(',').Select(s => s.Trim()).ToList();
            string correct = UIHelpers.Prompt("Правильна відповідь");
            _service.AddQuestionToTopic(tName, new SingleChoiceQuestion { Text = text, Options = opts, CorrectAnswer = correct, Points = 10 });
        }
        else if (type == "2")
        {
            var opts = UIHelpers.Prompt("Варіанти через кому").Split(',').Select(s => s.Trim()).ToList();
            var corrects = UIHelpers.Prompt("УСІ правильні через кому").Split(',').Select(s => s.Trim()).ToList();
            _service.AddQuestionToTopic(tName, new MultipleChoiceQuestion { Text = text, Options = opts, CorrectAnswers = corrects, Points = 10 });
        }
        else
        {
            string correct = UIHelpers.Prompt("Правильна відповідь");
            _service.AddQuestionToTopic(tName, new OpenEndedQuestion { Text = text, CorrectAnswer = correct, Points = 10 });
        }
        UIHelpers.PrintColored("Питання додано!", ConsoleColor.Green);
    }
}