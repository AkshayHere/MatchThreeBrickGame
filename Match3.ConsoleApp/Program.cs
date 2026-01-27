using Match3.Core.Enums;
using Match3.Core.Game;
using Match3.Core.Models;
using Match3.Core.Services;

class Program
{
    static void Main()
    {
        RunGame();
    }
    private static void RunGame()
    {
        Console.WriteLine("Welcome to Match-3 game!");
        Console.Write("Enter field size and bricks: ");

        var input = Console.ReadLine()!.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        int width = int.Parse(input[0]);
        int height = int.Parse(input[1]);

        var bricks = new List<Brick>();
        for (int i = 2; i < input.Length; i++)
        {
            var token = input[i];
            var orientation = token[0] == 'H'
                ? Orientation.Horizontal
                : Orientation.Vertical;
            bricks.Add(new Brick(orientation, token.Substring(1).ToCharArray()));
        }

        MatchDetectionService matchService = new MatchDetectionService();
        var engine = new GameEngine(new Field(width, height), bricks, matchService);

        while (!engine.GameOver)
        {
            Draw(engine);
            Console.Write("Commands (L R D): ");
            var cmds = Console.ReadLine()!.ToUpper();

            foreach (var cmd in cmds.Take(2))
            {
                switch (cmd)
                {
                    case 'L':
                        engine.Move(-1);
                        break;
                    case 'R':
                        engine.Move(1);
                        break;
                    case 'D':
                        engine.Drop();
                        break;
                }
            }

            engine.Tick();
        }

        Draw(engine);
        Console.WriteLine("Game Over!");
        Console.Write("Enter S to restart or Q to quit: ");
        if (Console.ReadLine()!.ToUpper() == "S")
            Main();
        else
            Console.WriteLine("Thank you for playing Match-3!");
    }

    static void Draw(GameEngine engine)
    {
        Console.Clear();
        for (int y = 0; y < engine.Field.Height; y++)
        {
            for (int x = 0; x < engine.Field.Width; x++)
            {
                char ch = engine.Field.Get(x, y) ?? '.';

                //if (engine.ActiveBrick != null &&
                //    engine.ActiveBrick.Cells().Any(c => c.x == x && c.y == y))
                //    ch = engine.ActiveBrick.Cells().First(c => c.x == x && c.y == y).symbol;
                if (!engine.GameOver && engine.ActiveBrick != null)
                {
                    var cell = engine.ActiveBrick.Cells()
                        .FirstOrDefault(c => c.x == x && c.y == y);

                    if (cell.symbol != default)
                        ch = cell.symbol;
                }


                Console.Write(ch);
            }
            Console.WriteLine();
        }
    }
}

