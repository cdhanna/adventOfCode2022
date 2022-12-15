namespace AdventOfCode;

public class Day15
{
    
    void PrintMap(int[][] map, Vec offset)
    {
        for (var y = 0; y < 11; y++)
        {
            Console.WriteLine();

            for (var x = -5; x < 25; x++)
            {
                var c = map[y + offset.y][x + offset.x];
                switch (c)
                {
                    case 0: 
                        Console.Write(".");
                        break;
                    case int n when n < 0:
                        Console.Write("B");
                        break;
                    default:
                        Console.Write("#");
                        break;
                }
            }
        }
    }
    
    // [TestCase(sample, 56000011)]
    // // [TestCase(input, 26)]
    // public void Part2(string input, int expected)
    // {
    //     var width = 4000000;
    //     var height = 4000000;
    //     var map = new int[height][];
    //     for (var i = 0; i < height; i++)
    //     {
    //         map[i] = new int[width];
    //     }
    //     
    //     var lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
    //     var sensors = new Vec[lines.Length];
    //     var beacons = new Vec[lines.Length];
    //     for (var i = 0 ; i < lines.Length; i ++)
    //     {
    //         var line = lines[i];
    //         var halves = line.Split(":");
    //
    //         halves[0] = halves[0].Replace("Sensor at x=", "").Replace(" y=", "");
    //         var sensorCoordStr = halves[0].Split(",", StringSplitOptions.RemoveEmptyEntries);
    //         var sensorPos = new Vec
    //         {
    //             x = int.Parse(sensorCoordStr[0]),
    //             y = int.Parse(sensorCoordStr[1]),
    //         } ;
    //         
    //         halves[1] = halves[1].Replace("closest beacon is at x=", "").Replace(" y=", "");
    //         var beaconCoordStr = halves[1].Split(",", StringSplitOptions.RemoveEmptyEntries);
    //         var beaconPos =new Vec
    //         {
    //             x = int.Parse(beaconCoordStr[0]),
    //             y = int.Parse(beaconCoordStr[1]),
    //         };
    //         beacons[i] = beaconPos;
    //         sensors[i] = sensorPos;
    //
    //         map[beaconPos.y][beaconPos.x] = -i;
    //         map[sensorPos.y][sensorPos.x] = -i;
    //
    //         var diff = beaconPos - sensorPos;
    //         var distance = Math.Abs(diff.x) + Math.Abs(diff.y);
    //         // fill in the map around the sensorpos
    //         for (var y = sensorPos.y - distance; y <= sensorPos.y + distance; y++)
    //         {
    //             if (y < 0 || y >= height) continue; // this line is out of our bounds...
    //             for (var x = sensorPos.x - distance; x <= sensorPos.x + distance; x++)
    //             {
    //                 if (x < 0 || x >= width) continue; // out of bounds.
    //
    //                 var pos = new Vec { x = x, y = y };
    //                 // if (map[y][x] != 0) continue;
    //                 var spotDiff = pos - sensorPos;
    //                 var spotDist = Math.Abs(spotDiff.x) + Math.Abs(spotDiff.y);
    //                 // if (spotDist <= distance && map[y][x] == 0)
    //                 // {
    //                 //     // map[y][x] = (i+1);
    //                 // }
    //             }
    //         }
    //     }
    //
    //     // var row = map[offset.y + 10];
    //     // var count = 0;
    //     // foreach (var n in row)
    //     // {
    //     //     if (n > 0) count++;
    //     // }
    //     // PrintMap(map, offset);
    //
    //     // Assert.AreEqual(expected, count);
    // }

    [TestCase(sample, 26, 10)]
    [TestCase(input, 5335787, 2000000)] // too low 4712648, 4712650
    public void Part1_Take2(string input, int expected, int yBand)
    {

        var lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        var sensors = new Vec[lines.Length];
        var beacons = new Vec[lines.Length];
        var min = new Vec();
        var max = new Vec();
        var setOfNoGo = new HashSet<Vec>();
        checked
        {
            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var halves = line.Split(":");

                halves[0] = halves[0].Replace("Sensor at x=", "").Replace(" y=", "");
                var sensorCoordStr = halves[0].Split(",", StringSplitOptions.RemoveEmptyEntries);
                var sensorPos = new Vec
                {
                    x = int.Parse(sensorCoordStr[0]),
                    y = int.Parse(sensorCoordStr[1]),
                };

                halves[1] = halves[1].Replace("closest beacon is at x=", "").Replace(" y=", "");
                var beaconCoordStr = halves[1].Split(",", StringSplitOptions.RemoveEmptyEntries);
                var beaconPos = new Vec
                {
                    x = int.Parse(beaconCoordStr[0]),
                    y = int.Parse(beaconCoordStr[1]),
                };
                beacons[i] = beaconPos;
                sensors[i] = sensorPos;

                var diff = beaconPos - sensorPos;
                var distance = Math.Abs(diff.x) + Math.Abs(diff.y);

                // if the sensor and beacon relate at all to the yband, then we group them in a list...
                var distToBand = Math.Abs(sensorPos.y - yBand);
                if (distToBand <= distance)
                {
                    // distance += 10;
                    var buffer = distance;
                    for (var y = sensorPos.y - buffer; y <= sensorPos.y + buffer; y++)
                    {
                        if (y != yBand) continue;
                        for (var x = sensorPos.x - buffer; x <= sensorPos.x + buffer; x++)
                        {
                            var pos = new Vec { x = x, y = y };
                            if (pos == beaconPos || pos == sensorPos) continue;
                            var spotDiff = pos - sensorPos;
                            var spotDist = Math.Abs(spotDiff.x) + Math.Abs(spotDiff.y);
                            if (spotDist <= distance)
                            {
                                setOfNoGo.Add(pos);
                            }
                        }
                    }
                }
            }

            Assert.AreEqual(expected, setOfNoGo.Count);
        }
    }

    [TestCase(sample, 56000011, 20)]
    [TestCase(input, 13673971349056, 4000000)] // too low 4712648, 4712650
    public void Part2_Take2(string input, long expected, int size)
    {
        var lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        var sensors = new Vec[lines.Length];
        var beacons = new Vec[lines.Length];

        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            var halves = line.Split(":");

            halves[0] = halves[0].Replace("Sensor at x=", "").Replace(" y=", "");
            var sensorCoordStr = halves[0].Split(",", StringSplitOptions.RemoveEmptyEntries);
            var sensorPos = new Vec
            {
                x = int.Parse(sensorCoordStr[0]),
                y = int.Parse(sensorCoordStr[1]),
            };

            halves[1] = halves[1].Replace("closest beacon is at x=", "").Replace(" y=", "");
            var beaconCoordStr = halves[1].Split(",", StringSplitOptions.RemoveEmptyEntries);
            var beaconPos = new Vec
            {
                x = int.Parse(beaconCoordStr[0]),
                y = int.Parse(beaconCoordStr[1]),
            };
            beacons[i] = beaconPos;
            sensors[i] = sensorPos;
        }
        var found = false;
        var foundPos = new Vec();
        for (var i = 0; i < beacons.Length; i++)
        {
            if (found) break;

            var beaconPos = beacons[i];
            var sensorPos = sensors[i];
            var diff = beaconPos - sensorPos;
            var sensorDist = Math.Abs(diff.x) + Math.Abs(diff.y);

            var ringDist = sensorDist + 1;
            // need to find the ring of positions around this sensor.
            var ring = new HashSet<Vec>();
            for (var r = 0; r < ringDist; r++)
            {

                /*
                 * ...T....
                 * ...#T...
                 * ..###T..
                 * .##x##T.
                 * ..###...
                 * ...#....
                 * ........
                 * 
                 */
                var x = r;
                var y = ringDist - r;
                var r1 = sensorPos + new Vec(x, y);
                var r2 = sensorPos + new Vec(x, -y);
                var r3 = sensorPos + new Vec(-x, y);
                var r4 = sensorPos + new Vec(-x, -y);
                ring.Add(r1);
                ring.Add(r2);
                ring.Add(r3);
                ring.Add(r4);
            }

            foreach (var rPos in ring)
            {
                if (found) break;

                if (rPos.x < 0 || rPos.x > size) continue;
                if (rPos.y < 0 || rPos.y > size) continue;

                var foundInBeacons = false;
                for (var j = 0; j < beacons.Length; j++)
                {
                    var testBeaconPos = beacons[j];
                    var testSensorPos = sensors[j];
                    var testDiff = testBeaconPos - testSensorPos;
                    var testSensorDist = Math.Abs(testDiff.x) + Math.Abs(testDiff.y);

                    var d = rPos - testSensorPos;
                    var dDiff = Math.Abs(d.x) + Math.Abs(d.y);
                    if (dDiff <= testSensorDist)
                    {
                        foundInBeacons = true;
                        break;
                    }
                }

                if (!foundInBeacons)
                {
                    found = true;
                    foundPos = rPos;
                }
            }
            
        }

        var tuning = foundPos.x * 4000000L + foundPos.y;
        Assert.AreEqual(expected, tuning);
        
    }


    #region sample
    public const string sample = @"Sensor at x=2, y=18: closest beacon is at x=-2, y=15
Sensor at x=9, y=16: closest beacon is at x=10, y=16
Sensor at x=13, y=2: closest beacon is at x=15, y=3
Sensor at x=12, y=14: closest beacon is at x=10, y=16
Sensor at x=10, y=20: closest beacon is at x=10, y=16
Sensor at x=14, y=17: closest beacon is at x=10, y=16
Sensor at x=8, y=7: closest beacon is at x=2, y=10
Sensor at x=2, y=0: closest beacon is at x=2, y=10
Sensor at x=0, y=11: closest beacon is at x=2, y=10
Sensor at x=20, y=14: closest beacon is at x=25, y=17
Sensor at x=17, y=20: closest beacon is at x=21, y=22
Sensor at x=16, y=7: closest beacon is at x=15, y=3
Sensor at x=14, y=3: closest beacon is at x=15, y=3
Sensor at x=20, y=1: closest beacon is at x=15, y=3";
    #endregion
    
    
    #region input
    public const string input = @"Sensor at x=1259754, y=1927417: closest beacon is at x=1174860, y=2000000
Sensor at x=698360, y=2921616: closest beacon is at x=1174860, y=2000000
Sensor at x=2800141, y=2204995: closest beacon is at x=3151616, y=2593677
Sensor at x=3257632, y=2621890: closest beacon is at x=3336432, y=2638865
Sensor at x=3162013, y=3094407: closest beacon is at x=3151616, y=2593677
Sensor at x=748228, y=577603: closest beacon is at x=849414, y=-938539
Sensor at x=3624150, y=2952930: closest beacon is at x=3336432, y=2638865
Sensor at x=2961687, y=2430611: closest beacon is at x=3151616, y=2593677
Sensor at x=142293, y=3387807: closest beacon is at x=45169, y=4226343
Sensor at x=3309479, y=1598941: closest beacon is at x=3336432, y=2638865
Sensor at x=1978235, y=3427616: closest beacon is at x=2381454, y=3683743
Sensor at x=23389, y=1732536: closest beacon is at x=1174860, y=2000000
Sensor at x=1223696, y=3954547: closest beacon is at x=2381454, y=3683743
Sensor at x=3827517, y=3561118: closest beacon is at x=4094575, y=3915146
Sensor at x=3027894, y=3644321: closest beacon is at x=2381454, y=3683743
Sensor at x=3523333, y=3939956: closest beacon is at x=4094575, y=3915146
Sensor at x=2661743, y=3988507: closest beacon is at x=2381454, y=3683743
Sensor at x=2352285, y=2877820: closest beacon is at x=2381454, y=3683743
Sensor at x=3214853, y=2572272: closest beacon is at x=3151616, y=2593677
Sensor at x=3956852, y=2504216: closest beacon is at x=3336432, y=2638865
Sensor at x=219724, y=3957089: closest beacon is at x=45169, y=4226343
Sensor at x=1258233, y=2697879: closest beacon is at x=1174860, y=2000000
Sensor at x=3091374, y=215069: closest beacon is at x=4240570, y=610698
Sensor at x=3861053, y=889064: closest beacon is at x=4240570, y=610698
Sensor at x=2085035, y=1733247: closest beacon is at x=1174860, y=2000000";
    #endregion
}