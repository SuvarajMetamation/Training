// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------
// Program.cs
// A09.1: Implementation of Generic type of Double Ended Queue using array.
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
   void EnsureCapacity () {
      if (!IsFull) return;
      T[] newArray = new T[Size * 2];
      for (int i = 0; i < mCount; i++) newArray[i] = mArray[(mStart + i) % Size];
      mArray = newArray;
      mStart = 0;
   }

   public void Enqueue (T item, QueueSide side) {
      EnsureCapacity ();
      if (side == HEAD) mStart = (mStart - 1 + Size) % Size;
      int index = (mStart + (side == HEAD ? 0 : mCount)) % Size;
      mArray[index] = item;
      mCount++;
   }

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
   public int Size => mArray.Length;
   public bool IsFull => mCount == Size;
   public int mCount;
   #endregion

   #region Private Data ---------------------------------------------
   T[] mArray;
   int mStart;
   #endregion
}
#endregion MyDQueue<T>

#region enum -----------------------------------------------------
enum QueueSide { HEAD, TAIL }
#endregion
