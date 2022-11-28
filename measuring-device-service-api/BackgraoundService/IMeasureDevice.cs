using DataModel.MDDataModel;
using System.Drawing;

namespace MeasureDeviceServiceAPIProject.BackgraoundService
{
    public interface IMeasureDevice
    {
        public MDIPAddress IPAddress { get; set; }

        public int MeasuringInterval { get; set; }

        public MDState MDState { get; set; }

        public void StopMeasuring();

        public void StartMeasuring();
    }

    public interface IMeasureDevice10 
    {
        public MDIPAddress IPAddress { get; set; }

        public int MeasuringInterval { get; set; }

        public MDState MDState { get; set; }

        public void StopMeasuring();

        public void StartMeasuring();
    }


    public interface IMeasureDevice20 
    {
        public MDIPAddress IPAddress { get; set; }

        public int MeasuringInterval { get; set; }

        public MDState MDState { get; set; }

        public void StopMeasuring();

        public void StartMeasuring();
    }

    public interface IMeasureDevice30 
    {
        public MDIPAddress IPAddress { get; set; }

        public int MeasuringInterval { get; set; }

        public MDState MDState { get; set; }

        public void StopMeasuring();

        public void StartMeasuring();
    }
}
