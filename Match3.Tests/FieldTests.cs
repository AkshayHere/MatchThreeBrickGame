using Match3.Core.Game;
using Xunit;

public class FieldTests
{
    [Fact]
    public void Field_Initializes_Empty()
    {
        var field = new Field(5, 5);

        for (int y = 0; y < 5; y++)
            for (int x = 0; x < 5; x++)
                Assert.Null(field.Get(x, y));
    }

    [Fact]
    public void Set_And_Get_Cell()
    {
        var field = new Field(3, 3);
        field.Set(1, 1, 'A');

        Assert.Equal('A', field.Get(1, 1));
    }

    [Fact]
    public void Inside_Returns_False_When_Out_Of_Bounds()
    {
        var field = new Field(3, 3);

        Assert.False(field.Inside(-1, 0));
        Assert.False(field.Inside(3, 2));
        Assert.False(field.Inside(0, 3));
    }

    [Fact]
    public void Empty_Returns_True_When_Cell_Is_Null()
    {
        var field = new Field(2, 2);
        Assert.True(field.Empty(0, 0));
    }
}
