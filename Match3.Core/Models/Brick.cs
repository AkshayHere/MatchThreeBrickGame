using Match3.Core.Enums;

namespace Match3.Core.Models;

public class Brick
{
    public Orientation Orientation { get; }
    public char[] Symbols { get; }
    public int X { get; private set; }
    public int Y { get; private set; }

    public Brick(Orientation orientation, char[] symbols)
    {
        Orientation = orientation;
        Symbols = symbols;
    }

    public void SetPosition(int x, int y)
    {
        X = x;
        Y = y;
    }

    public IEnumerable<(int x, int y, char symbol)> Cells()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return Orientation == Orientation.Horizontal
                ? (X + i, Y, Symbols[i])
                : (X, Y + i, Symbols[i]);
        }
    }
}
