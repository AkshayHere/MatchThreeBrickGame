using Match3.Core.Game;
using Match3.Core.Services;
using Xunit;

public class MatchDetectionServiceTests
{
    [Fact]
    public void Detects_Horizontal_Match_Of_Three()
    {
        var field = new Field(5, 1);
        field.Set(1, 0, 'A');
        field.Set(2, 0, 'A');
        field.Set(3, 0, 'A');

        var service = new MatchDetectionService();
        var matches = service.FindMatches(field);

        Assert.Equal(3, matches.Count());
    }

    [Fact]
    public void Detects_Vertical_Match_Of_Three()
    {
        var field = new Field(1, 5);
        field.Set(0, 1, 'B');
        field.Set(0, 2, 'B');
        field.Set(0, 3, 'B');

        var service = new MatchDetectionService();
        var matches = service.FindMatches(field);

        Assert.Equal(3, matches.Count());
    }

    [Fact]
    public void Detects_Match_Of_Four()
    {
        var field = new Field(5, 1);
        field.Set(0, 0, 'C');
        field.Set(1, 0, 'C');
        field.Set(2, 0, 'C');
        field.Set(3, 0, 'C');

        var service = new MatchDetectionService();
        var matches = service.FindMatches(field);

        Assert.Equal(4, matches.Count());
    }

    [Fact]
    public void Detects_Overlapping_Matches()
    {
        var field = new Field(3, 3);
        field.Set(1, 0, 'X');
        field.Set(1, 1, 'X');
        field.Set(1, 2, 'X');
        field.Set(0, 1, 'X');
        field.Set(2, 1, 'X');

        var service = new MatchDetectionService();
        var matches = service.FindMatches(field);

        Assert.Equal(5, matches.Count());
    }

    [Fact]
    public void No_Match_When_Less_Than_Three()
    {
        var field = new Field(3, 1);
        field.Set(0, 0, 'A');
        field.Set(1, 0, 'A');

        var service = new MatchDetectionService();
        var matches = service.FindMatches(field);

        Assert.Empty(matches);
    }
}
