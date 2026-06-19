namespace Trainer.UI.Handlers;

public static class UIHelpers
{
    public static void PrintColored(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    public static string Prompt(string message)
    {
        Console.Write($"{message}: ");
        return Console.ReadLine()?.Trim() ?? "";
    }

    public static int GetSafeInt(string message, int min, int max)
    {
        while (true)
        {
            Console.Write($"{message} ({min}-{max}): ");
            if (int.TryParse(Console.ReadLine(), out int result) && result >= min && result <= max)
                return result;

            PrintColored($"Помилка! Введіть число від {min} до {max}.", ConsoleColor.Red);
        }
    }

    public static double GetSafeDouble(string message)
    {
        while (true)
        {
            Console.Write($"{message}: ");
            if (double.TryParse(Console.ReadLine(), out double result) && result >= 0)
                return result;

            PrintColored("Помилка! Введіть додатне число (напр. 5.5).", ConsoleColor.Red);
        }
    }

    public static void Wait()
    {
        Console.WriteLine("\nНатисніть будь-яку клавішу...");
        Console.ReadKey();
    }
    public static List<int> GetSafeIntList(string message, int max)
    {
        while (true)
        {
            string input = Prompt(message);
            if (string.IsNullOrWhiteSpace(input))
            {
                PrintColored("Помилка! Ввід не може бути порожнім.", ConsoleColor.Red);
                continue;
            }

            string[] parts = input.Split(',');

            List<int> indices = new List<int>();

            foreach (string part in parts)
            {

                if (int.TryParse(part.Trim(), out int id))
                {

                    if (id >= 1 && id <= max)
                    {
                        indices.Add(id);
                    }
                }
            }

            PrintColored($"Помилка! Введіть коректні номери питань від 1 до {max} через кому.", ConsoleColor.Red);
        }
    }
}