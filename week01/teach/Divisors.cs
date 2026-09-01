public static class Divisors {
    /// <summary>
    /// Entry point for the Divisors class
    /// </summary>
    public static void Run() {
        List<int> list = FindDivisors(80);
        Console.WriteLine("<List>{" + string.Join(", ", list) + "}"); // <List>{1, 2, 4, 5, 8, 10, 16, 20, 40}
        List<int> list1 = FindDivisors(79);
        Console.WriteLine("<List>{" + string.Join(", ", list1) + "}"); // <List>{1}
    }

    /// <summary>
    /// Create a list of all divisors for a number including 1
    /// and excluding the number itself. Modulo will be used
    /// to test divisibility.
    /// </summary>
    /// <param name="number">The number to find the divisor</param>
    /// <returns>List of divisors</returns>
    private static List<int> FindDivisors(int number) {
        List<int> results = new();

        // Plan:
        // 1. Start an empty dynamic array (List<int>) called results.
        // 2. Try every candidate divisor from 1 up to (but NOT including) number.
        //    - 1 is included because 1 divides every number.
        // 3. A candidate i divides number when the remainder of number / i is zero,
        //    which in C# is written number % i == 0.
        // 4. When that is true, append i to results (append is O(1) amortized).
        // 5. Return results. If number is prime the list will only contain 1.

        for (int i = 1; i < number; i++) {
            if (number % i == 0) {
                results.Add(i);
            }
        }

        return results;
    }
}