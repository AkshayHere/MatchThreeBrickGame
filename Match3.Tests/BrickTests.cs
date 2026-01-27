using Match3.Core.Enums;
using Match3.Core.Models;
using Xunit;

public class BrickTests
{
    [Fact]
    public void Horizontal_Brick_Has_Three_Cells_In_Row()
    {
        var brick = new Brick(Orientation.Horizontal, new[] { 'A', 'B', 'C' });
        brick.SetPosition(1, 2);

        var cells = brick.Cells().ToList();

        Assert.Contains((1, 2, 'A'), cells);
        Assert.Contains((2, 2, 'B'), cells);
        Assert.Contains((3, 2, 'C'), cells);
    }

    [Fact]
    public void Vertical_Brick_Has_Three_Cells_In_Column()
    {
        var brick = new Brick(Orientation.Vertical, new[] { 'X', 'Y', 'Z' });
        brick.SetPosition(2, 1);

        var cells = brick.Cells().ToList();

        Assert.Contains((2, 1, 'X'), cells);
        Assert.Contains((2, 2, 'Y'), cells);
        Assert.Contains((2, 3, 'Z'), cells);
    }
}
