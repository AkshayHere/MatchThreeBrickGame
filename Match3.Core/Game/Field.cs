using Match3.Core.Models;

namespace Match3.Core.Game;

public class Field
{
    private readonly char?[,] _grid;

    public int Width { get; }
    public int Height { get; }

    public Field(int width, int height)
    {
        Width = width;
        Height = height;
        _grid = new char?[height, width];
    }

    public bool Inside(int x, int y) =>
        x >= 0 && x < Width && y >= 0 && y < Height;

    public bool Empty(int x, int y) =>
        Inside(x, y) && _grid[y, x] == null;

    public char? Get(int x, int y) => _grid[y, x];

    public void Set(int x, int y, char? value) =>
        _grid[y, x] = value;

    public void PlaceBrick(Brick brick)
    {
        foreach (var (x, y, s) in brick.Cells())
            _grid[y, x] = s;
    }
}
