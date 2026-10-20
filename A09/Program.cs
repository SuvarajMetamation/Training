// --------------------------------------------------------------------------------------------
// Training ~ A training program for new joinee at Trumpf Metamation, Batch - July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------
// Program.cs
// A09: Implementation of generic type of Queue using array.
// --------------------------------------------------------------------------------------------
namespace A09;

using static System.Console;
using static System.ConsoleColor;

#region class Program -----------------------------------------------------------------------------
class Program {
   #region Method ---------------------------------------------------
   static void Main () => MyQueueTests.Run ();
   #endregion
}
#endregion

#region class MyQueue -----------------------------------------------------------------------------
class MyQueue<T> {
   #region Constructors ---------------------------------------------
   public MyQueue (params T[] data) { foreach (T d in data) Enqueue (d); }
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Adds an item to the tail of the queue,increasing the array capacity if full.
   /// </summary>
   public void Enqueue (T item) {
      if (IsFull) {
         int size = Size;
         var temp = new T[size * 2];
         for (int i = 0; i < Count; i++) temp[i] = mArray[(mStart + i) % size];
         mArray = temp;
         mStart = 0;
         mEnd = Count;
      }
      mArray[mEnd] = item;
      mEnd = (mEnd + 1) % Size;
      Count++;
   }

   /// <summary>Removes and returns the item at the head of the queue.</summary>
   public T Dequeue () {
      if (IsEmpty) throw new InvalidOperationException ("Oops! There is no items in queue");
      T item = mArray[mStart];
      mArray[mStart] = default!;
      mStart = ++mStart % Size;
      Count--;
      return item;
   }
   #endregion

   #region Properties ----------------------------------------------- 
   /// <summary>Gets a value indicating whether the queue contains no items.</summary>
   public bool IsEmpty => Count == 0;
   /// <summary>Gets a value indicating whether the queue has reached its capacity.</summary>
   public bool IsFull => Count == Size;
   /// <summary>Gets the total number of items the queue can hold before resizing.</summary>
   public int Size => mArray.Length;
   /// <summary>Gets or sets the current number of items in the queue.</summary>
   public int Count;
   #endregion

   #region Private data ---------------------------------------------
   int mStart, mEnd;
   T[] mArray = new T[4];
   #endregion
}
#endregion

#region class MyQueueTests ------------------------------------------------------------------------
class MyQueueTests {
   #region Methods --------------------------------------------------
   public static void Run () {
      TestQueue (new MyQueue<int> (16, 18, 20, 22), 24, 18, "INT");
      TestQueue (new MyQueue<string> ("John", "Jane", "Alice", "Bob"), "Test", "John", "STRING");
      TestQueue (new MyQueue<double> (10.5, 20.5, 30.5, 40.5), 50.5, 10.5, "DOUBLE");
   }

   public static void TestQueue<T> (MyQueue<T> queue, T newItem, T expectedFirst, string type) {
      WriteLine ($"--------------------{type}--------------------------");
      Assert (queue.Count == 4, "Count");
      Assert (queue.IsFull, "IsFull");
      queue.Enqueue (newItem);
      Assert (queue.Size == 8, "Capacity After Resize");
      Assert (queue.Count == 5, "Count After Enqueue");
      Assert (queue.Dequeue ().Equals (expectedFirst), "Dequeue");
      Assert (queue.Count == 4, "Count After Dequeue");
      while (!queue.IsEmpty) queue.Dequeue ();
      Assert (queue.IsEmpty, "IsEmpty after Dequeue");
      try {
         queue.Dequeue ();
         Assert (false, "Dequeue when empty -- Exception not thrown");
      } catch (InvalidOperationException e) {
         Assert (e.Message == "Oops! There is no items in queue", "Empty Queue Exception");
      }
   }

   // Test the queue implementation
   static void Assert (bool condition, string testName) {
      ForegroundColor = condition ? DarkGreen : Red;
      WriteLine ($"{testName,-32} --- {(condition ? "Passed" : "Failed")}");
      ResetColor ();
      if (!condition) return;
   }
   #endregion
}
#endregion
