using System;
using System.Runtime.CompilerServices;

namespace DataModel.MDDataModel
{ 
    public class MDState : IDisposable
    {
        public bool IsWorking  { get; set; }=false;
        public bool IsMeasuring { get; set; } = false;
        
        public long MeasuringInterval { get; set; } = -1;

        public void StopWorking() { IsWorking = false; }
        public void StartWorking() { IsWorking = true; }

        public void StopMeasuring() { IsMeasuring = false; }
        public void StartMeasuring() { IsMeasuring = true; }



        public MDState()
        {
            IsWorking= false;
            IsMeasuring = false;
            MeasuringInterval= -1;
        }

        public void Dispose()
        {
        }
    }
}
