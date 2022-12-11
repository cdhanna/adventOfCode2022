namespace AdventOfCode;

public class Day11
{

  public enum Op
  {
    Add, Mult, Square
  }
  
  public class Monkey
  {
    public int index;
    public Queue<StupidNumber> items;
    public Op op;
    public int opArg;
    public int testArg;
    public int testTrueMonkeyIndex;
    public int testFalseMonkeyIndex;
  }

  public class StupidNumber
  {
    public int[] Divisibilities { get; }
    public Dictionary<int, int> divisibilityNumberToModuloSpace = new Dictionary<int, int>();

    public StupidNumber(int baseline, int[] divisibilities)
    {
      Divisibilities = divisibilities;
      foreach (var x in divisibilities)
      {
        divisibilityNumberToModuloSpace[x] = baseline % x;
      }
    }

    public bool IsDivisibleBy(int n) => divisibilityNumberToModuloSpace[n] == 0;

    public void Add(int offset)
    {
      foreach (var x in Divisibilities)
      {
        divisibilityNumberToModuloSpace[x] = (divisibilityNumberToModuloSpace[x] + offset) % x;
      }
    }
    
    public void Multiply(int number)
    {
      foreach (var x in Divisibilities)
      {
        divisibilityNumberToModuloSpace[x] = (divisibilityNumberToModuloSpace[x] * number) % x;
      }
    }


    public void Square()
    {
      foreach (var x in Divisibilities)
      {
        divisibilityNumberToModuloSpace[x] = (divisibilityNumberToModuloSpace[x] * divisibilityNumberToModuloSpace[x]) % x;
      }
    }
  }

  [Test]
  public void TestMult()
  {
    var n = new StupidNumber(4, new int[] { 3 });
    Assert.IsFalse(n.IsDivisibleBy(3));
    
    n.Multiply(3); // now its 12?
    
    Assert.IsTrue(n.IsDivisibleBy(3));
    
    n.Add(2); // now its 14
    
    Assert.IsFalse(n.IsDivisibleBy(3));

    n.Multiply(4);
    Assert.IsTrue(n.IsDivisibleBy(3));

  }
  
  [Test]
  public void Doop()
  {
    var r = 85;
    var n = new StupidNumber(r, new int[] { 2, 3, 5, 7, 11, 13, 17, 19 });

    foreach (var d in n.Divisibilities)
    {
      Assert.AreEqual(r%d == 0, n.IsDivisibleBy(d));
    }
    
    n.Add(5);
    Assert.IsTrue(n.IsDivisibleBy(5));
    
    n.Add(2);
    Assert.IsFalse(n.IsDivisibleBy(5));
    
    n.Add(3);
    Assert.IsTrue(n.IsDivisibleBy(5));

  }

  [TestCase(sample, 2713310158)]
  [TestCase(input, 32333418600)]
  public void Part2(string input, long expected)
  {
    var lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
    var monkeys = new List<Monkey>();
    
    // first, identify all of the divisibility checks we actually care about...
    var divisibilities = lines.Where(l => l.Contains("  Test: divisible by "))
      .Select(l => l.Substring("  Test: divisible by ".Length))
      .Select(int.Parse)
      .ToArray();
    
    for (var i = 0; i < lines.Length; i += 6)
    {
      var monkey = new Monkey
      {
        index = i / 6,
      };
      monkeys.Add(monkey);
      var startingItemsStr = lines[i + 1];
      var startingItems = startingItemsStr.Substring("  Starting items: ".Length).Split(", ")
        .Select(int.Parse)
        .Select(x => new StupidNumber(x, divisibilities))
        .ToList();
      monkey.items = new Queue<StupidNumber>(startingItems);

      monkey.op = lines[i + 2].Contains('+') ? Op.Add : Op.Mult;
      var opArtStr = lines[i + 2].Substring("  Operation: new = old * ".Length);
      if (!int.TryParse(opArtStr, out monkey.opArg))
      {
        monkey.op = Op.Square;
      }

      monkey.testArg = int.Parse(lines[i + 3].Substring("  Test: divisible by ".Length));
      monkey.testTrueMonkeyIndex = int.Parse(lines[i + 4].Substring("    If true: throw to monkey ".Length));
      monkey.testFalseMonkeyIndex = int.Parse(lines[i + 5].Substring("    If false: throw to monkey ".Length));
    }

    var monkeyLookup = monkeys.ToDictionary(m => m.index);
    var monkeyInspectionCount = monkeys.ToDictionary(m => m.index, m => 0);


    checked
    {
      // now its time to simulate the monkey business....
      for (var round = 0; round < 10000; round++)
      {
        for (var i = 0; i < monkeys.Count; i++)
        {
          var monkey = monkeys[i];
          // Console.WriteLine($"Monkey {monkey.index}");

          while (monkey.items.Count > 0)
          {
            monkeyInspectionCount[monkey.index]++;
            var item = monkey.items.Dequeue();
            // item %= monkey.testArg;
            // Console.WriteLine($"  Monkey inspects an item with with worry level of ${item}.");
            switch (monkey.op)
            {
              case Op.Add:
                item.Add(monkey.opArg);
                break;
              case Op.Mult:
                item.Multiply(monkey.opArg);
                break;
              case Op.Square:
                item.Square();
                break;
            }

            ;
            // Console.WriteLine($"    worry level is now ${nextWorry}.");

            // item.Divide(3); // phew.
            var outcome = item.IsDivisibleBy(monkey.testArg);

            var nextMonkey = outcome ? monkey.testTrueMonkeyIndex : monkey.testFalseMonkeyIndex;

            monkeyLookup[nextMonkey].items.Enqueue(item);
          }

        }
      }
    }
    var highestMonkeyActivity = monkeyInspectionCount.Select(x => (ulong)x.Value)
      .OrderByDescending(x => x)
      .ToList()
      ;
    checked
    {
      ulong monkeyBusiness = highestMonkeyActivity[0] * highestMonkeyActivity[1];
      Assert.AreEqual(expected, monkeyBusiness);
    }
  }
  
  
  // [TestCase(sample, 2713310158)]
  // // [TestCase(input, 10605)]
  // public void Part1(string input, uint expected)
  // {
  //   var lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
  //   var monkeys = new List<Monkey>();
  //   for (var i = 0; i < lines.Length; i += 6)
  //   {
  //     var monkey = new Monkey
  //     {
  //       index = i / 6,
  //     };
  //     monkeys.Add(monkey);
  //     var startingItemsStr = lines[i + 1];
  //     var startingItems = startingItemsStr.Substring("  Starting items: ".Length).Split(", ").Select(uint.Parse).ToList();
  //     monkey.items = new Queue<uint>(startingItems);
  //
  //     monkey.op = lines[i + 2].Contains('+') ? Op.Add : Op.Mult;
  //     var opArtStr = lines[i + 2].Substring("  Operation: new = old * ".Length);
  //     if (!uint.TryParse(opArtStr, out monkey.opArg))
  //     {
  //       monkey.op = Op.Square;
  //     }
  //
  //     monkey.testArg = uint.Parse(lines[i + 3].Substring("  Test: divisible by ".Length));
  //     monkey.testTrueMonkeyIndex = int.Parse(lines[i + 4].Substring("    If true: throw to monkey ".Length));
  //     monkey.testFalseMonkeyIndex = int.Parse(lines[i + 5].Substring("    If false: throw to monkey ".Length));
  //   }
  //
  //   var monkeyLookup = monkeys.ToDictionary(m => m.index);
  //   var monkeyInspectionCount = monkeys.ToDictionary(m => m.index, m => 0L);
  //   
  //   // now its time to simulate the monkey business....
  //   for (var round = 0; round < 20; round++)
  //   {
  //     for (var i = 0; i < monkeys.Count; i++)
  //     {
  //       var monkey = monkeys[i];
  //       while (monkey.items.Count > 0)
  //       {
  //         monkeyInspectionCount[monkey.index]++;
  //         var item = monkey.items.Dequeue();
  //         checked
  //         {
  //           var nextWorry = monkey.op switch
  //           {
  //             Op.Add => item + monkey.opArg,
  //             Op.Mult => item * monkey.opArg,
  //             Op.Square => item * item
  //           };
  //           nextWorry /= 3; // sad beans
  //           var outcome = nextWorry % monkey.testArg == 0;
  //           var nextMonkey = outcome ? monkey.testTrueMonkeyIndex : monkey.testFalseMonkeyIndex;
  //
  //           monkeyLookup[nextMonkey].items.Enqueue(nextWorry);
  //         }
  //       }
  //       
  //     }
  //   }
  //   
  //   Assert.AreEqual(99, monkeyInspectionCount[0]);
  //   
  //   // var highestMonkeyActivity = monkeyInspectionCount.Select(x => x.Value)
  //   //   .OrderByDescending(x => x)
  //   //   .ToList()
  //   //   ;
  //   // long monkeyBusiness = highestMonkeyActivity[0] * highestMonkeyActivity[1];
  //   //
  //   // Assert.AreEqual(expected, monkeyBusiness);
  // }

  #region sample

  public const string sample = @"Monkey 0:
  Starting items: 79, 98
  Operation: new = old * 19
  Test: divisible by 23
    If true: throw to monkey 2
    If false: throw to monkey 3

Monkey 1:
  Starting items: 54, 65, 75, 74
  Operation: new = old + 6
  Test: divisible by 19
    If true: throw to monkey 2
    If false: throw to monkey 0

Monkey 2:
  Starting items: 79, 60, 97
  Operation: new = old * old
  Test: divisible by 13
    If true: throw to monkey 1
    If false: throw to monkey 3

Monkey 3:
  Starting items: 74
  Operation: new = old + 3
  Test: divisible by 17
    If true: throw to monkey 0
    If false: throw to monkey 1";

  #endregion


  #region input

  public const string input = @"Monkey 0:
  Starting items: 85, 79, 63, 72
  Operation: new = old * 17
  Test: divisible by 2
    If true: throw to monkey 2
    If false: throw to monkey 6

Monkey 1:
  Starting items: 53, 94, 65, 81, 93, 73, 57, 92
  Operation: new = old * old
  Test: divisible by 7
    If true: throw to monkey 0
    If false: throw to monkey 2

Monkey 2:
  Starting items: 62, 63
  Operation: new = old + 7
  Test: divisible by 13
    If true: throw to monkey 7
    If false: throw to monkey 6

Monkey 3:
  Starting items: 57, 92, 56
  Operation: new = old + 4
  Test: divisible by 5
    If true: throw to monkey 4
    If false: throw to monkey 5

Monkey 4:
  Starting items: 67
  Operation: new = old + 5
  Test: divisible by 3
    If true: throw to monkey 1
    If false: throw to monkey 5

Monkey 5:
  Starting items: 85, 56, 66, 72, 57, 99
  Operation: new = old + 6
  Test: divisible by 19
    If true: throw to monkey 1
    If false: throw to monkey 0

Monkey 6:
  Starting items: 86, 65, 98, 97, 69
  Operation: new = old * 13
  Test: divisible by 11
    If true: throw to monkey 3
    If false: throw to monkey 7

Monkey 7:
  Starting items: 87, 68, 92, 66, 91, 50, 68
  Operation: new = old + 2
  Test: divisible by 17
    If true: throw to monkey 4
    If false: throw to monkey 3";

  #endregion
}