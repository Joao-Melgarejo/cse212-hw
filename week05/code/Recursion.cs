using System.Collections;

public static class Recursion
{
    /// <summary>
    /// #############
    /// # Problem 1 #
    /// #############
    /// Using recursion, find the sum of 1^2 + 2^2 + 3^2 + ... + n^2
    /// and return it.  Remember to both express the solution
    /// in terms of recursive call on a smaller problem and
    /// to identify a base case (terminating case).  If the value of
    /// n <= 0, just return 0.   A loop should not be used.
    /// </summary>
    public static int SumSquaresRecursive(int n)
    {
        // ##### PLAN (Problem 1) #####
        // Understand: we need 1^2 + 2^2 + ... + n^2 without using any loop.
        // Smaller problem: the sum up to n is exactly n^2 plus the sum up to n-1,
        //                  so SumSquaresRecursive(n) = n*n + SumSquaresRecursive(n-1).
        // Base case:       n <= 0 means there is nothing left to add, so return 0.
        //                  Every call lowers n by 1, so we always reach it.
        // Performance:     one call per value of n and O(1) work inside each call,
        //                  so this is O(n) time and O(n) stack space.

        // TODO Start Problem 1
        // Base case: nothing left to add.
        if (n <= 0)
            return 0;

        // Recursive case: n^2 plus the answer for the smaller problem (n - 1).
        return (n * n) + SumSquaresRecursive(n - 1);
    }

    /// <summary>
    /// #############
    /// # Problem 2 #
    /// #############
    /// Using recursion, insert permutations of length
    /// 'size' from a list of 'letters' into the results list.  This function
    /// should assume that each letter is unique (i.e. the
    /// function does not need to find unique permutations).
    ///
    /// In mathematics, we can calculate the number of permutations
    /// using the formula: len(letters)! / (len(letters) - size)
    ///
    /// For example, if letters was [A,B,C] and size was 2 then
    /// the following would the contents of the results array after the function ran: AB, AC, BA, BC, CA, CB (might be in
    /// a different order).
    ///
    /// You can assume that the size specified is always valid (between 1
    /// and the length of the letters list).
    /// </summary>
    public static void PermutationsChoose(List<string> results, string letters, int size, string word = "")
    {
        // ##### PLAN (Problem 2) #####
        // Understand: build every ordered arrangement of exactly 'size' letters taken
        //             from 'letters'. 'word' carries the part of the arrangement that
        //             has been built so far; 'letters' carries what is still unused.
        // Smaller problem: pick one of the remaining letters, append it to 'word' and
        //                  recurse with that letter removed from 'letters'. Each call
        //                  makes 'word' one longer and 'letters' one shorter.
        // Base case:       word.Length == size -> the arrangement is complete, add it to
        //                  results and stop going deeper (do NOT keep adding letters).
        // Why a for loop is fine here: the recursion is what explores the depth
        //                  (position by position); the loop only tries the options
        //                  available at the current position.
        // Performance:     O(P * size) where P = letters.Length! / (letters.Length - size)!
        //                  Each of the P branches builds a string of length 'size'.

        // TODO Start Problem 2
        // Base case: the word already has the requested length.
        if (word.Length == size)
        {
            results.Add(word);
            return;
        }

        // Recursive case: try each still-available letter in the next position.
        for (var i = 0; i < letters.Length; i++)
        {
            // Copy of the letters without the one we are using right now,
            // so the same letter cannot be reused inside this branch.
            var lettersLeft = letters.Remove(i, 1);
            PermutationsChoose(results, lettersLeft, size, word + letters[i]);
        }
    }

    /// <summary>
    /// #############
    /// # Problem 3 #
    /// #############
    /// Imagine that there was a staircase with 's' stairs.
    /// We want to count how many ways there are to climb
    /// the stairs.  If the person could only climb one
    /// stair at a time, then the total would be just one.
    /// However, if the person could choose to climb either
    /// one, two, or three stairs at a time (in any order),
    /// then the total possibilities become much more
    /// complicated.  If there were just three stairs,
    /// the possible ways to climb would be four as follows:
    ///
    ///     1 step, 1 step, 1 step
    ///     1 step, 2 step
    ///     2 step, 1 step
    ///     3 step
    ///
    /// With just one step to go, the ways to get
    /// to the top of 's' stairs is to either:
    ///
    /// - take a single step from the second to last step,
    /// - take a double step from the third to last step,
    /// - take a triple step from the fourth to last step
    ///
    /// We don't need to think about scenarios like taking two
    /// single steps from the third to last step because this
    /// is already part of the first scenario (taking a single
    /// step from the second to last step).
    ///
    /// These final leaps give us a sum:
    ///
    /// CountWaysToClimb(s) = CountWaysToClimb(s-1) +
    ///                       CountWaysToClimb(s-2) +
    ///                       CountWaysToClimb(s-3)
    ///
    /// To run this function for larger values of 's', you will need
    /// to update this function to use memoization.  The parameter
    /// 'remember' has already been added as an input parameter to
    /// the function for you to complete this task.
    /// </summary>
    public static decimal CountWaysToClimb(int s, Dictionary<int, decimal>? remember = null)
    {
        // ##### PLAN (Problem 3) #####
        // Understand: the recurrence and the base cases are already given. The real
        //             task is the performance: the plain version recomputes the same
        //             sub-problems over and over (an O(3^n) call tree), so s = 100
        //             would never finish inside the 5 second test timeout.
        // Fix:        memoization. Keep a dictionary key = number of stairs,
        //             value = ways to climb them.
        //             1) create the dictionary only on the very first call
        //                (remember == null) and hand that SAME dictionary down to
        //                every recursive call, otherwise nothing is shared;
        //             2) before doing any work, look 's' up: if it is already stored,
        //                return the stored value immediately;
        //             3) after computing a new value, store it under key 's'.
        // Base case:  s == 0, 1, 2, 3 are answered directly (already provided).
        // Performance: each value of s is computed once -> O(s) time, O(s) memory,
        //              down from O(3^s).

        // Base Cases
        if (s == 0)
            return 0;
        if (s == 1)
            return 1;
        if (s == 2)
            return 2;
        if (s == 3)
            return 4;

        // TODO Start Problem 3
        // First call only: create the dictionary that every deeper call will share.
        remember ??= new Dictionary<int, decimal>();

        // Have we already solved this number of stairs? Then reuse the answer.
        if (remember.TryGetValue(s, out var alreadyKnown))
            return alreadyKnown;

        // Solve using recursion
        decimal ways = CountWaysToClimb(s - 1, remember) + CountWaysToClimb(s - 2, remember) + CountWaysToClimb(s - 3, remember);

        // Remember the result so the other branches do not recompute it.
        remember[s] = ways;
        return ways;
    }

    /// <summary>
    /// #############
    /// # Problem 4 #
    /// #############
    /// A binary string is a string consisting of just 1's and 0's.  For example, 1010111 is
    /// a binary string.  If we introduce a wildcard symbol * into the string, we can say that
    /// this is now a pattern for multiple binary strings.  For example, 101*1 could be used
    /// to represent 10101 and 10111.  A pattern can have more than one * wildcard.  For example,
    /// 1**1 would result in 4 different binary strings: 1001, 1011, 1101, and 1111.
    ///
    /// Using recursion, insert all possible binary strings for a given pattern into the results list.  You might find
    /// some of the string functions like IndexOf and [..X] / [X..] to be useful in solving this problem.
    /// </summary>
    public static void WildcardBinary(string pattern, List<string> results)
    {
        // ##### PLAN (Problem 4) #####
        // Understand: every '*' can be either a '0' or a '1', so a pattern with k
        //             wildcards produces 2^k different binary strings.
        // Smaller problem: locate the FIRST '*' with IndexOf. Build two new patterns,
        //                  one with that position replaced by '0' and one replaced by
        //                  '1', and recurse on both. Each new pattern has one wildcard
        //                  fewer than the one we came from.
        // Base case:       IndexOf returns -1 -> there are no wildcards left, so the
        //                  pattern is already a finished binary string: add it to
        //                  results. (This also covers the empty string "", which has
        //                  no '*' and must produce exactly one result: "".)
        // Performance:     O(2^k * n) with k wildcards and n = pattern length, because
        //                  each of the 2^k leaves rebuilds a string of length n.

        // TODO Start Problem 4
        // Base case: no wildcard left, the pattern is a complete binary string.
        var index = pattern.IndexOf('*');
        if (index == -1)
        {
            results.Add(pattern);
            return;
        }

        // Split the pattern around that first wildcard.
        var before = pattern[..index];
        var after = pattern[(index + 1)..];

        // Recursive case: one branch replaces the wildcard with '0', the other with '1'.
        WildcardBinary(before + "0" + after, results);
        WildcardBinary(before + "1" + after, results);
    }

    /// <summary>
    /// Use recursion to insert all paths that start at (0,0) and end at the
    /// 'end' square into the results list.
    /// </summary>
    public static void SolveMaze(List<string> results, Maze maze, int x = 0, int y = 0, List<ValueTuple<int, int>>? currPath = null)
    {
        // If this is the first time running the function, then we need
        // to initialize the currPath list.
        if (currPath == null) {
            currPath = new List<ValueTuple<int, int>>();
        }

        // currPath.Add((1,2)); // Use this syntax to add to the current path

        // ##### PLAN (Problem 5) #####
        // Understand: find EVERY path from (0,0) to the square holding a 2, and store
        //             each one as a string. The maze helpers do the checking for us:
        //             IsEnd(x,y) says whether we arrived, and IsValidMove(currPath,x,y)
        //             already rejects squares outside the maze, walls (0) and squares
        //             that are already in the current path (no circles).
        // Step 1:     record that we are standing on (x,y) -> currPath.Add((x,y)).
        // Base case:  IsEnd(x,y) is true -> the path in currPath is a full solution, so
        //             add currPath.AsString() to results and stop going deeper from here.
        // Smaller problem: otherwise, try the four neighbours (right, left, down, up).
        //             Each valid neighbour is explored with a currPath that is one
        //             square longer. The board is finite and a square can never repeat
        //             inside one path, so the recursion always terminates.
        // Backtracking: after exploring all the branches that start at (x,y), remove
        //             (x,y) from the END of currPath. Without this the list would keep
        //             growing and the sibling branches would wrongly believe they had
        //             already visited this square.
        // Performance: O(number of paths * path length) in the worst case; this is an
        //             exhaustive search, so it is exponential in the size of the maze.

        // TODO Start Problem 5
        // ADD CODE HERE
        // Step 1: we are standing on this square, so it belongs to the current path.
        currPath.Add((x, y));

        if (maze.IsEnd(x, y))
        {
            // Base case: we reached the end, save a snapshot of the whole path.
            results.Add(currPath.AsString()); // Use this to add your path to the results array keeping track of complete maze solutions when you find the solution.
        }
        else
        {
            // Recursive case: try the four neighbours. IsValidMove filters out
            // boundaries, walls and squares already used in this path.
            if (maze.IsValidMove(currPath, x + 1, y))
                SolveMaze(results, maze, x + 1, y, currPath); // right
            if (maze.IsValidMove(currPath, x - 1, y))
                SolveMaze(results, maze, x - 1, y, currPath); // left
            if (maze.IsValidMove(currPath, x, y + 1))
                SolveMaze(results, maze, x, y + 1, currPath); // down
            if (maze.IsValidMove(currPath, x, y - 1))
                SolveMaze(results, maze, x, y - 1, currPath); // up
        }

        // Backtrack: leave this square before returning to the caller.
        currPath.RemoveAt(currPath.Count - 1);
    }
}