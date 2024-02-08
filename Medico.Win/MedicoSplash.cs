using DevExpress.ExpressApp.Win;

namespace SBT.Apps.Medico.Win
{
    public class MedicoSplash : ISplash, ISupportUpdateSplash
    {
        static private MedSplashScreen form;
        private static bool isStarted;
        public void Start()
        {
            isStarted = true;
            form = new MedSplashScreen();
            form.Show();
            System.Windows.Forms.Application.DoEvents();
        }
        public void Stop()
        {
            if (form != null)
            {
                form.Hide();
                form.Close();
                form = null;
            }
            isStarted = false;
        }
        public void SetDisplayText(string displayText)
        {
        }
        public bool IsStarted
        {
            get { return isStarted; }
        }

        public void UpdateSplash(string caption, string description, params object[] additionalParams)
        {
            form.UpdateInfo(description);
        }

    }
}
