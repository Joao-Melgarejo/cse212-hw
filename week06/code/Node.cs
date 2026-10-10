public class Node
{
    public int Data { get; set; }
    public Node? Right { get; private set; }
    public Node? Left { get; private set; }

    public Node(int data)
    {
        this.Data = data;
    }

    public void Insert(int value)
    {
        // TODO Start Problem 1

        // PLAN (Problem 1 - Insert unique values only)
        // 1. Understand: the BST must behave like a sorted SET, so a value that is
        //    already stored must not be added a second time.
        // 2. Find the defect: the original code used "if (value < Data) ... else ...",
        //    so a value EQUAL to Data fell into the else branch and was inserted again
        //    on the right side. We need a third case for equality.
        // 3. Design (recursive):
        //      - value == Data -> duplicate: stop, do nothing (new base case)
        //      - value <  Data -> go left : if Left is null insert here, else recurse
        //      - value >  Data -> go right: if Right is null insert here, else recurse
        // 4. Base cases: "duplicate found" and "empty spot found".
        // 5. Performance: O(log n) on a balanced tree, O(n) on a degenerate one.

        if (value == Data)
        {
            // Duplicate found: the value is already in the tree, so there is nothing
            // to insert. Returning here also stops the recursion.
            return;
        }

        if (value < Data)
        {
            // Insert to the left
            if (Left is null)
                Left = new Node(value);
            else
                Left.Insert(value);
        }
        else
        {
            // Insert to the right
            if (Right is null)
                Right = new Node(value);
            else
                Right.Insert(value);
        }
    }

    public bool Contains(int value)
    {
        // TODO Start Problem 2

        // PLAN (Problem 2 - Contains)
        // 1. Understand: return true if 'value' is stored anywhere in this subtree.
        // 2. Design: navigate exactly like Insert does, but instead of creating a node
        //    when we reach an empty spot, we report that the value was not found.
        //      - value == Data -> found      -> return true
        //      - value <  Data -> it could only be on the left. If Left is null the
        //                         value is not in the tree; otherwise ask Left.
        //      - value >  Data -> mirror of the previous case using Right.
        // 3. Base cases: value found (true) and empty subtree reached (false).
        // 4. Performance: O(log n) balanced / O(n) worst case. Every comparison throws
        //    away one whole subtree, which is why the search is logarithmic.

        if (value == Data)
            return true;

        if (value < Data)
            return Left is not null && Left.Contains(value);

        return Right is not null && Right.Contains(value);
    }

    public int GetHeight()
    {
        // TODO Start Problem 4

        // PLAN (Problem 4 - Tree height)
        // 1. Understand: the height of a node is the number of nodes on the longest
        //    path from this node down to a leaf. A single node (a leaf) has height 1.
        // 2. Design (recursive definition given in the assignment):
        //      height(node) = 1 + max(height(node.Left), height(node.Right))
        //    A missing child contributes 0, so a leaf returns 1 + max(0, 0) = 1.
        // 3. Base case: we never recurse into a null child, so the recursion always
        //    stops at the leaves. (BinarySearchTree.GetHeight already returns 0 when
        //    the whole tree is empty, so this method is never called on null.)
        // 4. Performance: O(n) - every node must be visited once; there is no way to
        //    skip a subtree because the tallest branch could be anywhere.

        int leftHeight = Left is null ? 0 : Left.GetHeight();
        int rightHeight = Right is null ? 0 : Right.GetHeight();
        return 1 + Math.Max(leftHeight, rightHeight);
    }
}