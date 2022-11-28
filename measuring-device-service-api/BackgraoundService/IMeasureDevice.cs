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

    public interface IMeasureDevice10 : IMeasureDevice
    {

    }

    public interface IMeasureDevice20 : IMeasureDevice
    {

    }

    public interface IMeasureDevice30 : IMeasureDevice
    {

    }
}
