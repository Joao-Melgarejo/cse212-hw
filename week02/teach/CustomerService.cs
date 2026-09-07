/// <summary>
/// Maintain a Customer Service Queue.  Allows new customers to be 
/// added and allows customers to be serviced.
/// </summary>
public class CustomerService {
    public static void Run() {
        // Example code to see what's in the customer service queue:
        // var cs = new CustomerService(10);
        // Console.WriteLine(cs);

        // Test Cases
        //
        // Plan: one test per requirement, written from the requirements only
        // (not from the code), so a wrong implementation cannot hide behind a
        // test that was written to match it.
        //   Req 1 -> Test 5 (invalid max size defaults to 10)
        //   Req 2 -> Test 1 and Test 2 (AddNewCustomer enqueues)
        //   Req 3 -> Test 4 (queue full -> error message)
        //   Req 4 -> Test 1 and Test 2 (ServeCustomer dequeues and displays)
        //   Req 5 -> Test 3 (queue empty -> error message)

        // Test 1
        // Scenario: Add one customer and then serve that customer.
        // Expected Result: The details of the customer that was just added are displayed,
        //                  and the queue ends up empty.
        Console.WriteLine("Test 1");
        var cs = new CustomerService(4);
        cs.AddNewCustomer();
        Console.WriteLine($"Before serving: {cs}");
        cs.ServeCustomer();
        Console.WriteLine($"After serving: {cs}");
        // Defect(s) Found: Defect #1 - ServeCustomer removed index 0 BEFORE reading it, so it
        // displayed the wrong record; with only one customer in the queue it crashed with
        // ArgumentOutOfRangeException.

        Console.WriteLine("=================");

        // Test 2
        // Scenario: Add two customers and serve them both.
        // Expected Result: They are displayed in the same order they were added (FIFO).
        Console.WriteLine("Test 2");
        cs = new CustomerService(4);
        cs.AddNewCustomer();
        cs.AddNewCustomer();
        Console.WriteLine($"Before serving: {cs}");
        cs.ServeCustomer();
        cs.ServeCustomer();
        Console.WriteLine($"After serving: {cs}");
        // Defect(s) Found: None once defect #1 was fixed. AddNewCustomer already appended to the
        // end of the list and ServeCustomer already took index 0, so the FIFO order was correct.

        Console.WriteLine("=================");

        // Test 3
        // Scenario: Serve a customer when the queue is empty.
        // Expected Result: An error message is displayed and the program does not crash.
        Console.WriteLine("Test 3");
        cs = new CustomerService(4);
        cs.ServeCustomer();
        // Defect(s) Found: Defect #2 - ServeCustomer never checked whether the queue was empty,
        // so it crashed with ArgumentOutOfRangeException instead of showing an error message.

        Console.WriteLine("=================");

        // Test 4
        // Scenario: Create a queue with max size 4 and try to add 5 customers.
        // Expected Result: The 5th one is rejected with an error message and the queue keeps size 4.
        Console.WriteLine("Test 4");
        cs = new CustomerService(4);
        cs.AddNewCustomer();
        cs.AddNewCustomer();
        cs.AddNewCustomer();
        cs.AddNewCustomer();
        cs.AddNewCustomer();
        Console.WriteLine($"Service Queue: {cs}");
        // Defect(s) Found: Defect #3 - the room check used > instead of >=, so it let a 5th
        // customer in and the queue grew to maxSize + 1.

        Console.WriteLine("=================");

        // Test 5
        // Scenario: Create the queue with an invalid max size (0 and a negative number).
        // Expected Result: The max size falls back to 10 in both cases.
        Console.WriteLine("Test 5");
        cs = new CustomerService(0);
        Console.WriteLine($"max_size should be 10: {cs}");
        cs = new CustomerService(-5);
        Console.WriteLine($"max_size should be 10: {cs}");
        // Defect(s) Found: None. The constructor already defaulted an invalid size to 10.
    }

    private readonly List<Customer> _queue = new();
    private readonly int _maxSize;

    public CustomerService(int maxSize) {
        if (maxSize <= 0)
            _maxSize = 10;
        else
            _maxSize = maxSize;
    }

    /// <summary>
    /// Defines a Customer record for the service queue.
    /// This is an inner class.  Its real name is CustomerService.Customer
    /// </summary>
    private class Customer {
        public Customer(string name, string accountId, string problem) {
            Name = name;
            AccountId = accountId;
            Problem = problem;
        }

        private string Name { get; }
        private string AccountId { get; }
        private string Problem { get; }

        public override string ToString() {
            return $"{Name} ({AccountId})  : {Problem}";
        }
    }

    /// <summary>
    /// Prompt the user for the customer and problem information.  Put the 
    /// new record into the queue.
    /// </summary>
    private void AddNewCustomer() {
        // Verify there is room in the service queue
        // Plan: the queue is full when it ALREADY holds _maxSize customers, so the
        // comparison has to be >=. With > the queue was allowed to reach _maxSize + 1.
        // if (_queue.Count > _maxSize)  // Defect #3 - off-by-one
        if (_queue.Count >= _maxSize) {
            Console.WriteLine("Maximum Number of Customers in Queue.");
            return;
        }

        Console.Write("Customer Name: ");
        var name = Console.ReadLine()!.Trim();
        Console.Write("Account Id: ");
        var accountId = Console.ReadLine()!.Trim();
        Console.Write("Problem: ");
        var problem = Console.ReadLine()!.Trim();

        // Create the customer object and add it to the queue
        var customer = new Customer(name, accountId, problem);
        _queue.Add(customer);
    }

    /// <summary>
    /// Dequeue the next customer and display the information.
    /// </summary>
    private void ServeCustomer() {
        // Plan:
        //  1) Requirement #5: if the queue is empty we must show an error message,
        //     not touch the list (defect #2 - the check did not exist).
        //  2) Requirement #4: read the customer at the front FIRST, then remove it,
        //     then display it (defect #1 - the original removed before reading).
        if (_queue.Count <= 0) {
            Console.WriteLine("No Customers in the queue."); // Fix for defect #2
            return;
        }

        var customer = _queue[0]; // Fix for defect #1 - read before removing
        _queue.RemoveAt(0);
        Console.WriteLine(customer);
    }

    /// <summary>
    /// Support the WriteLine function to provide a string representation of the
    /// customer service queue object. This is useful for debugging. If you have a 
    /// CustomerService object called cs, then you run Console.WriteLine(cs) to
    /// see the contents.
    /// </summary>
    /// <returns>A string representation of the queue</returns>
    public override string ToString() {
        return $"[size={_queue.Count} max_size={_maxSize} => " + string.Join(", ", _queue) + "]";
    }
}