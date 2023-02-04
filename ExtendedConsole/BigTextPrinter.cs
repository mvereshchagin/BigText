namespace ExtendedConsole;

public class BigTextPrinter : IPrinter
{
    #region const
    // Height of a symbol
    private const int HEIGHT = 6;

    // Horizontal spaing between symbols
    private const int X_SPACING = 1;

    // File for mapping
    private const string MAPPER_FILE_PATH = "symbol_mapper.txt";
    #endregion

    #region private fields
    // mapped symbols
    private Dictionary<char, (int Left, int Top)[]> _mapperDict = new();
    #endregion

    #region properties
    public ConsoleColor? ForegroundColor { get; set; }

    public ConsoleColor? BackgroundColor { get; set; }

    /// <summary>
    /// A symbol to be used for printing
    /// </summary>
    public char Symbol { get; set; }
    #endregion

    #region ctor
    public BigTextPrinter(ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null, char symbol = '*')
    {
        ForegroundColor = foregroundColor;
        BackgroundColor = backgroundColor;
        Symbol = symbol;

        try
        {
            SetupPoints();
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
    #endregion

    #region IPrinter
    /// <summary>
    /// Print text to console
    /// </summary>
    /// <param name="text">Text to be printed</param>
    public void Write(string text)
    {
        var (offset, _) = Console.GetCursorPosition();
        foreach (var letter in text)
        {
            var points = GetPointsForLetter(letter);
            offset += PrintPoints(points, offset);
        }
    }

    /// <summary>
    /// Print text to console and sets cursor to a new line
    /// </summary>
    /// <param name="text">Text to be printed</param>
    public void WriteLine(string text)
    {
        Write(text);
        var (_, top) = Console.GetCursorPosition();
        Console.SetCursorPosition(left: 0, top: top + HEIGHT);
    }
    #endregion

    #region private methods
    //private (int Left, int Top)[] MapLetter(char letter) => letter switch
    //{
    //    'а' => new[] { (2, 0), (1, 1), (3, 1), (0, 2), (1, 2), (2, 2), (3, 2), (4, 2), (0, 3), (4, 3), (0, 4), (4, 4) },
    //    'б' => new[] { (0, 0), (1, 0), (2, 0), (3, 0), (0, 1), (0, 2), (1, 2), (2, 2), (0, 3), (3, 3), (0, 4), (1, 4), (2, 4)},
    //    'м' => new[] { (0, 0), (0, 1), (0, 2), (0, 3), (0, 4), (2, 1), (3, 2), (4, 1), (6, 0), (6, 1), (6, 2), (6, 3), (6, 4)},
    //    _ => new[] { (0, 0) },
    //};

    /// <summary>
    /// Returns points that represent a letter
    /// </summary>
    /// <param name="letter">F letter</param>
    /// <returns>Points array</returns>
    private (int Left, int Top)[] GetPointsForLetter(char letter)
    {
        if (!_mapperDict.ContainsKey(letter))
            return new (int Left, int Top)[] { };
        return _mapperDict[letter];
    }

    /// <summary>
    /// Prints points
    /// </summary>
    /// <param name="points">points to be print</param>
    /// <param name="curOffset">Current offset</param>
    /// <returns>Offset after printing</returns>
    private int PrintPoints((int Left, int Top)[] points, int curOffset)
    {
        var (_, yOffset) = Console.GetCursorPosition();
        int offset = 0;
        foreach (var point in points)
        {
            ConsoleUtils.Write(
                text: new string(Symbol, 1),
                left: curOffset + point.Left,
                top: yOffset + point.Top,
                foregroundColor: ForegroundColor,
                backgroundColor: BackgroundColor
                );
            if (point.Left > offset)
                offset = point.Left;
        }
        offset += 1 + X_SPACING;
        ConsoleUtils.ShiftCursorPosition(leftShirt: offset);
        return offset;
    }

    /// <summary>
    /// Read mapping for letters from file
    /// </summary>
    /// <exception cref="FormatException">Throws when file does not suit the expected format</exception>
    /// <example>
    /// 
    /// 1: Symbols_to_be_mapped
    /// 2:
    /// 3:    S
    /// 4:    Y
    /// 5:    M
    /// 6:    B
    /// 7:    O
    /// 8:    L1
    /// 9:
    /// 10:    S
    /// 11:    Y
    /// 12:    M
    /// 13:    B
    /// 14:    O
    /// 15:    L2
    /// </example>
    private void SetupPoints()
    {
        // Open text file for reading
        using var reader = File.OpenText(MAPPER_FILE_PATH);

        // Reads symbols that are supposed to be mapped
        var symbols = reader.ReadLine();

        // If first string is empty, then format is wrong
        if (symbols is null)
            throw new FormatException("Incorrect mapper file format");

        // skips one line
        reader.ReadLine();

        // loop over all symbols to be mapped
        foreach(var symbol in symbols)
        {
            // create list of points for a symbol
            List<(int Left, int Top)> points = new();
            // loop HEIGHT rows
            for(var j = 0; j < HEIGHT; j++)
            { 
                // read line
                var str = reader.ReadLine();
                // if line is empty, then skip
                if (String.IsNullOrEmpty(str))
                    continue;
                // loop over symbols in line
                for (var i = 0; i < str.Length; i++)
                {
                    // if symbol is not '&', then skip
                    if (str[i] != '*')
                        continue;
                    // if symbol is '&', then add its position to the list of points
                    points.Add((i, j));
                }
            }

            // add symbols with its mapped points to the dictionary
            _mapperDict.Add(symbol, points.ToArray());
        }
    }
        
    #endregion
}
