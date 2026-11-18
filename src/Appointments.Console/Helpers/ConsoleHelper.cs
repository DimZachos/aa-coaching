namespace Appointments.Console.Helpers;

public static class ConsoleHelper
{
    public static int ShowMenu(string title, string[] options)
    {
        System.Console.WriteLine($"\n{title}");
        for (int i = 0; i < options.Length; i++)
        {
            System.Console.WriteLine($"{i + 1}. {options[i]}");
        }

        string input = GetInput($"Choose (1-{options.Length}): ");
        return int.TryParse(input, out int choice) ? choice : 0;
    }
    
    public static string GetInput(string input)
    {
        System.Console.Write(input);
        var value = System.Console.ReadLine()?.Trim() ?? string.Empty;
        return value;
    }
    
    public static void PrintError(string message)
    {
        System.Console.WriteLine($"{message}");
    }
    
    public static void Pause()
    {
        GetInput("\nPress Enter");
    }
    
    public static void Print(string text)
    {
        System.Console.WriteLine(text);
    }
}
