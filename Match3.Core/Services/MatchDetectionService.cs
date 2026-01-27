using Match3.Core.Game;

namespace Match3.Core.Services;

public class MatchDetectionService
{
    public IEnumerable<(int x, int y)> FindMatches(Field field)
    {
        var matches = new HashSet<(int, int)>();

        // Horizontal
        for (int y = 0; y < field.Height; y++)
        {
            int x = 0;
            while (x < field.Width)
            {
                var c = field.Get(x, y);
                if (c == null)
                {
                    x++;
                    continue;
                }

                int start = x;
                while (x < field.Width && field.Get(x, y) == c)
                    x++;

                int length = x - start;
                if (length >= 3)
                {
                    for (int i = start; i < x; i++)
                        matches.Add((i, y));
                }
            }
        }

        // Vertical
        for (int x = 0; x < field.Width; x++)
        {
            int y = 0;
            while (y < field.Height)
            {
                var c = field.Get(x, y);
                if (c == null)
                {
                    y++;
                    continue;
                }

                int start = y;
                while (y < field.Height && field.Get(x, y) == c)
                    y++;

                int length = y - start;
                if (length >= 3)
                {
                    for (int i = start; i < y; i++)
                        matches.Add((x, i));
                }
            }
        }

        return matches;
    }
}
