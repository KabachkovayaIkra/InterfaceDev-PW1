using Model;
using System.Collections.Generic;
using System;

public delegate bool TryParseDelegate(string cleanLine, out IShape shape, out string error);

public static class FigureRegistry
{
    private static readonly Dictionary<string, TryParseDelegate> _parsers =
        new Dictionary<string, TryParseDelegate>(StringComparer.OrdinalIgnoreCase);

    static FigureRegistry()
    {
        Register<Point>("Point");
        Register<Line>("Line");
        Register<Circle>("Circle");
    }

    private static void Register<T>(string typeName) where T : IShape
    {
        var method = typeof(T).GetMethod("TryParse", new[] { typeof(string), typeof(IShape).MakeByRefType(), typeof(string).MakeByRefType() });
        if (method != null && method.IsStatic && method.ReturnType == typeof(bool))
        {
            _parsers[typeName] = (TryParseDelegate)Delegate.CreateDelegate(typeof(TryParseDelegate), method);
        }
    }

    public static bool TryParse(string typeName, string cleanLine, out IShape shape, out string error)
    {
        shape = null;
        error = null;
        if (_parsers.TryGetValue(typeName, out var parser))
            return parser(cleanLine, out shape, out error);
        error = $"Неизвестный тип фигуры '{typeName}'";
        return false;
    }

    public static bool ContainsType(string typeName) => _parsers.ContainsKey(typeName);
}