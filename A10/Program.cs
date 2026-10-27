// --------------------------------------------------------------------------------------------
// Training ~ A training program for new joinee at Trumpf Metamation, Batch - July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------
// Program.cs
// A10: File name parser using a state machine.
// --------------------------------------------------------------------------------------------
namespace A10;

using System.Text;
using static System.Console;
using static System.ConsoleColor;
using static EState;

#region class Program -----------------------------------------------------------------------------
class Program {
   #region Method ---------------------------------------------------
   static void Main () {
      string[] inputs = [
      @"C:\PROGRAMS\DATA\README.TXT",
      @"D:\DATA\FILE.CS",
      @"C:\PROGRAMS\README",
      @"CC:\DATA\FILE.TXT",
      @"C:\DATA\FILE.",
      @"C:\DATA.V1\FILE.TXT"];
      foreach (string input in inputs) {
         WriteLine ();
         ForegroundColor = Yellow;
         WriteLine ($"Input: {input}");
         ResetColor ();
         if (FileNameParser.TryParse (input, out var parts)) {
            ForegroundColor = Green;
            WriteLine (parts);
         } else {
            ForegroundColor = Red;
            WriteLine ("Invalid input");
         }
         ResetColor ();
         WriteLine (new string ('-', 45));
      }
      ResetColor ();
   }
   #endregion
}
#endregion

#region class FileNameParser ----------------------------------------------------------------------
class FileNameParser {
   #region Methods --------------------------------------------------
   /// <summary>Parses the input into drive, path, file name, and extension</summary>
   public static bool TryParse (string input, out FileParts result) {
      result = default;
      if (string.IsNullOrEmpty (input)) return false;
      EState state = Drive;
      (StringBuilder component, StringBuilder path) = (new (), new ());
      string drive = "", fileName = "", extension = "";
      foreach (char ch in input) {
         state = state switch {
            Drive when ch >= 'A' && ch <= 'Z' => SetDrive (ch),
            Colon when ch == ':' => Rootslash,
            Rootslash when ch == '\\' => ComponentStart,
            ComponentStart when IsValidChar (ch) => AppendComponent (ch),
            Component when IsValidChar (ch) => AppendComponent (ch),
            Component when ch == '\\' => AppendPath (),
            Component when ch == '.' && path.Length > 0 => SetFileName (),
            ExtensionStart when IsValidChar (ch) => AppendExtension (ch),
            Extension when IsValidChar (ch) => AppendExtension (ch, Extension),
            _ => (EState)(-1)
         };
      }
      if (state != Extension) return false;
      result = new (drive, path.ToString (), fileName, extension);
      return true;

      #region Nested Method -----------------------------------------
      // Checks whether the character is an uppercase letter or digit allowed in names
      static bool IsValidChar (char ch) => ch >= 'A' && ch <= 'Z' || ch >= '0' && ch <= '9';
      #endregion

      #region State Transition Methods ------------------------------
      // Sets the drive letter and advances to the colon state
      EState SetDrive (char ch) { drive = ch.ToString (); return Colon; }

      // Appends a character to the current path component
      EState AppendComponent (char ch) { component.Append (ch); return Component; }

      // Moves the completed component into the directory path
      EState AppendPath () {
         if (path.Length > 0) path.Append ('/');
         path.Append (component);
         component.Clear ();
         return ComponentStart;
      }

      // Stores the file name and advances to the extension state
      EState SetFileName () {
         fileName = component.ToString ();
         component.Clear ();
         return ExtensionStart;
      }

      // Appends an extension character and returns the next state
      EState AppendExtension (char ch, EState next = Extension) {
         extension += ch;
         return next;
      }
      #endregion
   }
   #endregion
}
#endregion

#region enum EState -------------------------------------------------
/// <summary>Represents the states used while parsing a file path</summary>
enum EState {
   /// <summary>Reads the drive letter</summary>
   Drive,
   /// <summary>Validates the colon following the drive letter</summary>
   Colon,
   /// <summary>Validates the root directory separator</summary>
   Rootslash,
   /// <summary>Waits for the first character of a path component</summary>
   ComponentStart,
   /// <summary>Reads the characters of a directory or file name</summary>
   Component,
   /// <summary>Waits for the first character of the file extension</summary>
   ExtensionStart,
   /// <summary>Reads the remaining characters of the file extension</summary>
   Extension
}
#endregion

#region record struct FileParts -------------------------------------
readonly record struct FileParts (string Drive, string Path, string FileName, string Extension) {
   #region Method ---------------------------------------------------
   /// <summary>Returns the parsed file details as formatted text</summary>
   public override string ToString () => $"{"Drive",-10}: {Drive}\n{"Path",-10}: {Path}\n" +
      $"{"FileName",-10}: {FileName}\n{"Extension",-10}: {Extension}";
   #endregion
}
#endregion
