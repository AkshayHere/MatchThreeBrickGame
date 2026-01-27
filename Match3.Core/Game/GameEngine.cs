using Match3.Core.Models;
using Match3.Core.Enums;
using Match3.Core.Services;

namespace Match3.Core.Game;

public class GameEngine
{
    public Field Field { get; }
    public Brick? ActiveBrick { get; private set; }
    public bool GameOver { get; private set; }

    private readonly Queue<Brick> _bricks;
    private readonly MatchDetectionService _matchService;
    private bool _brickLockedThisFrame;


    public GameEngine(
        Field field,
        IEnumerable<Brick> bricks,
        MatchDetectionService matchService)
    {
        Field = field;
        _bricks = new Queue<Brick>(bricks);
        _matchService = matchService;

        SpawnBrick();
    }

    public void SpawnBrick()
    {
        if (_bricks.Count == 0)
        {
            GameOver = true;
            return;
        }

        ActiveBrick = _bricks.Dequeue();
        int startX = ActiveBrick.Orientation == Orientation.Horizontal
            ? (Field.Width - 3) / 2
            : Field.Width / 2;

        int startY = 0;

        ActiveBrick.SetPosition(startX, startY);

        if (!CanPlace(ActiveBrick))
            GameOver = true;
    }

    public bool CanPlace(Brick brick) =>
        brick.Cells().All(c => Field.Inside(c.x, c.y) && Field.Empty(c.x, c.y));

    public void Move(int dx)
    {
        if (ActiveBrick == null) return;

        ActiveBrick.SetPosition(ActiveBrick.X + dx, ActiveBrick.Y);
        if (!CanPlace(ActiveBrick))
            ActiveBrick.SetPosition(ActiveBrick.X - dx, ActiveBrick.Y);
    }

    public void Drop()
    {
        if (GameOver || ActiveBrick == null) return;

        while (true)
        {
            ActiveBrick.SetPosition(ActiveBrick.X, ActiveBrick.Y + 1);
            if (!CanPlace(ActiveBrick))
            {
                ActiveBrick.SetPosition(ActiveBrick.X, ActiveBrick.Y - 1);
                LockBrick();
                _brickLockedThisFrame = true;
                return;
            }
        }
    }

    public void Tick()
    {
        if (GameOver || ActiveBrick == null || _brickLockedThisFrame)
        {
            _brickLockedThisFrame = false;
            return;
        }

        ActiveBrick.SetPosition(ActiveBrick.X, ActiveBrick.Y + 1);
        if (!CanPlace(ActiveBrick))
        {
            ActiveBrick.SetPosition(ActiveBrick.X, ActiveBrick.Y - 1);
            LockBrick();
        }
    }

    private void LockBrick()
    {
        Field.PlaceBrick(ActiveBrick!);
        ActiveBrick = null;
        RemoveMatches();
        SpawnBrick();
    }

    private void RemoveMatches()
    {
        var matches = _matchService.FindMatches(Field);
        foreach (var (x, y) in matches)
            Field.Set(x, y, null);
    }
}
