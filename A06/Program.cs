// --------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------
// Program.cs
// A06: Program to print unique solutions for the 8-Queens problem.
// --------------------------------------------------------------------------------------------
namespace A06;

using static Console;

#region class Program -----------------------------------------------------------------------------
class Program {
   #region Methods --------------------------------------------------
   static void Main () {
      List<int[]> solutions = [];
      int[] board = new int[8];
      Solve (0,board,solutions);
      WriteLine ($"Total solutions: {solutions.Count}");
      WriteLine ("\nAll Solutions");
      WriteSolutions (solutions);
      var canonicalSolution = solutions.Where (IsCanonical).ToList ();
      WriteLine ($"\nCanonical solutions: {canonicalSolution.Count}");
      WriteLine ("\nUnique Solutions");
      WriteSolutions(canonicalSolution);
   }

   static void Solve (int row, int[] board, List<int[]> solutions) {
      if (row >= board.Length) { solutions.Add ((int[])board.Clone ()); return; }
      for (int column = 0; column < board.Length; column++) {
         if (!IsSafe (row, column, board)) continue;
            board[row] = column;
            Solve (row + 1, board, solutions);    
      }
   }

   static bool IsSafe (int row, int column, int[] board) {
      for (int previousRow = 0; previousRow < row; previousRow++) {
         int previousColumn = board[previousRow];
         if (previousColumn == column) return false;
         if (Math.Abs (previousRow - row) == Math.Abs (previousColumn - column)) return false;
      }
      return true;
   }

   static IEnumerable<int[]> GetSymmetries (int[] solution) {
      var board = solution;
      for (int rotation = 0; rotation < 4; rotation++) {
         yield return board;
         yield return Mirror (board);
         board = Rotate90 (board);
      }
   }

   static int[] Rotate90 (int[] solution) {
      var rotated = new int[8];
      for (int row = 0; row < 8; row++) {
         int column = solution[row];
         rotated[column] = 7 - row;
      }
      return rotated;
   }

   static int[] Mirror (int[] solution) {
      var mirrored = new int[8];
      for (int row = 0; row < 8; row++)
         mirrored[row] = 7 - solution[row];
      return mirrored;
   }

   static string Encode (int[] solution) => string.Join (",", solution);

   static bool IsCanonical (int[] solution) {
      var original = Encode (solution);
      var minimum = GetSymmetries (solution).Select (Encode).Min ();
      return original == minimum;
   }

   static void WriteSolutions (IEnumerable<int[]> solutions) {
      int number = 1;
      foreach(var solution in solutions) {
         WriteLine ($"\nSolution {number++}");
         WriteBoard (solution);
      }
   }

   static void WriteBoard (int[] solution) {
      if (solution.Length != 8)
         throw new ArgumentException ("A solution must contain exactly 8 queens.");
      WriteLine ("┌───┬───┬───┬───┬───┬───┬───┬───┐");
      for(int row = 0; row < 8; row++) {
         Write ("|");
         for(int column = 0;column < 8; column++) {
            var queen = solution[row] == column ? "♛" : " ";
            Write ($" {queen} │");
         }
         WriteLine ();
         if (row < 7) WriteLine ("├───┼───┼───┼───┼───┼───┼───┼───┤");
      }
      WriteLine ("└───┴───┴───┴───┴───┴───┴───┴───┘");
   }
   #endregion
}
#endregion
