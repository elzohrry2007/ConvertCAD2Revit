using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ConvertCAD;

namespace Convert
{
    [Transaction(TransactionMode.Manual)]
    public class Ribbon : IExternalApplication
    {
        // Kept so the language button can relabel everything without restarting Revit.
        static RibbonPanel panel;
        static PushButton autoBtn, wallBtn, doorBtn, windowBtn, columnBtn, axisBtn, langBtn;

        public Result OnStartup(UIControlledApplication uiapp)
        {
            Assembly assembly = typeof(Ribbon).Assembly;
            string assemblyPath = assembly.Location;

            // Revit can't rename a tab after it is created, so its name is language-neutral.
            uiapp.CreateRibbonTab("CAD2Revit");
            panel = uiapp.CreateRibbonPanel("CAD2Revit", "Create");

            autoBtn = panel.AddItem(new PushButtonData("auto_define", "Auto", assemblyPath, "ConvertCAD.ConvertCAD2Revit")) as PushButton;
            autoBtn.LargeImage = GetEmbeddedImage(assembly, "ConvertCAD.Icons.auto.png");

            panel.AddSeparator();

            wallBtn = panel.AddItem(new PushButtonData("wall_define", "Wall", assemblyPath, "ConvertCAD.ConvertWall2Revit")) as PushButton;
            wallBtn.LargeImage = GetEmbeddedImage(assembly, "ConvertCAD.Icons.wall.png");

            panel.AddSeparator();

            doorBtn = panel.AddItem(new PushButtonData("door_define", "Door", assemblyPath, "ConvertCAD.ConvertDoor2Revit")) as PushButton;
            doorBtn.LargeImage = GetEmbeddedImage(assembly, "ConvertCAD.Icons.door.png");

            panel.AddSeparator();

            windowBtn = panel.AddItem(new PushButtonData("window_define", "Window", assemblyPath, "ConvertCAD.ConvertWindow2Revit")) as PushButton;
            windowBtn.LargeImage = GetEmbeddedImage(assembly, "ConvertCAD.Icons.window.png");

            panel.AddSeparator();

            columnBtn = panel.AddItem(new PushButtonData("column_define", "Column", assemblyPath, "ConvertCAD.ConvertColumn2Revit")) as PushButton;
            columnBtn.LargeImage = GetEmbeddedImage(assembly, "ConvertCAD.Icons.column.png");

            panel.AddSeparator();

            axisBtn = panel.AddItem(new PushButtonData("axex_define", "Grid", assemblyPath, "ConvertCAD.ConvertAxis2Revit")) as PushButton;
            axisBtn.LargeImage = GetEmbeddedImage(assembly, "ConvertCAD.Icons.axis.png");

            panel.AddSeparator();

            langBtn = panel.AddItem(new PushButtonData("lang_switch", "Language", assemblyPath, "ConvertCAD.SwitchLanguage")) as PushButton;

            ApplyTexts();
            return Result.Succeeded;
        }

        public static void ApplyTexts()
        {
            panel.Title = Lang.T("Create", "إنشاء");

            autoBtn.ItemText = Lang.T("Auto Match", "مطابقة تلقائية");
            autoBtn.ToolTip = Lang.T("Match CAD layers by name and create the 3D model automatically.",
                                     "مطابقة طبقات الكاد بالاسم وإنشاء النموذج ثلاثي الأبعاد تلقائياً.");

            wallBtn.ItemText = Lang.T("Wall", "حوائط");
            wallBtn.ToolTip = Lang.T("Pick the wall layer to create walls.", "اختر طبقة الحوائط لإنشاء الحوائط.");

            doorBtn.ItemText = Lang.T("Door", "أبواب");
            doorBtn.ToolTip = Lang.T("Pick the door layer first, then the wall layer, to create doors.",
                                     "اختر طبقة الأبواب أولاً ثم طبقة الحوائط لإنشاء الأبواب.");

            windowBtn.ItemText = Lang.T("Window", "شبابيك");
            windowBtn.ToolTip = Lang.T("Pick the window layer to create windows.", "اختر طبقة الشبابيك لإنشاء الشبابيك.");

            columnBtn.ItemText = Lang.T("Column", "أعمدة");
            columnBtn.ToolTip = Lang.T("Pick the column layer to create columns.", "اختر طبقة الأعمدة لإنشاء الأعمدة.");

            axisBtn.ItemText = Lang.T("Grid", "محاور");
            axisBtn.ToolTip = Lang.T("Pick the axis layer to create grids.", "اختر طبقة المحاور لإنشاء المحاور.");

            // The button shows the language you will switch TO.
            langBtn.ItemText = Lang.T("عربي", "English");
            langBtn.ToolTip = Lang.T("Switch the interface to Arabic.", "تحويل الواجهة إلى الإنجليزية.");
        }

        public Result OnShutdown(UIControlledApplication uiapp)
        {
            return Result.Succeeded;
        }
        private ImageSource GetEmbeddedImage(Assembly assembly, string imageName)
        {
            System.IO.Stream file = assembly.GetManifestResourceStream(imageName);
            PngBitmapDecoder bd = new PngBitmapDecoder(file, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            return bd.Frames[0];
        }
    }
}
