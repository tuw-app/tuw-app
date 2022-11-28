using System;
using System.Runtime.CompilerServices;

namespace DataModel.MDDataModel
{ 
    public class MDState : IDisposable
    {
        public int IsWorking  { get; set; } = 1;
        public int IsMeasuring { get; set; } = 1;
        
        public int MeasuringInterval { get; set; } = -11;

        public void StopWorking() { IsWorking = 0; }
        public void StopMeasuring() { IsMeasuring = 0; }
        public void StartMeasuring() { IsMeasuring = 1; }
        public void StartWorking() { IsMeasuring = 1; }


        public MDState()
        {
            IsWorking= -1;
            IsMeasuring = -1;
            MeasuringInterval= -1;
        }

        public void Dispose()
        {
        }
    }
}
