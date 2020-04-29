using System;
using System.Threading;

namespace MiShotService
{

    class MiShotUser
    {
        private static FwInterface FWI;

        private static void OpenScreenshotTool()
        {
            HelperForm TempForm = new HelperForm();
            TempForm.Show();
            do
                Thread.Sleep(10);
            while (!TempForm.Ready);
            TempForm.Dispose();
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
