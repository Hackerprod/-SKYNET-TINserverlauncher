using System;
using System.Diagnostics;
using System.Timers;

namespace SKYNET
{


    public class CpuUtil
    {
        public EventHandler<PerformanceCounterEventArgs> PerformanceCounterEventHandler;

        private readonly PerformanceCounter _cpuUsage;

        private readonly Timer t = new Timer();

        public CpuUtil(int interval = 500)
        {
            try
            {
                _cpuUsage = new PerformanceCounter("Processor", "% Processor Time", "_Total", true);
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

        ~CpuUtil()
        {
            this.t.Stop();
            this.t.Dispose();

            try
            {
                this._cpuUsage.Close();
                this._cpuUsage.Dispose();
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
                CPUValue = _cpuUsage.NextValue()
            };

            OnPerfCounterUpdate(ea);
        }
    }
}