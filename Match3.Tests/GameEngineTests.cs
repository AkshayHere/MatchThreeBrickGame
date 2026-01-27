using Match3.Core.Enums;
using Match3.Core.Game;
using Match3.Core.Models;
using Match3.Core.Services;
using Xunit;

public class GameEngineTests
{
    [Fact]
    public void Game_Ends_When_Brick_Cannot_Be_Placed()
    {
        var field = new Field(3, 3);
        field.Set(1, 0, 'X');

        var bricks = new[]
        {
            new Brick(Orientation.Vertical, new[] { 'A','A','A' })
        };

        var engine = new GameEngine(field, bricks, new MatchDetectionService());

        Assert.True(engine.GameOver);
    }

    [Fact]
    public void Brick_Locks_When_Cannot_Move_Down()
    {
        var field = new Field(3, 3);

        var bricks = new[]
        {
            new Brick(Orientation.Horizontal, new[] { 'A','A','A' })
        };

        var engine = new GameEngine(field, bricks, new MatchDetectionService());

        engine.Drop();

        Assert.Null(engine.ActiveBrick);
        Assert.Null(field.Get(0, 2));
    }

    [Fact]
    public void Matches_Are_Removed_When_Brick_Locks()
    {
        var field = new Field(3, 4);

        var bricks = new[]
        {
            new Brick(Orientation.Vertical, new[] { 'A','A','A' })
        };

        var engine = new GameEngine(field, bricks, new MatchDetectionService());

        engine.Drop();

        Assert.True(field.Empty(1, 1));
        Assert.True(field.Empty(1, 2));
        Assert.True(field.Empty(1, 3));
    }

    [Fact]
    public void Game_Ends_When_All_Bricks_Used()
    {
        var field = new Field(3, 3);
        var bricks = new[]
        {
            new Brick(Orientation.Horizontal, new[] { 'A','B','C' })
        };

        var engine = new GameEngine(field, bricks, new MatchDetectionService());

        engine.Drop();

        Assert.True(engine.GameOver);
    }

    [Fact]
    public void Move_Left_Shifts_Brick_Left_By_One()
    {
        var field = new Field(7, 5);
        var bricks = new[]
        {
            new Brick(Orientation.Horizontal, new[] { 'A','B','C' })
        };

        var engine = new GameEngine(field, bricks, new MatchDetectionService());
        int startX = engine.ActiveBrick!.X;

        engine.Move(-1);

        Assert.Equal(startX - 1, engine.ActiveBrick!.X);
    }

    [Fact]
    public void Move_Right_Shifts_Brick_Right_By_One()
    {
        var field = new Field(7, 5);
        var bricks = new[]
        {
            new Brick(Orientation.Horizontal, new[] { 'A','B','C' })
        };

        var engine = new GameEngine(field, bricks, new MatchDetectionService());
        int startX = engine.ActiveBrick!.X;

        engine.Move(1);

        Assert.Equal(startX + 1, engine.ActiveBrick!.X);
    }

    [Fact]
    public void Move_Left_Is_Ignored_When_Out_Of_Bounds()
    {
        var field = new Field(3, 5);
        var bricks = new[]
        {
            new Brick(Orientation.Horizontal, new[] { 'A','B','C' })
        };

        var engine = new GameEngine(field, bricks, new MatchDetectionService());
        int startX = engine.ActiveBrick!.X;

        engine.Move(-1);

        Assert.Equal(startX, engine.ActiveBrick!.X);
    }

    [Fact]
    public void Move_Is_Ignored_When_Colliding_With_Field()
    {
        var field = new Field(7, 5);
        field.Set(2, 0, 'X');

        var bricks = new[]
        {
            new Brick(Orientation.Horizontal, new[] { 'A','B','C' })
        };

        var engine = new GameEngine(field, bricks, new MatchDetectionService());
        int startX = engine.ActiveBrick!.X;

        engine.Move(-1);

        Assert.Equal(startX, engine.ActiveBrick!.X);
    }

    [Fact]
    public void Tick_Moves_Brick_Down_By_One_Row()
    {
        var field = new Field(5, 6);
        var bricks = new[]
        {
            new Brick(Orientation.Horizontal, new[] { 'A','B','C' })
        };

        var engine = new GameEngine(field, bricks, new MatchDetectionService());
        int startY = engine.ActiveBrick!.Y;

        engine.Tick();

        Assert.Equal(startY + 1, engine.ActiveBrick!.Y);
    }

    [Fact]
    public void Tick_Does_Not_Move_Brick_When_GameOver()
    {
        var field = new Field(3, 3);
        field.Set(1, 0, 'X');

        var bricks = new[]
        {
            new Brick(Orientation.Vertical, new[] { 'A','B','C' })
        };

        var engine = new GameEngine(field, bricks, new MatchDetectionService());

        Assert.True(engine.GameOver);
        engine.Tick();
        Assert.True(engine.GameOver);
    }

    [Fact]
    public void Tick_Locks_Brick_When_Hitting_Bottom()
    {
        var field = new Field(5, 3);
        var bricks = new[]
        {
            new Brick(Orientation.Vertical, new[] { 'A','B','C' })
        };

        var engine = new GameEngine(field, bricks, new MatchDetectionService());

        engine.Tick();
        engine.Tick();

        Assert.Null(engine.ActiveBrick);
        Assert.Equal('A', field.Get(2, 0));
        Assert.Equal('B', field.Get(2, 1));
        Assert.Equal('C', field.Get(2, 2));
    }

    [Fact]
    public void Tick_Locks_Brick_When_Colliding_With_Another_Brick()
    {
        var field = new Field(5, 5);
        field.Set(2, 3, 'X');

        var bricks = new[]
        {
            new Brick(Orientation.Vertical, new[] { 'A','B','C' })
        };

        var engine = new GameEngine(field, bricks, new MatchDetectionService());

        engine.Tick();
        engine.Tick();
        engine.Tick();

        Assert.Null(engine.ActiveBrick);
        Assert.Equal('C', field.Get(2, 2));
    }

    [Fact]
    public void Tick_Removes_Matches_When_Brick_Locks()
    {
        var field = new Field(3, 4);

        var bricks = new[]
        {
            new Brick(Orientation.Vertical, new[] { 'A','A','A' })
        };

        var engine = new GameEngine(field, bricks, new MatchDetectionService());

        engine.Tick();
        engine.Tick();
        Assert.True(field.Empty(1, 1));
        Assert.True(field.Empty(1, 2));
        Assert.True(field.Empty(1, 3));
    }

    [Fact]
    public void Tick_Spawns_Next_Brick_After_Lock()
    {
        var field = new Field(5, 5);

        var bricks = new[]
        {
            new Brick(Orientation.Horizontal, new[] { 'A','B','C' }),
            new Brick(Orientation.Horizontal, new[] { 'D','E','F' })
        };

        var engine = new GameEngine(field, bricks, new MatchDetectionService());

        engine.Tick();
        engine.Tick();

        Assert.NotNull(engine.ActiveBrick);
        Assert.Equal('A', engine.ActiveBrick!.Cells().First().symbol);
    }

    [Fact]
    public void Tick_Does_Not_Double_Lock_After_Drop()
    {
        var field = new Field(5, 5);

        var bricks = new[]
        {
            new Brick(Orientation.Vertical, new[] { 'A','B','C' })
        };

        var engine = new GameEngine(field, bricks, new MatchDetectionService());

        engine.Drop();
        engine.Tick();
        Assert.True(engine.GameOver);
    }
}
