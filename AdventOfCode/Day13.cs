using System.Diagnostics;

namespace AdventOfCode;

public class Day13
{

    // [DebuggerDisplay("{isValue ? value : 0}")]
    public class Packet
    {
        public bool isValue;
        public int value;
        public List<Packet> children = new List<Packet>();
        public Packet parent;

        public override string ToString()
        {
            if (isValue) return value.ToString();
            return $"[{string.Join(",", children)}]";
        }

        public int Compare(Packet other)
        {
            if (isValue && other.isValue)
            {
                return value.CompareTo(other.value); // TODO; is it a problem that this can output zero?
            } else if (isValue && !other.isValue)
            {
                // convert the current value into an array packet and check it...
                return new Packet
                {
                    isValue = false, children = new List<Packet> { this }
                }.Compare(other);
            } else if (!isValue && other.isValue)
            {
                return Compare(new Packet
                {
                    isValue = false, children = new List<Packet> { other }
                });
            }

            for (var i = 0; i < children.Count; i++)
            {
                var child = children[i];

                if (i >= other.children.Count)
                {
                    return 1;
                }

                var otherChild = other.children[i];
                var childComparison = child.Compare(otherChild);
                if (childComparison != 0)
                {
                    return childComparison;
                }

            }

            if (children.Count < other.children.Count)
            {
                return -1;
            }

            return 0;
        }
    }

    
    
    // [TestCase("[1,1,3,1,1]", "[1,1,5,1,1]", -1)]
    // [TestCase("[[1],[2,3,4]]", "[[1],4]", -1)]
    // [TestCase("[9]", "[[8,7,6]]", 1)]
    // [TestCase("[[4,4],4,4]", "[[4,4],4,4,4]", -1)]
    // [TestCase("[7,7,7,7]", "[7,7,7]", 1)]
    // [TestCase("[]", "[3]", -1)]
    // [TestCase("[[[]]]", "[[]]", 1)]
    // [TestCase("[1,[2,[3,[4,[5,6,7]]]],8,9]", "[1,[2,[3,[4,[5,6,0]]]],8,9]", 1)]
    // [TestCase("[[[[5,2]],[10,10,[0,6,6],4],[[],4,[4,7,7,10]],2,[[2,1,4,8],[],[7,4,2],[7,0,8]]]]", "[[[9,0,6],[0],9,[[],5],[3,10,0,[0,5]]],[[],[[1],[],[3,8]],8]]", -1)]
    // [TestCase("[[[[],[0,2,4]],7,5,0]]", "[[[],8,[4,2,0,[7,9,9]]],[[[]],[[2,0,6,4,7],[5]],[[10,5],[5,7,7]],9],[7,5,[10,7,7,[9,7]],[[]],[[1],10,9,[9,1],[5,1]]],[[[],[9,10],7],3],[]]", 1)]
    // [TestCase("[[7,8,6,10],[8,[]]]", "[[7,[8],10],[5,4,[]]]", 1)]
    [TestCase("[2,2,2]", "[3,3]", -1)]
    public void TestCompare(string left, string right, int expected)
    {
        var value = Parse(left).Compare(Parse(right));
        Assert.AreEqual(expected, value);
    }

    [TestCase("[[1],[2,3,4]]")]
    [TestCase("[[[[5,2]],[10,10,[0,6,6],4],[[],4,[4,7,7,10]],2,[[2,1,4,8],[],[7,4,2],[7,0,8]]]]")]
    public void TestParse(string input)
    {
        var packet = Parse(input);
    }
    
    Packet Parse(string str)
    {
        Packet root = null, curr = null;
        var startIndexes = new Stack<int>();
        var buffer = "";
            
        for (var j = 0; j < str.Length; j++)
        {
            var lc = str[j];
            switch (lc)
            {
                case '[':
                    // start of a list! 
                   
                    var sub = new Packet { isValue = false, parent = curr };
                    if (curr != null)
                    {
                        curr.children.Add(sub);
                    }
                    curr = sub;
                    
                    if (root == null)
                    {
                        root = curr;
                    }
                    startIndexes.Push(j);
                    break;
                case ']':
                    // end of a list
                    var end = j;
                    var start = startIndexes.Pop();
                    
                    if (int.TryParse(buffer, out var n2))
                    {
                        curr.children.Add(new Packet
                        {
                            isValue = true,
                            value = n2
                        });
                    }

                    buffer = "";
                    
                    // var numbers = buffer.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                    // var numberChildren = numbers.Select(n => new Packet { isValue = true, value = n }).ToList();
                    // curr.children = numberChildren;
                    curr = curr.parent;
                    //
                    // buffer = "";
                    
                    break;
                case ',':
                    // the end of _some_ value... Either numbers, or a list
                    if (int.TryParse(buffer, out var n))
                    {
                        curr.children.Add(new Packet
                        {
                            isValue = true,
                            value = n
                        });
                    }
                    else
                    {
                        
                    }

                    buffer = "";
                    break;
                default:
                    
                    buffer += lc;
                    break;
            }
        }

        return root;
    }

    [TestCase(sample, 140)]
    [TestCase(input, 23520)]
    public void Part2(string input, int expected)
    {
        var lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        var packets = lines.Select(Parse).ToList();

        var decoderA = Parse("[[2]]");
        var decoderB = Parse("[[6]]");
        packets.Add(decoderA);
        packets.Add(decoderB);
        
        packets.Sort((a, b) => a.Compare(b));

        var displayHelper = packets.Select(x => x.ToString()).ToList();
        var aIndex = packets.IndexOf(decoderA);
        var bIndex = packets.IndexOf(decoderB);
        var product = (aIndex+1) * (bIndex+1);
        Assert.AreEqual(expected, product);
    }
    
    [TestCase(sample, 13)]
    [TestCase(input, 13)] // 6373 is too high, 3980 is too low
    public void Part1(string input, int expected)
    {
        var lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);


        var pairs = new List<(Packet, Packet)>();
        for (var i = 0; i < lines.Length; i += 2)
        {
            var leftStr = lines[i];
            var rightStr = lines[i + 1];
            pairs.Add((Parse(leftStr), Parse(rightStr)));
        }

        var sum = 0;
        for (var i = 0; i < pairs.Count; i++)
        {
            var (left, right) = pairs[i];

            if (left.Compare(right) == -1)
            {
                sum += (i + 1);
            }
        }
        Assert.AreEqual(expected, sum);
    }
    
    #region sample
    public const string sample = @"[1,1,3,1,1]
[1,1,5,1,1]

[[1],[2,3,4]]
[[1],4]

[9]
[[8,7,6]]

[[4,4],4,4]
[[4,4],4,4,4]

[7,7,7,7]
[7,7,7]

[]
[3]

[[[]]]
[[]]


[1,[2,[3,[4,[5,6,7]]]],8,9]
[1,[2,[3,[4,[5,6,0]]]],8,9]
";
    #endregion
    
    #region input
    public const string input = @"
[[[[],[0,2,4]],7,5,0]]
[[[],8,[4,2,0,[7,9,9]]],[[[]],[[2,0,6,4,7],[5]],[[10,5],[5,7,7]],9],[7,5,[10,7,7,[9,7]],[[]],[[1],10,9,[9,1],[5,1]]],[[[],[9,10],7],3],[]]

[[1,[[4,6,1,8,3]]]]
[[9,[[],10,[6,5,4,1,5],1],[3],[7,3,7,10,0],[[9,6],[1],0,[]]],[0,2,2],[[[2,7,4,7,6]]],[],[[[4,10,1],4,7,[],[4,6]]]]

[[[7,[10,8],1,2],[[8,6,8,0,5],1,[4,5]],1,4,4],[0,[],[],[],[8,2,[],[4],[10,8]]]]
[[3,7,[2,0,[9,2,1,10,8],0],[3,9,[9,3],[1],[2,5,5,6,10]]],[[[10,1,8,8],[1,2,8,3,6]]],[]]

[[[]],[0,[0,[4,3]],4,[10]],[3,9,9]]
[[[9,[1,5,0],[10]],[[9,1],[9,7,3,4,5],[],[0,0,3,1,2],2],6],[[[5,7,0,4,0],[9],[],[4,1,0],4],6],[]]

[[7,8,6,10],[8,[]]]
[[7,[8],10],[5,4,[]]]

[[6,1,[]],[1,0,10,[0]]]
[[[[3,9,5],[4,0,3],[1,2,0],0],9],[[],[[0,0,5]]],[2,[[6,3,4],[10,9]],3,4,8],[]]

[[[[5,2]],[10,10,[0,6,6],4],[[],4,[4,7,7,10]],2,[[2,1,4,8],[],[7,4,2],[7,0,8]]]]
[[[9,0,6],[0],9,[[],5],[3,10,0,[0,5]]],[[],[[1],[],[3,8]],8]]

[[],[[9,0,4],10,2,6,[[6,10]]]]
[[[0,[],10,2],[[],[],[7,7,3,6],[9,2,6,7],[9,9,4,2,8]],[[]],[[3],[4,0,1],4,10,1],[[7,9],4,[7,0]]],[[[2,3,3],[4,2,4],[4,5,5,6,4],[6,5,8,3,8]],[5,8],[],9,1]]

[[]]
[[6,6,6,[[0,1,7,10],8,9]],[],[[[8],[],[],2],4,6,5]]

[[],[1,[7]]]
[[2,4,6,[[4]],0]]

[[[1,[],0],[]],[8,[[3,5,3],[0,10,1],[7,7,10],1],[],[[8,4,10],[7,2,4],3,3]],[8,9,3],[5,0,3,6],[0,[[0,0,4,3,2],6,5,[]]]]
[[1,5,[9,5,[4,0,6,3]]],[[10,[4,3,6],1,[6],[1,1,8]],7,[[]],[[10,1,2,4,5],[4,2],[2]]],[[[6,2,9,9,0],7],10,[]],[],[10,[[7,5,7],[5,6,7,4,6],[10,1],[7],[2,2,2,9]]]]

[[[],[[2,10,7,2,0],9,8],3,[[]],[9]],[4,[0,10,4,2]]]
[[[[9],8,[6,0],[7,8]],[10,0,6,0],[6],[4,0,[5,8,6]]],[],[],[[[8,10],[6],[10,6]],2,[5,2,4,[1],[7,2,2,6]],[1,1,[2],0],[0,[],0,[6],[]]],[8,[[],[2,2,2,6],[],[9,4,10,5,8]],8,2]]

[[2,[[0],9],[]]]
[[[[10,5,3,9],6,[10,4,10,3],3,[0,1,4,2]],5,[9,0],3,[9]],[5,10,6,5]]

[[1,[0,6,[],3,[6]],1],[1,[[0],8,1,[1,1,4],[5,1,0]]],[[[],7,[5,6,2,0],[1,8,10,9,0]]],[8,[5,3,0],[4,[0,2],[10,10,9,8],[0,9]],0]]
[[9,3,[[9],[0,1,8],[1,8,6,8],[8,7,2,6]],[6]],[[2,5,3,[3,3,9,6]],[[7,3,10],[2],[],4],5],[[[5,9,2,10],[7,0,7,3],2,[6]],9,4,[[2,10,2,1,0],4,9,[6],[3,0,6,5]],[8]]]

[[10,3,[2,5,9],7,8],[[0,[]]],[[3]],[1,6,[],3,[1,1,6,[1]]],[5]]
[[8,[],[[0,7,8,2],1,[]],[],[3,[6],[3,6,6,4,7],[2,3,5,9,0],[6,3,8,1]]],[[10,8,3,3,[2,4,3]],[[5,8,0],10]]]

[[10,[[]],2,[[6]],1],[[],3,8,7],[4],[8,[[8,9,6],[]],[[0],7,[3,7,7,0,6],[4,4,3],[7,1,3,2,2]],[10,6,10],1]]
[[],[8,[[7],[8,3,5]],[],[7]],[10,[[10,7],[]],[[],[1,0],[0],9]]]

[[[[0,8],2],[2,[],[7,3],9,3],8,[2,2,[5,8],4,[9,1]]],[],[],[[2,8],9,[0,7,[3,0,5],1,4],8]]
[[5,[],1,8],[[7,1,[8,10,7,8]],3,[8,7,4],[10,10,2,6]]]

[[10,9,[2],[10],9],[4,4,5,3,[[3,7]]],[8,8]]
[[2,[]],[],[[[7],9,5]]]

[[4,[[1,3,0,6],[7],6],2,[[10,10,10,9,5]]],[7,8,[],0,8]]
[[[]],[[3,[0,9],2,[2,8],[7,3,5]]],[8,[2,[],[7,1,4],1,[2,7,1,7,2]]],[10,[],5]]

[[[7,[10,5],8],[5],3],[[[],[7,0,9,1],[6,7,7,6,2],2,8],5],[],[],[[10,[6,0,3,4,0],[1,7],7,[10,4,7,4,8]],4,3,[]]]
[[],[7,1,4,6,[[6,3,8,5,6]]]]

[[[2,7,[],[2,8,9],[0]],[0,[],[]],[1,3,[3,7,5,9],[6,9]],[[4],[0,9]]],[4,3,[4,[10,6,10,3],[9,2,0]]]]
[[],[[],[6,[1,7,4],5,[0,9,6,6,3],4],4,8,[6,10]],[2,[[],[],0,[],[5,5,3,4]],[[9,6,10,10,9],[6,7,1,8,6],[7,1,2,4]],3,2]]

[[[],7,[[],6,[5,2]],7],[],[],[]]
[[],[[[4],9,[1],9,0],6,7,10],[9,7,[8,[10,5,1,5,1],[10,6,0,6,10]]],[[[3],[7,9],[9,10,6,5],[3,5,3],9],[[6,2,2,2],8,4,[7]],[[2],1,9],[[8,3],1],[[0,9,0],[8,6,2],[10]]]]

[[[[3,0,8,8],1],8,9],[5,10,10,[],[7,[9]]],[[[0,9,0,1]],4,[[7,6,4,7],2,2,0],[4,[0,0]]],[[2]],[1]]
[[1,8,3]]

[[[[]],[[0,4],8,3,1,[4,8,1,6]],[[4],4,0],[[]]]]
[[9,[[10,0,1,7,4],6],7,1],[5,9],[6,0,1,[[5,10]]],[1]]

[[[],5,[[0],1]],[[3,[8,8,3,7,4]],8,0],[8,[[0,5,2],4],1,[3,9,[7,0,10]]],[0],[[[6],[8,5,7,9]],[[10,8,3,8],[10,8,7,4]]]]
[[[[2,3,4,8],[]],[8,[7],[0,1,3],[10,4,0,9]],7,3],[3,9,[]]]

[[],[9,2,5,[],0]]
[[7,[],9],[]]

[[[],[[0],[7,5,5,3],[],9,8],[[3],[10,9,3],1]],[[3]],[[8,3,[7,2,10,5]],6,4,1,10],[1,[[6],[],[10,5,3]],1]]
[[1,[[9,1,1,5],7],7,[],[3,8,[6,3,9,9]]]]

[[[[9,9,3,5,3],6]],[2,[4,[8,1,5,7]],4],[7],[[8]],[1]]
[[[[3,3,0,0],[8],8,5,0]],[4,2],[8,[[5,1,3,2,9],[1,10,9,8],[9,7]],[[9,10],5],[[],[10,0,3,4]]],[[9,[1,7],8,[5,0,2,1,8]],[2,[7,6]]]]

[[9,2,1,9,[[4,10,8,1],[0,8,5],[5]]],[],[[[1,0,8,8,0],10,5,[]],6,8],[[2,7,[7,0,9,4,7],3]]]
[[4,[[8,7,9],5,2],[7,3,[6,5,5],6],6],[[[10,2]],9,[[],4],9,9],[[[3,4],7,10,[1,3],10],8,9,6,[[7,4,3],[3,6]]],[6,[7,0,[7,4],[]],9,1]]

[[],[[[7,3,3,5],5,9],[10,[2],[],1]],[6,[0]],[[[8,10,6,9],3],1],[5,3]]
[[1,[[2,7,0],[4,10,9,7]],[[3,7,9]],6],[10,3,6,3]]

[[[3],[4],[8,[],10]]]
[[[[7,0,8,6,2]],[9,[8,6],6,5,[9]],6],[5,[[4,2],4,[],[2]],[[5,3,0,2,9],4,[10,7,8]],7,10],[],[10,9],[[10,[4],0],[]]]

[[[],[7,[5,0,8,0,0],6,[2,10,2,4]],0,[6],[[10,10,9]]],[[[9,1,3,1],10],[[4,6]],[[0],[5,1,5,6],[3],[5],[9]]],[[8,0,[7,4]],10],[6,0,[5,[],[5,4,5]]],[[9],1,5]]
[[[[],2],[0,3]],[[6,[6,1,2,3,2]],0,3,[6]],[[9,[10],8,[3,7,10,8]],[],10,5,8],[],[[0,[8]],5,[[],[0,0,1],3],[[5,4,8,10,6],[6,1,4,0],[0],[2,1,8,4,10]]]]

[[6],[[[2,7,7],[],[1,3,4]],2,6],[[0,[6,3,5,7],[10]],[5,[3,3,10,6,2],8,[0,2,3]]],[[6,10,[2,7]],[5,2,3,0],[[5,2,2],[5,7,5]],[]],[6]]
[[9,[[4,10,1,3],[7],[7],7],[8],[[8,2,2],6,2,[],[6,5]],[2,[3,2],[2,3],[],3]],[2],[[[2,3,0,2,8],5],[5]],[0]]

[[],[[[8,5]],[8,[],5,[6,6,10,0,9]],0,3,1],[]]
[[9,4],[],[7,6,[7,3],[[4,7,4,8],[7,9],5]],[],[]]

[[1,1,7],[],[[[1,5,10,2,5],[8,8,3],[9,8,0,10],[4],4],9]]
[[1],[],[9,[1,2,10,4],3],[10,9,10,[3,[1,5,6],9]],[5,6]]

[[[[4,7],[6],1,[1,7]],7,0],[[[3,1,8,9],3,1,[10]],8],[5,6,5,[7],0],[[0],[10],[[1,6,6,10,6]],[[7,3],2]],[[]]]
[[10,8,[8,[2,2,1,6,3],2],[[9,4,5,10]],[[2,5,10,6],6,0,6]],[[[1,8,1],[9,5,6],2],[[5,8,2,5]],2],[[1],9,[[8,0,7],4],1,8]]

[[5,[1,8,7],9,1]]
[[],[[4,6,[7,9],0,4],[9,9,[9,0,2,2]],8,5]]

[[7,[3,0,[2]],6,4,10]]
[[],[[0,[6,9],5,0]],[],[[[5,4,4],[],[],4],[[1,5,9,6,7],3,[2,3,9,1,5],3]]]

[[],[2,6,[8,[7]],2],[3,0]]
[[[[],5],3,[],[[10,5,4,2,5],[10]],6],[10,0,[],[[9,3]],3],[[[4,6,0],4,[]],[[9,7,9,7],[9,4,3,4,7],[8]]],[6,0,5,[[4,0,2],7,8,[],3]],[]]

[[7,[8,3,[],[10,6,4]]]]
[[[[0,6],9],8,6],[10,6,[7,[9]],5,[[1,1,2]]],[4,[[1,5,2,3,0]],[0],[6,[2,2,7,9],2,3,7],4]]

[[6,2,4],[7,[[]]],[4,[[],[1,4,2,0],[0,5,0,2]],8],[],[0,[[5,5,4,2]],1,[7,3,2,[],5],[7,[8],1,8,[3]]]]
[[[[7,3,1,1,6],[4,1,0,0],2,0,[8,6,2,1,9]],8,5,3,[9,4,9]],[[0,[10],[2,9,3],5],[7,[3,7,7,10,0]],0,10],[[8,[9]],9,[10,6,2],[1]],[]]

[[8,10,10,[5,[2,2,1,2],[],[3,10]]]]
[[8],[1,[[7]],4],[3],[[7,[4,1,10,3,5]],2],[0,[1],7]]

[[8,0],[2,[[1,9,10,7,9],7,[2,3],[5,5,5]],1]]
[[[10,10,10],4,3,8],[1,[],[5,6,3,[7,10]],9,[[2,6,4,1,1],[2,10,5,10],[4,9],[6,2]]]]

[[[3],8,3,[[7],7,2],0],[7]]
[[[[3],7,[1,3,5,1],[1,1],[4,3,3,4]],1,[],7,[[1,9,10],[],[4]]],[1,[[6],[]],[[],[0]],4],[[],[]],[3,[[],4,1],9]]

[[],[]]
[[8,2,[[9,2,8],[9,5,0,0,7],7,7]]]

[[[0,[6]],[[9],[9],10,9,[]],[2,[9],9,[],10],8,4],[],[],[]]
[[4,[]]]

[[[],[]],[3,9],[2],[2,2,7]]
[[[7,8,[7],7,[3,8,5]],[]],[[[],[6,10,7,8]]],[[2],8]]

[[[[6]],[1,[1,8,4],1],[[3,2,8,5,0],10],3,[[10],1,[10,8,5,0],7,[2,10,5]]],[9],[[1,2,[7,4,7,4,5],7,[1,8,1,6,5]],[[4,10,1],[1,1,0],[6],4]],[6],[8,[10],5]]
[[0],[4,7,[[2,5,4],[],[3,3,8,5,8],7,[9,1,4]]],[4,[[],[9,4],6],5],[[],[[8],3,2,[5,3]]]]

[[3,8,8]]
[[4,[3],[10]],[5,[2,[7,5,10,2],10]],[8,[4],[]],[[9]]]

[[[[3,0,2],[7,8]],[[],10,3],[9,[8],[1]]]]
[[4,[[],4],2],[5,4],[1],[[7,[8,3,0,6,6]],[[6]],4,[9,1,10,[],7],6]]

[[[9,[8],[10,2]],[5,[0],2]]]
[[[[9,10,1,8,8],10,0,[8,9,7,9]],[[10,1],2]],[0,[[7,8],1,[]],[[5,8],[5,8,8,9],[4,2,9,4,8],9,[]]],[[1,5,[2,4,5,5,10],[10,3,3,8]]]]

[[[],10,[[6,9,7,2,1],9,0]],[[6,[4,2]],[]],[]]
[[[[2,2,10,1]],7,[[0],8,10],[[1,4,0,10,2],[4,0,8],0],[1]],[8,3,[[1,4,1,6]],[7,1,[4]],8],[7],[[1,[],10,[8,8,6,0,1],[]],9,3],[[1,[2,5,7,5]]]]

[[[2,[4,5]]],[],[[[4,3,7],[8,0,5],10,4]],[4],[[[7,1],9,3,[6,5,1,1,9],6],5,6,[3],[4,7,1]]]
[[[3,3],[[8,2,3],1,[3,5,10,7,0],9,[6]],[4,2]],[2,2,[9,[1,1],[7,8,2,2],[3,9,10]],4]]

[[[[6,2,1,2],9,5,[8,2],[2,2,0,3,7]],[[],2,[4],10]],[],[8,8,[2,6,4,[1,3]],[6,[10,8,10],6,[2,9,4]]]]
[[[],4,0,0],[[5,[6,6,7],5,9,[0,1,9,3]],[]],[],[],[[[2],[10,3,10,4],7,[8,10,2,10,9],[1,7]],2,[8]]]

[[4],[3,[[3,0],1,[3],[]]]]
[[3,10,4,10],[[[],5,[],[8,7,6]],4,[[3]],[[6],9,6],[[5,8,7,8],3,[4]]],[5,[],4,[8],[3,[3,10,7,5,7],2,[]]],[[[10,7],5,10,10,[]],[]],[[1,8,0,0,4],9]]

[[[7,2],7,2]]
[[9,9],[5,[[9,1]],[[],[5,7,4],9,[3,3,9,3]],9]]

[[],[[[9,1],1,9,2],9,[],10,[[7],[4,0,9,6,7]]],[[[],3,[0],10],[[4,0,8,9,4],[5,7,9,0]],[[5],[0,10,6],[5]],[[0],[3,1],[],[0,9,8,1]],[[],[7],[0]]]]
[[4,[[8],2,[8,0]],8,[9],[[7,0,0,4,0],[1,1,6],4]]]

[[[0,[3,1,0],5]],[5,0,[1,[8,6,7]],[[7,8,5,7,4],[8,5,6,1],0,[8,7],[1,10,8]],[2,[2,6,2,6],6,4,3]]]
[[],[[[2],[5,2,5],7],[3,[],[0],2,[3]],[[9,8,3],[],1,[0,7,1,4,6],3],[4,[0,10],[6,7,9,7,10],[9,8,0,3,9]]]]

[[[[8,10,4,2,2],5,[4,9,4,2],1],[[2],[3,6,1]],2,[0,1]]]
[[8,[[10,7,10,1,1]]],[],[],[4,8,10,[0],[]],[[[6,5],[5,5,4,6,2],[9]],10,6,[[3,7],9,[3,1,6,4],[2,4]]]]

[[[[4,0,9,8,0],9,[4,10],[8,8],[1]],[[]],[9,[0,1],[]]],[]]
[[[],9,5,8,0]]

[[5,5],[0,[3,4,[6,7,6]]]]
[[],[],[[6,[],9],[[3,2,7,1,10],[10,3,5,2,7],2],[]],[[[6,0,5,2,7],[9,9]],0,8,[[0],[],[6,6,8],1],5],[[7,[5,8,8],[5,7,3],10,[1]],2]]

[[2,[7,[9],5],[]],[[[3,8],[5,10,3,8,1]],[6,0,0],1,10],[],[4,7,3,5]]
[[],[6,6,8,8],[1,[[],1,1,0,[4,5,2]],0,4,[]],[9,5,1,[8,3,[3,1,5,3,8],[2,6]],10]]

[[4,0],[],[6,1,6,8],[[6]],[[10,[],[6],7,[]],9]]
[[[3]],[[[],7,0,[4]],0],[[]],[]]

[[9,6,5]]
[[1,8,3,2,[5,[10,1,3,3],[3,7]]],[0,[[6,4,6,9],8,2],[],[1,[1,4,8,7,10]]],[[[6,2,2],[5,8,3,0,10],[],8],7,7,[8,2]],[[8],[],[],[[]]],[[[3,10,2,4,2],5,10,[1,4,10,9]],1]]

[[9,[3,2],[]],[],[[],8],[[[6,2],4,9,4],5]]
[[[[3,3,7,8,1],[]]],[4,2],[]]

[[5,[[3,5,6,2,7],10,0,[2,3,1]]],[6,3,[[7,2,0],[],[],8,0],[[2,5,0,0],[1,8,8,9,7],[0],[],0]],[],[10,[[0,2,0,6,3],2,[],8,7],[[4],[4],[5,2]]]]
[[[[0,10,6,5],[],[1,7,7,4,6],[2,2,10],[10,4,0]]]]

[[],[[[],4,[9],[3,2]],[1,[0,0,6,4,2]],10],[[1],0,[9,9,[9,2,8,10,5]]],[]]
[[[[2,6,3],[9,0,10],0,[9,5]],0,5,2,[0,5]]]

[[],[[[5,3,9],[1,4,2,8,5],7],8],[[[6],9,5],3,[],2,3],[0,4],[7,2]]
[[[7,9,[0,2,2,5,9]],6,5,[8,[5,7,7]],10],[9,5],[8,4,6,5,[2]],[[[2,10,0],6,9,[]],3,[6,[1,4,4,5,4],[2,8],[],0],[0,3,7,10,[3,3,1,3,9]],7]]

[[8,10,[[5,9,3,3],5,[8,10,8,9]],4,6],[],[],[[[4,4],7,[4,9,3,10]]],[]]
[[[[10,5,6,5],1,6,4,10],10,8,[8,1],5]]

[[],[[],6,0,[]]]
[[2,[[3,4,6,2,2],[],6],7],[7],[],[[[],[3,8,3,1,7],[3]],8,10,9]]

[[],[1,[[1,2,8,4],6,1,[9,1]],[[6,0,5]],[[8,6,3],3,6,[8,0,9,5],7]],[[],9]]
[[3,[[1,3,0],0,[0],3,[1,1,6,10]],[],[1,7,[2,1,8,1]],1]]

[[6,2,3],[5,5,[[7,4,2,10,7],0,6,[3,2,9,3,0]]],[[9,[8,9,5],[8,2,0,0],7,[]]],[[[],4]]]
[[],[[9,2,[0,2,9],10,[]],10,[3,9],4],[0],[9,1,[[3,7,6,10,7],[7,7,5],[4,3],10]],[[[5,3,0,2],10,[]],[[3],10],2,9]]

[[2,[4,[10],3,[],5],6,[[5,4,1,5],[4,0,7],[5]]],[[[8,9,3,6,5],[1,6],[9,0,7],1]],[2,[7,8,[0,10,6,0,0]],[8,[],[1,10],7,[]]],[9],[5,0]]
[[3,[7,4,10,9,[7,10,2]]],[[[0,8,4],[10],2,[1,5,9,10,8],4]],[[4,5,[4,10],[2,7,0,0]]]]

[[[[],5,[5,7]]]]
[[[[5,3,3,10,4],[10],6,6,4],[5,[2]]]]

[[[[0,0,6],3,[6]]],[],[8,8,[[],[8,3],1,[5,0,2,4]],[1,9],[10,[],[6]]],[3,6]]
[[],[[7,10,2,1,2],[[8,5,5],[],[],[10,7,6]],8,2],[4,6,[[]]]]

[[2,10,8,10],[9],[[[6],[2,2,2,10,4],4]],[[[3,2]],3,1,[6,3,[10,6,7],10,8],[10,[8,9,2,9]]],[[5],[4,[7,5,6,3],2],[5,8,1,[],[1,1]]]]
[[9,6,[[8,6,4,6,2]],[[0,6,10],[]]]]

[[[[10,0,6,1,2],[7,8,2,0,0]],3,5,[[10,9],[8]],1],[9,[[6,6],10,3,2]],[0],[2,[1,3],9,7,[[0],[1,4,4],4]]]
[[[[8,6,5,2],[1,6,3,1]],2,[9,6]],[],[[6,5,6,1,[10,1,7,4]],9,0,4,[[6,8],0,9,9,[6,6,9]]]]

[[1,[[],[3,4,5],5,[],[0,1,4,8]],[[0,5,3],[5,7,8,7,0],5],6,7],[2,9,[[0],[10,9,8,2]],10,4],[]]
[[],[[],[6,[2,6,8,0,2],2]],[[[3],[],[10,8,1,0,10],5],[3,[0,0,5],6,[9,8],[5]]]]

[[2,0,9],[5,2]]
[[[7,[7,7,3]],4,[],1],[0,[[6,4],8,[9,6,0],[2]],3,[1,[5,9],9,[]]],[[[5,7],8],[[6,6,5],[],9],2],[[],1]]

[[[8,[9]],[1,8,[5,0,1,4]]]]
[[[],[6,[9,3,1,9],[4,0,8],8,9],[[9,4,7,4,9],[],3,[0,8,10,2,7]]],[3]]

[[3,[2],[9,[8,4,5,7],0,10],[],[[9],2]],[0,0,[[0]]],[0,9,7,[],[[0],0,[4]]],[[5,[3,8,6,1,7],[3,0]]],[[[4],[]]]]
[[2,[1],9],[],[6,4,[9,9,0],3,[]],[[[4],7],2,[[7,8,5],4,[6,4,4,2],8,[10,3,7,1,6]],2,[5]]]

[[[[3],[6,3],7],[7],[[7],[6,3,6,1],[7,3],[7,4,4]]],[10,10,2,[0,4,[3,1,8,10],0],[[9,4],[8,4,9,10,8],6,[],8]],[3,[0]]]
[[[],10,6,6,3]]

[[[1,9,[5,3]],6],[[[7,0,3,3,6],4,4,7,[7]],3,6,3]]
[[[[4,4,7],0,7,[10,4,3]],[3,[6,10,5,9,8],[],3],6,0],[[2],9],[[[8,1,0],[1],9,[4]]]]

[[],[[[4],[5,3,8,5,8]],2],[9,6,10],[[[],0,2,[9,0,7,2]],[9,8],[],10,5],[5,[3]]]
[[],[[[1,9,10],0]]]

[[7],[],[1],[10,2,9,[]]]
[[10,2,[[9],1]],[0,[[0,10,10],0],2,3],[9,3],[5,2,[[1,3,7,2,2]]]]

[[[7,8,7]],[4,9,7],[2,[[6,8,2],2,3],3],[]]
[[6,[]],[[[2],[0,2,3,0],1,4,8]],[[9,4,[1,1],10],[6,7],9]]

[[1,10,[[5],2,[3]],[[1],4]],[[3,[5,1],[4]],[7,2,[10,7,0],[2,2,9,7],[6,6]],3,10,[9,[5,8],6]],[],[[4,7,10,7],4,9,[0,[7,4,7,1],5,1,1],10],[1,10,0]]
[]

[[6,1],[],[[8,10],[],5,5]]
[[],[[[4,9,6,0],[3,6,8]],4,10],[[],1,[10,4,[3,10],3,[5]],9,8],[[],10,[[9,1]],9],[4]]

[[[],2,1,[8,9],[1,4,0,[2,5],[]]],[4,[2,5,[4,4,2,9,8]]],[4,7,6],[[[9,9,8],7],4],[[10,[4,4,4],[10,10,9,8],5,3]]]
[[],[0,[[7,6,2,3,3],5],2,[],[[]]],[[2,6],[1,4],[5,8,9,4,[5,1,9,4]]],[]]

[[[],[[0,8],0,[3,7],[0,4]],5,3],[1,5,7,[7,[4,9,9],10,2]],[0,5,[]],[]]
[[[[3,3,10,5],[]],[0,1,7,[2,5,1,0],[0,9,8,2]]],[[3],[[9,6,3,10,4],7,[7],6],[[6,0]],[1,[7,2]]],[1,[[4,6,0,7],9,10,[2,8,4,9,2],[7,3,4]],[[7,9,1],3,0,4,[]],[]],[],[9,5]]

[[6,[8,5,[4,2,6,0,10],[8,9,5,10],[4,9,6]],0],[[6],9,8,0],[[8,[5,3],[5,0,9,6,3],6],[[2,0]]]]
[[3,[],7,8]]

[[6],[9],[5,1],[1,[[0,2,1],7],0,[4,0,2],1],[[10]]]
[[[0,0,9,[1,4,2,2]],5,0,4],[[[9,9,4,0,7]],7,0,10]]

[[10,9,2],[7,7,9,[3,2]],[7,[6,8],8]]
[[[1,3,8],5,[[4,7],[4,5,0],[],[5,2]],5],[7,[[2,0,0,10,6],8,[9],6],[],1,10],[[]],[[[4,10],[5],[8,6,3,10],6,[10,0,0]],0,[9],[[2,8,0],[0],5,[10,8,2,10]]]]

[[4,8,1],[],[8]]
[[1,[2,4],9,[]],[[[9,2,1,4,2]],[],[[],[]],2,[]],[],[],[[]]]

[[[],[[2,5,10],[2,9,3,6,8],6],[[]]],[9],[10,3,2,[[],2]],[[9,[10,5,9,7],[9]],9,[[8],7,2,[6,4],[6,8,8,0]],[8,4,[],10],[0,[9,0,7,9,8],0,[5,0,5,4,10],1]],[[1,10,3,3,3],[1,0,1,[]],[9,8,[9,8,7,2,9],8],7]]
[[10,1]]

[[[],[],[[0],[2],5,[],1],[],[]],[[],[],3],[[[5,10,8],3,2,4,[0,8,0]],1,[5,5,[10]],[],[8,[5,10],4]],[8,7,0,2],[1]]
[[8],[]]

[[10,5,[[7]],[0],3],[[],1,2,8],[[7]]]
[[7]]

[[[[10,9,2,4],[10]],[[],[],2,[0,7]],1,[1,7]]]
[[[8,9,[9],[6,1,10,6,8],[3,10,3]],9,[[4,6,5],3,[8,9],10,3],[10,1,10,1]],[4,[[8,5,10,2,7],8,[9,6,0],[10,3,5,1]],[[3],0,1,[4],[7,8]]],[5,[[8,1,2,7,7],[10,9,0,9,5],8,[0,4],[7,7,8,0,5]]],[1,4,3,[],[[1,8,10,10],1,[1,5,7],[9,6,10,7],2]],[[3,[],[0,0,6,9]],6,3,[[],10],[[7,6],[1,2],8]]]

[[[[8,3,5],5],[6,4],[4,1]],[[],4],[],[4,7,2,[[9,9],[]],[]]]
[[[[],[2,8,7,5],[8,2,6,5]],[[],[],10,5,[10]]]]

[[0,4,[4,[3,4]],4],[[]],[[6,10,[8,1],10,[6,6,7,10]],[[8,10],[1,1,7]],[3,[6],1,7],10,[[4,10]]],[5,7,2,[8,9,[3,4,10],2,[]]]]
[[[[1],[5,0,8,5,1],4,[]],[6,10,[7,2,1],[1,10,5],0],9,2,6],[7,9,[2],5,6],[0,[[2],7,8],8],[[[],[],[1,9,9],4,[]],[8,9,8],7]]

[[[[0,6,5,8],[5,5,3,10],0,[9],[9,0,3,0,1]],6,10,4,[[6,2],[10],[4,4,10]]],[10,10,10,10]]
[[10,10,0],[[[5],8],[[2],[8,8],1,[0,3,10,6],[4,6,2]],[9,6,0],1],[],[[[],[9,2,0,9]],3,[9,[0,5],7],[[],1]],[[[10,1,7,8],[5],[1,6,7,9],6],6,2,2,[[7,4,5,0,8],0,4,8]]]

[[[],8,8,[]],[10,[10,[8,7],1,[5,9,9,1,7],4],5],[1,8,[5,1,[9,7,10,5],7],[[6,7],[8],[9],0,6],4]]
[[[]],[[8,4],7,3,[]],[[],10,[5,[1],[8,3,2,1]]],[[[2,3,10,2]],4,1,4],[9,2]]

[[8,[6]],[],[],[],[2,1]]
[[3],[[9,[2],5,3,[]]],[8,3,[[5,5,9],[]]],[[3,4,[10],[2]],8],[3]]

[[[[8,7,7],[9,5,10,7]],[],[],5],[7,[9,[1]],2,6],[[]],[[6,1],[6,2]],[]]
[[9,2,0,[[6],4],[6,[1],[6,8,10,5],9,4]]]

[[5,[[6,4,7],4,[],[1,10]]],[[2],[[0,7],[10],[6,7,1,3,5],[7]],9,6,7],[9,9,2],[[1,[3,10,5,0,3],7,3],[7],4,[[5,8],[2]],[0,6]]]
[[0,[[6,10,3,2],[7,0,2,10]],10],[1,1,0],[[],1,[7],[5,1]]]

[[[[10],0,[7,9,0,7,7]]],[],[[[7,8],2],[[4,2]]],[[[2,7,2,3],[0]],9,1,[[8,4,5],[4,10,0,0,4],[]],[3,[6,4,9,5],4]]]
[[[6,[9,6,9,1],9],[[0,0],8,[1,7,3,1],4,[]]]]

[[[2,3],3],[10,4,[[10,8,4,6,9],5],[8,[0,9,5,5,4],3]]]
[[0],[[[1]]],[1,9,2,2,[[4],4,8]],[]]

[[[[8,0],[10,7],[9,2],[4]],[3,[2,10,2],7,4],[8,[],[0,5,7],3,[5,10,1,7,4]],[6,3,8,[0]],[3]],[[5,[],[7,9],[8,5,10,2]],[]]]
[[10,6,[9,[2,6,5],1,0,[5,0,3,3,10]],[[10,1]],5],[1,[10,[1,2,6,6],[],[5,6,3]],10,5],[1,[2,[9,0,5,7],5]],[[3,[4,10,9],[7,6,3],[9],10],10],[[7,[10,10,7]],8,[4,7,7,6],[[1,0,4],[3]],4]]

[[[0,[5,8]],4,[1],[1,4,7,[]]],[4,2,[[4,0,4,8]],[4,[],[],3,[3,0]]],[]]
[[1,[7,6,5,7,[7]],4],[6]]

[[[[7,9]],[[8,4,10],2,7,9,2]],[7,7,[[7,9,1],4,[0],0],7,[8,[6,1,2,5],4,4,10]],[7,3,4,6,1]]
[[[8,0,[2],0],0],[[]],[6,4],[6,10]]

[[],[[[7,10,10,1],7],0,8],[9],[0,2]]
[[8,[[0,2,5],[6,10,1],3,0],[[10,7]],[[3,3,2,9,3],4,[8,6,2],5,0],[8,[],4]],[7],[[]],[]]

[[6,[],[[6,9,7,5],[7,4,5,9,10],10,[],[0,5,3,7,5]],[[0,9,5],10]],[[3,5],[10,3],10],[2],[[[5,7],[8,6],5],[[0,9,8,3],[1,8],9,[0,6,0,7,1],3]],[[[]],2,1,[],5]]
[[[[3,8,10,5],6,3],[4,2,0,2],[9,[],3]],[],[1],[8,0,10,[]],[[7,[],[8,5,7,3,2]],9,1,1,[[9,3,5],[],3]]]

[[10,6],[5,[2],[1,4],1],[[],[],[8,6,[0,4],9,[9,10,0,1,6]]],[10,3],[]]
[[],[[2,[10],[5],0],[9,9,6,1],0,[[10,1,8]]],[],[]]

[[4],[0,9,9,[3,7,[0,3,6,2],5]],[[],[9,0,2],10],[0,0,[[4],3,10,5],4,5],[]]
[[8],[10,2,[5,[6,10,4,7],[]],7,0]]

[[[],9,2,5,0],[[[10,6,6],10,[1,3]],1,10,[]],[9,4],[[[5,6],8,[2,7,8,3,8],10,[4,2]],6,4,[[4],2],[[10],1,10,[],6]]]
[[0,[]]]

[[0,[[5],10,5,8,2],[6],10,8],[0,3,9,2],[[0,[]]],[[[1],9,6,[8,1,8]],8]]
[[[9,[10,0],9,[]],[[8,5],2,[],0,6],[3,2,9],[3,[5],7,10,6],5],[[],[8,9],[4],[[7,6,9,6,9],[1,6,3],[7,3,6,1,6],[5]],[[6,7,4,5],5]]]

[[6,[[3,8,10,5]],2],[],[[],9]]
[[1,[[7,2,5,1,7],7,8,9],[6,[4,9,9,9,7],0],5]]

[[],[[1,8,[8],8],10],[],[2,[],[[0,5],6,[2,0],[1,3],[5,7,9,8]],2]]
[[[[6,7,5],[5,0,10,3],[8,10,8],[1]],1,[6,[8,4,3],[5]],[0,[6,10,2,7,1],[7,10],1,[8,1,10]],[2,2,[10,6,7,9,10]]],[9,1],[4,3]]

[[8],[[5,[],[],[5,5,2,1]],4,2,4],[10]]
[[[[0,6,0,9,3],5,3],[[],0,[9,9,0,6],7,2],[[10],10,[9,5,6,10],6],[6,8,9]],[[[3,0],[],10],7],[[],3]]

[9,7,7,7]
[9,7,7,7,2]

[[[2,[]],7,[1]]]
[[3,[0,[8,10,1,9,7],[]],7],[[[2,10,0,8,4],0,10,[]],[[1,1,3,5],3,[1,7],2,0],10,6,[6,[4],10,[9,9,1,4,2],[6]]],[[5,[9,6,2]],3]]

[[0,[8,[5,3,1,10,3],[7,3,5],[5,5,3]],3],[7,1]]
[[9],[[],9,9,[9,[],[1,1,7,4,1],[9,1,7]]]]

[[[[]],[10,[6,10,5,1,1],8,[2,3,1,7]],[6]]]
[[[[5,6,4,8,4],10]]]

[[],[],[3,3,[[2,5,6],0,[6,6,9],[],4],[]],[[2],1,[[3,10,3,2,7],[5],[9],[3,5,1],10],5,7],[[[6,3,6],7],2,4]]
[[3],[[10],10,[[1,5],6,0],[[6],3,[2]]],[5,7,8,9,[[3,4,3,5,9]]],[[6,[5,1],10,8],2,8,[8,[10,0,5,4,0],5,1,10],6]]

[[],[9],[3,2],[[[6,2,1,7,3],[7],[5,6,6]],[0,[]]]]
[[2,[7]],[]]

[[1,[7,4,3,7,[9,1,10,6]],6],[7],[],[]]
[[3,[1,9,[3,1]],[[],[10]],10],[8],[3],[[6,8,6],1,8]]

[[[6],[2,2,3,[5,8,9,0,0],2],3,[]],[5,[1]],[],[9,[[0],[6,3]],[2],3,8]]
[[],[[[0,7,2,3],[10,0],0,9,5],5,[]],[],[3],[[3,8],2]]

[[],[[[6,7]],9,[4,3,[7,4,4,3,10],[9,4]],7]]
[[[10,7],[[],2,[],1,4],[9,8],[[1],[7,1,3,1,1],[8,1,1,0,5],[]]],[[[2,10,6,5]]],[],[[6,[2]],0,7],[[6,2,[10,5,9,8]]]]

[[1,3,[4],10,[[9,1,5,8,3],[7,7,10,0,2],[10,4],[0,0,1]]],[8,7,2,[[9],3],[[6,9]]]]
[[1,4,[]],[],[9]]

[[[6,[],[3,2,3,5,6],[]]],[[2,[5,1],1]],[0,[],[[3],7,1,0],[]]]
[[[]],[[8,7,7,[6]]],[0]]

[[],[0],[],[7,1,1,5]]
[[[],[[],[9,3,5,4,7],[6],[8],6],[7,2,[3,9],[4,0,7],[8,10]],[0,[8,3,8,10,7],[5]],1],[1,[],[],[]],[[[5],[],[2,2,2,1,9]],9,[[2],6],10],[7,4,[3,5]]]

[[10,[],[7]],[[10]],[[[],1,[0,8,3],3,0],5,[],[],4]]
[[6,[],3],[[],[[6],[4,9,10]],[[],3,[],8,[1,8]],[[10,2,10]],9]]

[[[1,[2,6,5,9,0],1,2],[0,4,9],[[6,1,4,6,0],[5,9,5,8],[2,7,7,2],[1],[9,7,0]]]]
[[5,7,[[9,6,2,0,8]]],[[[],5,[5,8,10,8,5],[10,4,1],5],[2,3],3,0],[3,[9,[10,0,5,9,2],3,[],9]],[7,8,2,3,10],[[4,[5,10],[2],[]],[[7],2,1,1]]]

[[6,[4,[0,8],9],[[3,7,1],[10,9],[7,0,1],6,3],4],[0,2],[[]],[[6,5,[]],[9,10],[],0,[5]]]
[[[],[0,2],[],9],[6]]

[[4],[3,2,2],[[[9,5,5,2,2],[10,4],3,7,7],3]]
[[[5,8,[2,6,2,3],[8,10,0]]]]

[[4,10,[[4],6]]]
[[3],[0,[9,7,[4,4,3]],10],[[[],4,[3,5,8],[0]],[[1,0],0,[1,6,8],[5,3,9,6]],[9,[2],2,8,[]],[[3,0],[2,5],9,0]],[6,[7,0],[[2,0,5,3,6],[],[9,8,0,8,1],[1]]]]

[[5,2,2,[],[2,[],3]]]
[[1,2,[[8,3],2,[8,10,4,5,9],[7,0],3],3,[[],[10,5,8,9],6,6]],[2],[[1],[5,[4,7,10],8,7,9]],[8,4,[[],7],0,8],[]]

[[2,[],[4,[8,4,3,4,8],[9,2,2],[7,5,5,10]]]]
[[[[6],[0,10]]]]

[[3],[],[]]
[[[],[1,[],6,[2,6],2],4,[1]],[7,[0],7,[5,[4,6,4,6],5,[],[10,6]],0],[[9,[0,9,3,7],5],[[2,1,9,10,2],[4,6],0],[],8],[],[]]

[[[[10,7],[0,9,9,4],4,[3,8,7,10,4],7],[[7]]],[4,[9,[]]],[0,[4,[6,7],[10,8,3,4]],[7,6,[7,9,5,9,6],[6],[7,8,8]],4,1],[[[7,5,9,3],9],[[],[0,4,3],[10,10]]]]
[[[6,[6,0,0,1],2,4],7,4,[8,10,8,7,[0,9,0,10]],7],[10,1,0,6],[],[[],9,[4,[5,10,10,5,6],[6,1,5],8]],[]]

[[],[2,[[10,6],4,[6]],[[3,5,8],[4,8,9]],0,8]]
[[],[[6,9,[10,8,0,0]],[8]],[4,7,[[6],[4],[3],[]],2,[0,7,[6,6],[6,3,0,7],[5,4,3,3]]],[4,[[4,10,7],[],[9,10,6,3,10],9],0]]

[[4]]
[[[10,10,[1],[7,2,8,6,3]],[0,9,[2,1,7],2,[5,5,4]]],[[],0,[8,6,[10],7,[10,5,4]],5,6]]

[[],[],[1],[[[10,1,1,3,7],[],[9,8,2,10],[],[9,7,9]],[[6,6,7],6],2,5,[9,0,[1,1,3,10],[0,6,7,2]]]]
[[[10],[10,[4,5,8,6,3]]],[[[2,9],9,10],[],[9,7]],[],[[[8,4,3,10,5],9,7,3],4,10,4,5],[]]

[[],[[]]]
[[5,6,[[2,8,7,7,9],6,7,[6]]],[],[3,10,[9,[0,1,7],[9,3,0]],6,4],[10,[[],[1,0,6]],[],[[],[4,2,3,8,4]],8],[[],[4,8,[5,5,3],9],10,[10,4],3]]

[[5,[5,[3,10,4],1,[10,8,6,1,10]],[[9],[0,8,0],8,[7,6,4],[]]]]
[[[[6,7,5,3,3],[3,7,8]]],[5,7,[[],[8,3,0],[2,10],3,[5,3,10,1]],[[3,6,5],7],2],[[3]],[10],[[[9,9],5,1],7,[[5,0,1],2,[]],10,8]]

[[[[1,7],5,2,[4,8]],[[10,1],[1,6],1,[6,10,4,5,0],[0,0,6,4,3]],[],[]],[[],[],[1,[],[4,0,9,10]]],[0,[]],[2,3],[[3,[]]]]
[[8,[7,4,6]]]

[[5,10],[],[[5,8,6,1],[[0,10],1,[1,5,5,6]],8,[[10],[10,0,2,1,2],[10,2,10]]],[2,2,[],10],[0,10,2]]
[[0,10,[],[5,4,[5,7,8],2,6]]]

[[],[[]],[]]
[[[[8,1,7],[2]],[[],[9,3]]],[],[[1,[7,0,2,5],[10],[8,4],4],[[10],[1,4],7,[1,2,10,10,2]]],[[[9],[8,2,7,3]],[[],7,[1],6]],[[2,[6,3],10,6,[9,2,0,0]],6,9,4,[[3,6,5],[8,2,5,4]]]]

[[[[0]]],[[[],[1],[3,1,10]],[[],[],[4,4,5],8,4],[[0,5,0],4],[[0,1],1,6],7],[6,[]]]
[[3,[6,[7,3]],[[4,7],8,[10],7]],[6],[[[8,9,5],[1,0,4,2],1,[8,8]],[3,[1]],[7,7,[0,7],[5,10,1,9],3],[[0,9,4,10],5]]]

[9,1,3,2,3]
[9,1,3,2]";
    #endregion
}