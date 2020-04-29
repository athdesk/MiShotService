using System;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using MiShotService.Properties;

namespace MiShotService
{
    
    public partial class MiShot : ServiceBase
    {
        private static string HelperPath;

        private static void ExtractHelper()
        {
            HelperPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\MiShotHelper.exe";
            try
            {
                using (FileStream HelperStream = new FileStream(HelperPath, FileMode.CreateNew, FileAccess.Write))
                {
                    byte[] HelperFile = Resources.MiShotHelper;
                    HelperStream.Write(HelperFile, 0, HelperFile.Length);
                }
            }
            catch (IOException)
            {
                File.Delete(HelperPath);
                ExtractHelper();
            }
        }

		public static void OpenScreenshotTool()
		{
			ExecUtil.StartHelper(HelperPath);
        }

        private static void AttachHandler()
        {
            FwInterface.EventScreenshot += OpenScreenshotTool;
            FwInterface.AttachEvents();
        }

		public MiShot()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            ExtractHelper();
            AttachHandler();
        }

        protected override void OnStop()
        {
            try
            {
                File.Delete(HelperPath);
            }
            catch (Exception) { }
        }
    }
}
