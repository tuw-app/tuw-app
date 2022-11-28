using System;
using System.Runtime.CompilerServices;

namespace DataModel.MDDataModel
{ 
    public class MDState : IDisposable
    {
        private int working  { get; set; } = 1;
        private int measuring { get; set; } = 1;
        
        private int measuringInterval;
        public int MeasuringInterval
        {
            get { return measuringInterval; }
            set { measuringInterval = value; }
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
