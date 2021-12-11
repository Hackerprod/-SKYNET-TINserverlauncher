using System;

namespace SKYNET
{
    public class PerformanceCounterEventArgs : EventArgs
    {
        public float CPUValue { get; set; }
        public float DISKValue { get; set; }
        public float RAMValue { get; set; }
    }
}