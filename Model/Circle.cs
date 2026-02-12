using Model;
using System.Globalization;
using System.Text.RegularExpressions;

public class Circle : IShape
{
    public Point Center { get; }
    public double Radius { get; }

    private Circle(Point center, double radius) { Center = center; Radius = radius; }

    public override string ToString() => $"Circle({Center}, {Radius})";

    public static bool TryParse(string cleanLine, out IShape shape, out string error)
    {
        shape = null;
        error = null;

        var regex = new Regex(@"^Circle\(Point\(([^,]+),([^)]+)\),([^)]+)\)$", RegexOptions.IgnoreCase);
        var match = regex.Match(cleanLine);
        if (!match.Success)
        {
            error = "Неверный формат окружности";
            return false;
        }

        var invariant = CultureInfo.InvariantCulture;
        if (!double.TryParse(match.Groups[1].Value, NumberStyles.Float, invariant, out double cx) ||
            !double.TryParse(match.Groups[2].Value, NumberStyles.Float, invariant, out double cy) ||
            !double.TryParse(match.Groups[3].Value, NumberStyles.Float, invariant, out double r))
        {
            error = "Координаты и радиус должны быть числами";
            return false;
        }

        if (r <= 0)
        {
            error = "Радиус должен быть положительным, иначе это точка";
            return false;
        }

        shape = new Circle(new Point(cx, cy), r);
        return true;
    }
}