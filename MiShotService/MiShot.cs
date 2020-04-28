using System;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using MiShotService.Properties;

namespace MiShotService
{
    
    public partial class MiShot : ServiceBase
    {
        private static string HelperPath;
        private static IntPtr UserToken;

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

        private static void GetUserToken()
        {
            do
            {
                Thread.Sleep(100);
            } while (!ExecUtil.GetSessionUserToken(ref UserToken));
        }

		public static void OpenScreenshotTool()
		{
            GetUserToken();
            ExtractHelper();
            ExecUtil.StartWithToken(HelperPath, UserToken);
            Thread.Sleep(700);
            try { File.Delete(HelperPath); } catch(Exception) { }
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
            GetUserToken();
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
