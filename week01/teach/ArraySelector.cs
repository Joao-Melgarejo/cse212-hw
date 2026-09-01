public static class ArraySelector
{
    public static void Run()
    {
        var l1 = new[] { 1, 2, 3, 4, 5 };
        var l2 = new[] { 2, 4, 6, 8, 10};
        var select = new[] { 1, 1, 1, 2, 2, 1, 2, 2, 2, 1};
        var intResult = ListSelector(l1, l2, select);
        Console.WriteLine("<int[]>{" + string.Join(", ", intResult) + "}"); // <int[]>{1, 2, 3, 2, 4, 4, 6, 8, 10, 5}
    }

    private static int[] ListSelector(int[] list1, int[] list2, int[] select)
    {
        // PLAN:
        // 1. The result has exactly one item per entry of 'select', so its size is select.Length.
        //    Because the size is known ahead of time, a fixed array is enough (no growth needed).
        // 2. Keep two independent "read heads" (indexes): one for list1 and one for list2.
        //    They both start at 0 and only advance when their own array is chosen.
        // 3. Walk through 'select' from left to right:
        //      - a 1 means: take list1[list1Index], then move list1Index forward by one.
        // 4. Store the chosen value at the current position of the result array.
        
        // 5. Return the result array.
    

        var result = new int[select.Length];
        var list1Index = 0;
        var list2Index = 0;

        for (var i = 0; i < select.Length; i++)
        {
            if (select[i] == 1)
            {
                result[i] = list1[list1Index];
                list1Index++;
            }
            else
            {
                result[i] = list2[list2Index];
                list2Index++;
            }
        }

        return result;
    }
}