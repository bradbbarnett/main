using System.Timers;
using System.ServiceProcess;
using System.Diagnostics;
using System.Threading;

namespace AutomationAPI
{
    public partial class MazakService : ServiceBase
    {
        Thread mainThread;
        Thread pingThread;

        public MazakService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //Start Main Program Loop
            mainThread = new Thread(new ThreadStart(MainRoutine.MainMethod));
            mainThread.Start();

            //Start ping thread
            pingThread = new Thread(new ThreadStart(MainRoutine.Ping));
            pingThread.Start();
        }

        protected override void OnStop()
        {
            //Disconnect communications
            MainRoutine.Stop();
            mainThread.Abort();
            pingThread.Abort();
        }
    }
}