using System;
using System.IO;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ConvertCAD
{
    // UI language (English / Arabic). The choice is saved per Windows user and survives Revit restarts.
    public static class Lang
    {
        static readonly string SettingsFile = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ConvertCAD2Revit", "lang.txt");

        public static bool Arabic = Load();

        static bool Load()
        {
            try { return File.ReadAllText(SettingsFile).Trim() == "ar"; }
            catch { return false; } // no file yet → English
        }

        public static void Toggle()
        {
            Arabic = !Arabic;
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(SettingsFile));
                File.WriteAllText(SettingsFile, Arabic ? "ar" : "en");
            }
            catch { } // can't save → language still switches for this session
        }

        public static string T(string en, string ar) { return Arabic ? ar : en; }
    }

    [Transaction(TransactionMode.Manual)]
    public class SwitchLanguage : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Lang.Toggle();
            Convert.Ribbon.ApplyTexts();
            return Result.Succeeded;
        }
    }
}
