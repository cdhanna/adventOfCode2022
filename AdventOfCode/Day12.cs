namespace AdventOfCode;

public class Day12
{
    [TestCase('b', 'a')]
    [TestCase('c', 'b')]
    [TestCase('z', 'a')]
    public void CharTesting(char a, char b)
    {
        Assert.IsTrue(a > b);
    }
    
    [TestCase('b', 'a')]
    // [TestCase('c', 'a')]
    public void CharTestingHigher(char a, char b)
    {
        Assert.IsTrue(a - b <= 1);
    }
    
    
    [TestCase(sample, 31)]
    [TestCase(input, 520)] // 34 is not correct
    public void Part1(string input, int expected)
    {
        var rows = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);

        var heights = new int[rows.Length,rows[0].Length];
        // find start, find end
        Vec start=default, end=default;
        for (var y = 0; y < rows.Length; y++)
        {
            for (var x = 0; x < rows[y].Length; x++)
            {
                var c = rows[y][x];
                var pos = new Vec { x = x, y = y };
                heights[y, x] = (int)c;
                switch (c)
                {
                    case 'S':
                        start = pos;
                        heights[y, x] = (int)'a';
                        break;
                    case 'E':
                        end = pos;
                        heights[y, x] = (int)'z';
                        break;
                }
            }
        }

        // do a BFS 
        var toExplore = new Queue<Vec>();
        var seen = new HashSet<string>();
        toExplore.Enqueue(start);
        var steps = 0;
        var path = new Dictionary<Vec, Vec>();

        while (toExplore.Count > 0)
        {
            var curr = toExplore.Dequeue();
            if (seen.Contains($"{curr.x},{curr.y}")) continue;
            seen.Add($"{curr.x},{curr.y}");
            var c = heights[curr.y,curr.x];

            if (curr == end)
            {
                break;
            }

            
            // expand neighbors...
            
            // left
            if (curr.x > 0 && (heights[curr.y,curr.x - 1] - c) <= 1)
            {
                var key = curr + Vec.Left;
                if (!path.ContainsKey(key)) path[key] = curr;
                toExplore.Enqueue(curr + Vec.Left);
            }
            // right
            if (curr.x < rows[0].Length-1 && (heights[curr.y,curr.x + 1] - c) <= 1)
            {
                var key = curr + Vec.Right;
                if (!path.ContainsKey(key)) path[key] = curr;
                toExplore.Enqueue(curr + Vec.Right);
            }
            // down
            if (curr.y < rows.Length -1 && (heights[curr.y+1,curr.x] - c) <= 1)
            {
                var key = curr + Vec.Down;
                if (!path.ContainsKey(key)) path[key] = curr;
                toExplore.Enqueue(curr + Vec.Down);
            }
            // top
            if (curr.y > 0 && (heights[curr.y-1,curr.x] - c) <= 1)
            {
                var key = curr + Vec.Up;
                if (!path.ContainsKey(key)) path[key] = curr;
                toExplore.Enqueue(curr + Vec.Up);
            }
        }

        var tip = path[end];
        steps = 1;
        while (tip != start && steps <= 99999999)
        {
            tip = path[tip];
            steps++;
        }
        
        Assert.AreEqual(expected, steps);
    }
    
    
    
    [TestCase(sample, 29)]
    [TestCase(input, 508)] 
    public void Part2(string input, int expected)
    {
        var rows = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);

        var heights = new int[rows.Length,rows[0].Length];
        // find start, find end
        Vec end=default;
        List<Vec> starts = new List<Vec>();
        for (var y = 0; y < rows.Length; y++)
        {
            for (var x = 0; x < rows[y].Length; x++)
            {
                var c = rows[y][x];
                var pos = new Vec { x = x, y = y };
                heights[y, x] = (int)c;
                switch (c)
                {
                    case 'a':
                        starts.Add(pos);
                        break;
                    case 'S':
                        heights[y, x] = (int)'a';
                        starts.Add(pos);
                        break;
                    case 'E':
                        end = pos;
                        heights[y, x] = (int)'z';
                        break;
                }
            }
        }

        // do a BFS 
        var bestMap = new Dictionary<Vec, int>();
        var lowestSteps = int.MaxValue;
        foreach (var start in starts)
        {
            var toExplore = new Queue<Vec>();
            var seen = new HashSet<string>();
            toExplore.Enqueue(start);
            var steps = 0;
            var path = new Dictionary<Vec, Vec>();
            var found = false;
            while (toExplore.Count > 0)
            {
                var curr = toExplore.Dequeue();
                if (seen.Contains($"{curr.x},{curr.y}")) continue;
                seen.Add($"{curr.x},{curr.y}");
                var c = heights[curr.y, curr.x];

                if (curr == end)
                {
                    found = true;
                    break;
                }


                // expand neighbors...

                // left
                if (curr.x > 0 && (heights[curr.y, curr.x - 1] - c) <= 1)
                {
                    var key = curr + Vec.Left;
                    if (!path.ContainsKey(key)) path[key] = curr;
                    toExplore.Enqueue(curr + Vec.Left);
                }

                // right
                if (curr.x < rows[0].Length - 1 && (heights[curr.y, curr.x + 1] - c) <= 1)
                {
                    var key = curr + Vec.Right;
                    if (!path.ContainsKey(key)) path[key] = curr;
                    toExplore.Enqueue(curr + Vec.Right);
                }

                // down
                if (curr.y < rows.Length - 1 && (heights[curr.y + 1, curr.x] - c) <= 1)
                {
                    var key = curr + Vec.Down;
                    if (!path.ContainsKey(key)) path[key] = curr;
                    toExplore.Enqueue(curr + Vec.Down);
                }

                // top
                if (curr.y > 0 && (heights[curr.y - 1, curr.x] - c) <= 1)
                {
                    var key = curr + Vec.Up;
                    if (!path.ContainsKey(key)) path[key] = curr;
                    toExplore.Enqueue(curr + Vec.Up);
                }
            }

            if (found)
            {
                var tip = path[end];
                steps = 1;
                while (tip != start && steps <= 99999999)
                {
                    tip = path[tip];
                    steps++;
                    if (steps > lowestSteps) break;
                }

                if (steps < lowestSteps)
                {
                    lowestSteps = steps;
                }
            }
        }

        Assert.AreEqual(expected, lowestSteps);
    }
    
    #region sample
    public const string sample = @"Sabqponm
abcryxxl
accszExk
acctuvwj
abdefghi";
    #endregion
    
    
    #region input
    public const string input = @"abcccccaaaaaacccaaaccaaaaaaaacccaaaaaaccccccccccccccccccccccccccccaaaaaaaaaaaaaacacccccccccccccccccccccccccccccccaaaaaaaacccccccccccccccccccccccccccccccccccccccccccccaaaaa
abcccccaaaaaaaacaaaaccaaaaaaccccaaaaaaccccccccccaaacccccccccccccccaaaaaaaaaaaaaaaacccccccccccccccccccccccccccccccaaaaaaaaaccccccaaaccccccccccccccccccccccccccccccccccaaaaaa
abccccaaaaaaaaacaaaaccaaaaaaccccaaaaaaaaccccccccaaaccccccccccccccccaaaaaaaaaaaaaaccccaaaccccccccccccccccccccccccccaaaaaaaaccccacaaaccccccccccccccccaaccccccccccccccccaaaaaa
abcccaaaaaaaaaacaaaccaaaaaaaacccccaaccaaacaaccccaaaaaaaccccccccccccaacaaaaaaaaaccccccaaaaccccccccccccccccccccaaacaaaaaaccccccaaaaaaaacccccccccccccaaaacccccccccccccccaaacaa
abcccaaacaaaccccccccaaaaaaaaaaccccccccaaaaaacaaaaaaaaaaccccccccccccccaaaaaaaaaaccccccaaaaccccccccccccccccaaccaaacaaaaaaacccccaaaaaaaacccccccccccccaaaaccaaaccccccccccccccaa
abcccccccaaaccccccccaaaaaaaaaaccccccaaaaaaaccaaaaaaaaacccccccccccccccaaaacaaaaaaaacccaaaaccccccccccccccccaaaaaaacaaccaaacccccccaaaaacccccccccccccccaaaaaaaaacccccccccccccaa
abcccccccaacccccccccacaaaaaccaccccccaaaaaaacccaaaaaaaccccccaaacccccccaacacaaaaaaaacccccccccccccccccccccccaaaaaaccccccaaaccccccaaaaaccccccccccccjjkkaaaaaaaacccccccccccccccc
abaccccccccccccccccccccaaaacccccccccccaaaaaaccccaaaaaaccccaaaacccccccccccccaaaaaaccccccccccaccaaccccccccccaaaaaaaaccccccccccccaaaaaaccccccccccjjjkkkkkaaaaccccccccaaccccccc
abaccccccccaaaccccccccccaaccccccccccccaacaaacccaaaaaaaccccaaaacccccccccccccaaaaaaccccccccccaaaaacccccccccaaaaaaaaaccccccccccccaccaaacccccccccjjjjkkkkkkkaacccccccccaccccccc
abaacccccccaaaaccccccccccaaaccccccccccaacccccccaaaacaaccccaaaacccccccccccccaaaaaacccccccccccaaaaaccccccccaaaaaaaacccccccaaccccccccccccccccccjjjjoooookkkkkllllllccccaaacccc
abaacccccccaaaaccccccccccaaaaccccccccccccccccccaacaaacaaaccccccccccaaccccccaaacaaccccccccccaaaaaacccccccaaaaaaaccccccccaaaacccccccccccccccccjjjoooooopkkkklllllllccccaacccc
abaccccccccaaacccccccccccaaaacccccccccccccccccccccaaaaaaacccccccccaaaccccccccccccccccccccccaaaaccccccaaaccccaaaccccccccaaaacccccccccccccccccjjooooooopppklppplllllcccaccccc
abacccaaaaaccccccccccccccaaaccccccccccccccccccccccaaaaaaccccccaaaaaaaccccccccccccccccccccccccaaacccccaaacacccaaccccccccaaaaccccccccccccccccjjjooouuuupppppppppplllcccaccccc
abccccaaaaacccccccccccccccccccccccccccccccccccccaaaaaaaaccccccaaaaaaaaaaccaacccccccccccccccccccccccaacaaaaaccccccccccccccccaaacccccaccaccccjjjoouuuuuupppppppppllllccaccccc
abcccaaaaaacccccccccccccaaccccccaaacccccccccccccaaaaaaaaaccccccaaaaaaaaacaaaaccccccccccccccccccccccaaaaaaaaccccccccccccacccaaccccccaaaacccjjjjoouuuuuuupuuuvvpqqlllccaccccc
abcccaaaaaaccccccccccccaaacaacccaaaaacccccccccccaaaaaaaaaaccccccaaaaaaaccaaaacccccccccccccccccccccccaaaaaccccccccccccccaaaaaaacccccaaaaaccijjooouuuxxxuuuuvvvqqqlmccccccccc
abcccaaaaaaccccaacccccccaaaaaccaaaaaccccccccccccaaaaaacaaacccccaaaaaaccccaaaacccccccccccccccccccaaaccaaaaaccccaaaccccccaaaaaaaacccaaaaaaciiiinootuxxxxuuyyyvvqqqmmccccccccc
abcccacaacccccaaacaaccaaaaaacccaaaaaccccccccccccaaaaaacccccccccaaaaaaaccccccccccccaaccacccccccccaaacaaacaaccaaaaaccccccaaaaaaaaaccaaaaaaiiinnnnnttxxxxxyyyyvvqqqmmdddcccccc
abcccaaacaaacccaaaaaccaaaaaaaaccaaaaacccccccccccaaacaaaccccccccaaccaaaccccccccccccaaaaaccccccaaaaaaaaaacccccaaaaaacccccaaaaaaaaaccccaaciiinnnnttttxxxxxyyyyvvqqmmmdddcccccc
abcccaaaaaaacaaaaaacccaacaaaaaccaacccccccccccaaaaaaaaaaaccccccccccccaacccccccccccaaaaacccccccaaaaaaaacccccccaaaaaacccccaaaaaaaaccccccciiinnnnttttxxxxxyyyyvvqqqmmmdddcccccc
SbcccaaaaaaccaaaaaaaaccccaacccccccccccaacccccaaaaaaaaaaccccccccccccccccccccccccccaaaaaaccccccccaaaaaccccccccaaaaacccccaaaaaaaccccccccciiinnntttxxxEzzzzyyvvvqqqmmmdddcccccc
abccaaaaaaaacaacaaaaacccaaccccccccccccaaacccccaaaaaaaaaaaccccccccccccccccccccccccccaaaacccccccaaaaaaccccccccaaaaaccccccacaaaaccccccccciiinnntttxxxxxyyyyyyvvvqqmmmdddcccccc
abcaaaaaaaaaacccaacccccaacccccccccacacaaaccccccaaaaaaaaaacccccccccccccccccccccacccaaccccccccccaaaaaaccccccccccccccccccccaaaaaccccccccciiinnntttxxxxyyyyyyyyvvvqqmmmdddccccc
abcaaaaaaaaaaccaaccaaaaaacccccccccaaaaaaaaaacccaaaaaaaaaacaaccccccccccccaaaaaaaaccccccccccccccaccaaaccccccccccccccccaaacaaacccccccccaaiiinnnttttxxwwyyyyyyyvvvqqmmmdddccccc
abaaccaaacaaaccccccaaaaaaaccccccccaaaaaaaaaacccaaaaaaaacccaaaccccccccccccaaaaaacccccccccccccccccccccccccccccccccccccaaaaaaaaaacccaaaachhhnnnntttsswwyywwwwwvvvrrqkmdddccccc
abaaacaaacccccccccccaaaaaaacccccccccaaaaaacccccaacccaaacccaaaaaaaccccccccaaaaaaccccccccccccccaaccccccccccccccccccccccaaaaaaaaacccaaaaaahhhmmmmmsssswwywwwwwwvrrrrkkdddccccc
abaaaaaaacccccccccccaaaaaaacccccccccaaaaaaccccccaaccccccaaaaaaaaccccccccaaaaaaaaccccccccaaccaaaccccccccccccccccaacccccaaaaaaacccccaaaaahhhhhmmmmssswwwwwrrrrrrrrkkkeeeccccc
abcaaaaaaccccccccccaaaaaacccccccccccaaaaaacccccaaaaccccaaaaaaaaacccccccaaaaaaaaaacccccccaaacaaacccccccccccccacaaaccccaaaaaaccccccaaaaacchhhhhmmmmsswwwwrrrrrrrrrkkkeeeccccc
abaaaaaacccccccccccaaaaaaccccccccccaaaaaaaaccccaaaaccccaaaaaaaaccccccccaaaaaaaaaacaaacccaaaaaaccccccaacccccaaaaaccaccaaaaaaacccccaccaacccchhhhmmmssswwsrrrrrrrkkkkkeeeccccc
abaaaaaaaccccccccaaccccaaccccccccccaaaaaaacccccaaaacccccccaaaaaacccaaccacaaaaaccccaaaccccaaaaaaaaaacaaaccccaaaaaaaaccaaacaaaccccccccccccccchhhhmmssssssrlllkkkkkkkeeecccccc
abaaaaaaaaccccccaaacaacccccccccccccaaaaaaaaccccccccccccccaaaaaaacccaacccccaaaaccccaaaaaaaaaaaaaaaaaaaacccccccaaaaaccccccccaaaacccccccccccccchhgmmmsssssllllkkkkkeeeeeaacccc
abcaaacaaacccccccaaaaaccccccccccccaaaaaaaaaccccccccccccccaaaccaaaaaaaaaacccaaccaaaaaaaaaaaaaaaaacaaaaaaaccccaaaaacccccccaaaaaacccccccccccccccggmmmlssslllllffeeeeeeeaaacccc
abcaaacccccccccaaaaaacccccccaaacaaacaaaaaaacccccccccccccaaaaccccaaaaaaaacccccccaaaaaaaaaaaaaaaccaaaaaaaaccccaacaacccccccaaaaaacccccccccccccccgggmmlllllllfffffeeeeaaaaacccc
abcaaccccccccccaaaaaaaacccccaaaaaaaaaaaaaccccccccccccccccaaaacccccaaaacccccaacccaaaaaaaccccaaaccaaaaaaaacccccccaaccccccccaaaaaaaccccccccccccccggglllllllffffffecacaaacccccc
abcccccccccaaccaacaaaaaccccccaaaaaaaaaaaaccccccaaccaaccaaaaaaccccaaaaaccccaaccccccaaaaaacccaaaccaacaaaccaaaacccccccccccccaaaaaaaccccccccccccccggggllllfffffcccccccaaacccccc
abcccccccaaaaaaccaaacccccccccaaaaaaaaccaaacccccaaaaaaccaaaaacccccaaaaaacccaaaccaaaaaaaaacccccccccccaaaccaaaaacccccccccccaaaaaaaaaccaaccccccccccggggggfffffccccccccccccccccc
abcaaccccaaaaaaccaaccccccccaaaaaaaaaacccaacccccaaaaaccccaaaaaccccacccaaccaaaaccaaaaaccaacccccccccccccccaaaaaacccccccaaacaaaaaaaaaaaaacccccccccccgggggfffaacccccccccccccaccc
abaaaccccaaaaaaccccccccaaacaaaaaaaaaaaaaaaaaaccaaaaaacccaaccacccccccaaaaaaaaaaaaaaaccccccccccccccaaaaccaaaaaacccccccaaaaaaaaaacaaaaaaccccccccccccagggfcaaaccccccccccccaaaaa
abaaaacccaaaaaccccccccaaaacaaacaaacccaaaaaaaacaaaaaaaaccccccccccccccaaaaaaaaaaaaaaacccccccccccccaaaaacccaaaaaccccccccaaaaaacaaaaaaaaccccccccccccccaccccaaacccccccccccccaaaa
abaaaaccccaaaaccccccccaaaacccccaaacccccaaaacccaaaaaaaacccccccccccccccaaaaaaaaaaaaaacccccccccccccaaaaaaccaaaccccccccccaaaaaaaaaaaaaaaaccccccccccccccccccccacccccccccccccaaaa
abaacccccccccccccccccccaaacccccaacccccaaaaaacccccaacccccccccccccccccccccaaaaaaaaacccccccccccccccaaaaaacccccccccccccaaaaaaaaaaaaaaaaaaacccccccccccccccccccccccccccccccaaaaaa";
    #endregion
}