// --------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------
// Program.cs
// A09: Implementation of Generic type of Queue using array.
// --------------------------------------------------------------------------------------------
namespace A09;

using static System.Console;
using static System.ConsoleColor;

#region class Program -----------------------------------------------------------------------------
class Program {
   #region Methods --------------------------------------------------
   static void Main () {
      var queue = new MyQueue<int> (16, 18, 20, 22);
      Assert (queue.Count == 4, "Count");
      Assert (queue.IsFull, "IsFull");
      queue.Enqueue (24);
      Assert (queue.Size == 8, "Capacity After Resize");
      Assert (queue.Count == 5, "Count +");
      Assert (queue.Dequeue () == 10, "Dequeue");
      Assert (queue.Count == 4, "Count after Dequeue");
      for (int i = 0; i <= 3; i++) queue.Dequeue ();
      Assert (queue.IsEmpty, "IsEmpty after Dequeue");
      try {
         Assert (queue.Dequeue () == 0, "Dequeue when empty -- Exception not thrown");
      } catch (InvalidOperationException e) {
         Assert (string.IsNullOrEmpty (e.Message), e.Message);
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

#region class MyQueue -----------------------------------------------------------------------------
class MyQueue<T> {
   #region Constructors ---------------------------------------------
   public MyQueue (params T[] datas) { foreach (T data in datas) Enqueue (data); }
   #endregion

   #region Methods --------------------------------------------------
   public void Enqueue (T item) {
      if (IsFull) {
         int size = Size;
         var temp = new T[size * 2];
         for (int i = 0; i < mCount; i++) temp[i] = mArray[(mStart + i) % size];
         mArray = temp;
         mStart = 0;
         mEnd = mCount;
      }
      mArray[mEnd] = item;
      mEnd = (mEnd + 1) % Size;
      mCount++;
   }

   public T Dequeue () {
      if (IsEmpty) throw new InvalidOperationException ("Oops! There is no items in queue");
      T item = mArray[mStart];
      mArray[mStart] = default!;
      mStart = ++mStart % Size;
      mCount--;
      return item;
   }
   #endregion

   #region Properties -----------------------------------------------
   public bool IsEmpty => mCount == 0;
   public bool IsFull => mCount == Size;
   public int Size => mArray.Length;
   public int Count => mCount;
   #endregion

   #region Private data ---------------------------------------------
   int mStart, mEnd, mCount;
   T[] mArray = new T[4];
   #endregion
}
#endregion