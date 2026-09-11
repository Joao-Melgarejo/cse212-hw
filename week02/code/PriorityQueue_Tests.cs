using Microsoft.VisualStudio.TestTools.UnitTesting;

// TODO Problem 2 - Write and run test cases and fix the code to match requirements.

[TestClass]
public class PriorityQueueTests
{
    // Plan: the tests below were written from the four requirements ONLY, before reading
    // PriorityQueue.cs, so a wrong implementation cannot hide behind a test shaped to fit it.
    //   Req 1 (Enqueue adds to the back)             -> TestPriorityQueue_1
    //   Req 2 (Dequeue removes the highest priority) -> TestPriorityQueue_2, _3, _5
    //   Req 3 (ties are broken FIFO)                 -> TestPriorityQueue_4
    //   Req 4 (empty queue throws)                   -> TestPriorityQueue_6

    [TestMethod]
    // Scenario: Enqueue three values with different priorities and inspect the queue without
    // removing anything. Requirement 1 says an item always goes to the BACK, whatever its priority.
    // Expected Result: The queue reads front-to-back in insertion order:
    //                  [Alpha (Pri:1), Bravo (Pri:9), Charlie (Pri:5)]
    // Defect(s) Found: None for Enqueue. It already appended with _queue.Add, which is correct.
    public void TestPriorityQueue_1()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("Alpha", 1);
        priorityQueue.Enqueue("Bravo", 9);
        priorityQueue.Enqueue("Charlie", 5);

        Assert.AreEqual("[Alpha (Pri:1), Bravo (Pri:9), Charlie (Pri:5)]", priorityQueue.ToString());
    }

    [TestMethod]
    // Scenario: Enqueue three values whose highest priority sits in the MIDDLE of the queue and
    // dequeue once. Requirement 2 says the highest priority item is removed and its value returned.
    // Expected Result: Dequeue returns "Bravo", and Bravo is gone from the queue afterwards:
    //                  [Alpha (Pri:1), Charlie (Pri:5)]
    // Defect(s) Found: Defect #3 - Dequeue read the value but never removed the item, so the
    // queue still showed [Alpha (Pri:1), Bravo (Pri:9), Charlie (Pri:5)] after dequeuing and every
    // later Dequeue returned Bravo again. Fixed by adding _queue.RemoveAt(highPriorityIndex).
    public void TestPriorityQueue_2()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("Alpha", 1);
        priorityQueue.Enqueue("Bravo", 9);
        priorityQueue.Enqueue("Charlie", 5);

        Assert.AreEqual("Bravo", priorityQueue.Dequeue());
        Assert.AreEqual("[Alpha (Pri:1), Charlie (Pri:5)]", priorityQueue.ToString());
    }

    [TestMethod]
    // Scenario: Put the highest priority item at the very END of the queue and dequeue.
    // This is the boundary case of the search loop: the last element must be able to win.
    // Expected Result: Dequeue returns "Charlie" (priority 9) and leaves [Alpha (Pri:1), Bravo (Pri:5)]
    // Defect(s) Found: Defect #1 - the search loop ran while index < _queue.Count - 1, so it never
    // looked at the last element. It returned "Bravo" instead of "Charlie". Fixed by looping
    // while index < _queue.Count.
    public void TestPriorityQueue_3()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("Alpha", 1);
        priorityQueue.Enqueue("Bravo", 5);
        priorityQueue.Enqueue("Charlie", 9);

        Assert.AreEqual("Charlie", priorityQueue.Dequeue());
        Assert.AreEqual("[Alpha (Pri:1), Bravo (Pri:5)]", priorityQueue.ToString());
    }

    [TestMethod]
    // Scenario: Three items share the same highest priority. Requirement 3 says the one closest
    // to the FRONT must go first (FIFO among equals).
    // Expected Result: Dequeue returns "First", then "Second", then "Third", and the low priority
    //                  "Low" is the last one left.
    // Defect(s) Found: Defect #2 - the comparison used >= , which kept moving the index to the LAST
    // tied item seen, so FIFO among equals was broken. Observed before the fix: Dequeue returned
    // "Second" instead of "First" (it would have been "Third", but defect #1 stopped the loop one
    // element early). Fixed by using > so the first item with the maximum priority is kept.
    public void TestPriorityQueue_4()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("First", 7);
        priorityQueue.Enqueue("Low", 2);
        priorityQueue.Enqueue("Second", 7);
        priorityQueue.Enqueue("Third", 7);

        Assert.AreEqual("First", priorityQueue.Dequeue());
        Assert.AreEqual("Second", priorityQueue.Dequeue());
        Assert.AreEqual("Third", priorityQueue.Dequeue());
        Assert.AreEqual("[Low (Pri:2)]", priorityQueue.ToString());
    }

    [TestMethod]
    // Scenario: Drain a mixed queue completely, then confirm that a queue emptied by Dequeue
    // behaves like a brand new empty queue.
    // Expected Result: The values come out in priority order High(10), Mid(6), Low(3), Lowest(1),
    //                  the queue prints [] and the next Dequeue throws.
    // Defect(s) Found: Same defects #1, #2 and #3 as above; this test is the regression check that
    // all three are fixed at once. Nothing new.
    public void TestPriorityQueue_5()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("Lowest", 1);
        priorityQueue.Enqueue("High", 10);
        priorityQueue.Enqueue("Low", 3);
        priorityQueue.Enqueue("Mid", 6);

        Assert.AreEqual("High", priorityQueue.Dequeue());
        Assert.AreEqual("Mid", priorityQueue.Dequeue());
        Assert.AreEqual("Low", priorityQueue.Dequeue());
        Assert.AreEqual("Lowest", priorityQueue.Dequeue());
        Assert.AreEqual("[]", priorityQueue.ToString());

        try
        {
            priorityQueue.Dequeue();
            Assert.Fail("Exception should have been thrown after draining the queue.");
        }
        catch (InvalidOperationException e)
        {
            Assert.AreEqual("The queue is empty.", e.Message);
        }
    }

    [TestMethod]
    // Scenario: Dequeue from a queue that has never had anything in it.
    // Expected Result: Requirement 4 - an InvalidOperationException whose message is exactly
    //                  "The queue is empty."
    // Defect(s) Found: None. The empty check and the exception message were already correct.
    public void TestPriorityQueue_6()
    {
        var priorityQueue = new PriorityQueue();

        try
        {
            priorityQueue.Dequeue();
            Assert.Fail("Exception should have been thrown.");
        }
        catch (InvalidOperationException e)
        {
            Assert.AreEqual("The queue is empty.", e.Message);
        }
        catch (AssertFailedException)
        {
            throw;
        }
        catch (Exception e)
        {
            Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
        }
    }

    // Add more test cases as needed below.
}