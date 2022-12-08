using System.Diagnostics;

namespace AdventOfCode;

public class Day4
{

    [DebuggerDisplay("{Start} - {End}")]
    public struct Range
    {
        public int Start, End;

        public bool Contains(Range other) => Start <= other.Start && End >= other.End;

        public bool Overlaps(Range other)
        {
            
            /*
             * (this) .....678
             * (othe) .234....
             */
            if (Start >= other.Start && other.End <= End && Start <= other.End) return true;
            return false;
        }
    }
    
    
    [TestCase(sample, 2)]
    // [TestCase(input, 2)]
    public void Part1(string input, int expected)
    {
        Range GetRange(string assignmentStr)
        {
            var numStrs = assignmentStr.Split('-');
            return new Range
            {
                Start = int.Parse(numStrs[0]),
                End = int.Parse(numStrs[1]),
            };
        }

        var total = 0;
        var lines = input.Split("\n");
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line)) continue;
            var assignmentStrs = line.Split(',');
            var r1 = GetRange(assignmentStrs[0]);
            var r2 = GetRange(assignmentStrs[1]);

            if (r1.Contains(r2) || r2.Contains(r1) )
            {
                total++;
            }
        }
        Assert.AreEqual(expected, total);
    }
    
    [TestCase(sample, 4)]
    // [TestCase(input, 2)]
    public void Part2(string input, int expected)
    {
        Range GetRange(string assignmentStr)
        {
            var numStrs = assignmentStr.Split('-');
            return new Range
            {
                Start = int.Parse(numStrs[0]),
                End = int.Parse(numStrs[1]),
            };
        }

        var total = 0;
        var lines = input.Split("\n");
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line)) continue;
            var assignmentStrs = line.Split(',');
            var r1 = GetRange(assignmentStrs[0]);
            var r2 = GetRange(assignmentStrs[1]);

            if (r1.Contains(r2) || r2.Contains(r1)|| r1.Overlaps(r2) || r2.Overlaps(r1))
            {
                total++;
            }
        }
        Assert.AreEqual(expected, total);
    }
    
    #region sampleinput

    public const string sample = @"2-4,6-8
2-3,4-5
5-7,7-9
2-8,3-7
6-6,4-6
2-6,4-8
";

    #endregion
    
    #region input

    public const string input = @"67-84,66-87
70-70,40-69
32-77,31-78
10-84,11-96
15-95,14-94
53-55,48-54
40-92,39-93
67-91,66-66
74-99,74-98
2-49,50-86
3-95,32-94
22-68,7-23
28-28,27-58
35-71,70-71
12-13,13-61
78-78,53-78
98-99,69-89
3-94,3-94
13-27,12-13
49-97,50-90
35-61,36-62
14-14,15-96
46-84,84-84
53-77,13-50
55-93,55-92
71-71,40-72
95-97,3-96
7-98,8-98
17-98,16-97
47-62,46-62
58-98,57-59
19-74,18-75
85-86,84-88
10-98,10-98
4-83,84-84
47-85,46-84
91-95,5-91
9-42,9-41
1-94,93-95
31-90,90-91
1-94,94-96
36-70,35-71
72-79,72-79
42-42,42-89
7-81,6-81
5-7,4-8
7-97,7-98
22-81,80-82
3-91,90-90
1-98,2-97
54-54,55-78
31-44,31-51
26-63,25-43
34-96,34-95
43-64,28-71
84-86,8-85
10-11,11-84
15-48,16-47
34-80,35-80
10-69,10-70
89-89,79-88
41-92,71-85
45-61,17-44
6-88,3-48
48-87,26-88
24-47,45-58
44-52,44-60
22-74,12-65
12-13,13-99
1-80,19-80
42-55,43-49
1-24,23-65
24-81,23-99
28-79,29-79
3-4,4-74
41-70,69-69
4-46,47-76
31-73,32-74
62-63,62-62
22-59,2-23
66-69,52-69
15-61,3-4
22-48,39-43
93-97,94-96
35-35,21-36
52-76,39-76
1-13,1-96
90-96,1-92
26-69,25-63
22-58,10-58
24-88,89-89
38-77,38-78
4-58,51-58
57-82,58-83
10-25,11-25
73-95,73-96
80-91,79-96
3-81,5-80
24-82,97-99
2-59,58-60
7-70,70-72
83-83,36-84
3-16,17-97
15-96,87-99
34-56,9-56
2-55,13-55
80-97,35-80
4-98,3-51
75-86,76-85
21-82,21-83
30-31,30-62
69-79,68-92
6-80,5-6
20-81,20-82
62-74,62-63
90-90,79-89
79-79,37-79
2-3,2-98
60-61,45-61
42-84,9-85
32-72,12-72
63-98,24-64
2-95,3-81
39-41,14-40
13-33,14-14
3-93,6-93
31-71,30-30
36-92,35-92
22-78,77-77
6-7,7-87
57-57,22-57
36-79,21-73
18-84,1-93
70-70,26-70
70-72,65-68
4-90,5-89
35-98,34-98
9-58,14-58
71-71,19-70
64-99,64-98
51-80,50-51
14-52,13-52
9-78,2-8
8-72,8-73
27-82,28-81
6-85,85-85
20-86,93-99
8-94,7-95
2-4,3-95
24-45,44-45
40-41,41-87
30-97,96-98
21-23,22-51
58-85,40-68
43-62,2-43
29-31,30-97
2-95,3-99
2-93,3-3
36-95,36-96
55-56,56-90
26-95,25-94
84-92,85-91
22-70,69-69
10-74,4-40
23-24,23-28
73-75,74-82
71-82,4-90
59-77,43-78
29-48,47-47
12-19,17-17
14-44,6-15
4-83,62-84
21-48,3-49
7-8,7-63
65-65,64-88
2-4,3-92
27-41,27-54
17-66,6-79
7-37,37-37
6-28,29-86
24-24,24-38
67-94,93-95
4-84,36-84
98-98,4-97
5-66,4-5
14-45,15-44
82-94,82-95
24-24,24-25
54-68,68-68
61-69,68-86
40-63,40-62
10-53,11-11
18-25,60-67
18-79,17-78
52-99,52-55
7-16,2-16
3-96,99-99
55-57,56-81
3-63,63-63
5-23,4-42
10-94,2-93
64-79,4-99
52-94,10-94
75-99,69-76
40-98,51-97
15-55,15-54
68-68,25-69
44-95,43-99
33-36,13-34
35-88,36-87
1-8,6-53
23-80,45-80
59-87,59-90
34-38,33-47
6-11,7-12
73-73,56-74
18-59,59-60
89-89,38-89
2-26,27-85
79-79,43-80
15-33,32-32
32-33,32-89
98-99,54-99
6-84,3-62
20-34,15-27
35-36,36-90
12-44,11-43
3-3,2-38
16-85,15-90
30-60,58-64
8-99,7-15
25-50,26-26
23-23,22-87
43-58,44-70
13-32,17-23
16-72,17-27
5-83,6-82
28-34,29-29
32-96,33-97
4-27,5-27
25-72,25-25
95-97,18-95
69-91,69-84
2-91,90-92
9-78,9-84
26-26,25-55
78-94,78-95
5-92,4-6
16-98,16-99
40-90,41-96
4-95,4-20
4-99,4-98
99-99,6-28
7-95,94-96
13-18,17-19
25-69,20-22
80-90,7-77
6-6,10-89
72-80,78-88
3-63,11-89
45-76,44-46
45-89,90-96
21-59,20-58
80-81,10-81
21-23,22-84
35-73,26-36
77-86,78-86
3-67,1-96
26-41,25-27
15-63,14-69
16-48,16-47
11-83,6-83
1-41,2-24
89-99,89-98
70-75,47-83
10-80,11-81
25-34,33-35
52-61,51-62
2-3,4-99
46-87,43-86
84-85,84-84
16-85,17-85
7-37,7-61
16-22,15-50
4-14,3-16
77-87,76-77
43-45,44-47
3-4,4-61
13-27,13-66
12-86,11-87
81-83,12-82
28-84,29-32
1-92,91-91
32-33,33-33
42-80,45-65
22-98,2-98
10-97,21-97
2-90,2-91
3-67,2-66
6-43,2-5
41-70,70-70
2-96,2-39
24-28,27-32
56-91,92-94
38-38,38-39
3-92,92-92
6-7,6-85
11-11,10-97
38-42,39-42
49-68,67-95
79-80,23-80
14-71,13-70
98-99,6-99
74-92,91-91
35-82,35-81
8-80,8-80
84-84,49-85
12-45,11-46
31-66,30-67
27-78,25-35
11-45,11-73
5-93,4-98
19-49,20-49
13-21,14-22
18-59,29-59
49-51,48-52
36-68,68-73
14-97,98-99
5-70,5-71
48-54,52-52
13-38,47-55
67-67,32-67
95-99,3-94
44-97,45-90
16-91,17-92
29-93,92-92
12-34,12-33
87-92,87-93
59-73,62-77
58-74,73-74
19-75,74-75
77-79,79-87
27-27,27-47
95-99,5-96
11-77,77-77
79-98,61-84
1-78,2-77
2-95,2-96
29-60,29-61
14-79,13-80
2-48,47-49
2-5,4-63
7-87,8-86
3-69,9-70
82-82,37-83
61-70,61-66
83-94,6-84
13-39,13-40
3-83,1-91
85-89,84-89
15-94,2-5
60-75,4-83
25-86,24-26
5-5,4-75
4-99,3-97
67-82,82-91
12-66,13-13
12-72,13-71
11-81,12-43
16-29,15-28
87-88,14-88
1-91,87-92
3-4,4-49
16-33,17-34
10-94,11-11
70-70,33-71
51-52,50-82
14-67,13-15
22-89,22-88
55-61,16-60
3-87,89-89
36-65,29-66
48-68,48-68
28-28,10-27
18-68,93-95
50-94,49-93
10-58,9-9
20-21,21-43
16-97,17-98
20-94,93-93
15-69,68-81
84-84,42-85
83-92,91-91
5-81,6-90
71-73,26-72
4-26,3-56
50-51,49-51
72-76,76-76
8-47,9-46
2-86,1-85
95-95,76-94
44-69,43-70
86-94,7-85
38-69,38-39
96-96,6-97
17-84,13-38
17-69,80-86
35-89,53-88
83-93,84-87
98-98,92-98
59-60,21-60
87-89,56-88
31-90,5-75
65-94,9-91
26-59,25-26
1-97,54-60
45-99,2-99
2-97,1-98
36-92,91-93
7-97,30-97
19-88,19-88
8-96,95-98
46-48,26-47
18-99,17-96
45-61,44-46
3-98,4-91
1-2,2-41
14-22,13-21
55-56,40-56
5-49,5-6
37-40,37-41
15-72,15-48
43-57,42-99
48-68,49-67
39-40,40-81
3-82,2-14
68-94,67-68
79-79,2-79
1-83,1-84
49-50,14-50
80-82,81-86
99-99,72-97
34-73,35-72
39-56,54-56
5-64,6-63
30-66,94-94
33-73,32-74
85-99,12-99
6-97,3-52
6-97,28-99
72-97,28-73
11-61,1-59
25-45,26-68
4-5,5-5
40-89,89-89
97-99,90-97
32-80,32-50
16-97,42-96
42-72,71-73
4-89,34-53
34-71,35-72
2-89,4-88
12-12,12-44
69-89,88-90
1-99,2-2
6-76,30-76
8-13,14-82
20-23,23-23
50-81,50-81
7-73,8-73
6-40,7-47
9-64,31-58
42-74,42-75
11-80,79-81
89-89,88-89
5-78,2-3
7-93,2-94
97-99,6-97
5-92,2-93
34-39,72-94
53-83,50-90
7-8,8-70
63-64,3-64
68-69,69-70
26-57,26-54
2-2,1-97
36-66,36-59
13-64,23-63
24-24,23-61
64-64,44-64
62-64,63-63
88-90,89-90
9-10,10-98
33-95,33-96
8-92,8-91
57-72,20-57
10-21,20-56
30-31,31-49
81-89,57-89
89-99,9-99
8-75,7-97
7-91,7-92
46-78,77-99
53-97,69-76
3-71,4-72
10-47,15-46
13-47,12-46
26-52,27-51
14-15,14-63
6-94,1-5
6-36,7-35
54-57,54-58
14-57,56-58
72-79,43-79
6-95,95-99
9-9,8-73
39-39,39-39
76-91,65-92
80-83,14-80
40-49,40-50
4-98,4-98
57-64,64-64
44-45,42-66
5-94,3-94
29-57,29-57
42-72,42-73
28-29,28-84
13-46,15-61
14-47,15-48
24-31,24-33
54-55,53-98
10-42,9-95
66-66,15-66
67-80,52-81
4-11,1-12
6-78,5-79
71-79,72-80
59-90,4-59
34-35,35-96
7-95,7-94
32-98,32-32
36-57,56-56
65-71,65-71
89-94,95-95
10-95,10-95
4-6,5-49
12-87,13-86
3-96,96-96
16-83,15-84
1-72,1-99
53-86,21-85
34-94,34-86
37-70,8-90
54-54,54-94
5-99,3-3
96-96,11-96
15-45,13-16
2-98,1-99
44-70,45-68
96-97,96-97
19-58,57-85
33-33,8-32
26-31,26-77
3-92,92-92
15-96,83-92
4-91,4-4
97-97,17-97
87-99,28-86
22-99,22-23
60-83,60-61
41-48,44-85
51-78,3-52
21-86,3-85
1-33,2-2
3-80,73-96
16-19,8-20
18-97,64-97
72-73,3-73
3-96,95-95
6-89,7-97
43-43,42-63
70-70,61-69
20-29,21-29
38-93,93-94
65-66,35-66
92-94,3-93
13-34,33-67
22-31,22-32
2-27,2-26
42-97,2-97
35-66,15-36
16-60,2-17
46-88,47-93
58-71,72-72
84-86,10-85
12-13,12-87
5-98,98-98
3-88,4-90
58-83,64-83
20-65,20-95
4-32,1-92
23-94,23-95
1-88,3-87
19-20,19-62
3-97,99-99
52-58,21-53
30-73,72-73
5-92,4-91
7-95,4-7
69-79,2-70
19-83,20-20
56-96,18-96
64-66,22-66
43-89,44-89
50-82,73-75
62-72,63-71
19-98,20-98
19-97,19-98
32-83,32-33
2-67,2-67
59-88,4-88
1-1,3-88
46-48,47-96
10-54,11-11
11-78,17-77
3-89,95-98
8-70,26-26
18-56,55-55
16-25,16-20
2-89,2-89
59-71,61-70
7-64,7-8
20-78,4-20
8-51,7-50
34-70,36-70
1-98,1-97
4-7,5-8
1-75,2-86
12-69,68-70
21-97,99-99
47-59,52-53
18-36,9-37
6-15,7-10
8-30,13-29
2-56,55-57
64-95,88-95
97-99,48-88
3-44,24-62
29-78,29-66
96-96,58-95
10-88,9-85
87-87,2-88
5-98,97-98
3-94,2-89
17-54,18-53
18-23,12-23
11-97,8-10
97-98,23-96
1-94,93-93
33-65,32-34
91-95,62-90
19-49,18-48
74-97,1-96
50-98,51-98
77-99,84-90
2-91,1-92
15-69,14-68
21-96,96-99
61-91,92-92
8-31,23-28
88-88,89-90
61-62,60-62
59-60,59-74
19-75,19-75
8-99,8-9
23-65,23-66
8-42,9-12
15-90,15-90
3-3,2-30
1-86,85-86
11-93,11-94
35-66,34-34
7-35,71-93
80-81,68-81
21-99,21-98
45-97,45-94
13-41,13-40
20-93,20-94
14-89,4-90
50-86,50-87
69-87,68-70
3-98,2-98
19-34,33-35
4-4,4-13
96-97,19-97
1-53,53-54
23-36,23-37
83-97,98-99
88-91,2-89
6-73,7-99
86-98,11-86
66-66,64-68
41-69,68-70
18-98,17-19
6-37,2-37
36-64,35-65
3-3,2-95
11-87,86-87
8-67,8-86
11-67,10-68
20-92,67-92
29-80,7-12
20-41,40-41
4-5,4-19
18-70,17-71
11-87,88-88
24-59,10-97
51-73,33-52
49-86,48-87
27-93,66-93
16-83,84-86
13-53,28-77
40-88,35-96
26-27,25-27
46-99,46-47
20-81,21-80
23-98,22-24
95-95,63-96
48-49,9-49
23-51,12-18
34-87,34-88
74-74,10-75
10-96,7-88
3-43,17-68
40-60,41-71
6-20,5-7
79-94,18-50
46-83,82-83
28-97,96-96
18-73,74-74
46-98,33-99
85-90,83-85
32-90,91-94
95-97,45-95
44-76,44-77
51-86,51-52
71-73,20-72
65-75,64-65
5-5,6-98
87-89,88-90
29-68,67-88
79-85,51-85
12-18,18-18
12-61,12-44
8-25,11-24
50-59,23-58
41-92,40-92
30-95,30-96
30-60,39-74
4-99,4-98
56-65,57-64
76-76,5-75
69-96,68-69
48-88,48-96
2-24,1-15
9-69,9-76
27-86,1-14
68-99,67-97
72-73,3-73
36-71,18-36
40-40,12-39
11-95,8-12
30-46,45-46
4-23,11-25
10-64,12-41
21-65,65-65
75-98,86-86
10-12,11-78
22-83,21-83
84-86,77-85
65-88,10-65
80-85,25-29
13-74,12-14
82-83,83-91
86-87,61-87
3-29,26-52
13-35,12-36
38-82,38-83
5-5,4-98
15-16,16-65
24-64,29-65
36-79,29-80
54-62,53-55
15-16,15-92
42-44,43-88
9-98,9-97
8-79,45-80
24-96,20-98
5-37,4-6
37-38,38-86
72-74,24-73
47-47,37-47
9-92,8-8
52-52,52-57
68-84,80-99
31-71,32-70
10-81,9-81
80-81,28-81
12-98,13-98
8-11,9-10
11-60,12-12
6-90,6-91
24-55,23-55
7-33,92-98
31-51,32-50
50-50,13-49
62-70,20-69
2-66,66-66
42-61,10-95
89-89,6-88
35-66,34-35
73-93,10-72
91-95,59-91
15-25,23-32
22-95,22-92
1-14,4-91
57-61,57-64
43-44,43-94
68-83,69-84
61-61,3-61
7-61,8-14
8-42,8-41
77-87,77-88
64-82,18-73
25-85,26-26
37-37,36-37
71-73,16-71
72-76,49-76
39-87,40-86
24-93,25-59
78-86,79-83
22-24,23-81
36-90,89-91
3-45,5-81
13-91,14-14
58-68,43-79
15-25,24-25
11-19,12-18
37-81,32-62
43-92,93-93
78-82,46-76
92-97,96-96
90-93,43-90
15-95,3-96
15-45,15-46
66-66,66-72
12-12,10-13
64-68,63-69
67-73,66-74
13-97,2-10
1-96,1-95
4-56,56-56
7-94,8-46
83-85,4-84
68-68,12-68
8-28,9-27
11-70,10-69
31-57,31-70
10-51,8-52
7-64,7-63
44-49,43-58
25-43,41-42
44-44,43-92
61-88,39-62
5-64,5-5
37-42,43-43
52-96,57-97
98-99,92-95
4-14,3-97
26-96,27-96
57-77,63-77
25-79,78-96
20-40,21-58
42-90,42-91
26-37,26-38
37-41,42-62
3-84,2-4
20-96,45-99
8-57,20-56
21-94,20-21
66-91,65-84
2-97,1-95
3-60,2-3
2-50,3-79
2-92,2-92
15-16,15-22
1-2,3-88
3-91,79-90
34-37,37-68
9-85,89-92
77-93,29-31
31-31,11-30
10-19,10-32
5-93,6-30
57-78,76-90
34-35,34-73
31-67,11-68
6-80,6-6
36-43,42-44
45-90,46-90
5-6,5-66
19-26,18-40
33-99,32-34
7-83,82-87
45-58,57-57
2-62,1-63
5-97,99-99
2-90,91-91
39-40,40-61
57-75,25-75
1-80,1-43
10-36,35-99
17-34,35-35
66-84,34-84
77-77,31-77
14-47,14-78
54-58,22-93
92-92,5-92
74-91,75-95
2-98,97-99
44-44,43-43
8-98,97-97
36-36,25-37
81-82,79-85
38-54,88-94
18-97,7-97
12-73,12-72
62-62,2-63
2-3,3-78
73-79,47-73
31-74,31-75
2-59,3-8
30-77,5-29
30-82,30-85
6-99,6-99
41-81,81-99
30-57,25-58
82-83,23-83
22-36,21-37
8-85,9-9
25-36,25-26
10-82,81-82
65-93,65-65
21-75,22-75
95-98,47-96
42-68,41-69
96-97,43-96
1-2,2-99
56-77,42-56
56-66,58-66
95-95,1-95
60-71,61-72
42-79,7-69
4-6,5-73
39-88,27-40
28-62,27-63
21-99,3-22
15-65,60-73
52-98,51-52
3-98,97-97
30-43,29-42
31-70,30-71
26-85,84-86
45-90,3-90
53-57,52-58
59-86,85-87
41-43,42-78
5-89,7-89
76-93,94-94
3-97,1-98
31-72,6-72
5-90,4-89
12-62,17-62
6-7,7-47
24-55,9-17
17-60,59-59
63-76,62-65
3-77,4-34
17-52,17-18
63-99,88-98
2-42,16-35
85-85,17-85
25-38,10-38
16-17,17-86
50-89,42-89
47-98,97-98
9-63,10-10
16-46,15-16
41-60,94-94
28-60,3-29";

    #endregion
}