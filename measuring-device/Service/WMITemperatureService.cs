using System;
using System.Collections.Generic;
using System.Text;

using System.Management;
using MeasuringDevice.Service;
using System.Linq;

namespace MeasuringDevice.Service
{

    public class WMITemperatureService : ITemperatureService
    {
        private List<TemperatureResult> remperatureResult = new List<TemperatureResult>();

        private bool canGetWMITemperature=true;

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
                    Double temperature = Convert.ToDouble(obj["CurrentTemperature"].ToString());
                    temperature = (temperature - 2732) / 10.0;
                    remperatureResult.Add(new TemperatureResult { CurrentValue = temperature, InstanceName = obj["InstanceName"].ToString() });
                }
                canGetWMITemperature = true;
                return;

            }
            catch (Exception e)
            {
                canGetWMITemperature = false;
                throw new TemperatureException(e.Message);;                
            }
        }
    

        public string GetTemperatureString()
        {
            return ToString();
        }

        public override string ToString()
        {
            string result = string.Empty;
            try
            {
                if (!canGetWMITemperature)
                    return string.Empty;
                else
                {
                    if (remperatureResult.Count == 1)
                    {
                        return $"{remperatureResult.ElementAt(0).InstanceName}:{remperatureResult.ElementAt(0).CurrentValue}";
                    }
                    else
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (TemperatureResult tr in remperatureResult)
                        {
                            sb.Append(tr.InstanceName).Append(":").Append(tr.CurrentValue).Append(";");
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
    }
}
