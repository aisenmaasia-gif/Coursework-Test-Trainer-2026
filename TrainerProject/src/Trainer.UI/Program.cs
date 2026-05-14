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

    try {
        switch (cmd) {
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
            default: Console.WriteLine("Невідома команда."); break;
        }
    } catch (Exception ex) { UIHelpers.PrintColored($"Помилка: {ex.Message}", ConsoleColor.Red); }
}

void ShowHelp() {
    Console.WriteLine("\nКоманди: list, start <Topic>, history, stats, create-topic, add-question <Topic>, delete-topic <Topic>, settings, exit");
}