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
using static EParseState;

#region class Program -----------------------------------------------------------------------------
class Program {
   #region Method ---------------------------------------------------
   static void Main () {
      string input = @"C:\PROGRAMS\DATA\README.TXT";
      if (FileNameParser.TryParse (input, out var parts)) {
         foreach (var (label, value) in new[] {("Drive",parts.Drive),("Path",parts.Path),
           ("FileName",parts.FileName),("Extension",parts.Extension) })
            WriteLine ($"{label,-10}: {value}");
      } else WriteLine ("Invalid file name");
   }
   #endregion
}
#endregion

#region class FileNameParser ----------------------------------------------------------------------
class FileNameParser {
   #region Public Method --------------------------------------------
   /// <summary>Parses the input into drive, path, file name, and extension</summary>
   public static bool TryParse (string input, out ParsedFileName result) {
      result = default;
      if (string.IsNullOrEmpty (input)) return false;
      EParseState state = Drive;
      (StringBuilder component, StringBuilder path) = (new (), new ());
      var (drive, fileName, extension) = ("", "", "");
      foreach (char ch in input) {
         state = state switch {
            Drive when ch >= 'A' && ch <= 'Z' => SetDrive (ch),
            Colon when ch == ':' => Rootslash,
            Rootslash when ch == '\\' => Com_Start,
            Com_Start when IsValidChar (ch) => AppendComponent (ch),
            Component when IsValidChar (ch) => AppendComponent (ch),
            Component when ch == '\\' => AppendPath (),
            Component when ch == '.' && path.Length > 0 => SetFileName (),
            Ext_Start when IsValidChar (ch) => AppendExtension (ch),
            Extension when IsValidChar (ch) => AppendExtension (ch, Extension),
            _ => (EParseState)(-1)
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
      EParseState SetDrive (char ch) { drive = ch.ToString (); return Colon; }

      // Appends a character to the current path component
      EParseState AppendComponent (char ch) { component.Append (ch); return Component; }

      // Moves the completed component into the directory path
      EParseState AppendPath () {
         if (path.Length > 0) path.Append ('/');
         path.Append (component);
         component.Clear ();
         return Com_Start;
      }

      // Stores the file name and advances to the extension state
      EParseState SetFileName () {
         fileName = component.ToString ();
         component.Clear ();
         return Ext_Start;
      }

      // Appends an extension character and returns the next state
      EParseState AppendExtension (char ch, EParseState next = Extension) {
         extension += ch;
         return next;
      }
      #endregion
   }
   #endregion
}
#endregion

#region enum EParseState --------------------------------------------
enum EParseState { Drive, Colon, Rootslash, Com_Start, Component, Ext_Start, Extension }
#endregion

#region record struct -----------------------------------------------
readonly record struct ParsedFileName (string Drive, string Path, string FileName,
   string Extension);
#endregion
