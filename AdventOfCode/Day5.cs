using System.Diagnostics;

namespace AdventOfCode;

public class Day5
{
    [DebuggerDisplay("{Count}x {SrcStackIndex}->{DestStackIndex}")]
    public struct Instruction
    {
        public int SrcStackIndex, DestStackIndex, Count;
    }
    
    [TestCase(sample, "CMZ")]
    [TestCase(input, "CWMTGHBDW")]
    public void Part1(string input, string expected)
    {
        var lines = input.Split("\n");
        var expectedStackCount = (lines[0].Length+1)/4;
        Stack<char>[] stacks = new Stack<char>[expectedStackCount];
        for (var i = 0; i < stacks.Length; i++)
        {
            stacks[i] = new Stack<char>();
        }
        
        var lineIndex = 0;
        for (var i = 0; i < input.Length; i++)
        {
            lineIndex = i;
            if (string.IsNullOrEmpty(lines[i])) break; // finished parsing the stacks
            
            for (var n = 0; n < lines[i].Length; n ++)
            {
                var c = lines[i][n];
                switch (c)
                {
                    case '[':
                        n++;
                        var stackIndex = n / 4;
                        stacks[stackIndex].Push(lines[i][n]);
                        break;
                    case ']':
                    case ' ':
                        break;
                }
            }
        }
        
        // do we need to reverse?
        for (var i = 0; i < stacks.Length; i++)
        {
            stacks[i] = new Stack<char>(stacks[i]);
        }
        
        // parse the move instructions...
        var instructions = new List<Instruction>();
        for (var i = lineIndex; i < lines.Length; i++)
        {
            if (string.IsNullOrEmpty(lines[i])) continue;

            var line = lines[i].Replace("move", "").Replace("to", "").Replace("from", "");
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var instruction = new Instruction
            {
                DestStackIndex = int.Parse(parts[2]),
                SrcStackIndex = int.Parse(parts[1]),
                Count = int.Parse(parts[0]),
            };
            instructions.Add(instruction);
        }
        
        
        // finally, do the problem!!!
        // foreach (var instruction in instructions)
        for (var instructionIndex = 0 ; instructionIndex < instructions.Count; instructionIndex ++)
        {
            var instruction = instructions[instructionIndex];
            var src = stacks[instruction.SrcStackIndex - 1];
            var dest = stacks[instruction.DestStackIndex - 1];
            for (var i = 0; i < instruction.Count; i++)
            {
                if (src.Count == 1)
                {
                    
                }
                var element = src.Pop();
                dest.Push(element);
            }
        }

        var output = "";
        foreach (var stack in stacks)
        {
            output += stack.Peek();
        }
        Assert.AreEqual(expected, output);
    }
    
    [TestCase(sample, "MCD")]
    [TestCase(input, "SSCGWJCRB")]
    public void Part2(string input, string expected)
    {
        var lines = input.Split("\n");
        var expectedStackCount = (lines[0].Length+1)/4;
        Stack<char>[] stacks = new Stack<char>[expectedStackCount];
        for (var i = 0; i < stacks.Length; i++)
        {
            stacks[i] = new Stack<char>();
        }
        
        var lineIndex = 0;
        for (var i = 0; i < input.Length; i++)
        {
            lineIndex = i;
            if (string.IsNullOrEmpty(lines[i])) break; // finished parsing the stacks
            
            for (var n = 0; n < lines[i].Length; n ++)
            {
                var c = lines[i][n];
                switch (c)
                {
                    case '[':
                        n++;
                        var stackIndex = n / 4;
                        stacks[stackIndex].Push(lines[i][n]);
                        break;
                    case ']':
                    case ' ':
                        break;
                }
            }
        }
        
        // do we need to reverse?
        for (var i = 0; i < stacks.Length; i++)
        {
            stacks[i] = new Stack<char>(stacks[i]);
        }
        
        // parse the move instructions...
        var instructions = new List<Instruction>();
        for (var i = lineIndex; i < lines.Length; i++)
        {
            if (string.IsNullOrEmpty(lines[i])) continue;

            var line = lines[i].Replace("move", "").Replace("to", "").Replace("from", "");
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var instruction = new Instruction
            {
                DestStackIndex = int.Parse(parts[2]),
                SrcStackIndex = int.Parse(parts[1]),
                Count = int.Parse(parts[0]),
            };
            instructions.Add(instruction);
        }
        
        
        // finally, do the problem!!!
        // foreach (var instruction in instructions)
        for (var instructionIndex = 0 ; instructionIndex < instructions.Count; instructionIndex ++)
        {
            var instruction = instructions[instructionIndex];
            var src = stacks[instruction.SrcStackIndex - 1];
            var dest = stacks[instruction.DestStackIndex - 1];
            var moveStack = new Stack<char>();
            for (var i = 0; i < instruction.Count; i++)
            {
                var element = src.Pop();
                moveStack.Push(element);
            }

            while (moveStack.Count > 0)
            {
                var element = moveStack.Pop();
                dest.Push(element);
            }
        }

        var output = "";
        foreach (var stack in stacks)
        {
            output += stack.Peek();
        }
        Assert.AreEqual(expected, output);
    }
    
    #region sample input

    public const string sample = @"    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2";

    #endregion
    
    #region input

    public const string input = @"                [M]     [V]     [L]
[G]             [V] [C] [G]     [D]
[J]             [Q] [W] [Z] [C] [J]
[W]         [W] [G] [V] [D] [G] [C]
[R]     [G] [N] [B] [D] [C] [M] [W]
[F] [M] [H] [C] [S] [T] [N] [N] [N]
[T] [W] [N] [R] [F] [R] [B] [J] [P]
[Z] [G] [J] [J] [W] [S] [H] [S] [G]
 1   2   3   4   5   6   7   8   9 

move 1 from 5 to 2
move 7 from 7 to 1
move 1 from 1 to 7
move 1 from 4 to 1
move 7 from 9 to 1
move 1 from 3 to 7
move 4 from 5 to 4
move 6 from 4 to 9
move 2 from 7 to 6
move 6 from 8 to 2
move 2 from 4 to 5
move 2 from 3 to 7
move 11 from 1 to 4
move 6 from 6 to 1
move 3 from 5 to 3
move 5 from 9 to 8
move 1 from 2 to 3
move 2 from 7 to 9
move 7 from 1 to 2
move 1 from 5 to 3
move 1 from 5 to 3
move 5 from 8 to 5
move 3 from 5 to 4
move 1 from 1 to 7
move 1 from 3 to 8
move 2 from 6 to 3
move 3 from 3 to 4
move 1 from 6 to 2
move 5 from 4 to 2
move 2 from 5 to 3
move 2 from 7 to 1
move 1 from 8 to 1
move 7 from 1 to 7
move 4 from 4 to 2
move 7 from 4 to 1
move 10 from 1 to 5
move 10 from 5 to 2
move 11 from 2 to 3
move 1 from 1 to 6
move 1 from 4 to 7
move 4 from 7 to 1
move 6 from 2 to 5
move 2 from 1 to 3
move 1 from 9 to 5
move 2 from 9 to 6
move 1 from 6 to 1
move 3 from 5 to 4
move 20 from 3 to 9
move 3 from 7 to 1
move 3 from 5 to 2
move 3 from 4 to 8
move 3 from 1 to 3
move 3 from 1 to 2
move 2 from 6 to 1
move 10 from 9 to 6
move 6 from 6 to 7
move 4 from 6 to 3
move 11 from 2 to 6
move 1 from 8 to 9
move 13 from 2 to 3
move 1 from 1 to 9
move 1 from 9 to 4
move 1 from 8 to 2
move 1 from 8 to 2
move 4 from 7 to 8
move 8 from 6 to 9
move 3 from 2 to 3
move 3 from 8 to 4
move 11 from 9 to 2
move 7 from 9 to 6
move 1 from 1 to 5
move 4 from 4 to 9
move 21 from 3 to 1
move 1 from 3 to 9
move 7 from 6 to 3
move 6 from 1 to 2
move 13 from 1 to 5
move 2 from 1 to 2
move 3 from 9 to 3
move 2 from 2 to 3
move 2 from 6 to 4
move 3 from 3 to 5
move 13 from 5 to 2
move 5 from 3 to 4
move 2 from 7 to 9
move 2 from 4 to 2
move 1 from 3 to 8
move 1 from 6 to 1
move 4 from 3 to 7
move 2 from 5 to 7
move 1 from 7 to 2
move 1 from 5 to 9
move 4 from 7 to 8
move 1 from 1 to 9
move 6 from 8 to 1
move 4 from 4 to 8
move 25 from 2 to 9
move 1 from 4 to 3
move 1 from 3 to 7
move 4 from 8 to 1
move 1 from 7 to 4
move 3 from 1 to 6
move 5 from 2 to 1
move 1 from 5 to 1
move 1 from 4 to 1
move 24 from 9 to 6
move 9 from 1 to 6
move 1 from 5 to 6
move 1 from 1 to 9
move 1 from 2 to 8
move 1 from 8 to 1
move 3 from 1 to 8
move 36 from 6 to 3
move 2 from 7 to 3
move 1 from 2 to 5
move 1 from 5 to 2
move 1 from 6 to 2
move 10 from 3 to 2
move 3 from 8 to 2
move 1 from 1 to 7
move 2 from 2 to 6
move 10 from 9 to 1
move 2 from 6 to 4
move 13 from 3 to 4
move 8 from 3 to 7
move 8 from 1 to 2
move 5 from 3 to 8
move 3 from 1 to 9
move 1 from 7 to 1
move 7 from 4 to 5
move 1 from 1 to 2
move 14 from 2 to 6
move 2 from 7 to 2
move 8 from 4 to 8
move 3 from 7 to 9
move 2 from 9 to 8
move 2 from 7 to 1
move 1 from 7 to 8
move 1 from 6 to 8
move 1 from 9 to 3
move 4 from 2 to 7
move 6 from 6 to 1
move 3 from 1 to 9
move 1 from 1 to 7
move 6 from 5 to 6
move 1 from 5 to 2
move 1 from 6 to 8
move 5 from 7 to 5
move 1 from 2 to 9
move 2 from 3 to 4
move 9 from 8 to 4
move 8 from 4 to 8
move 6 from 6 to 7
move 5 from 6 to 4
move 7 from 9 to 7
move 7 from 8 to 7
move 5 from 8 to 4
move 3 from 1 to 6
move 1 from 2 to 7
move 1 from 1 to 4
move 4 from 5 to 2
move 2 from 6 to 9
move 1 from 3 to 7
move 1 from 5 to 1
move 1 from 8 to 9
move 1 from 6 to 1
move 1 from 2 to 7
move 2 from 8 to 1
move 2 from 1 to 8
move 3 from 2 to 4
move 1 from 6 to 1
move 17 from 4 to 1
move 3 from 2 to 7
move 13 from 7 to 8
move 1 from 2 to 6
move 14 from 1 to 4
move 2 from 8 to 5
move 1 from 9 to 7
move 2 from 5 to 4
move 1 from 9 to 3
move 5 from 1 to 5
move 3 from 4 to 1
move 1 from 3 to 2
move 7 from 4 to 5
move 9 from 7 to 8
move 5 from 4 to 2
move 1 from 1 to 3
move 1 from 9 to 2
move 15 from 8 to 6
move 1 from 3 to 7
move 11 from 6 to 5
move 1 from 4 to 8
move 3 from 1 to 7
move 5 from 7 to 5
move 27 from 5 to 1
move 8 from 8 to 4
move 1 from 2 to 6
move 3 from 6 to 1
move 9 from 1 to 5
move 5 from 5 to 7
move 2 from 2 to 1
move 2 from 5 to 4
move 6 from 7 to 6
move 1 from 5 to 2
move 1 from 7 to 8
move 4 from 6 to 8
move 5 from 6 to 3
move 1 from 7 to 1
move 5 from 4 to 3
move 6 from 8 to 2
move 1 from 7 to 8
move 2 from 8 to 9
move 10 from 3 to 5
move 9 from 5 to 2
move 3 from 4 to 8
move 1 from 5 to 7
move 2 from 9 to 7
move 2 from 8 to 3
move 1 from 3 to 8
move 19 from 1 to 7
move 4 from 2 to 7
move 2 from 4 to 3
move 3 from 3 to 2
move 2 from 8 to 3
move 2 from 5 to 8
move 1 from 2 to 3
move 2 from 8 to 3
move 5 from 2 to 5
move 9 from 7 to 5
move 13 from 5 to 9
move 7 from 2 to 6
move 2 from 6 to 9
move 1 from 2 to 1
move 5 from 6 to 7
move 1 from 5 to 7
move 6 from 1 to 2
move 5 from 3 to 6
move 6 from 7 to 2
move 3 from 6 to 4
move 3 from 7 to 4
move 12 from 7 to 6
move 5 from 4 to 1
move 2 from 7 to 4
move 3 from 4 to 6
move 16 from 6 to 3
move 4 from 1 to 4
move 1 from 1 to 9
move 3 from 9 to 2
move 1 from 4 to 6
move 9 from 3 to 7
move 2 from 6 to 3
move 3 from 3 to 9
move 15 from 2 to 7
move 19 from 7 to 4
move 15 from 9 to 2
move 16 from 2 to 8
move 6 from 3 to 5
move 4 from 7 to 5
move 15 from 8 to 7
move 19 from 4 to 2
move 1 from 8 to 3
move 16 from 2 to 1
move 9 from 7 to 6
move 7 from 2 to 8
move 2 from 2 to 7
move 1 from 9 to 5
move 1 from 3 to 4
move 6 from 1 to 2
move 8 from 5 to 1
move 1 from 5 to 1
move 18 from 1 to 8
move 7 from 7 to 5
move 7 from 5 to 3
move 4 from 3 to 6
move 13 from 8 to 5
move 12 from 8 to 1
move 5 from 1 to 6
move 15 from 5 to 4
move 1 from 1 to 6
move 12 from 6 to 3
move 8 from 3 to 4
move 2 from 7 to 3
move 9 from 3 to 1
move 5 from 2 to 9
move 16 from 4 to 3
move 10 from 1 to 3
move 2 from 1 to 5
move 1 from 3 to 1
move 5 from 6 to 1
move 4 from 9 to 3
move 1 from 2 to 8
move 1 from 8 to 1
move 1 from 9 to 8
move 2 from 5 to 9
move 9 from 4 to 1
move 3 from 1 to 3
move 2 from 6 to 8
move 3 from 8 to 5
move 2 from 1 to 5
move 2 from 9 to 8
move 1 from 8 to 6
move 2 from 5 to 3
move 19 from 3 to 1
move 2 from 4 to 2
move 1 from 5 to 6
move 2 from 2 to 3
move 1 from 8 to 6
move 8 from 3 to 9
move 6 from 3 to 7
move 2 from 6 to 2
move 1 from 6 to 1
move 1 from 1 to 8
move 1 from 8 to 9
move 1 from 7 to 3
move 19 from 1 to 5
move 21 from 5 to 2
move 13 from 2 to 6
move 13 from 1 to 8
move 7 from 9 to 7
move 2 from 9 to 2
move 10 from 8 to 3
move 1 from 1 to 6
move 10 from 2 to 4
move 11 from 3 to 5
move 8 from 5 to 6
move 1 from 3 to 7
move 2 from 8 to 6
move 2 from 2 to 8
move 3 from 7 to 6
move 2 from 8 to 6
move 1 from 1 to 2
move 24 from 6 to 5
move 2 from 3 to 8
move 1 from 8 to 6
move 7 from 7 to 9
move 4 from 6 to 9
move 1 from 8 to 9
move 21 from 5 to 9
move 2 from 7 to 2
move 1 from 8 to 5
move 1 from 7 to 3
move 12 from 9 to 6
move 6 from 6 to 3
move 12 from 9 to 4
move 4 from 5 to 6
move 13 from 4 to 2
move 8 from 4 to 8
move 10 from 6 to 8
move 11 from 8 to 9
move 4 from 8 to 4
move 2 from 4 to 3
move 8 from 3 to 8
move 2 from 6 to 8
move 1 from 3 to 8
move 6 from 2 to 4
move 1 from 4 to 8
move 1 from 9 to 7
move 13 from 8 to 4
move 1 from 7 to 1
move 1 from 1 to 4
move 8 from 4 to 7
move 3 from 5 to 7
move 19 from 9 to 7
move 3 from 2 to 7
move 1 from 8 to 2
move 13 from 7 to 6
move 1 from 2 to 4
move 4 from 6 to 2
move 1 from 8 to 3
move 7 from 6 to 8
move 1 from 6 to 2
move 1 from 2 to 7
move 9 from 2 to 3
move 1 from 6 to 2
move 21 from 7 to 5
move 9 from 5 to 3
move 19 from 3 to 9
move 5 from 8 to 5
move 2 from 2 to 1
move 2 from 1 to 8
move 6 from 4 to 5
move 3 from 8 to 7
move 15 from 9 to 2
move 2 from 2 to 5
move 3 from 9 to 6
move 5 from 4 to 5
move 11 from 2 to 6
move 1 from 8 to 6
move 1 from 9 to 5
move 1 from 7 to 3
move 6 from 5 to 6
move 1 from 4 to 6
move 1 from 3 to 4
move 13 from 5 to 2
move 16 from 6 to 9
move 4 from 4 to 5
move 2 from 6 to 2
move 2 from 6 to 4
move 2 from 4 to 5
move 2 from 7 to 8
move 2 from 6 to 3
move 2 from 5 to 8
move 14 from 5 to 7
move 4 from 8 to 1
move 4 from 1 to 6
move 1 from 3 to 9
move 1 from 6 to 1
move 2 from 7 to 3
move 2 from 3 to 7
move 2 from 5 to 2
move 9 from 9 to 2
move 13 from 7 to 3
move 12 from 3 to 9
move 2 from 6 to 8
move 14 from 2 to 9
move 2 from 8 to 9
move 10 from 2 to 1
move 1 from 7 to 4
move 2 from 3 to 8
move 4 from 2 to 1
move 1 from 8 to 3
move 1 from 2 to 6
move 1 from 8 to 3
move 4 from 9 to 4
move 1 from 3 to 5
move 1 from 5 to 1
move 1 from 3 to 9
move 12 from 1 to 8
move 10 from 8 to 5
move 7 from 5 to 6
move 1 from 1 to 9
move 3 from 5 to 1
move 1 from 1 to 3
move 16 from 9 to 7
move 4 from 4 to 3
move 1 from 4 to 9
move 15 from 7 to 8
move 15 from 9 to 1
move 8 from 1 to 6
move 1 from 9 to 3
move 17 from 6 to 2
move 1 from 9 to 1
move 15 from 2 to 7
move 14 from 8 to 9
move 12 from 7 to 9
move 12 from 9 to 3
move 3 from 7 to 9
move 1 from 7 to 4
move 7 from 9 to 6
move 1 from 4 to 6
move 11 from 9 to 6
move 2 from 1 to 2
move 18 from 6 to 4
move 4 from 2 to 7
move 2 from 7 to 3
move 2 from 7 to 8
move 4 from 1 to 5
move 1 from 9 to 2
move 2 from 5 to 4
move 5 from 1 to 3
move 2 from 3 to 7
move 2 from 3 to 9
move 1 from 6 to 7
move 1 from 2 to 9
move 2 from 8 to 1
move 3 from 1 to 3
move 2 from 5 to 8
move 2 from 3 to 5
move 1 from 5 to 2
move 1 from 1 to 3
move 1 from 9 to 2
move 1 from 9 to 1
move 3 from 7 to 6
move 1 from 1 to 9
move 2 from 8 to 9
move 1 from 2 to 3
move 2 from 8 to 2
move 2 from 6 to 5
move 1 from 8 to 5
move 3 from 2 to 5
move 3 from 4 to 8
move 1 from 8 to 2
move 3 from 9 to 7
move 3 from 7 to 1
move 1 from 9 to 6
move 3 from 1 to 2
move 2 from 8 to 7
move 2 from 7 to 9
move 2 from 6 to 5
move 3 from 5 to 3
move 1 from 2 to 5
move 3 from 2 to 7
move 2 from 5 to 6
move 15 from 4 to 9
move 1 from 3 to 1
move 25 from 3 to 4
move 3 from 7 to 3
move 5 from 9 to 5
move 10 from 9 to 5
move 9 from 5 to 1
move 5 from 5 to 2
move 1 from 6 to 7
move 5 from 5 to 8";

    #endregion
}