using System;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using MiShotService.Properties;
using murrayju.ProcessExtensions;

namespace MiShotService
{
    
    public partial class MiShot : ServiceBase
    {
        private static string HelperPath;

        private static void ExtractHelper()
        {
            HelperPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\MiShotHelper.exe";
            try
            {
                using (FileStream fsDst = new FileStream(HelperPath, FileMode.CreateNew, FileAccess.Write))
                {
                    byte[] bytes = Resources.MiShotHelper;
                    fsDst.Write(bytes, 0, bytes.Length);
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
            ExtractHelper();
			ProcessExtensions.StartProcessAsCurrentUser(HelperPath, null, null, false);
            Thread.Sleep(150);
            File.Delete(HelperPath);
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
