using DataModel.MDDataModel;
using System.Drawing;

namespace MeasureDeviceServiceAPIProject.BackgraoundService
{
    public interface IMeasureDevice
    {
        public MDIPAddress IPAddress { get; set; }
        public long MDMeasuringInterval { get; set; }
        public MDState MDState { get; set; }

        public void StopMDMeasuring();

        public void StartMDMeasuring();
    }

    public interface IMeasureDevice10 
    {
        public void StopMeasuring();
        public void StartMeasuring();

        public void StartAsync();
        public void StopAsync();

        public void SetInterval(long ms);
        public long GetInterval();
        public MDState GetState();
    }


    public interface IMeasureDevice20 
    {

        public void StopMeasuring();
        public void StartMeasuring();

        public void StartAsync();
        public void StopAsync();

        public void SetInterval(long ms);
        public long GetInterval();
        public MDState GetState();
    }

    public interface IMeasureDevice30 
    {
       
        public void StopMeasuring();
        public void StartMeasuring();

        public void StartAsync();
        public void StopAsync();

        public void SetInterval(long ms);
        public long GetInterval();
        public MDState GetState();
    }
}
