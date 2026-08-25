// --------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------
// Program.cs
// A06: Program to print unique solutions for the 8-Queens problem.
// --------------------------------------------------------------------------------------------
namespace A06;

using System.Security;
using static Console;

#region class Program -----------------------------------------------------------------------------
class Program {
   #region Methods --------------------------------------------------
   static void Main () {
      OutputEncoding = System.Text.Encoding.UTF8;
      List<int[]> solutions = [];
      int[] board = new int[8];
      Solve (0, board, solutions);
      WriteLine ("8 Queens Puzzle");
      WriteLine ("1. All Solutions (92)");
      WriteLine ("2. Canonical Solutions (12)");
      Write ("\nEnter your choice (1/2): ");
      string? choice = ReadLine ()?.Trim ();
      switch (choice) {
         case "1":
            WriteLine ($"\nTotal solutions: {solutions.Count}");
            WriteSolutions (solutions);
            break;
         case "2":
            var canonicalSolutions = solutions.Where (IsCanonical).ToList ();
            WriteLine ($"\nCanonical solutions: {canonicalSolutions.Count}");
            WriteSolutions (canonicalSolutions);
            break;
         default:
            WriteLine ("\nInvalid choice. Please enter 1 or 2.");
            break;
      }

      // Uses backtracking to place one queen in each row and
      // stores the board when all 8 queens have been placed.
      static void Solve (int row, int[] board, List<int[]> solutions) {
         if (row >= board.Length) { solutions.Add ((int[])board.Clone ()); return; }
         for (int column = 0; column < board.Length; column++) {
            if (!IsSafe (row, column, board)) continue;
            board[row] = column;
            Solve (row + 1, board, solutions);
         }
      }

      // Checks whether a queen can be placed without sharing
      // the same column or diagonal with an existing queen.
      static bool IsSafe (int row, int column, int[] board) {
         for (int previousRow = 0; previousRow < row; previousRow++) {
            int previousColumn = board[previousRow];
            if (previousColumn == column) return false;
            if (Math.Abs (previousRow - row) == Math.Abs (previousColumn - column)) return false;
         }
         return true;
      }

      // Generates all 8 symmetrical forms of a solution:
      // four rotations and their corresponding mirror images.
      static IEnumerable<int[]> GetSymmetries (int[] solution) {
         var board = solution;
         for (int rotation = 0; rotation < 4; rotation++) {
            yield return board;
            yield return Mirror (board);
            board = Rotate90 (board);
         }
      }

      // Rotates the board 90 degrees clockwise and returns
      // the transformed queen positions.
      static int[] Rotate90 (int[] solution) {
         var rotated = new int[8];
         for (int row = 0; row < 8; row++) {
            int column = solution[row];
            rotated[column] = 7 - row;
         }
         return rotated;
      }

      // Creates the vertical mirror image of the board
      // by reversing the column position of every queen.
      static int[] Mirror (int[] solution) {
         var mirrored = new int[8];
         for (int row = 0; row < 8; row++)
            mirrored[row] = 7 - solution[row];
         return mirrored;
      }

      // Converts a solution into a string so that different
      // symmetrical representations can be compared easily.
      static string Encode (int[] solution) => string.Join (",", solution);

      // Checks whether the solution is the smallest representation
      // among all of its rotations and mirror images.
      static bool IsCanonical (int[] solution) {
         var original = Encode (solution);
         var minimum = GetSymmetries (solution).Min (Encode);
         return original == minimum;
      }

      // Prints each solution with its solution number and
      // displays the corresponding chess board.
      static void WriteSolutions (IEnumerable<int[]> solutions) {
         var solutionList = solutions.ToList ();
         int current = 0;
         while (true) {
            Clear ();
            WriteLine ($"Solution {current + 1} of {solutionList.Count}\n");
            WriteBoard (solutionList[current]);
            WriteLine ("\n <- Previous    -> Next   Esc Exit");
            var key = ReadKey (true).Key;
            if (key == ConsoleKey.RightArrow && current < solutionList.Count - 1) current++;
            else if (key == ConsoleKey.LeftArrow && current > 0) current--;
            else if (key == ConsoleKey.Escape) break;
         }
      }

      // Displays a solution as an 8x8 chess board.
      static void WriteBoard (int[] solution) {
         if (solution.Length != 8)
            throw new ArgumentException ("A solution must contain exactly 8 queens.");
         string top = $"\u250C{string.Join ("\u252C", Enumerable.Repeat ("───", 8))}\u2510";
         string middle = $"\u251C{string.Join ("\u253C", Enumerable.Repeat ("───", 8))}\u2524";
         string bottom = $"\u2514{string.Join ("\u2534", Enumerable.Repeat ("───", 8))}\u2518";
         WriteLine (top);
         for (int row = 0; row < 8; row++) {
            Write ("\u2502");
            for (int column = 0; column < 8; column++) {
               var queen = solution[row] == column ? "\u265B" : " ";
               Write ($" {queen} \u2502");
            }
            WriteLine ();
            if (row < 7) WriteLine (middle);
         }
         WriteLine (bottom);

      }
   #endregion
   }
}
#endregion
