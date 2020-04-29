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
        private static FwInterface FWI;
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

        private void AttachHandler()
        {
            FWI = new FwInterface();
            FWI.EventScreenshot += OpenScreenshotTool;
        }

		public MiShot()
        {
            InitializeComponent();
        }

        public void Start()
        {
            ExtractHelper();
            AttachHandler();
        }

        protected override void OnStart(string[] args)
        {
            Start();
        }

        protected override void OnStop()
        {
            FWI.Dispose();
            try
            {
                File.Delete(HelperPath);
            }
            catch (Exception) { }
        }
    }
}
