using CommandLine;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IfDevPW1
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Использование:\n  -f <файл> -o <print|count>\n  --help");
                return 1;
            }

            var result = Parser.Default.ParseArguments<Options>(args);
            int exitCode = 0;

            result.WithParsed<Options>(opts =>
            {
                var figures = LoadFigures(opts.FilePath);
                if (figures != null)
                    ExecuteOperation(figures, opts.Operation);
                else
                    exitCode = 1;
            })
            .WithNotParsed(errors =>
            {
                exitCode = 1;
            });

            return exitCode;
        }

        static List<IShape> LoadFigures(string filePath)
        {
            var figures = new List<IShape>();
            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    string line;
                    int lineNumber = 0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        lineNumber++;
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        string clean = string.Concat(line.Where(c => !char.IsWhiteSpace(c)));
                        int openParen = clean.IndexOf('(');
                        if (openParen <= 0)
                        {
                            Console.WriteLine($"Предупреждение: строка {lineNumber} не содержит имени типа: {line}");
                            continue;
                        }

                        string typeName = clean.Substring(0, openParen);
                        if (!FigureRegistry.ContainsType(typeName))
                        {
                            Console.WriteLine($"Предупреждение: строка {lineNumber} содержит неизвестный тип '{typeName}': {line}");
                            continue;
                        }

                        if (FigureRegistry.TryParse(typeName, clean, out IShape shape, out string error))
                        {
                            figures.Add(shape);
                        }
                        else
                        {
                            Console.WriteLine($"Предупреждение: строка {lineNumber} – {error}: {line}");
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Ошибка: файл '{filePath}' не найден.");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
                return null;
            }
            return figures;
        }

        static void ExecuteOperation(List<IShape> figures, string operation)
        {
            switch (operation?.ToLower())
            {
                case "print":
                    foreach (var fig in figures)
                        Console.WriteLine(fig);
                    break;
                case "count":
                    Console.WriteLine(figures.Count);
                    break;
                default:
                    Console.WriteLine($"Ошибка: неизвестная операция '{operation}'. Допустимо: print, count.");
                    break;
            }
        }
    }
}