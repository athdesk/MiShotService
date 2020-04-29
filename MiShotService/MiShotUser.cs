using System;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Windows.Forms;
using MiShotService.Properties;

namespace MiShotService
{

    class MiShotUser
    {
        private static FwInterface FWI;

        public static void OpenScreenshotTool()
        {
            MessageBox.Show("You are awesome!");
        }
        private static void AttachHandler()
        {
            FWI = new FwInterface();
            FWI.EventScreenshot += OpenScreenshotTool;
        }
        
        public static void Start()
        {
            if (!ExecUtil.KillOthers())
                ExecUtil.ElevateMe(Program.ARG_STANDALONE);

            AttachHandler();
        }

        public static void Stop()
        {
            try
            {
                FWI.EventScreenshot -= OpenScreenshotTool;
                FWI.Dispose();
            }
            catch (Exception)
            {

            }
        }

    }
}
