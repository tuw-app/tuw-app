using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using MeasuringDevice.Service;
using OpenHardwareMonitor.Hardware;

namespace MeasuringDevice.Service
{
    public class UpdateVisitor : IVisitor
    {
        public void VisitComputer(IComputer computer)
        {
            computer.Traverse(this);
        }

        public void VisitHardware(IHardware hardware)
        {
            hardware.Update();
            foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
        }

        public void VisitSensor(ISensor sensor)
        {

        }

        public void VisitParameter(IParameter parameter)
        {

        }
    }


    public class OHMTemperatureService : ITemperatureService
    {
        public string ErrorMessage = string.Empty;

        public List<TemperatureResult> getTemperatureResults()
        {
            try
            {
                List<TemperatureResult> temperatureResults = new List<TemperatureResult>();

                UpdateVisitor updateVisitor = new UpdateVisitor();
                Computer computer = new Computer();
                computer.Open();
                computer.CPUEnabled = true;
                computer.Accept(updateVisitor);
                for (int i = 0; i < computer.Hardware.Length; i++)
                {
                    if (computer.Hardware[i].HardwareType == HardwareType.CPU)
                    {
                        for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
                        {
                            if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Temperature)
                            {
                                temperatureResults.Add(new TemperatureResult
                                {
                                    CurrentValue = (double)computer.Hardware[i].Sensors[j].Value,
                                    InstanceName = computer.Hardware[i].Sensors[j].Name
                                });                                
                            }
                        }
                        computer.Close();
                    }
                }
                return temperatureResults;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return null;
            }
        }

        public string getTemperatureString()
        {
            return ToString();
        }

        public override string ToString()
        {
            string result = string.Empty;
            List<TemperatureResult> temperatures = new List<TemperatureResult>();
            temperatures = getTemperatureResults();

            if (temperatures != null)
            {
                foreach (TemperatureResult tr in temperatures)
                {
                     result += tr.ToString();
                }
                return result;
            }
            else
                return string.Empty;
        }
    }
}
