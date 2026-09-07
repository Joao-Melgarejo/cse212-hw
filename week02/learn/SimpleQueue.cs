public class SimpleQueue {
    public static void Run() {
        // Test Cases

        // Test 1
        // Scenario: Enqueue one value and then Dequeue it.
        // Expected Result: It should display 100
        Console.WriteLine("Test 1");
        var queue = new SimpleQueue();
        queue.Enqueue(100);
        var value = queue.Dequeue();
        Console.WriteLine(value);
        // Defect(s) Found: Defect #1 - Dequeue read and removed index 1 instead of index 0.
        // With a single item in the list, index 1 does not exist, so the program crashed with
        // ArgumentOutOfRangeException instead of printing 100.

        Console.WriteLine("------------");

        // Test 2
        // Scenario: Enqueue multiple values and then Dequeue all of them
        // Expected Result: It should display 200, then 300, then 400 in that order
        Console.WriteLine("Test 2");
        queue = new SimpleQueue();
        queue.Enqueue(200);
        queue.Enqueue(300);
        queue.Enqueue(400);
        value = queue.Dequeue();
        Console.WriteLine(value);
        value = queue.Dequeue();
        Console.WriteLine(value);
        value = queue.Dequeue();
        Console.WriteLine(value);
        // Defect(s) Found: Defect #2 - Enqueue inserted at index 0 (the front), so the list was
        // reversed and the queue behaved like a stack (LIFO). Combined with defect #1 the output
        // was 300, 200 and then a crash, instead of 200, 300, 400.

        Console.WriteLine("------------");

        // Test 3
        // Scenario: Dequeue from an empty Queue
        // Expected Result: An exception should be raised
        Console.WriteLine("Test 3");
        queue = new SimpleQueue();
        try {
            queue.Dequeue();
            Console.WriteLine("Oops ... This shouldn't have worked.");
        }
        catch (IndexOutOfRangeException) {
            Console.WriteLine("I got the exception as expected.");
        }
        // Defect(s) Found: None. The empty check was already correct and it throws
        // IndexOutOfRangeException as requirement #3 demands.
    }

    private readonly List<int> _queue = new();

    /// <summary>
    /// Enqueue the value provided into the queue
    /// </summary>
    /// <param name="value">Integer value to add to the queue</param>
    private void Enqueue(int value) {
        // Plan: requirement #1 says a new item goes to the BACK of the queue.
        // In a List the back is the end, so use Add (which appends) instead of
        // Insert(0, ...) which put the item at the front.
        _queue.Add(value); // Fix for defect #2
    }

    /// <summary>
    /// Dequeue the next value and return it
    /// </summary>
    /// <exception cref="IndexOutOfRangeException">If queue is empty</exception>
    /// <returns>First integer in the queue</returns>
    private int Dequeue() {
        if (_queue.Count <= 0)
            throw new IndexOutOfRangeException();

        // Plan: requirement #2 says we remove from the FRONT of the queue.
        // In a List the front is index 0, not index 1.
        var value = _queue[0]; // Fix for defect #1
        _queue.RemoveAt(0);    // Fix for defect #1
        return value;
    }
}