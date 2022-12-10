using System.Text;

namespace AdventOfCode;

public class Day10
{
    [TestCase(sample, 13140)]
    [TestCase(input, 12740)]
    public void Part1(string input, int expected)
    {
        var lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);

        var cycle = 0;
        var x = 1;
        var checks = new HashSet<int> { 20, 60, 100, 140, 180, 220 };
        var total = 0;
        void PassCycle()
        {
            cycle++;
            if (checks.Contains(cycle))
            {
                var strength = x * cycle;
                total += strength;
            }
        }
        
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            if (line == "noop")
            {
                // only pass a cycle...
                PassCycle();
            }
            else // if must be an an "addx V"
            {
                var arg = int.Parse(line.Substring("addx".Length).Trim());
                PassCycle();
                PassCycle();
                
                x += arg;
            }
        }
        
        Assert.AreEqual(expected, total);

    }
    
    [TestCase(sample, @"##..##..##..##..##..##..##..##..##..##..
###...###...###...###...###...###...###.
####....####....####....####....####....
#####.....#####.....#####.....#####.....
######......######......######......####
#######.......#######.......#######.....")]
    [TestCase(input, @"###..###..###...##..###...##...##..####.
#..#.#..#.#..#.#..#.#..#.#..#.#..#.#....
#..#.###..#..#.#..#.#..#.#..#.#....###..
###..#..#.###..####.###..####.#.##.#....
#.#..#..#.#....#..#.#.#..#..#.#..#.#....
#..#.###..#....#..#.#..#.#..#..###.#....")]
    public void Part2(string input, string expected)
    {
        var lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);

        var cycle = 0;
        var x = 1;
        var checks = new HashSet<int> { 40, 80, 120, 160, 200 };
        // var total = 0;
        var message = new StringBuilder();
        void PassCycle()
        {
            var isLit = false;

            var diff = Math.Abs(x - (cycle%40));
            if (diff <= 1)
            {
                isLit = true;
            }
            
            message.Append(isLit ? '#' : '.');
            
            cycle++;
            if (checks.Contains(cycle))
            {
                message.Append('\n');
                // var strength = x * cycle;
                // total += strength;
            }
        }
        
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            if (line == "noop")
            {
                // only pass a cycle...
                PassCycle();
            }
            else // if must be an an "addx V"
            {
                var arg = int.Parse(line.Substring("addx".Length).Trim());
                PassCycle();
                PassCycle();
                
                x += arg;
            }

            if (cycle >= 240)
            {
                // its game over, we have drawn the screen!
                break;
            }
        }
        
        // Assert.AreEqual(expected, total);
        var display = message.ToString();
        Console.WriteLine(display);
        Assert.AreEqual(expected, display);

    }

    
    #region sample
    public const string sample = @"addx 15
addx -11
addx 6
addx -3
addx 5
addx -1
addx -8
addx 13
addx 4
noop
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx -35
addx 1
addx 24
addx -19
addx 1
addx 16
addx -11
noop
noop
addx 21
addx -15
noop
noop
addx -3
addx 9
addx 1
addx -3
addx 8
addx 1
addx 5
noop
noop
noop
noop
noop
addx -36
noop
addx 1
addx 7
noop
noop
noop
addx 2
addx 6
noop
noop
noop
noop
noop
addx 1
noop
noop
addx 7
addx 1
noop
addx -13
addx 13
addx 7
noop
addx 1
addx -33
noop
noop
noop
addx 2
noop
noop
noop
addx 8
noop
addx -1
addx 2
addx 1
noop
addx 17
addx -9
addx 1
addx 1
addx -3
addx 11
noop
noop
addx 1
noop
addx 1
noop
noop
addx -13
addx -19
addx 1
addx 3
addx 26
addx -30
addx 12
addx -1
addx 3
addx 1
noop
noop
noop
addx -9
addx 18
addx 1
addx 2
noop
noop
addx 9
noop
noop
noop
addx -1
addx 2
addx -37
addx 1
addx 3
noop
addx 15
addx -21
addx 22
addx -6
addx 1
noop
addx 2
addx 1
noop
addx -10
noop
noop
addx 20
addx 1
addx 2
addx 2
addx -6
addx -11
noop
noop
noop";
    #endregion
    
    #region input
    public const string input = @"noop
noop
noop
addx 4
addx 1
addx 5
addx 1
addx 5
noop
addx -1
addx -6
addx 11
noop
noop
noop
noop
addx 6
addx 5
noop
noop
noop
addx -30
addx 34
addx 2
addx -39
noop
addx 5
addx 2
addx 19
addx -18
addx 2
addx 5
addx 2
addx 3
noop
addx 2
addx 3
noop
addx 2
addx 3
noop
addx 2
addx 3
noop
addx 2
addx -15
addx -22
noop
noop
addx 5
addx 2
noop
noop
addx 14
addx -11
addx 5
addx 2
addx 3
noop
addx 2
addx -16
addx 17
addx 2
addx 5
addx 2
addx -6
addx -25
addx 35
addx 1
addx -36
addx 1
addx 22
addx -19
addx 5
addx 2
noop
noop
addx 5
noop
noop
noop
addx 1
addx 4
noop
noop
noop
addx 5
noop
addx 1
addx 2
addx 3
addx 4
addx -34
addx 21
addx -24
addx 2
addx 5
addx 7
addx -6
addx 2
addx 30
addx -23
addx 10
addx -9
addx 2
addx 2
addx 5
addx -12
addx 13
addx 2
addx 5
addx 2
addx -12
addx -24
addx -1
noop
addx 3
addx 3
addx 1
addx 5
addx 21
addx -16
noop
addx 19
addx -18
addx 2
addx 5
addx 2
addx 3
noop
addx 3
addx -1
addx 1
addx 2
addx -18
addx 1
noop";
    #endregion
}