namespace SKYNET
{
    using System;
    using System.Diagnostics;
    using System.Timers;

    public class DiskUtil
    {
        public EventHandler<PerformanceCounterEventArgs> PerformanceCounterEventHandler;

        private readonly PerformanceCounter _diskReadTotal = new PerformanceCounter("PhysicalDisk", "Disk Reads/sec", "_Total", true);

        private Timer t;

        public DiskUtil(int interval = 500)
        {
            this.t = new Timer();
            this.t.Elapsed += PerfCounterUpdate;
            this.t.Interval = interval;
            this.t.Start();
        }

        ~DiskUtil()
        {
            this.t.Stop();
            this.t.Dispose();

            this._diskReadTotal.Close();
            this._diskReadTotal.Dispose();
        }

        protected virtual void OnPerfCounterUpdate(PerformanceCounterEventArgs e)
        {
            PerformanceCounterEventHandler?.Invoke(this, e);
        }

        private void PerfCounterUpdate(object sender, ElapsedEventArgs e)
        {
            PerformanceCounterEventArgs ea = new PerformanceCounterEventArgs()
            {
                RAMValue = _diskReadTotal.NextValue()
            };

            OnPerfCounterUpdate(ea);
        }
    }
}