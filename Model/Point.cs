using Model;
using System.Globalization;
using System.Text.RegularExpressions;

public class Point : IShape
{
    public double X { get; }
    public double Y { get; }

    public Point(double x, double y) { X = x; Y = y; }

    public override string ToString() => $"Point({X}, {Y})";

    public static bool TryParse(string cleanLine, out IShape shape, out string error)
    {
        shape = null;
        error = null;

        var regex = new Regex(@"^Point\(([^,]+),([^)]+)\)$", RegexOptions.IgnoreCase);
        var match = regex.Match(cleanLine);
        if (!match.Success)
        {
            error = "Неверный формат точки";
            return false;
        }

        var invariant = CultureInfo.InvariantCulture;
        if (!double.TryParse(match.Groups[1].Value, NumberStyles.Float, invariant, out double x) ||
            !double.TryParse(match.Groups[2].Value, NumberStyles.Float, invariant, out double y))
        {
            error = "Координаты должны быть числами";
            return false;
        }

        shape = new Point(x, y);
        return true;
    }
}