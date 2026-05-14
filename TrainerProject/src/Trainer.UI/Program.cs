using Trainer.DataAccess;
using Trainer.BusinessLogic;
using Trainer.UI.Handlers;

var context = new DataContext();
var quizHandler = new QuizHandler(new QuizService(context), new TopicService(context));
var adminHandler = new AdminHandler(new TopicService(context));
var statsHandler = new StatsHandler(new StatisticsService(context), context);
var settingsHandler = new SettingsHandler(context);

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("=== Trainer CLI 2026 | 'help' для довідки ===");

while (true)
{
    Console.Write("\n> ");
    var input = Console.ReadLine()?.Trim().Split(' ');
    if (input == null || input.Length == 0) continue;
    string cmd = input[0].ToLower();

    try
    {
        switch (cmd)
        {
            case "help": ShowHelp(); break;
            case "exit": return;
            case "list": adminHandler.ListTopics(); break;
            case "start": quizHandler.StartQuiz(input); break;
            case "history": statsHandler.ShowHistory(); break;
            case "stats": statsHandler.ShowStats(); break;
            case "create-topic": adminHandler.CreateTopic(); break;
            case "add-question": adminHandler.AddQuestion(input); break;
            case "delete-topic": adminHandler.DeleteTopic(input); break;
            case "settings": settingsHandler.ShowSettings(); break;
            case "edit-topic": adminHandler.EditTopicName(input); break;
            case "edit-question": adminHandler.EditQuestion(input); break;
            case "delete-question": adminHandler.DeleteQuestion(input); break;
            case "edit-topic-name": adminHandler.EditTopic(input); break;
            default: Console.WriteLine("Невідома команда."); break;
        }
    }
    catch (Exception ex) { UIHelpers.PrintColored($"Помилка: {ex.Message}", ConsoleColor.Red); }
}

void ShowHelp()

{
    Console.WriteLine("\nДоступні команди:");
    Console.WriteLine("  list                  - Показати всі теми");
    Console.WriteLine("  start <Topic>         - Пройти тест (випадковий або ручний вибір)");
    Console.WriteLine("  history               - Історія результатів");
    Console.WriteLine("  stats                 - Статистика успішності");

    Console.WriteLine("\nАдміністрування тем:");
    Console.WriteLine("  create-topic          - Створити нову тему");
    Console.WriteLine("  edit-topic <Topic>    - Змінити назву теми");
    Console.WriteLine("  delete-topic <Topic>  - Видалити тему");

    Console.WriteLine("\nАдміністрування питань:");
    Console.WriteLine("  add-question <Topic>  - Додати нове питання");
    Console.WriteLine("  edit-question <Topic> - Редагувати питання");
    Console.WriteLine("  delete-question <Topic>- Видалити питання");

    Console.WriteLine("\nІнше:");
    Console.WriteLine("  settings              - Налаштування застосунку");
    Console.WriteLine("  exit                  - Вихід");
}
