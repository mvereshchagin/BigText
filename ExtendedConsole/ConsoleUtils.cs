namespace ExtendedConsole;

public static class ConsoleUtils
{
    public static void Write(string text, 
        int? left = null, int? top = null, 
        ConsoleColor? foregroundColor = null, 
        ConsoleColor? backgroundColor = null)
    {
        // setup variables and save initial states
        var (curLeft, curTop) = Console.GetCursorPosition();
        left ??= curLeft;
        top ??= curTop;

        var curForegroundColor = Console.ForegroundColor;
        foregroundColor ??= curForegroundColor;

        var curBackgroundColor = Console.BackgroundColor;
        backgroundColor ??= curBackgroundColor;

        // setup color and position
        Console.SetCursorPosition(left.Value, top.Value);
        Console.BackgroundColor = backgroundColor.Value;
        Console.ForegroundColor = foregroundColor.Value;

        // print text
        Console.WriteLine(text);

        // restore initial states
        Console.SetCursorPosition(curLeft, curTop);
        Console.ForegroundColor = curForegroundColor;
        Console.BackgroundColor = curBackgroundColor;
    }

    public static void ShiftCursorPosition(int leftShirt = 0, int topShift = 0)
    {
        var (curLeft, curTop) = Console.GetCursorPosition();
        Console.SetCursorPosition(curLeft + leftShirt, curTop + topShift);
    }
}
