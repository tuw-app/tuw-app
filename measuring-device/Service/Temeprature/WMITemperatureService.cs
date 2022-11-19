using System;
using System.Collections.Generic;
using System.Text;

using System.Management;
using System.Linq;
using MeasuringDevice.Model;

namespace MeasuringDevice.Service.Temeprature
{

    public class WMITemperatureService : ITemperatureService
    {
        private List<TemperatureResult> temperatureResults = new List<TemperatureResult>();

        private bool canGetWMITemperature = true;

        public bool CanGetWMITemperature
        {
            get { return canGetWMITemperature; }
            set { canGetWMITemperature = value; }
        }


        // Még nincs tesztelve
        public void ReadTemperature()
        {

            try
            {
                //ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"root\WMI", "SELECT * FROM SAcpi_ThermalZoneTemperature");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"root\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature");

                foreach (ManagementObject obj in searcher.Get())
                {
                    double temperature = Convert.ToDouble(obj["CurrentTemperature"].ToString());
                    temperature = (temperature - 2732) / 10.0;
                    temperatureResults.Add(new TemperatureResult { CurrentValue = temperature, InstanceName = obj["InstanceName"].ToString() });
                }
                canGetWMITemperature = true;
                return;

            }
            catch (Exception e)
            {
                canGetWMITemperature = false;
                throw new TemperatureException(e.Message); ;
            }
        }


        public string GetTemperature(bool log = false)
        {
            string result = string.Empty;
            try
            {
                if (!canGetWMITemperature)
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
