// --------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------
// Program.cs
// A07: Double Parse.
// --------------------------------------------------------------------------------------------

namespace A07;

using System.Globalization;
using static System.Console;

#region class Program -----------------------------------------------------------------------------
class Program {
   #region Method ---------------------------------------------------
   static void Main () {
      string[] testCases = { "10.54E23.4E3", "-1.546234E-4", "0", "0.0", "12345", "0.00000325",
         "10.54E2", "1.5E2.5", "1.E3", "1.E+3", "12.54e3.", "12.", "12.e1", ".325", " +625 ",
         "6.25e0", "6.0e0", "6.25e-1", "+6.25E1", "6.25", "10.625", "15a1", "1.5672", "+-12",
         "12.-5", ".e1", "-0.325", " 12.456 " };
      WriteLine ($"{"Input",-20} | {"Custom parser",-22} | {"Double.Parse",-22} | Result");
      WriteLine (new string ('-', 78));
      foreach (string input in testCases) {
         bool isCustomSuccess = DoubleParser.TryParse (input, out double customValue);
         bool isFrameworkSuccess = double.TryParse (input, out double frameworkValue);
         bool valuesMatch = isCustomSuccess == isFrameworkSuccess &&
            (!isCustomSuccess || customValue == frameworkValue);
         WriteLine (
                $"{input,-20} | " +
                $"{FormatResult (isCustomSuccess, customValue),-22} | " +
                $"{FormatResult (isFrameworkSuccess, frameworkValue),-22} | " +
                $"{(valuesMatch ? "PASS" : "FAIL")}");
      }
   }

   static string FormatResult (bool succeeded, double value) =>
      succeeded ? value.ToString ("R", CultureInfo.InvariantCulture) : "Invalid";

   #endregion
}
#endregion

#region class DoubleParser ------------------------------------------------------------------------
static class DoubleParser {
   #region Method ---------------------------------------------------
   public static bool TryParse (string input, out double result) {
      result = 0;
      if (string.IsNullOrWhiteSpace (input)) return false;
      sInput = input.Trim ();
      sPosition = 0;
      try {
         // Read the optional sign
         bool isNegative = false;
         if (Current == '+' || Current == '-') {
            isNegative = Current == '-';
            sPosition++;
         }
         decimal number = 0;
         // Read the integer and fractional parts.
         bool hasInt = false, hasFrac = false;
         while (char.IsDigit (Current)) {
            number = number * 10m + (Current - '0');
            hasInt = true;
            sPosition++;
         }
         if (Current == '.') {
            sPosition++;
            decimal fraction = 0.1m;
            while (char.IsDigit (Current)) {
               number += (Current - '0') * fraction;
               fraction /= 10m;
               hasFrac = true;
               sPosition++;
            }
         }
         // At least one digit must be present
         if (!hasInt && !hasFrac) throw new ParseException ();
         // Read the optional exponent and its sign
         int exponent = 0;
         if (Current == 'e' || Current == 'E') {
            sPosition++;
            bool isExpNegative = false;
            if (Current == '+' || Current == '-') {
               isExpNegative = Current == '-';
               sPosition++;
            }
            // An exponent must contain at least one digit
            if (!char.IsDigit (Current)) throw new ParseException ();
            while (char.IsDigit (Current)) {
               exponent = exponent * 10 + (Current - '0');
               sPosition++;
            }
            if (isExpNegative) exponent = -exponent;
         }
         // Reject any characters left after parsing
         if (!EndOfInput) return false;
         // Apply the exponent to obtain the final value
         if (exponent >= -28 && exponent <= 28) {
            decimal factor = 1m;
            if (exponent >= 0) for (int index = 0; index < exponent; index++) factor *= 10m;
            else for (int index = 0; index < -exponent; index++) factor /= 10m;
            result = (double)(number * factor);
         } else result = (double)number * Math.Pow (10.0, exponent);
         // Apply the original number's sign
         if (isNegative) result = -result;
         return true;
      } catch (ParseException) { return false; }
   }
   #endregion

   #region Private Properties ---------------------------------------
   // Get the current character, or '\0' when the input ends
   static char Current => EndOfInput ? '\0' : sInput[sPosition];
   // Check whether all input characters have been processed
   static bool EndOfInput => sPosition >= sInput.Length;
   #endregion

   #region Static Fields --------------------------------------------
   static string sInput = "";
   static int sPosition = 0;
   #endregion
}
#endregion

#region class ParseException ----------------------------------------------------------------------
class ParseException : Exception { }
#endregion
