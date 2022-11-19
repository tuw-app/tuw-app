using System;
using System.Collections.Generic;
using System.Text;

using System.Management;
using MeasuringDevice.Service;

namespace MeasuringDevice.Service
{

    public class WMITemperatureService : ITemperatureService
    {
        // Még nincs tesztelve
        public List<TemperatureResult> getTemperatureResults()
        {
            List<TemperatureResult> result = new List<TemperatureResult>();
            try
            {
                //ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"root\WMI", "SELECT * FROM SAcpi_ThermalZoneTemperature");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"root\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature");

                foreach (ManagementObject obj in searcher.Get())
                {
                    Double temperature = Convert.ToDouble(obj["CurrentTemperature"].ToString());
                    temperature = (temperature - 2732) / 10.0;
                    result.Add(new TemperatureResult { CurrentValue = temperature, InstanceName = obj["InstanceName"].ToString() });
                }
                return result;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
            if (getTemperatureResults != null)
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
