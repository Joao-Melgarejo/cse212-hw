public static class Trees
{
    /// <summary>
    /// Given a sorted list (sorted_list), create a balanced BST.  If the values in the
    /// sortedNumbers were inserted in order from left to right into the BST, then it
    /// would resemble a linked list (unbalanced). To get a balanced BST, the
    /// InsertMiddle function is called to find the middle item in the list to add
    /// first to the BST. The InsertMiddle function takes the whole list but also takes
    /// a range (first to last) to consider.  For the first call, the full range of 0 to
    /// Length-1 used.
    /// </summary>
    public static BinarySearchTree CreateTreeFromSortedList(int[] sortedNumbers)
    {
        var bst = new BinarySearchTree(); // Create an empty BST to start with 
        InsertMiddle(sortedNumbers, 0, sortedNumbers.Length - 1, bst);
        return bst;
    }

    /// <summary>
    /// This function will attempt to insert the item in the middle of 'sortedNumbers' into
    /// the 'bst' tree. The middle is determined by using indices represented by 'first' and
    /// 'last'.
    /// For example, if the function was called on:
    ///
    /// sortedNumbers = new[]{10, 20, 30, 40, 50, 60};
    /// first = 0;
    /// last = 5;
    /// 
    /// then the value 30 (index 2 which is the middle) would be added 
    /// to the 'bst' (the insert function in the <see cref="BinarySearchTree"/> can be used
    /// to do this).   
    ///
    /// Subsequent recursive calls are made to insert the middle from the values 
    /// before 30 and the values after 30.  If done correctly, the order
    /// in which values are added (which results in a balanced bst) will be:
    /// 
    /// 30, 10, 20, 50, 40, 60
    /// 
    /// This function is intended to be called the first time by CreateTreeFromSortedList.
    ///
    /// The purpose for having the first and last parameters is so that we do 
    /// not need to create new sub-lists when we make recursive calls.  Avoid 
    /// using list slicing to create sub-lists to solve this problem.    
    /// </summary>
    /// <param name="sortedNumbers">input numbers that are already sorted</param>
    /// <param name="first">the first index in the sortedNumbers to insert</param>
    /// <param name="last">the last index in the sortedNumbers to insert</param>
    /// <param name="bst">the BinarySearchTree in which to insert the values</param>
    private static void InsertMiddle(int[] sortedNumbers, int first, int last, BinarySearchTree bst)
    {
        // TODO Start Problem 5

        // PLAN (Problem 5 - Build a balanced BST from a sorted list)
        // 1. Understand: inserting a sorted list from left to right degenerates the BST
        //    into a linked list (height n, search O(n)). We want the tree balanced.
        // 2. Key idea: whichever value is inserted FIRST becomes the root of that
        //    range. If we always insert the MIDDLE of the range first, the values that
        //    are left over split into two halves of (almost) the same size, and the
        //    same reasoning applies recursively to each half.
        // 3. Design (recursive):
        //      - base case: first > last  -> the range is empty, insert nothing
        //      - middle = first + (last - first) / 2
        //        (same value as (first + last) / 2 but it cannot overflow int)
        //      - insert sortedNumbers[middle] into the bst
        //      - recurse on the left half  (first,      middle - 1)
        //      - recurse on the right half (middle + 1, last)
        //    Only indices are passed around: no slicing, no sub-arrays, no copies.
        // 4. Trace for {10, 20, 30, 40, 50, 60} with first=0, last=5:
        //      (0,5) mid=2 -> insert 30
        //        (0,1) mid=0 -> insert 10
        //          (0,-1) empty        (1,1) mid=1 -> insert 20
        //        (3,5) mid=4 -> insert 50
        //          (3,3) mid=3 -> insert 40        (5,5) mid=5 -> insert 60
        //    Insertion order 30, 10, 20, 50, 40, 60 -> height 3 (balanced)
        // 5. Performance: O(n log n) - n insertions, each one O(log n) because the tree
        //    stays balanced. Extra memory O(log n) for the recursion stack.

        // Base case: an empty range (this also covers an empty input array, where the
        // first call arrives as first = 0 and last = -1).
        if (first > last)
            return;

        var middle = first + (last - first) / 2;
        bst.Insert(sortedNumbers[middle]);

        // Smaller problems: the values before the middle and the values after it.
        InsertMiddle(sortedNumbers, first, middle - 1, bst);
        InsertMiddle(sortedNumbers, middle + 1, last, bst);
    }
}