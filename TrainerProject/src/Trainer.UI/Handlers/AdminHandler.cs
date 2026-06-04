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
            _service.AddQuestionToTopic(tName, new SingleChoiceQuestion { Text = text, Options = opts, CorrectAnswer = correct });
        }
        else if (type == "2")
        {
            var opts = UIHelpers.Prompt("Варіанти через кому").Split(',').Select(s => s.Trim()).ToList();
            var corrects = UIHelpers.Prompt("УСІ правильні через кому").Split(',').Select(s => s.Trim()).ToList();
            _service.AddQuestionToTopic(tName, new MultipleChoiceQuestion { Text = text, Options = opts, CorrectAnswers = corrects });
        }
        else
        {
            string correct = UIHelpers.Prompt("Правильна відповідь");
            _service.AddQuestionToTopic(tName, new OpenEndedQuestion { Text = text, CorrectAnswer = correct });
        }
        UIHelpers.PrintColored("Питання додано!", ConsoleColor.Green);
    }
    public void EditTopicName(string[] args)
    {
        if (args.Length < 2) { Console.WriteLine("Вкажіть стару назву!"); return; }
        string oldName = string.Join(" ", args.Skip(1));

        string newName = UIHelpers.Prompt("Введіть нову назву теми");

        _service.UpdateTopicName(oldName, newName);
        UIHelpers.PrintColored("Назву змінено!", ConsoleColor.Green);
    }
    public void DeleteQuestion(string[] args)
    {
        if (args.Length < 2) { Console.WriteLine("Вкажіть назву теми!"); return; }
        string tName = string.Join(" ", args.Skip(1));

        var topics = _service.GetTopics();
        var topic = topics.FirstOrDefault(t => t.Name == tName);

        if (topic == null) { Console.WriteLine("Тему не знайдено."); return; }

        for (int i = 0; i < topic.Questions.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {topic.Questions[i].Text}");
        }

        int index = int.Parse(UIHelpers.Prompt("Номер питання для ВИДАЛЕННЯ")) - 1;
        _service.DeleteQuestion(tName, index);
        UIHelpers.PrintColored("Питання видалено.", ConsoleColor.Yellow);
    }
    public void EditQuestion(string[] args)
    {
        if (args.Length < 2) { Console.WriteLine("Вкажіть тему!"); return; }
        string tName = string.Join(" ", args.Skip(1));

        var topic = _service.GetTopics().FirstOrDefault(t => t.Name == tName);
        if (topic == null) return;

        for (int i = 0; i < topic.Questions.Count; i++)
            Console.WriteLine($"{i + 1}. {topic.Questions[i].Text}");

        int index = int.Parse(UIHelpers.Prompt("Яке питання редагуємо?")) - 1;
        var q = topic.Questions[index];

        Console.WriteLine($"Поточний текст: {q.Text}");
        q.Text = UIHelpers.Prompt("Новий текст (або залиште порожнім)");

        q.Points = double.Parse(UIHelpers.Prompt($"Нові бали (зараз {q.Points})"));

        if (q is SingleChoiceQuestion scq)
        {
            Console.WriteLine("Поточні варіанти: " + string.Join(", ", scq.Options));
            scq.Options = UIHelpers.Prompt("Нові варіанти (через кому)").Split(',').Select(s => s.Trim()).ToList();
            scq.CorrectAnswer = UIHelpers.Prompt("Нова правильна відповідь");
        }

        _service.UpdateQuestion(tName, index, q);
        UIHelpers.PrintColored("Дані оновлено!", ConsoleColor.Green);
    }
    public void EditTopic(string[] args)
    {
        if (args.Length < 2)
        {
            UIHelpers.PrintColored("Помилка! Використання: edit-topic <СтараНазва>", ConsoleColor.Red);
            return;
        }

        string oldName = string.Join(" ", args.Skip(1));

        string newName = UIHelpers.Prompt($"Введіть нову назву для теми '{oldName}'");

        if (string.IsNullOrWhiteSpace(newName))
        {
            UIHelpers.PrintColored("Назва не може бути порожньою!", ConsoleColor.Red);
            return;
        }

        _service.UpdateTopicName(oldName, newName);

        UIHelpers.PrintColored("Назву теми успішно змінено!", ConsoleColor.Green);
    }
}