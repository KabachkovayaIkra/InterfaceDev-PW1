using Model;
using System.Globalization;
using System.Text.RegularExpressions;

public class Line : IShape
{
    public Point Start { get; }
    public Point End { get; }

    private Line(Point start, Point end) { Start = start; End = end; }

    public override string ToString() => $"Line({Start}, {End})";

    public static bool TryParse(string cleanLine, out IShape shape, out string error)
    {
        shape = null;
        error = null;

        var regex = new Regex(@"^Line\(Point\(([^,]+),([^)]+)\),Point\(([^,]+),([^)]+)\)\)$", RegexOptions.IgnoreCase);
        var match = regex.Match(cleanLine);
        if (!match.Success)
        {
            error = "Неверный формат линии";
            return false;
        }

        var invariant = CultureInfo.InvariantCulture;
        if (!double.TryParse(match.Groups[1].Value, NumberStyles.Float, invariant, out double x1) ||
            !double.TryParse(match.Groups[2].Value, NumberStyles.Float, invariant, out double y1) ||
            !double.TryParse(match.Groups[3].Value, NumberStyles.Float, invariant, out double x2) ||
            !double.TryParse(match.Groups[4].Value, NumberStyles.Float, invariant, out double y2))
        {
            error = "Координаты должны быть числами";
            return false;
        }

        if (x1 == x2 && y1 == y2)
        {
            error = "Точки совпадают, линия вырождена в точку";
            return false;
        }

        shape = new Line(new Point(x1, y1), new Point(x2, y2));
        return true;
    }
}