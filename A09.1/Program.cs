// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------
// Program.cs
// A09.1: Implementation of Generic type of Double Ended Queue using array.
// ------------------------------------------------------------------------------------------------

namespace A09._1;

using static System.Console;

#region class Program -----------------------------------------------------------------------------
class Program {
   static void Main () {
      MyDQueue<int> queue = new ();
      queue.EnqueueTail (10);
      queue.EnqueueTail (20);
      queue.EnqueueHead (5);
      queue.EnqueueHead (1);
      WriteLine (queue.DequeueHead ()); // 1
      WriteLine (queue.DequeueTail ()); // 20
      WriteLine (queue.DequeueHead ()); // 5
      WriteLine (queue.DequeueTail ()); //10
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

   public void EnqueueHead (T item) {
      EnsureCapacity ();
      mStart = (mStart - 1 + Size) % Size;
      mArray[mStart] = item;
      mCount++;
   }

   public void EnqueueTail (T item) {
      EnsureCapacity ();
      mArray[(mStart + mCount) % Size] = item;
      mCount++;
   }

   public T DequeueHead () {
      if (IsEmpty) throw new InvalidOperationException ("Dequeue is Empty");
      T item = mArray[mStart];
      mArray[mStart] = default!;
      mStart = (mStart + 1) % Size;
      mCount--;
      return item;
   }

   public T DequeueTail () {
      if (IsEmpty) throw new InvalidOperationException ("Dequeue is Empty");
      int index = (mStart + mCount - 1) % Size;
      T item = mArray[index];
      mArray[index] = default!;
      mCount--;
      return item;
   }
   #endregion

   #region Properties -----------------------------------------------
   public int Size => mArray.Length;
   public bool IsEmpty => mCount == 0;
   public bool IsFull => mCount == Size;

   public int mCount;
   #endregion

   #region Private Data ---------------------------------------------
   T[] mArray;
   int mStart;
   #endregion
}
#endregion MyDQueue<T>
