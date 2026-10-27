// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinee at Trumpf Metamation, Batch - July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------
// Program.cs
// A09.1: Implementation of generic type of double ended queue using array.
// ------------------------------------------------------------------------------------------------

namespace A09._1;

using static EQueueSide;
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
      WriteLine (queue.Dequeue (TAIL)); // 10
   }
}
#endregion

#region class MyDQueue<T> -------------------------------------------------------------------------
class MyDQueue<T> {
   #region Constructor ----------------------------------------------
   public MyDQueue () => mData = new T[4];
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Adds an item to the specified side of the queue</summary>
   public void Enqueue (T item, EQueueSide side) {
      EnsureCapacity ();
      int index;
      if (side == HEAD) {
         mStart = (mStart - 1 + mData.Length) % mData.Length;
         index = mStart;
      } else index = (mStart + mCount) % mData.Length;
      mData[index] = item;
      mCount++;
   }

   /// <summary>Removes and returns an item from the specified side of the queue</summary>
   public T Dequeue (EQueueSide side) {
      if (mCount == 0) throw new InvalidOperationException ("Queue is empty");
      int index;
      if (side == HEAD) {
         index = mStart;
         mStart = (mStart + 1) % mData.Length;
      } else index = (mStart + mCount - 1) % mData.Length;
      T item = mData[index];
      mData[index] = default!;
      mCount--;
      return item;
   }
   #endregion

   #region Implementation -------------------------------------------
   // Resize the array
   void EnsureCapacity () {
      if (mCount != mData.Length) return;
      T[] temp = new T[mData.Length * 2];
      for (int i = 0; i < mCount; i++) temp[i] = mData[(mStart + i) % mData.Length];
      mData = temp;
      mStart = 0;
   }
   #endregion

   #region Properties -----------------------------------------------
   /// <summary>Gets the current number of elements present in the array</summary>
   public int Count => mCount;
   #endregion

   #region Private Data ---------------------------------------------
   T[] mData;
   int mStart, mCount;
   #endregion
}
#endregion

#region enum EQueueSide ---------------------------------------------
enum EQueueSide { HEAD, TAIL }
#endregion
