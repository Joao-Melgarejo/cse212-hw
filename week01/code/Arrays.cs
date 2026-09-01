public static class Arrays
{
    /// <summary>
    /// This function will produce an array of size 'length' starting with 'number' followed by multiples of 'number'.  For 
    /// example, MultiplesOf(7, 5) will result in: {7, 14, 21, 28, 35}.  Assume that length is a positive
    /// integer greater than 0.
    /// </summary>
    /// <returns>array of doubles that are the multiples of the supplied number</returns>
    public static double[] MultiplesOf(double number, int length)
    {
        // PLAN (step by step, written before coding):
        // 1. The amount of results is known in advance: it is exactly 'length'.
        //    Because the size never changes, a FIXED array of doubles of size 'length'
        //    is the right structure (no growing/copying is needed, so it is cheaper than a List).
        // 2. The i-th multiple (using a 0-based index i) is number * (i + 1):
        //      index 0 -> number * 1
        //      index 1 -> number * 2
        //      ...
        //      index length-1 -> number * length
        // 3. Loop with an index from 0 to length - 1 and write number * (i + 1) at that index.
        //    NOTE: multiplying (instead of repeatedly adding 'number' to a running total)
        //    avoids accumulating floating point error for fractional values like 1.5.
        // 4. Return the array. The loop runs exactly 'length' times, so this is O(n).

        double[] multiples = new double[length];

        for (int i = 0; i < length; i++)
        {
            multiples[i] = number * (i + 1);
        }

        return multiples;
    }

    /// <summary>
    /// Rotate the 'data' to the right by the 'amount'.  For example, if the data is 
    /// List<int>{1, 2, 3, 4, 5, 6, 7, 8, 9} and an amount is 3 then the list after the function runs should be 
    /// List<int>{7, 8, 9, 1, 2, 3, 4, 5, 6}.  The value of amount will be in the range of 1 to data.Count, inclusive.
    ///
    /// Because a list is dynamic, this function will modify the existing data list rather than returning a new list.
    /// </summary>
    public static void RotateListRight(List<int> data, int amount)
    {
        // PLAN (step by step, written before coding):
        // 1. Rotating right by 'amount' means: the LAST 'amount' items must end up at the FRONT,
        //    keeping their relative order, and everything else shifts right.
        //    Example: {1,2,3,4,5,6,7,8,9} with amount = 3
        //             tail = {7,8,9}  head = {1,2,3,4,5,6}  result = {7,8,9,1,2,3,4,5,6}
        // 2. Compute where the tail starts: startOfTail = data.Count - amount.
        // 3. Copy that tail into a temporary list with GetRange(startOfTail, amount).
        //    (A copy is required, because the next step removes those items from 'data'.)
        // 4. Remove the tail from the original list with RemoveRange(startOfTail, amount).
        // 5. Insert the temporary list at index 0 with InsertRange(0, tail).
        //    InsertRange shifts the remaining items to the right, which is exactly the rotation.
        // 6. The list is modified in place, so nothing is returned.
        //    Edge case amount == data.Count: the whole list is removed and re-inserted, so the
        //    list ends up unchanged, which is the correct result for a full rotation.
        // Performance: GetRange, RemoveRange and InsertRange each move at most n items -> O(n).

        int startOfTail = data.Count - amount;

        List<int> tail = data.GetRange(startOfTail, amount);
        data.RemoveRange(startOfTail, amount);
        data.InsertRange(0, tail);
    }
}