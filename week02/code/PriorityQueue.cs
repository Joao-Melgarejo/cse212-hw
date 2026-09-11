public class PriorityQueue
{
    private List<PriorityItem> _queue = new();

    /// <summary>
    /// Add a new value to the queue with an associated priority.  The
    /// node is always added to the back of the queue regardless of 
    /// the priority.
    /// </summary>
    /// <param name="value">The value</param>
    /// <param name="priority">The priority</param>
    public void Enqueue(string value, int priority)
    {
        var newNode = new PriorityItem(value, priority);
        _queue.Add(newNode);
    }

    public string Dequeue()
    {
        if (_queue.Count == 0) // Verify the queue is not empty
        {
            throw new InvalidOperationException("The queue is empty.");
        }

        // Find the index of the item with the highest priority to remove
        //
        // Plan:
        //  1) Scan every element from index 1 to the LAST one. The original loop stopped at
        //     Count - 1 (exclusive), so the last item in the list was never inspected and could
        //     never win. Defect #1 fix: the condition is index < _queue.Count.
        //  2) Keep the FIRST item that has the maximum priority, so ties are broken FIFO
        //     (requirement #3). That means we only move highPriorityIndex on a strictly greater
        //     priority. The original used >=, which kept the LAST tied item instead.
        //     Defect #2 fix: use > instead of >=.
        var highPriorityIndex = 0;
        for (int index = 1; index < _queue.Count; index++) // Defect #1 fix
        {
            if (_queue[index].Priority > _queue[highPriorityIndex].Priority) // Defect #2 fix
                highPriorityIndex = index;
        }

        // Remove and return the item with the highest priority
        //  3) Requirement #2 says Dequeue has to REMOVE the item, not just read it. The original
        //     returned the value and left the item in the list forever.
        //     Defect #3 fix: save the value, remove the item, then return the saved value.
        var value = _queue[highPriorityIndex].Value;
        _queue.RemoveAt(highPriorityIndex); // Defect #3 fix
        return value;
    }

    // DO NOT MODIFY THE CODE IN THIS METHOD
    // The graders rely on this method to check if you fixed all the bugs, so changes to it will cause you to lose points.
    public override string ToString()
    {
        return $"[{string.Join(", ", _queue)}]";
    }
}

internal class PriorityItem
{
    internal string Value { get; set; }
    internal int Priority { get; set; }

    internal PriorityItem(string value, int priority)
    {
        Value = value;
        Priority = priority;
    }

    // DO NOT MODIFY THE CODE IN THIS METHOD
    // The graders rely on this method to check if you fixed all the bugs, so changes to it will cause you to lose points.
    public override string ToString()
    {
        return $"{Value} (Pri:{Priority})";
    }
}