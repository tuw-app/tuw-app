using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MeasuringDevice.Model;
using OpenHardwareMonitor.Hardware;

namespace MeasuringDevice.Service.CPUTemeprature
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
        List<TemperatureResult> temperatureResults = new List<TemperatureResult>();

        private bool canGetOHMTemperature = true;

        public bool CanGetWMITemperature
        {
            get { return canGetOHMTemperature; }
            set { canGetOHMTemperature = value; }
        }

        public void ReadTemperature()
        {
            try
            {
                temperatureResults.Clear();
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
                canGetOHMTemperature = true;
                return;
            }
            catch (Exception e)
            {
                canGetOHMTemperature = false;
                throw new TemperatureException(e.Message);
            }
        }

        public string GetTemperature(bool log = false)
        {
            string result = string.Empty;
            try
            {
                if (!canGetOHMTemperature)
                    return string.Empty;
                else
                {
                    if (temperatureResults.Count == 1)
                    {
                        if (log)
                            return temperatureResults.ElementAt(0).ToString();
                        else
                            return temperatureResults.ElementAt(0).GetShortString();
                    }
                    else
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (TemperatureResult tr in temperatureResults)
                        {
                            if (log)
                                sb.Append(tr.GetShortString());
                            else
                                sb.Append(tr.ToString());
                        }
                        return sb.ToString();
                    }
                }
            }
            catch (TemperatureException)
            {
            }
            return string.Empty;
        }

        public override string ToString()
        {
            bool log = true;
            return GetTemperature(log);
        }


    }
}
