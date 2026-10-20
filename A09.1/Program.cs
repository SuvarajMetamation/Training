// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinee at Trumpf Metamation, Batch - July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------
// Program.cs
// A09.1: Implementation of generic type of double ended queue using array.
// ------------------------------------------------------------------------------------------------

namespace A09._1;

using static QueueSide;
using static System.Console;

#region class Program -----------------------------------------------------------------------------
class Program {
   static void Main () {
      MyDQueue<int> queue = new ();
      queue.Enqueue (10, TAIL);
      queue.Enqueue (20, TAIL);
      queue.Enqueue (5, HEAD);
      queue.Enqueue (1, HEAD);
      WriteLine (queue.Dequeue (HEAD)); // 1
      WriteLine (queue.Dequeue (TAIL)); // 20
      WriteLine (queue.Dequeue (HEAD)); // 5
      WriteLine (queue.Dequeue (TAIL)); //10
   }
}
#endregion Program

#region class MyDQueue<T> -------------------------------------------------------------------------
class MyDQueue<T> {
   #region Constructor ----------------------------------------------
   public MyDQueue (int size = 4) => mArray = new T[Math.Max (4, size)];
   #endregion

   #region Methods --------------------------------------------------
   // Resize the array size.
   void EnsureCapacity () {
      if (!IsFull) return;
      T[] temp = new T[Size * 2];
      for (int i = 0; i < mCount; i++) temp[i] = mArray[(mStart + i) % Size];
      mArray = temp;
      mStart = 0;
   }

   /// <summary>
   /// Adds an item to the specified side of the queue.
   /// </summary>
   /// <param name="item"></param>
   /// <param name="side"></param>
   public void Enqueue (T item, QueueSide side) {
      EnsureCapacity ();
      if (side == HEAD) mStart = (mStart - 1 + Size) % Size;
      int index = (mStart + (side == HEAD ? 0 : mCount)) % Size;
      mArray[index] = item;
      mCount++;
   }

   /// <summary>
   /// Removes and returns an item from the specified side of the queue.
   /// </summary>
   /// <param name="side">The side from which the item is removed (HEAD or TAIL).</param>
   /// <returns>The item removed from the queue.</returns>
   /// <exception cref="InvalidOperationException">
   /// Thrown when the queue is empty.
   /// </exception>
   public T Dequeue (QueueSide side) {
      if (mCount == 0) throw new InvalidOperationException ("Queue is empty");
      int index = side == HEAD ? mStart : (mStart + mCount - 1) % Size;
      T item = mArray[index];
      mArray[index] = default!;
      mCount--;
      if (side == HEAD) mStart = (mStart + 1) % Size;
      return item;
   }
   #endregion

   #region Properties -----------------------------------------------
   /// <summary>
   /// Gets the current capacity of the queue.
   /// </summary>
   public int Size => mArray.Length;
   /// <summary>
   /// Gets a value indicating whether the queue is full.
   /// </summary>
   public bool IsFull => mCount == Size;
   #endregion

   #region Private Data ---------------------------------------------
   T[] mArray;
   int mStart, mCount;
   #endregion
}
#endregion MyDQueue<T>

#region enum -----------------------------------------------------
enum QueueSide { HEAD, TAIL }
#endregion
