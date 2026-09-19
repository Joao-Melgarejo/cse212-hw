public static class DisplaySums {
    public static void Run() {
        DisplaySumPairs([1, 2, 3, 4, 5, 6, 7, 8, 9, 10]);
        // Should show something like (order does not matter):
        // 6 4
        // 7 3
        // 8 2
        // 9 1 

        Console.WriteLine("------------");
        DisplaySumPairs([-20, -15, -10, -5, 0, 5, 10, 15, 20]);
        // Should show something like (order does not matter):
        // 10 0
        // 15 -5
        // 20 -10

        Console.WriteLine("------------");
        DisplaySumPairs([5, 11, 2, -4, 6, 8, -1]);
        // Should show something like (order does not matter):
        // 8 2
        // -1 11
    }

    /// <summary>
    /// Display pairs of numbers (no duplicates should be displayed) that sum to
    /// 10 using a set in O(n) time.  We are assuming that there are no duplicates
    /// in the list.
    /// </summary>
    /// <param name="numbers">array of integers</param>
    // TODO Problem 2 - This should print pairs of numbers in the given array
    // PLAN (written before implementing):
    // Brute force would be two nested loops testing every pair => O(n^2).
    // Key idea: a pair that sums to 10 is fully determined by one of its members,
    // because the partner of x can only be 10 - x. So we never need to search: we
    // only need to ask "have I already seen 10 - x?", and a set answers that in O(1).
    // 1. Create an empty HashSet<int> named "seen".
    // 2. Walk the array once (=> O(n)). For the current number n:
    //    a. Ask the set if it contains 10 - n.
    //       - Yes: its partner appeared EARLIER in the array, so print the pair now.
    //       - No: do nothing yet; if the partner shows up later, the pair will be
    //    b. Add n to the set so later numbers can find it.
    // 3. Because the pair is only printed when the SECOND member of the pair is
    //    reached, each pair is printed exactly once: "3+7" and "7+3" cannot both fire.
    // Edge case: the value 5. 10 - 5 = 5, and 5 is added to the set only AFTER the
    // check, so a single 5 does not pair with itself. (The list is assumed to have no
    // duplicates, so there is never a second 5.)
    // Performance: n numbers x O(1) per number = O(n) time, O(n) memory.
    private static void DisplaySumPairs(int[] numbers) {
        var seen = new HashSet<int>();

        foreach (var number in numbers) {
            var partner = 10 - number;

            if (seen.Contains(partner))
                Console.WriteLine($"{number} {partner}");

            seen.Add(number);
        }
    }
}