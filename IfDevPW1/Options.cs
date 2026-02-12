using CommandLine;

public class Options
{
    [Option('f', "file", Required = true, HelpText = "Путь к файлу с описанием фигур.")]
    public string FilePath { get; set; }

    [Option('o', "oper", Required = true, HelpText = "Операция: print (вывести список) или count (количество).")]
    public string Operation { get; set; }
}