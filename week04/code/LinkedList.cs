using System.Collections;

public class LinkedList : IEnumerable<int>
{
    private Node? _head;
    private Node? _tail;

    /// <summary>
    /// Insert a new node at the front (i.e. the head) of the linked list.
    /// </summary>
    public void InsertHead(int value)
    {
        // Create new node
        Node newNode = new(value);
        // If the list is empty, then point both head and tail to the new node.
        if (_head is null)
        {
            _head = newNode;
            _tail = newNode;
        }
        // If the list is not empty, then only head will be affected.
        else
        {
            newNode.Next = _head; // Connect new node to the previous head
            _head.Prev = newNode; // Connect the previous head to the new node
            _head = newNode; // Update the head to point to the new node
        }
    }

    /// <summary>
    /// Insert a new node at the back (i.e. the tail) of the linked list.
    /// </summary>
    public void InsertTail(int value)
    {
        // PLAN (Problem 1 - InsertTail):
        // Mirror image of InsertHead, but working on the tail end of the list.
        // 1. Create the new node.
        // 2. If the list is empty (_tail is null), head and tail both point to it.
        // 3. Otherwise: newNode.Prev = _tail, _tail.Next = newNode, then move _tail.
        // Big O: O(1) - we already hold a pointer to the tail, so there is no traversal.

        // Create new node
        Node newNode = new(value);
        // If the list is empty, then point both head and tail to the new node.
        if (_tail is null)
        {
            _head = newNode;
            _tail = newNode;
        }
        // If the list is not empty, then only tail will be affected.
        else
        {
            newNode.Prev = _tail; // Connect new node back to the previous tail
            _tail.Next = newNode; // Connect the previous tail forward to the new node
            _tail = newNode; // Update the tail to point to the new node
        }
    }


    /// <summary>
    /// Remove the first node (i.e. the head) of the linked list.
    /// </summary>
    public void RemoveHead()
    {
        // If the list has only one item in it, then set head and tail 
        // to null resulting in an empty list.  This condition will also
        // cover an empty list.  Its okay to set to null again.
        if (_head == _tail)
        {
            _head = null;
            _tail = null;
        }
        // If the list has more than one item in it, then only the head
        // will be affected.
        else if (_head is not null)
        {
            _head.Next!.Prev = null; // Disconnect the second node from the first node
            _head = _head.Next; // Update the head to point to the second node
        }
    }


    /// <summary>
    /// Remove the last node (i.e. the tail) of the linked list.
    /// </summary>
    public void RemoveTail()
    {
        // PLAN (Problem 2 - RemoveTail):
        // Mirror image of RemoveHead.
        // 1. If the list is empty or has exactly one node (_head == _tail),
        //    set both head and tail to null, which leaves an empty list.
        // 2. Otherwise disconnect the last node: _tail.Prev.Next = null, then move _tail back.
        // Big O: O(1) - the Prev pointer gives direct access to the second to last node.

        // If the list has only one item in it, then set head and tail
        // to null resulting in an empty list.  This condition will also
        // cover an empty list.  Its okay to set to null again.
        if (_head == _tail)
        {
            _head = null;
            _tail = null;
        }
        // If the list has more than one item in it, then only the tail
        // will be affected.
        else if (_tail is not null)
        {
            _tail.Prev!.Next = null; // Disconnect the second to last node from the last node
            _tail = _tail.Prev; // Update the tail to point to the second to last node
        }
    }

    /// <summary>
    /// Insert 'newValue' after the first occurrence of 'value' in the linked list.
    /// </summary>
    public void InsertAfter(int value, int newValue)
    {
        // Search for the node that matches 'value' by starting at the 
        // head of the list.
        Node? curr = _head;
        while (curr is not null)
        {
            if (curr.Data == value)
            {
                // If the location of 'value' is at the end of the list,
                // then we can call insert_tail to add 'new_value'
                if (curr == _tail)
                {
                    InsertTail(newValue);
                }
                // For any other location of 'value', need to create a 
                // new node and reconnect the links to insert.
                else
                {
                    Node newNode = new(newValue);
                    newNode.Prev = curr; // Connect new node to the node containing 'value'
                    newNode.Next = curr.Next; // Connect new node to the node after 'value'
                    curr.Next!.Prev = newNode; // Connect node after 'value' to the new node
                    curr.Next = newNode; // Connect the node containing 'value' to the new node
                }

                return; // We can exit the function after we insert
            }

            curr = curr.Next; // Go to the next node to search for 'value'
        }
    }

    /// <summary>
    /// Remove the first node that contains 'value'.
    /// </summary>
    public void Remove(int value)
    {
        // PLAN (Problem 3 - Remove):
        // 1. Walk from the head looking for the FIRST node whose Data == value.
        // 2. When it is found there are three cases:
        //    a. It is the head reuse RemoveHead() (this also covers a one item list).
        //    b. It is the tail -> reuse RemoveTail().
        //    c. It is in the middle -> bypass it: curr.Next.Prev = curr.Prev and
        //       curr.Prev.Next = curr.Next, so nothing points at 'curr' anymore.
        // 3. Return right away: only the first match is removed.
        // 4. If the loop ends without a match, the list is left untouched.
        // Big O: O(n) - the search dominates; the unlink itself is O(1).

        Node? curr = _head;
        while (curr is not null)
        {
            if (curr.Data == value)
            {
                // Head case (also covers the one item list, because RemoveHead
                // already handles the _head == _tail condition).
                if (curr == _head)
                {
                    RemoveHead();
                }
                // Tail case
                else if (curr == _tail)
                {
                    RemoveTail();
                }
                // Middle case: reconnect the two neighbors directly to each other
                else
                {
                    curr.Next!.Prev = curr.Prev; // Node after 'curr' points back to the node before 'curr'
                    curr.Prev!.Next = curr.Next; // Node before 'curr' points forward past 'curr'
                }

                return; // Stop searching once the first match has been removed
            }

            curr = curr.Next; // Go to the next node to keep searching for 'value'
        }
    }

    /// <summary>
    /// Search for all instances of 'oldValue' and replace the value to 'newValue'.
    /// </summary>
    public void Replace(int oldValue, int newValue)
    {
        // PLAN (Problem 4 - Replace):
        // 1. Walk the whole list from the head. Unlike Remove, there is no early exit.
        // 2. Every time Data == oldValue, overwrite it with newValue.
        // 3. No pointer is touched - only the value inside the node - so the shape
        //    of the list stays exactly the same.
        // Big O: O(n) - one full pass over the list.

        Node? curr = _head;
        while (curr is not null)
        {
            if (curr.Data == oldValue)
            {
                curr.Data = newValue; // Only the value changes; the links stay intact
            }

            curr = curr.Next; // Keep going: ALL matches have to be replaced
        }
    }

    /// <summary>
    /// Yields all values in the linked list
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator()
    {
        // call the generic version of the method
        return this.GetEnumerator();
    }

    /// <summary>
    /// Iterate forward through the Linked List
    /// </summary>
    public IEnumerator<int> GetEnumerator()
    {
        var curr = _head; // Start at the beginning since this is a forward iteration.
        while (curr is not null)
        {
            yield return curr.Data; // Provide (yield) each item to the user
            curr = curr.Next; // Go forward in the linked list
        }
    }

    /// <summary>
    /// Iterate backward through the Linked List
    /// </summary>
    public IEnumerable Reverse()
    {
        // PLAN (Problem 5 - Reverse):
        // Same idea as GetEnumerator, but walking the other way around:
        // start at the tail and follow the Prev pointers until we fall off the front.
        // 'yield return' hands one value at a time to the foreach loop and pauses here.
        // Big O: O(n) to walk the whole list, O(1) extra memory (no copy of the list is made).

        var curr = _tail; // Start at the end since this is a backward iteration.
        while (curr is not null)
        {
            yield return curr.Data; // Provide (yield) each item to the user
            curr = curr.Prev; // Go backward in the linked list
        }
    }

    public override string ToString()
    {
        return "<LinkedList>{" + string.Join(", ", this) + "}";
    }

    // Just for testing.
    public Boolean HeadAndTailAreNull()
    {
        return _head is null && _tail is null;
    }

    // Just for testing.
    public Boolean HeadAndTailAreNotNull()
    {
        return _head is not null && _tail is not null;
    }
}

public static class IntArrayExtensionMethods {
    public static string AsString(this IEnumerable array) {
        return "<IEnumerable>{" + string.Join(", ", array.Cast<int>()) + "}";
    }
}