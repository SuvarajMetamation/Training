// --------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------
// Program.cs
// A07: Double Parse.
// --------------------------------------------------------------------------------------------

namespace A07;

using static System.Console;

#region class Program -----------------------------------------------------------------------------
class Program {
   #region Method ---------------------------------------------------
   static void Main () {
      string[] testCases = {"123","-123","+123.45","-123.45","123e-45","+123.45e45",
         "-123.45e45",".45e3","123.","4.e45","34.4E3","","-12-3e3","e24","123+",".e-",
         ".e+","-+98","-123.-1","1..1","8-e", "1e-1"};
      foreach (string input in testCases) 
         if (DoubleParser.TryParse (input, out double result)) 
            WriteLine ($"{input,-15} => {result}");
         else WriteLine ($"{input,-15} => Invalid");
   }
   #endregion
}
#endregion

#region class DoubleParser ------------------------------------------------------------------------
static class DoubleParser {
   static string _input = "";
   static int _position = 0;

   #region Methods --------------------------------------------------
   public static bool TryParse (string input, out double result) {
      result = 0;
      if (string.IsNullOrWhiteSpace (input)) return false;
      _input = input.Trim ();
      _position = 0;
      try {
         bool negative = ReadSign ();
         double number = ReadNumber ();
         int exponent = ReadExponent ();
         if (!EndOfInput) return false;
         result = number * Math.Pow (0, exponent);
         if (negative) result = -result;
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

   static double ReadNumber () {
      double number = 0;
      bool hasIntegerDigits = ReadDigits (ref number);
      if (Current == '.') {
         MoveNext ();
         double fraction = 0.1;
         bool hasFractionDigits = false;
         while (IsDigit (Current)) {
            number += DigitValue (Current) * fraction;
            fraction /= 10;
            hasFractionDigits = true;
            MoveNext ();
         }
         if (!hasIntegerDigits && !hasFractionDigits) throw new ParseException ();
         if (hasIntegerDigits && hasFractionDigits) throw new ParseException ();
      }
      if (!hasIntegerDigits && Current != '.') throw new ParseException ();
      return number;
   }

   static bool ReadDigits (ref double number) {
      bool found = false;
      while (IsDigit (Current)) {
         number = number * 10 + DigitValue (Current);
         found = true;
         MoveNext ();
      }
      return found;
   }

   static int ReadExponent () {
      if (Current != 'e' && Current != 'E') return 0;
      MoveNext ();
      bool negative = false;
      if (Current == '+' || Current == '-') {
         negative = Current == '-';
         MoveNext ();
      }
      if (!IsDigit (Current)) throw new ParseException ();
      int exponent = 0;
      while (IsDigit (Current)) {
         exponent = exponent * 10 + DigitValue (Current);
         MoveNext ();
      }
      return negative ? -exponent : exponent;
   }

   static char Current => EndOfInput ? '\0' : _input[_position];
   static bool EndOfInput => _position >= _input.Length;
   static void MoveNext () => _position++;
   static bool IsDigit (char c) => c >= '0' && c <= '9';
   static int DigitValue (char c) => c - '0';
   #endregion
}
#endregion

#region class ParseException ----------------------------------------------------------------------
class ParseException : Exception { }
#endregion