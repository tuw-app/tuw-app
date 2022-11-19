using MeasuringDevice.Service;
using System;
using System.Security.Cryptography.X509Certificates;

namespace MeasuringDevice
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // VMI Temperature
            Console.WriteLine("WMI hőméréskletmeghatározás");
            WMITemperatureService wmiTS=new WMITemperatureService();
            try
            {
                wmiTS.ReadTemperature();
                if (wmiTS.CanGetWMITemperature)
                {
                    wmiTS.ReadTemperature();
                    Console.WriteLine(wmiTS.GetTemperature());
                    bool log = true;
                    Console.WriteLine(wmiTS.GetTemperature(log));
                }

                else
                    Console.WriteLine("WMI hőmérésklet meghatározás nem működik");

            }
            catch (TemperatureException te)
            {
                Console.WriteLine(te.Message);
                
            }

            Console.WriteLine("OHM hőméréskletmeghatározás");
            OHMTemperatureService ohmTS = new OHMTemperatureService();
            try
            {
                ohmTS.ReadTemperature();
                if (!ohmTS.CanGetWMITemperature)
                    Console.WriteLine("OHM hőmérésklet meghatározás nem működik");
                for (int i = 0; i < 30; i++)
                {
                    ohmTS.ReadTemperature();
                    Console.WriteLine(ohmTS.GetTemperature());
                    bool log = true;
                    Console.WriteLine(ohmTS.GetTemperature(log));
                }
            }
            catch (TemperatureException te)
            {
                Console.WriteLine(te.Message);
            }

            Console.ReadKey();
        }
    }
}
