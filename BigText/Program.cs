using ExtendedConsole;

//ConsoleUtils.Write(
//    text: "Привет, всем", 
//    left: 10, 
//    top: 7, 
//    foregroundColor: ConsoleColor.DarkBlue,
//    backgroundColor: ConsoleColor.Yellow);

//var printer = new SimplePrinter();
var printer = new BigTextPrinter(
    foregroundColor: ConsoleColor.DarkBlue,
    backgroundColor: ConsoleColor.Yellow,
    symbol: '&'
    );
//printer.Write("мабаб");
//printer.Write("аааббб");
//printer.WriteLine("ааа");
//printer.WriteLine("ббб");
//printer.WriteLine("ааа");
//printer.WriteLine("ббб");

printer.WriteLine("абвгде");
printer.WriteLine("едгвба");
printer.WriteLine("баба и деда");
printer.WriteLine("забава");

Console.ReadLine();