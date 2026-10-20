namespace Appointments.Console.Helpers;

public static class ConsoleHelper
{
    public static void PrintHeader(string title)
    {
        System.Console.Clear();
        System.Console.WriteLine(new string('=', 50));
        System.Console.WriteLine(title.PadLeft((50 + title.Length) / 2));
        System.Console.WriteLine(new string('=', 50));
        System.Console.WriteLine();
    }
    
    public static int ShowMenu(string title, params string[] options)
    {
        System.Console.WriteLine($"\n{title}");
        for (int i = 0; i < options.Length; i++)
            System.Console.WriteLine($"{i + 1}. {options[i]}");

        while (true)
        {
            System.Console.Write($"Choose (1-{options.Length}): ");
            if (int.TryParse(System.Console.ReadLine(), out int choice) && choice >= 1 && choice <= options.Length)
                return choice;
            System.Console.WriteLine("Invalid choice.");
        }
    }
    
    public static string GetInput(string prompt)
    {
        while (true)
        {
            System.Console.Write(prompt);
            string? input = System.Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(input))
                return input;
            System.Console.WriteLine("Cannot be empty.");
        }
    }
    
    public static int GetIntInput(string prompt)
    {
        while (true)
        {
            System.Console.Write(prompt);
            if (int.TryParse(System.Console.ReadLine(), out int value))
                return value;
            System.Console.WriteLine("Invalid number.");
        }
    }
    
    public static void PrintError(string message)
    {
        System.Console.WriteLine($"✗ {message}");
    }
    
    public static void Pause()
    {
        System.Console.WriteLine("\nPress Enter");
        System.Console.ReadLine();
    }
    
    public static void PrintTable(IEnumerable<string> headers, IEnumerable<IEnumerable<string>> rows, int colWidth = 20)
    {
        void PrintRow(IEnumerable<string> cells)
        {
            foreach (var cell in cells)
                System.Console.Write((cell ?? "").PadRight(colWidth));
            System.Console.WriteLine();
        }

        PrintRow(headers);
        System.Console.WriteLine(new string('-', colWidth * headers.Count()));
        
        foreach (var row in rows)
            PrintRow(row);
    }
}
