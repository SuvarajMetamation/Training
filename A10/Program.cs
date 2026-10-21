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
   public static bool TryParse (string input, out (string Drive, string Path, string FileName,
      string Extension) result) {
      result = ("", "", "", "");
      if (string.IsNullOrEmpty (input)) return false;
      EParseState state = DRIVE;
      (StringBuilder component, StringBuilder path) = (new (), new ());
      var (drive, fileName, extension) = ("", "", "");
      foreach (char ch in input) {
         switch (state) {
            case DRIVE:
               if (ch < 'A' || ch > 'Z') return false;
               (drive, state) = (ch.ToString (), COLON);
               break;
            case COLON:
               if (ch != ':') return false;
               state = ROOTSLASH;
               break;
            case ROOTSLASH:
               if (ch != '\\') return false;
               state = COM_START;
               break;
            case COM_START:
               if (!IsValidChar (ch)) return false;
               component.Append (ch);
               state = COMPONENT;
               break;
            case COMPONENT:
               if (IsValidChar (ch)) component.Append (ch);
               else if (ch == '\\') {
                  if (path.Length > 0) path.Append ('/');
                  path.Append (component);
                  component.Clear ();
                  state = COM_START;
               } else if (ch == '.') {
                  if (path.Length == 0) return false;
                  fileName = component.ToString ();
                  component.Clear ();
                  state = EXT_START;
               } else return false;
               break;
            case EXT_START:
               if (!IsValidChar (ch)) return false;
               extension += ch;
               state = EXTENSION;
               break;
            case EXTENSION:
               if (!IsValidChar (ch)) return false;
               extension += ch;
               break;
         }
      }
      if (state != EXTENSION) return false;
      result = (drive, path.ToString (), fileName, extension);
      return true;

      #region Nested Method -----------------------------------------
      // Checks the whether the character is valid or not
      static bool IsValidChar (char ch) => ch >= 'A' && ch <= 'Z' || ch >= '0' && ch <= '9';
      #endregion
   }
   #endregion
}
#endregion

#region enum EParseState --------------------------------------------
enum EParseState { DRIVE, COLON, ROOTSLASH, COM_START, COMPONENT, EXT_START, EXTENSION }
#endregion
