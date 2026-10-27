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
using static EState;

#region class Program -----------------------------------------------------------------------------
class Program {
   #region Method ---------------------------------------------------
   static void Main () {
      string input = @"C:\PROGRAMS\DATA\README.TXT";
      if (FileNameParser.TryParse (input, out var parts)) WriteLine (parts);
      else WriteLine ("Invalid file name");
   }
   #endregion
}
#endregion

#region class FileNameParser ----------------------------------------------------------------------
class FileNameParser {
   #region Methods --------------------------------------------------
   /// <summary>Parses the input into drive, path, file name, and extension</summary>
   public static bool TryParse (string input, out ParsedFileName result) {
      result = default;
      if (string.IsNullOrEmpty (input)) return false;
      EState state = Drive;
      (StringBuilder component, StringBuilder path) = (new (), new ());
      string drive = "", fileName = "", extension = "";
      foreach (char ch in input) {
         state = state switch {
            Drive when ch >= 'A' && ch <= 'Z' => SetDrive (ch),
            Colon when ch == ':' => Rootslash,
            Rootslash when ch == '\\' => Component_Start,
            Component_Start when IsValidChar (ch) => AppendComponent (ch),
            Component when IsValidChar (ch) => AppendComponent (ch),
            Component when ch == '\\' => AppendPath (),
            Component when ch == '.' && path.Length > 0 => SetFileName (),
            Extension_Start when IsValidChar (ch) => AppendExtension (ch),
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
         return Component_Start;
      }

      // Stores the file name and advances to the extension state
      EState SetFileName () {
         fileName = component.ToString ();
         component.Clear ();
         return Extension_Start;
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
   Drive,
   Colon,
   Rootslash,
   Component_Start, // Waits for the first character of a path component
   Component,       // Reads the remaining characters of a folder or file name
   Extension_Start,
   Extension
}
#endregion

#region record struct ParsedFileName --------------------------------
readonly record struct ParsedFileName (string Drive, string Path, string FileName,
   string Extension) {
   #region Method ---------------------------------------------------
   /// <summary>Returns the parsed file details as formatted text</summary>
   public override string ToString () => $"Drive     :{Drive}\nPath      :{Path}" +
      $"\nFileName  :{FileName}\nExtension :{Extension}";
   #endregion
}
#endregion
