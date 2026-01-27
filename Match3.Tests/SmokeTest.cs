using Match3.Core;
using Match3.Core.Game;
using Xunit;

public class SmokeTest
{
    [Fact]
    public void Core_Project_Is_Usable()
    {
        var field = new Field(5, 5);
        Assert.NotNull(field);
    }
}
