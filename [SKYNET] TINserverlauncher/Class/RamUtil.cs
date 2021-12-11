namespace SKYNET
{
    using System;
    using System.Diagnostics;
    using System.Timers;

    public class RamUtil
    {
        public EventHandler<PerformanceCounterEventArgs> PerformanceCounterEventHandler;

        private readonly PerformanceCounter _ramPerfCounter;

        private readonly Timer t = new Timer();

        public RamUtil(int interval = 500)
        {
            try
            {
                _ramPerfCounter = new PerformanceCounter("Memory", "Available MBytes", true);
            }
            catch
            {
                frmMain.frm.HideCPUUtils();
                goto Label_1;
            }

            this.t.Elapsed += PerfCounterUpdate;
            this.t.Interval = interval;
            this.t.Start();

            Label_1:;
        }


        ~RamUtil()
        {
            this.t.Stop();
            this.t.Dispose();


            try
            {
                this._ramPerfCounter.Close();
                this._ramPerfCounter.Dispose();
            }
            catch
            {
                frmMain.frm.HideCPUUtils();
                goto Label_1;
            }

        Label_1:;

        }

        protected virtual void OnPerfCounterUpdate(PerformanceCounterEventArgs e)
        {
            PerformanceCounterEventHandler?.Invoke(this, e);
        }

        private void PerfCounterUpdate(object sender, ElapsedEventArgs e)
        {
            PerformanceCounterEventArgs ea = new PerformanceCounterEventArgs()
            {
                RAMValue = _ramPerfCounter.NextValue()
            };

            OnPerfCounterUpdate(ea);
        }
    }
}