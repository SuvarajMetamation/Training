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
         bool customSucceeded = DoubleParser.TryParse (input, out double customValue);
         bool frameworkSucceeded = TryParseWithDoubleParse (input, out double frameworkValue);
         bool valuesMatch = customSucceeded == frameworkSucceeded &&
            (!customSucceeded || customValue == frameworkValue);
         WriteLine (
                $"{input,-20} | " +
                $"{FormatResult (customSucceeded, customValue),-22} | " +
                $"{FormatResult (frameworkSucceeded, frameworkValue),-22} | " +
                $"{(valuesMatch ? "PASS" : "FAIL")}");
      }
   }

   static bool TryParseWithDoubleParse (string input, out double result) {
      try {
         result = double.Parse (input, NumberStyles.Float, CultureInfo.InvariantCulture);
         return true;
      } catch (FormatException) {
         result = 0;
         return false;
      } catch (OverflowException) {
         result = 0;
         return false;
      }
   }

   static string FormatResult (bool succeeded, double value) =>
      succeeded ? value.ToString ("R", CultureInfo.InvariantCulture) : "Invalid";

   #endregion
}
#endregion

#region class DoubleParser ------------------------------------------------------------------------
static class DoubleParser {
   static string sInput = "";
   static int sPosition = 0;

   #region Methods --------------------------------------------------
   public static bool TryParse (string input, out double result) {
      result = 0;
      if (string.IsNullOrWhiteSpace (input)) return false;
      sInput = input.Trim ();
      sPosition = 0;
      try {
         bool isNegative = ReadSign ();
         decimal number = ReadNumber ();
         int exponent = ReadExponent ();
         if (!EndOfInput) return false;
         result = ApplyExponent (number, exponent);
         if (isNegative) result = -result;
         return true;
      } catch (ParseException) { return false; }
   }

   static bool ReadSign () {
      if (Current == '+') {
         MoveNext ();
         return false;
      }
      if (Current == '-') {
         MoveNext ();
         return true;
      }
      return false;
   }

   static decimal ReadNumber () {
      decimal number = 0;
      var (hasIntegerDigits, hasFractionDigits) = (ReadDigits (ref number), false);
      if (Current == '.') {
         MoveNext ();
         decimal fraction = 0.1m;
         while (char.IsDigit (Current)) {
            number += DigitValue (Current) * fraction;
            fraction /= 10m;
            hasFractionDigits = true;
            MoveNext ();
         }
      }
      if (!hasIntegerDigits && !hasFractionDigits) throw new ParseException ();
      return number;
   }

   static bool ReadDigits (ref decimal number) {
      bool found = false;
      while (char.IsDigit (Current)) {
         number = number * 10m + DigitValue (Current);
         found = true;
         MoveNext ();
      }
      return found;
   }

   static int ReadExponent () {
      if (Current != 'e' && Current != 'E') return 0;
      MoveNext ();
      bool isNegative = false;
      if (Current == '+' || Current == '-') {
         isNegative = Current == '-';
         MoveNext ();
      }
      if (!char.IsDigit (Current)) throw new ParseException ();
      int exponent = 0;
      while (char.IsDigit (Current)) {
         exponent = exponent * 10 + DigitValue (Current);
         MoveNext ();
      }
      return isNegative ? -exponent : exponent;
   }

   static double ApplyExponent (decimal number, int exponent) {
      if (exponent >= -28 && exponent <= 28) {
         decimal factor = 1m;
         if (exponent >= 0) for (int index = 0; index < exponent; index++) factor *= 10m;
         else for (int index = 0; index < -exponent; index++) factor /= 10m;
         return (double)(number * factor);
      }
      return (double)number * Math.Pow (10.0, exponent);
   }

   static void MoveNext () => sPosition++;

   static int DigitValue (char c) => c - '0';
   #endregion

   #region Private Properties ---------------------------------------
   static char Current => EndOfInput ? '\0' : sInput[sPosition];
   static bool EndOfInput => sPosition >= sInput.Length;
   #endregion
}
#endregion

#region class ParseException ----------------------------------------------------------------------
class ParseException : Exception { }
#endregion