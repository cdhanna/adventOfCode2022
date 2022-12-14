namespace AdventOfCode;

public class Day14
{
    public Vec CreateVec(string str)
    {
        var parts = str.Split(",");
        return new Vec
        {
            x = int.Parse(parts[0]),
            y = int.Parse(parts[1]),
        };
    }

    void PrintMap(char[][] map)
    {
        for (var y = 0; y < 20; y++)
        {
            Console.WriteLine();

            for (var x = 480; x < 505; x++)
            {
                var c = map[y][x];
                switch (c)
                {
                    case '#': 
                        Console.Write("#");
                        break;
                    case 's':
                        Console.Write("@");
                        break;
                    default:
                        Console.Write(".");
                        break;
                }
            }
        }
    }
    
    [TestCase(sample, 24)]
    [TestCase(input, 1406)]
    public void Part1(string input, int expected)
    {
        var width = 1024;
        var height = 1024;
        var map = new char[height][];
        for (var y = 0; y < height; y++)
        {
            map[y] = new char[width];
        }
        foreach (var line in input.Split("\n", StringSplitOptions.RemoveEmptyEntries))
        {
            var points = line.Split(" -> ", StringSplitOptions.RemoveEmptyEntries);
            var a = CreateVec(points[0]);
            for (var i = 1; i < points.Length; i++)
            {
                var b = CreateVec(points[i]);
                var diff = b - a;
                var dir = diff.Normalize();
                var length = Math.Abs(diff.x + diff.y);
                for (var x = 0; x <= length; x++)
                {
                    var spot = a + dir * x;
                    map[spot.y][spot.x] = '#';
                }
                a = b;
            }
        }
        
        // simulate!
        var start = new Vec { x = 500, y = 0 };

        var foundOutOfBounds = false;
        var outerMaxIter = 999999;
        var count = 0;
        // for (var i = 0; i < 29; i++)
        while (!foundOutOfBounds && outerMaxIter-- > 0)
        {
            var pos = start;

            // try to fall...
            var atRest = false;
            var maxIter = 99999;
            while (!atRest && maxIter-- > 0)
            {
                var isOutOfBounds = pos.y + 1 >= map.Length;
                if (isOutOfBounds)
                {
                    foundOutOfBounds = true;
                    break;
                }
                var isBottomOpen = map[pos.y+1][pos.x] == default;
                if (isBottomOpen)
                {
                    pos += Vec.Down;
                } else if (map[pos.y + 1][pos.x - 1] == default)
                {
                    pos += Vec.Down + Vec.Left;
                } else if (map[pos.y + 1][pos.x + 1] == default)
                {
                    pos += Vec.Down + Vec.Right;
                }
                else
                {
                    atRest = true;
                }
            }


            if (foundOutOfBounds)
            {
                break;
            }

            count++;
            map[pos.y][pos.x] = 's';

        }

        // PrintMap(map);
        Assert.AreEqual(expected, count);
    }
    
    
    
    [TestCase(sample, 93)]
    [TestCase(input, 20870)]
    public void Part2(string input, int expected)
    {
        var width = 1024;
        var height = 1024;
        var map = new char[height][];
        for (var y = 0; y < height; y++)
        {
            map[y] = new char[width];
        }

        var highestPointFound = 0;
        foreach (var line in input.Split("\n", StringSplitOptions.RemoveEmptyEntries))
        {
            var points = line.Split(" -> ", StringSplitOptions.RemoveEmptyEntries);
            var a = CreateVec(points[0]);
            for (var i = 1; i < points.Length; i++)
            {
                var b = CreateVec(points[i]);
                var diff = b - a;
                var dir = diff.Normalize();
                var length = Math.Abs(diff.x + diff.y);
                for (var x = 0; x <= length; x++)
                {
                    var spot = a + dir * x;
                    map[spot.y][spot.x] = '#';
                    if (spot.y > highestPointFound)
                    {
                        highestPointFound = spot.y;
                    }
                }
                a = b;
            }
        }

        for (var x = 0; x < width; x++)
        {
            map[highestPointFound+2][x] = '#';
        }
        
        // simulate!
        var start = new Vec { x = 500, y = 0 };

        var foundOutOfBounds = false;
        var outerMaxIter = 999999;
        var count = 0;
        // for (var i = 0; i < 29; i++)
        while (!foundOutOfBounds && outerMaxIter-- > 0)
        {
            var pos = start;
        
            // try to fall...
            var atRest = false;
            var maxIter = 99999;
            while (!atRest && maxIter-- > 0)
            {
                var isOutOfBounds = pos.y + 1 >= map.Length;
                if (isOutOfBounds)
                {
                    foundOutOfBounds = true;
                    break;
                }
                var isBottomOpen = map[pos.y+1][pos.x] == default;
                if (isBottomOpen)
                {
                    pos += Vec.Down;
                } else if (map[pos.y + 1][pos.x - 1] == default)
                {
                    pos += Vec.Down + Vec.Left;
                } else if (map[pos.y + 1][pos.x + 1] == default)
                {
                    pos += Vec.Down + Vec.Right;
                }
                else
                {
                    atRest = true;
                }
            }

            if (maxIter == 99999 - 1)
            {
                foundOutOfBounds = true;
                break;
            }
            if (foundOutOfBounds)
            {
                break;
            }
        
            count++;
            map[pos.y][pos.x] = 's';
        
        }

        PrintMap(map);
        Assert.AreEqual(expected, count + 1);
    }
    
    #region sample

    public const string sample = @"498,4 -> 498,6 -> 496,6
503,4 -> 502,4 -> 502,9 -> 494,9";
    #endregion
    
    
    #region input

    public const string input = @"500,30 -> 504,30
503,28 -> 507,28
477,136 -> 477,133 -> 477,136 -> 479,136 -> 479,135 -> 479,136 -> 481,136 -> 481,127 -> 481,136 -> 483,136 -> 483,131 -> 483,136 -> 485,136 -> 485,129 -> 485,136 -> 487,136 -> 487,135 -> 487,136
496,97 -> 496,99 -> 488,99 -> 488,103 -> 508,103 -> 508,99 -> 502,99 -> 502,97
537,66 -> 541,66
491,23 -> 491,15 -> 491,23 -> 493,23 -> 493,18 -> 493,23 -> 495,23 -> 495,15 -> 495,23 -> 497,23 -> 497,15 -> 497,23 -> 499,23 -> 499,18 -> 499,23 -> 501,23 -> 501,14 -> 501,23 -> 503,23 -> 503,21 -> 503,23 -> 505,23 -> 505,19 -> 505,23 -> 507,23 -> 507,15 -> 507,23
479,85 -> 479,87 -> 473,87 -> 473,94 -> 486,94 -> 486,87 -> 483,87 -> 483,85
540,64 -> 544,64
477,136 -> 477,133 -> 477,136 -> 479,136 -> 479,135 -> 479,136 -> 481,136 -> 481,127 -> 481,136 -> 483,136 -> 483,131 -> 483,136 -> 485,136 -> 485,129 -> 485,136 -> 487,136 -> 487,135 -> 487,136
528,68 -> 532,68
477,136 -> 477,133 -> 477,136 -> 479,136 -> 479,135 -> 479,136 -> 481,136 -> 481,127 -> 481,136 -> 483,136 -> 483,131 -> 483,136 -> 485,136 -> 485,129 -> 485,136 -> 487,136 -> 487,135 -> 487,136
537,62 -> 541,62
514,47 -> 514,41 -> 514,47 -> 516,47 -> 516,42 -> 516,47 -> 518,47 -> 518,42 -> 518,47 -> 520,47 -> 520,44 -> 520,47 -> 522,47 -> 522,44 -> 522,47 -> 524,47 -> 524,42 -> 524,47 -> 526,47 -> 526,38 -> 526,47 -> 528,47 -> 528,43 -> 528,47 -> 530,47 -> 530,44 -> 530,47
500,34 -> 504,34
477,136 -> 477,133 -> 477,136 -> 479,136 -> 479,135 -> 479,136 -> 481,136 -> 481,127 -> 481,136 -> 483,136 -> 483,131 -> 483,136 -> 485,136 -> 485,129 -> 485,136 -> 487,136 -> 487,135 -> 487,136
482,154 -> 482,152 -> 482,154 -> 484,154 -> 484,149 -> 484,154 -> 486,154 -> 486,146 -> 486,154 -> 488,154 -> 488,153 -> 488,154 -> 490,154 -> 490,144 -> 490,154 -> 492,154 -> 492,147 -> 492,154 -> 494,154 -> 494,151 -> 494,154 -> 496,154 -> 496,151 -> 496,154
514,47 -> 514,41 -> 514,47 -> 516,47 -> 516,42 -> 516,47 -> 518,47 -> 518,42 -> 518,47 -> 520,47 -> 520,44 -> 520,47 -> 522,47 -> 522,44 -> 522,47 -> 524,47 -> 524,42 -> 524,47 -> 526,47 -> 526,38 -> 526,47 -> 528,47 -> 528,43 -> 528,47 -> 530,47 -> 530,44 -> 530,47
491,23 -> 491,15 -> 491,23 -> 493,23 -> 493,18 -> 493,23 -> 495,23 -> 495,15 -> 495,23 -> 497,23 -> 497,15 -> 497,23 -> 499,23 -> 499,18 -> 499,23 -> 501,23 -> 501,14 -> 501,23 -> 503,23 -> 503,21 -> 503,23 -> 505,23 -> 505,19 -> 505,23 -> 507,23 -> 507,15 -> 507,23
482,154 -> 482,152 -> 482,154 -> 484,154 -> 484,149 -> 484,154 -> 486,154 -> 486,146 -> 486,154 -> 488,154 -> 488,153 -> 488,154 -> 490,154 -> 490,144 -> 490,154 -> 492,154 -> 492,147 -> 492,154 -> 494,154 -> 494,151 -> 494,154 -> 496,154 -> 496,151 -> 496,154
482,82 -> 498,82 -> 498,81
486,115 -> 486,117 -> 484,117 -> 484,123 -> 494,123 -> 494,117 -> 490,117 -> 490,115
482,154 -> 482,152 -> 482,154 -> 484,154 -> 484,149 -> 484,154 -> 486,154 -> 486,146 -> 486,154 -> 488,154 -> 488,153 -> 488,154 -> 490,154 -> 490,144 -> 490,154 -> 492,154 -> 492,147 -> 492,154 -> 494,154 -> 494,151 -> 494,154 -> 496,154 -> 496,151 -> 496,154
491,23 -> 491,15 -> 491,23 -> 493,23 -> 493,18 -> 493,23 -> 495,23 -> 495,15 -> 495,23 -> 497,23 -> 497,15 -> 497,23 -> 499,23 -> 499,18 -> 499,23 -> 501,23 -> 501,14 -> 501,23 -> 503,23 -> 503,21 -> 503,23 -> 505,23 -> 505,19 -> 505,23 -> 507,23 -> 507,15 -> 507,23
514,47 -> 514,41 -> 514,47 -> 516,47 -> 516,42 -> 516,47 -> 518,47 -> 518,42 -> 518,47 -> 520,47 -> 520,44 -> 520,47 -> 522,47 -> 522,44 -> 522,47 -> 524,47 -> 524,42 -> 524,47 -> 526,47 -> 526,38 -> 526,47 -> 528,47 -> 528,43 -> 528,47 -> 530,47 -> 530,44 -> 530,47
546,64 -> 550,64
482,154 -> 482,152 -> 482,154 -> 484,154 -> 484,149 -> 484,154 -> 486,154 -> 486,146 -> 486,154 -> 488,154 -> 488,153 -> 488,154 -> 490,154 -> 490,144 -> 490,154 -> 492,154 -> 492,147 -> 492,154 -> 494,154 -> 494,151 -> 494,154 -> 496,154 -> 496,151 -> 496,154
530,50 -> 530,53 -> 523,53 -> 523,57 -> 541,57 -> 541,53 -> 534,53 -> 534,50
482,154 -> 482,152 -> 482,154 -> 484,154 -> 484,149 -> 484,154 -> 486,154 -> 486,146 -> 486,154 -> 488,154 -> 488,153 -> 488,154 -> 490,154 -> 490,144 -> 490,154 -> 492,154 -> 492,147 -> 492,154 -> 494,154 -> 494,151 -> 494,154 -> 496,154 -> 496,151 -> 496,154
506,34 -> 510,34
482,154 -> 482,152 -> 482,154 -> 484,154 -> 484,149 -> 484,154 -> 486,154 -> 486,146 -> 486,154 -> 488,154 -> 488,153 -> 488,154 -> 490,154 -> 490,144 -> 490,154 -> 492,154 -> 492,147 -> 492,154 -> 494,154 -> 494,151 -> 494,154 -> 496,154 -> 496,151 -> 496,154
514,47 -> 514,41 -> 514,47 -> 516,47 -> 516,42 -> 516,47 -> 518,47 -> 518,42 -> 518,47 -> 520,47 -> 520,44 -> 520,47 -> 522,47 -> 522,44 -> 522,47 -> 524,47 -> 524,42 -> 524,47 -> 526,47 -> 526,38 -> 526,47 -> 528,47 -> 528,43 -> 528,47 -> 530,47 -> 530,44 -> 530,47
482,154 -> 482,152 -> 482,154 -> 484,154 -> 484,149 -> 484,154 -> 486,154 -> 486,146 -> 486,154 -> 488,154 -> 488,153 -> 488,154 -> 490,154 -> 490,144 -> 490,154 -> 492,154 -> 492,147 -> 492,154 -> 494,154 -> 494,151 -> 494,154 -> 496,154 -> 496,151 -> 496,154
514,47 -> 514,41 -> 514,47 -> 516,47 -> 516,42 -> 516,47 -> 518,47 -> 518,42 -> 518,47 -> 520,47 -> 520,44 -> 520,47 -> 522,47 -> 522,44 -> 522,47 -> 524,47 -> 524,42 -> 524,47 -> 526,47 -> 526,38 -> 526,47 -> 528,47 -> 528,43 -> 528,47 -> 530,47 -> 530,44 -> 530,47
503,108 -> 508,108
512,34 -> 516,34
561,77 -> 561,78 -> 578,78 -> 578,77
515,32 -> 519,32
543,62 -> 547,62
482,154 -> 482,152 -> 482,154 -> 484,154 -> 484,149 -> 484,154 -> 486,154 -> 486,146 -> 486,154 -> 488,154 -> 488,153 -> 488,154 -> 490,154 -> 490,144 -> 490,154 -> 492,154 -> 492,147 -> 492,154 -> 494,154 -> 494,151 -> 494,154 -> 496,154 -> 496,151 -> 496,154
534,64 -> 538,64
514,110 -> 519,110
482,154 -> 482,152 -> 482,154 -> 484,154 -> 484,149 -> 484,154 -> 486,154 -> 486,146 -> 486,154 -> 488,154 -> 488,153 -> 488,154 -> 490,154 -> 490,144 -> 490,154 -> 492,154 -> 492,147 -> 492,154 -> 494,154 -> 494,151 -> 494,154 -> 496,154 -> 496,151 -> 496,154
543,66 -> 547,66
482,154 -> 482,152 -> 482,154 -> 484,154 -> 484,149 -> 484,154 -> 486,154 -> 486,146 -> 486,154 -> 488,154 -> 488,153 -> 488,154 -> 490,154 -> 490,144 -> 490,154 -> 492,154 -> 492,147 -> 492,154 -> 494,154 -> 494,151 -> 494,154 -> 496,154 -> 496,151 -> 496,154
482,154 -> 482,152 -> 482,154 -> 484,154 -> 484,149 -> 484,154 -> 486,154 -> 486,146 -> 486,154 -> 488,154 -> 488,153 -> 488,154 -> 490,154 -> 490,144 -> 490,154 -> 492,154 -> 492,147 -> 492,154 -> 494,154 -> 494,151 -> 494,154 -> 496,154 -> 496,151 -> 496,154
477,136 -> 477,133 -> 477,136 -> 479,136 -> 479,135 -> 479,136 -> 481,136 -> 481,127 -> 481,136 -> 483,136 -> 483,131 -> 483,136 -> 485,136 -> 485,129 -> 485,136 -> 487,136 -> 487,135 -> 487,136
503,32 -> 507,32
504,112 -> 509,112
561,77 -> 561,78 -> 578,78 -> 578,77
479,85 -> 479,87 -> 473,87 -> 473,94 -> 486,94 -> 486,87 -> 483,87 -> 483,85
482,154 -> 482,152 -> 482,154 -> 484,154 -> 484,149 -> 484,154 -> 486,154 -> 486,146 -> 486,154 -> 488,154 -> 488,153 -> 488,154 -> 490,154 -> 490,144 -> 490,154 -> 492,154 -> 492,147 -> 492,154 -> 494,154 -> 494,151 -> 494,154 -> 496,154 -> 496,151 -> 496,154
512,30 -> 516,30
491,23 -> 491,15 -> 491,23 -> 493,23 -> 493,18 -> 493,23 -> 495,23 -> 495,15 -> 495,23 -> 497,23 -> 497,15 -> 497,23 -> 499,23 -> 499,18 -> 499,23 -> 501,23 -> 501,14 -> 501,23 -> 503,23 -> 503,21 -> 503,23 -> 505,23 -> 505,19 -> 505,23 -> 507,23 -> 507,15 -> 507,23
496,97 -> 496,99 -> 488,99 -> 488,103 -> 508,103 -> 508,99 -> 502,99 -> 502,97
500,110 -> 505,110
488,161 -> 493,161
549,66 -> 553,66
486,115 -> 486,117 -> 484,117 -> 484,123 -> 494,123 -> 494,117 -> 490,117 -> 490,115
514,47 -> 514,41 -> 514,47 -> 516,47 -> 516,42 -> 516,47 -> 518,47 -> 518,42 -> 518,47 -> 520,47 -> 520,44 -> 520,47 -> 522,47 -> 522,44 -> 522,47 -> 524,47 -> 524,42 -> 524,47 -> 526,47 -> 526,38 -> 526,47 -> 528,47 -> 528,43 -> 528,47 -> 530,47 -> 530,44 -> 530,47
494,34 -> 498,34
491,23 -> 491,15 -> 491,23 -> 493,23 -> 493,18 -> 493,23 -> 495,23 -> 495,15 -> 495,23 -> 497,23 -> 497,15 -> 497,23 -> 499,23 -> 499,18 -> 499,23 -> 501,23 -> 501,14 -> 501,23 -> 503,23 -> 503,21 -> 503,23 -> 505,23 -> 505,19 -> 505,23 -> 507,23 -> 507,15 -> 507,23
530,50 -> 530,53 -> 523,53 -> 523,57 -> 541,57 -> 541,53 -> 534,53 -> 534,50
477,136 -> 477,133 -> 477,136 -> 479,136 -> 479,135 -> 479,136 -> 481,136 -> 481,127 -> 481,136 -> 483,136 -> 483,131 -> 483,136 -> 485,136 -> 485,129 -> 485,136 -> 487,136 -> 487,135 -> 487,136
482,154 -> 482,152 -> 482,154 -> 484,154 -> 484,149 -> 484,154 -> 486,154 -> 486,146 -> 486,154 -> 488,154 -> 488,153 -> 488,154 -> 490,154 -> 490,144 -> 490,154 -> 492,154 -> 492,147 -> 492,154 -> 494,154 -> 494,151 -> 494,154 -> 496,154 -> 496,151 -> 496,154
546,68 -> 550,68
482,154 -> 482,152 -> 482,154 -> 484,154 -> 484,149 -> 484,154 -> 486,154 -> 486,146 -> 486,154 -> 488,154 -> 488,153 -> 488,154 -> 490,154 -> 490,144 -> 490,154 -> 492,154 -> 492,147 -> 492,154 -> 494,154 -> 494,151 -> 494,154 -> 496,154 -> 496,151 -> 496,154
514,47 -> 514,41 -> 514,47 -> 516,47 -> 516,42 -> 516,47 -> 518,47 -> 518,42 -> 518,47 -> 520,47 -> 520,44 -> 520,47 -> 522,47 -> 522,44 -> 522,47 -> 524,47 -> 524,42 -> 524,47 -> 526,47 -> 526,38 -> 526,47 -> 528,47 -> 528,43 -> 528,47 -> 530,47 -> 530,44 -> 530,47
491,23 -> 491,15 -> 491,23 -> 493,23 -> 493,18 -> 493,23 -> 495,23 -> 495,15 -> 495,23 -> 497,23 -> 497,15 -> 497,23 -> 499,23 -> 499,18 -> 499,23 -> 501,23 -> 501,14 -> 501,23 -> 503,23 -> 503,21 -> 503,23 -> 505,23 -> 505,19 -> 505,23 -> 507,23 -> 507,15 -> 507,23
511,112 -> 516,112
547,73 -> 547,74 -> 567,74
496,97 -> 496,99 -> 488,99 -> 488,103 -> 508,103 -> 508,99 -> 502,99 -> 502,97
496,97 -> 496,99 -> 488,99 -> 488,103 -> 508,103 -> 508,99 -> 502,99 -> 502,97
518,34 -> 522,34
486,115 -> 486,117 -> 484,117 -> 484,123 -> 494,123 -> 494,117 -> 490,117 -> 490,115
514,47 -> 514,41 -> 514,47 -> 516,47 -> 516,42 -> 516,47 -> 518,47 -> 518,42 -> 518,47 -> 520,47 -> 520,44 -> 520,47 -> 522,47 -> 522,44 -> 522,47 -> 524,47 -> 524,42 -> 524,47 -> 526,47 -> 526,38 -> 526,47 -> 528,47 -> 528,43 -> 528,47 -> 530,47 -> 530,44 -> 530,47
491,23 -> 491,15 -> 491,23 -> 493,23 -> 493,18 -> 493,23 -> 495,23 -> 495,15 -> 495,23 -> 497,23 -> 497,15 -> 497,23 -> 499,23 -> 499,18 -> 499,23 -> 501,23 -> 501,14 -> 501,23 -> 503,23 -> 503,21 -> 503,23 -> 505,23 -> 505,19 -> 505,23 -> 507,23 -> 507,15 -> 507,23
491,23 -> 491,15 -> 491,23 -> 493,23 -> 493,18 -> 493,23 -> 495,23 -> 495,15 -> 495,23 -> 497,23 -> 497,15 -> 497,23 -> 499,23 -> 499,18 -> 499,23 -> 501,23 -> 501,14 -> 501,23 -> 503,23 -> 503,21 -> 503,23 -> 505,23 -> 505,19 -> 505,23 -> 507,23 -> 507,15 -> 507,23
482,140 -> 482,141 -> 489,141 -> 489,140
514,47 -> 514,41 -> 514,47 -> 516,47 -> 516,42 -> 516,47 -> 518,47 -> 518,42 -> 518,47 -> 520,47 -> 520,44 -> 520,47 -> 522,47 -> 522,44 -> 522,47 -> 524,47 -> 524,42 -> 524,47 -> 526,47 -> 526,38 -> 526,47 -> 528,47 -> 528,43 -> 528,47 -> 530,47 -> 530,44 -> 530,47
496,97 -> 496,99 -> 488,99 -> 488,103 -> 508,103 -> 508,99 -> 502,99 -> 502,97
491,23 -> 491,15 -> 491,23 -> 493,23 -> 493,18 -> 493,23 -> 495,23 -> 495,15 -> 495,23 -> 497,23 -> 497,15 -> 497,23 -> 499,23 -> 499,18 -> 499,23 -> 501,23 -> 501,14 -> 501,23 -> 503,23 -> 503,21 -> 503,23 -> 505,23 -> 505,19 -> 505,23 -> 507,23 -> 507,15 -> 507,23
477,136 -> 477,133 -> 477,136 -> 479,136 -> 479,135 -> 479,136 -> 481,136 -> 481,127 -> 481,136 -> 483,136 -> 483,131 -> 483,136 -> 485,136 -> 485,129 -> 485,136 -> 487,136 -> 487,135 -> 487,136
491,23 -> 491,15 -> 491,23 -> 493,23 -> 493,18 -> 493,23 -> 495,23 -> 495,15 -> 495,23 -> 497,23 -> 497,15 -> 497,23 -> 499,23 -> 499,18 -> 499,23 -> 501,23 -> 501,14 -> 501,23 -> 503,23 -> 503,21 -> 503,23 -> 505,23 -> 505,19 -> 505,23 -> 507,23 -> 507,15 -> 507,23
491,23 -> 491,15 -> 491,23 -> 493,23 -> 493,18 -> 493,23 -> 495,23 -> 495,15 -> 495,23 -> 497,23 -> 497,15 -> 497,23 -> 499,23 -> 499,18 -> 499,23 -> 501,23 -> 501,14 -> 501,23 -> 503,23 -> 503,21 -> 503,23 -> 505,23 -> 505,19 -> 505,23 -> 507,23 -> 507,15 -> 507,23
514,47 -> 514,41 -> 514,47 -> 516,47 -> 516,42 -> 516,47 -> 518,47 -> 518,42 -> 518,47 -> 520,47 -> 520,44 -> 520,47 -> 522,47 -> 522,44 -> 522,47 -> 524,47 -> 524,42 -> 524,47 -> 526,47 -> 526,38 -> 526,47 -> 528,47 -> 528,43 -> 528,47 -> 530,47 -> 530,44 -> 530,47
477,136 -> 477,133 -> 477,136 -> 479,136 -> 479,135 -> 479,136 -> 481,136 -> 481,127 -> 481,136 -> 483,136 -> 483,131 -> 483,136 -> 485,136 -> 485,129 -> 485,136 -> 487,136 -> 487,135 -> 487,136
491,23 -> 491,15 -> 491,23 -> 493,23 -> 493,18 -> 493,23 -> 495,23 -> 495,15 -> 495,23 -> 497,23 -> 497,15 -> 497,23 -> 499,23 -> 499,18 -> 499,23 -> 501,23 -> 501,14 -> 501,23 -> 503,23 -> 503,21 -> 503,23 -> 505,23 -> 505,19 -> 505,23 -> 507,23 -> 507,15 -> 507,23
477,136 -> 477,133 -> 477,136 -> 479,136 -> 479,135 -> 479,136 -> 481,136 -> 481,127 -> 481,136 -> 483,136 -> 483,131 -> 483,136 -> 485,136 -> 485,129 -> 485,136 -> 487,136 -> 487,135 -> 487,136
491,23 -> 491,15 -> 491,23 -> 493,23 -> 493,18 -> 493,23 -> 495,23 -> 495,15 -> 495,23 -> 497,23 -> 497,15 -> 497,23 -> 499,23 -> 499,18 -> 499,23 -> 501,23 -> 501,14 -> 501,23 -> 503,23 -> 503,21 -> 503,23 -> 505,23 -> 505,19 -> 505,23 -> 507,23 -> 507,15 -> 507,23
479,85 -> 479,87 -> 473,87 -> 473,94 -> 486,94 -> 486,87 -> 483,87 -> 483,85
482,154 -> 482,152 -> 482,154 -> 484,154 -> 484,149 -> 484,154 -> 486,154 -> 486,146 -> 486,154 -> 488,154 -> 488,153 -> 488,154 -> 490,154 -> 490,144 -> 490,154 -> 492,154 -> 492,147 -> 492,154 -> 494,154 -> 494,151 -> 494,154 -> 496,154 -> 496,151 -> 496,154
482,154 -> 482,152 -> 482,154 -> 484,154 -> 484,149 -> 484,154 -> 486,154 -> 486,146 -> 486,154 -> 488,154 -> 488,153 -> 488,154 -> 490,154 -> 490,144 -> 490,154 -> 492,154 -> 492,147 -> 492,154 -> 494,154 -> 494,151 -> 494,154 -> 496,154 -> 496,151 -> 496,154
514,47 -> 514,41 -> 514,47 -> 516,47 -> 516,42 -> 516,47 -> 518,47 -> 518,42 -> 518,47 -> 520,47 -> 520,44 -> 520,47 -> 522,47 -> 522,44 -> 522,47 -> 524,47 -> 524,42 -> 524,47 -> 526,47 -> 526,38 -> 526,47 -> 528,47 -> 528,43 -> 528,47 -> 530,47 -> 530,44 -> 530,47
514,47 -> 514,41 -> 514,47 -> 516,47 -> 516,42 -> 516,47 -> 518,47 -> 518,42 -> 518,47 -> 520,47 -> 520,44 -> 520,47 -> 522,47 -> 522,44 -> 522,47 -> 524,47 -> 524,42 -> 524,47 -> 526,47 -> 526,38 -> 526,47 -> 528,47 -> 528,43 -> 528,47 -> 530,47 -> 530,44 -> 530,47
496,97 -> 496,99 -> 488,99 -> 488,103 -> 508,103 -> 508,99 -> 502,99 -> 502,97
514,47 -> 514,41 -> 514,47 -> 516,47 -> 516,42 -> 516,47 -> 518,47 -> 518,42 -> 518,47 -> 520,47 -> 520,44 -> 520,47 -> 522,47 -> 522,44 -> 522,47 -> 524,47 -> 524,42 -> 524,47 -> 526,47 -> 526,38 -> 526,47 -> 528,47 -> 528,43 -> 528,47 -> 530,47 -> 530,44 -> 530,47
514,47 -> 514,41 -> 514,47 -> 516,47 -> 516,42 -> 516,47 -> 518,47 -> 518,42 -> 518,47 -> 520,47 -> 520,44 -> 520,47 -> 522,47 -> 522,44 -> 522,47 -> 524,47 -> 524,42 -> 524,47 -> 526,47 -> 526,38 -> 526,47 -> 528,47 -> 528,43 -> 528,47 -> 530,47 -> 530,44 -> 530,47
496,97 -> 496,99 -> 488,99 -> 488,103 -> 508,103 -> 508,99 -> 502,99 -> 502,97
552,68 -> 556,68
491,23 -> 491,15 -> 491,23 -> 493,23 -> 493,18 -> 493,23 -> 495,23 -> 495,15 -> 495,23 -> 497,23 -> 497,15 -> 497,23 -> 499,23 -> 499,18 -> 499,23 -> 501,23 -> 501,14 -> 501,23 -> 503,23 -> 503,21 -> 503,23 -> 505,23 -> 505,19 -> 505,23 -> 507,23 -> 507,15 -> 507,23
506,26 -> 510,26
514,47 -> 514,41 -> 514,47 -> 516,47 -> 516,42 -> 516,47 -> 518,47 -> 518,42 -> 518,47 -> 520,47 -> 520,44 -> 520,47 -> 522,47 -> 522,44 -> 522,47 -> 524,47 -> 524,42 -> 524,47 -> 526,47 -> 526,38 -> 526,47 -> 528,47 -> 528,43 -> 528,47 -> 530,47 -> 530,44 -> 530,47
479,85 -> 479,87 -> 473,87 -> 473,94 -> 486,94 -> 486,87 -> 483,87 -> 483,85
479,85 -> 479,87 -> 473,87 -> 473,94 -> 486,94 -> 486,87 -> 483,87 -> 483,85
494,157 -> 499,157
486,115 -> 486,117 -> 484,117 -> 484,123 -> 494,123 -> 494,117 -> 490,117 -> 490,115
482,154 -> 482,152 -> 482,154 -> 484,154 -> 484,149 -> 484,154 -> 486,154 -> 486,146 -> 486,154 -> 488,154 -> 488,153 -> 488,154 -> 490,154 -> 490,144 -> 490,154 -> 492,154 -> 492,147 -> 492,154 -> 494,154 -> 494,151 -> 494,154 -> 496,154 -> 496,151 -> 496,154
530,50 -> 530,53 -> 523,53 -> 523,57 -> 541,57 -> 541,53 -> 534,53 -> 534,50
514,47 -> 514,41 -> 514,47 -> 516,47 -> 516,42 -> 516,47 -> 518,47 -> 518,42 -> 518,47 -> 520,47 -> 520,44 -> 520,47 -> 522,47 -> 522,44 -> 522,47 -> 524,47 -> 524,42 -> 524,47 -> 526,47 -> 526,38 -> 526,47 -> 528,47 -> 528,43 -> 528,47 -> 530,47 -> 530,44 -> 530,47
482,154 -> 482,152 -> 482,154 -> 484,154 -> 484,149 -> 484,154 -> 486,154 -> 486,146 -> 486,154 -> 488,154 -> 488,153 -> 488,154 -> 490,154 -> 490,144 -> 490,154 -> 492,154 -> 492,147 -> 492,154 -> 494,154 -> 494,151 -> 494,154 -> 496,154 -> 496,151 -> 496,154
486,115 -> 486,117 -> 484,117 -> 484,123 -> 494,123 -> 494,117 -> 490,117 -> 490,115
498,159 -> 503,159
510,108 -> 515,108
530,50 -> 530,53 -> 523,53 -> 523,57 -> 541,57 -> 541,53 -> 534,53 -> 534,50
477,136 -> 477,133 -> 477,136 -> 479,136 -> 479,135 -> 479,136 -> 481,136 -> 481,127 -> 481,136 -> 483,136 -> 483,131 -> 483,136 -> 485,136 -> 485,129 -> 485,136 -> 487,136 -> 487,135 -> 487,136
477,136 -> 477,133 -> 477,136 -> 479,136 -> 479,135 -> 479,136 -> 481,136 -> 481,127 -> 481,136 -> 483,136 -> 483,131 -> 483,136 -> 485,136 -> 485,129 -> 485,136 -> 487,136 -> 487,135 -> 487,136
479,85 -> 479,87 -> 473,87 -> 473,94 -> 486,94 -> 486,87 -> 483,87 -> 483,85
514,47 -> 514,41 -> 514,47 -> 516,47 -> 516,42 -> 516,47 -> 518,47 -> 518,42 -> 518,47 -> 520,47 -> 520,44 -> 520,47 -> 522,47 -> 522,44 -> 522,47 -> 524,47 -> 524,42 -> 524,47 -> 526,47 -> 526,38 -> 526,47 -> 528,47 -> 528,43 -> 528,47 -> 530,47 -> 530,44 -> 530,47
491,23 -> 491,15 -> 491,23 -> 493,23 -> 493,18 -> 493,23 -> 495,23 -> 495,15 -> 495,23 -> 497,23 -> 497,15 -> 497,23 -> 499,23 -> 499,18 -> 499,23 -> 501,23 -> 501,14 -> 501,23 -> 503,23 -> 503,21 -> 503,23 -> 505,23 -> 505,19 -> 505,23 -> 507,23 -> 507,15 -> 507,23
491,23 -> 491,15 -> 491,23 -> 493,23 -> 493,18 -> 493,23 -> 495,23 -> 495,15 -> 495,23 -> 497,23 -> 497,15 -> 497,23 -> 499,23 -> 499,18 -> 499,23 -> 501,23 -> 501,14 -> 501,23 -> 503,23 -> 503,21 -> 503,23 -> 505,23 -> 505,19 -> 505,23 -> 507,23 -> 507,15 -> 507,23
491,23 -> 491,15 -> 491,23 -> 493,23 -> 493,18 -> 493,23 -> 495,23 -> 495,15 -> 495,23 -> 497,23 -> 497,15 -> 497,23 -> 499,23 -> 499,18 -> 499,23 -> 501,23 -> 501,14 -> 501,23 -> 503,23 -> 503,21 -> 503,23 -> 505,23 -> 505,19 -> 505,23 -> 507,23 -> 507,15 -> 507,23
534,68 -> 538,68
514,47 -> 514,41 -> 514,47 -> 516,47 -> 516,42 -> 516,47 -> 518,47 -> 518,42 -> 518,47 -> 520,47 -> 520,44 -> 520,47 -> 522,47 -> 522,44 -> 522,47 -> 524,47 -> 524,42 -> 524,47 -> 526,47 -> 526,38 -> 526,47 -> 528,47 -> 528,43 -> 528,47 -> 530,47 -> 530,44 -> 530,47
477,136 -> 477,133 -> 477,136 -> 479,136 -> 479,135 -> 479,136 -> 481,136 -> 481,127 -> 481,136 -> 483,136 -> 483,131 -> 483,136 -> 485,136 -> 485,129 -> 485,136 -> 487,136 -> 487,135 -> 487,136
486,115 -> 486,117 -> 484,117 -> 484,123 -> 494,123 -> 494,117 -> 490,117 -> 490,115
514,47 -> 514,41 -> 514,47 -> 516,47 -> 516,42 -> 516,47 -> 518,47 -> 518,42 -> 518,47 -> 520,47 -> 520,44 -> 520,47 -> 522,47 -> 522,44 -> 522,47 -> 524,47 -> 524,42 -> 524,47 -> 526,47 -> 526,38 -> 526,47 -> 528,47 -> 528,43 -> 528,47 -> 530,47 -> 530,44 -> 530,47
477,136 -> 477,133 -> 477,136 -> 479,136 -> 479,135 -> 479,136 -> 481,136 -> 481,127 -> 481,136 -> 483,136 -> 483,131 -> 483,136 -> 485,136 -> 485,129 -> 485,136 -> 487,136 -> 487,135 -> 487,136
514,47 -> 514,41 -> 514,47 -> 516,47 -> 516,42 -> 516,47 -> 518,47 -> 518,42 -> 518,47 -> 520,47 -> 520,44 -> 520,47 -> 522,47 -> 522,44 -> 522,47 -> 524,47 -> 524,42 -> 524,47 -> 526,47 -> 526,38 -> 526,47 -> 528,47 -> 528,43 -> 528,47 -> 530,47 -> 530,44 -> 530,47
477,136 -> 477,133 -> 477,136 -> 479,136 -> 479,135 -> 479,136 -> 481,136 -> 481,127 -> 481,136 -> 483,136 -> 483,131 -> 483,136 -> 485,136 -> 485,129 -> 485,136 -> 487,136 -> 487,135 -> 487,136
482,140 -> 482,141 -> 489,141 -> 489,140
482,154 -> 482,152 -> 482,154 -> 484,154 -> 484,149 -> 484,154 -> 486,154 -> 486,146 -> 486,154 -> 488,154 -> 488,153 -> 488,154 -> 490,154 -> 490,144 -> 490,154 -> 492,154 -> 492,147 -> 492,154 -> 494,154 -> 494,151 -> 494,154 -> 496,154 -> 496,151 -> 496,154
491,23 -> 491,15 -> 491,23 -> 493,23 -> 493,18 -> 493,23 -> 495,23 -> 495,15 -> 495,23 -> 497,23 -> 497,15 -> 497,23 -> 499,23 -> 499,18 -> 499,23 -> 501,23 -> 501,14 -> 501,23 -> 503,23 -> 503,21 -> 503,23 -> 505,23 -> 505,19 -> 505,23 -> 507,23 -> 507,15 -> 507,23
497,112 -> 502,112
509,28 -> 513,28
530,50 -> 530,53 -> 523,53 -> 523,57 -> 541,57 -> 541,53 -> 534,53 -> 534,50
514,47 -> 514,41 -> 514,47 -> 516,47 -> 516,42 -> 516,47 -> 518,47 -> 518,42 -> 518,47 -> 520,47 -> 520,44 -> 520,47 -> 522,47 -> 522,44 -> 522,47 -> 524,47 -> 524,42 -> 524,47 -> 526,47 -> 526,38 -> 526,47 -> 528,47 -> 528,43 -> 528,47 -> 530,47 -> 530,44 -> 530,47
514,47 -> 514,41 -> 514,47 -> 516,47 -> 516,42 -> 516,47 -> 518,47 -> 518,42 -> 518,47 -> 520,47 -> 520,44 -> 520,47 -> 522,47 -> 522,44 -> 522,47 -> 524,47 -> 524,42 -> 524,47 -> 526,47 -> 526,38 -> 526,47 -> 528,47 -> 528,43 -> 528,47 -> 530,47 -> 530,44 -> 530,47
547,73 -> 547,74 -> 567,74
482,154 -> 482,152 -> 482,154 -> 484,154 -> 484,149 -> 484,154 -> 486,154 -> 486,146 -> 486,154 -> 488,154 -> 488,153 -> 488,154 -> 490,154 -> 490,144 -> 490,154 -> 492,154 -> 492,147 -> 492,154 -> 494,154 -> 494,151 -> 494,154 -> 496,154 -> 496,151 -> 496,154
491,23 -> 491,15 -> 491,23 -> 493,23 -> 493,18 -> 493,23 -> 495,23 -> 495,15 -> 495,23 -> 497,23 -> 497,15 -> 497,23 -> 499,23 -> 499,18 -> 499,23 -> 501,23 -> 501,14 -> 501,23 -> 503,23 -> 503,21 -> 503,23 -> 505,23 -> 505,19 -> 505,23 -> 507,23 -> 507,15 -> 507,23
540,60 -> 544,60
482,82 -> 498,82 -> 498,81
477,136 -> 477,133 -> 477,136 -> 479,136 -> 479,135 -> 479,136 -> 481,136 -> 481,127 -> 481,136 -> 483,136 -> 483,131 -> 483,136 -> 485,136 -> 485,129 -> 485,136 -> 487,136 -> 487,135 -> 487,136
477,136 -> 477,133 -> 477,136 -> 479,136 -> 479,135 -> 479,136 -> 481,136 -> 481,127 -> 481,136 -> 483,136 -> 483,131 -> 483,136 -> 485,136 -> 485,129 -> 485,136 -> 487,136 -> 487,135 -> 487,136
502,161 -> 507,161
477,136 -> 477,133 -> 477,136 -> 479,136 -> 479,135 -> 479,136 -> 481,136 -> 481,127 -> 481,136 -> 483,136 -> 483,131 -> 483,136 -> 485,136 -> 485,129 -> 485,136 -> 487,136 -> 487,135 -> 487,136
506,30 -> 510,30
491,23 -> 491,15 -> 491,23 -> 493,23 -> 493,18 -> 493,23 -> 495,23 -> 495,15 -> 495,23 -> 497,23 -> 497,15 -> 497,23 -> 499,23 -> 499,18 -> 499,23 -> 501,23 -> 501,14 -> 501,23 -> 503,23 -> 503,21 -> 503,23 -> 505,23 -> 505,19 -> 505,23 -> 507,23 -> 507,15 -> 507,23
514,47 -> 514,41 -> 514,47 -> 516,47 -> 516,42 -> 516,47 -> 518,47 -> 518,42 -> 518,47 -> 520,47 -> 520,44 -> 520,47 -> 522,47 -> 522,44 -> 522,47 -> 524,47 -> 524,42 -> 524,47 -> 526,47 -> 526,38 -> 526,47 -> 528,47 -> 528,43 -> 528,47 -> 530,47 -> 530,44 -> 530,47
491,23 -> 491,15 -> 491,23 -> 493,23 -> 493,18 -> 493,23 -> 495,23 -> 495,15 -> 495,23 -> 497,23 -> 497,15 -> 497,23 -> 499,23 -> 499,18 -> 499,23 -> 501,23 -> 501,14 -> 501,23 -> 503,23 -> 503,21 -> 503,23 -> 505,23 -> 505,19 -> 505,23 -> 507,23 -> 507,15 -> 507,23
491,23 -> 491,15 -> 491,23 -> 493,23 -> 493,18 -> 493,23 -> 495,23 -> 495,15 -> 495,23 -> 497,23 -> 497,15 -> 497,23 -> 499,23 -> 499,18 -> 499,23 -> 501,23 -> 501,14 -> 501,23 -> 503,23 -> 503,21 -> 503,23 -> 505,23 -> 505,19 -> 505,23 -> 507,23 -> 507,15 -> 507,23
514,47 -> 514,41 -> 514,47 -> 516,47 -> 516,42 -> 516,47 -> 518,47 -> 518,42 -> 518,47 -> 520,47 -> 520,44 -> 520,47 -> 522,47 -> 522,44 -> 522,47 -> 524,47 -> 524,42 -> 524,47 -> 526,47 -> 526,38 -> 526,47 -> 528,47 -> 528,43 -> 528,47 -> 530,47 -> 530,44 -> 530,47
482,154 -> 482,152 -> 482,154 -> 484,154 -> 484,149 -> 484,154 -> 486,154 -> 486,146 -> 486,154 -> 488,154 -> 488,153 -> 488,154 -> 490,154 -> 490,144 -> 490,154 -> 492,154 -> 492,147 -> 492,154 -> 494,154 -> 494,151 -> 494,154 -> 496,154 -> 496,151 -> 496,154
531,66 -> 535,66
491,159 -> 496,159
491,23 -> 491,15 -> 491,23 -> 493,23 -> 493,18 -> 493,23 -> 495,23 -> 495,15 -> 495,23 -> 497,23 -> 497,15 -> 497,23 -> 499,23 -> 499,18 -> 499,23 -> 501,23 -> 501,14 -> 501,23 -> 503,23 -> 503,21 -> 503,23 -> 505,23 -> 505,19 -> 505,23 -> 507,23 -> 507,15 -> 507,23
518,112 -> 523,112
561,77 -> 561,78 -> 578,78 -> 578,77
491,23 -> 491,15 -> 491,23 -> 493,23 -> 493,18 -> 493,23 -> 495,23 -> 495,15 -> 495,23 -> 497,23 -> 497,15 -> 497,23 -> 499,23 -> 499,18 -> 499,23 -> 501,23 -> 501,14 -> 501,23 -> 503,23 -> 503,21 -> 503,23 -> 505,23 -> 505,19 -> 505,23 -> 507,23 -> 507,15 -> 507,23
486,115 -> 486,117 -> 484,117 -> 484,123 -> 494,123 -> 494,117 -> 490,117 -> 490,115
514,47 -> 514,41 -> 514,47 -> 516,47 -> 516,42 -> 516,47 -> 518,47 -> 518,42 -> 518,47 -> 520,47 -> 520,44 -> 520,47 -> 522,47 -> 522,44 -> 522,47 -> 524,47 -> 524,42 -> 524,47 -> 526,47 -> 526,38 -> 526,47 -> 528,47 -> 528,43 -> 528,47 -> 530,47 -> 530,44 -> 530,47
530,50 -> 530,53 -> 523,53 -> 523,57 -> 541,57 -> 541,53 -> 534,53 -> 534,50
497,32 -> 501,32
540,68 -> 544,68
530,50 -> 530,53 -> 523,53 -> 523,57 -> 541,57 -> 541,53 -> 534,53 -> 534,50
482,154 -> 482,152 -> 482,154 -> 484,154 -> 484,149 -> 484,154 -> 486,154 -> 486,146 -> 486,154 -> 488,154 -> 488,153 -> 488,154 -> 490,154 -> 490,144 -> 490,154 -> 492,154 -> 492,147 -> 492,154 -> 494,154 -> 494,151 -> 494,154 -> 496,154 -> 496,151 -> 496,154
482,154 -> 482,152 -> 482,154 -> 484,154 -> 484,149 -> 484,154 -> 486,154 -> 486,146 -> 486,154 -> 488,154 -> 488,153 -> 488,154 -> 490,154 -> 490,144 -> 490,154 -> 492,154 -> 492,147 -> 492,154 -> 494,154 -> 494,151 -> 494,154 -> 496,154 -> 496,151 -> 496,154
491,23 -> 491,15 -> 491,23 -> 493,23 -> 493,18 -> 493,23 -> 495,23 -> 495,15 -> 495,23 -> 497,23 -> 497,15 -> 497,23 -> 499,23 -> 499,18 -> 499,23 -> 501,23 -> 501,14 -> 501,23 -> 503,23 -> 503,21 -> 503,23 -> 505,23 -> 505,19 -> 505,23 -> 507,23 -> 507,15 -> 507,23
482,140 -> 482,141 -> 489,141 -> 489,140
509,32 -> 513,32
479,85 -> 479,87 -> 473,87 -> 473,94 -> 486,94 -> 486,87 -> 483,87 -> 483,85
506,106 -> 511,106
507,110 -> 512,110
495,161 -> 500,161
491,23 -> 491,15 -> 491,23 -> 493,23 -> 493,18 -> 493,23 -> 495,23 -> 495,15 -> 495,23 -> 497,23 -> 497,15 -> 497,23 -> 499,23 -> 499,18 -> 499,23 -> 501,23 -> 501,14 -> 501,23 -> 503,23 -> 503,21 -> 503,23 -> 505,23 -> 505,19 -> 505,23 -> 507,23 -> 507,15 -> 507,23
514,47 -> 514,41 -> 514,47 -> 516,47 -> 516,42 -> 516,47 -> 518,47 -> 518,42 -> 518,47 -> 520,47 -> 520,44 -> 520,47 -> 522,47 -> 522,44 -> 522,47 -> 524,47 -> 524,42 -> 524,47 -> 526,47 -> 526,38 -> 526,47 -> 528,47 -> 528,43 -> 528,47 -> 530,47 -> 530,44 -> 530,47";
    #endregion
}