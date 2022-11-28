using System;
using System.Runtime.CompilerServices;

namespace MeasureDeviceServiceAPIProject.Model
{ 
    public class MDState : IDisposable
    {
        private int working  { get; set; } = 1;
        private int measuring { get; set; } = 1;
        private int measuringIntervall;

        public int MeasuringIntervall
        {
            get { return measuringIntervall; }
            set { measuringIntervall = value; }
        }


        public bool IsWorking { get { return working == 1; } }
        public bool IsMeasuring { get { return measuring == 1; } }

        public void StopWorking() { working = 0; }
        public void StopMeasuring() { measuring= 0; }
        public void StartMeasuring() { measuring = 1; }
        public void StartWorking() { working = 1; }

        public void Dispose()
        {
        }
    }
}
