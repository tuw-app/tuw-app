using MeasuringDevice.Service.CPUUsage;
using MeasuringDevice.Service.CPUTemeprature;
using System;
using System.Threading.Tasks;

namespace MeasuringDevice
{
    public class Program
    {

        static async Task CPUUsagePrinting()
        {
            // https://stackoverflow.com/questions/17630506/using-async-in-a-console-application-in-c-sharp
            try
            {

                for (int i = 0; i < 5; i++)
                {
                    CPUUsageService cu = new CPUUsageService();
                    await cu.ReadCPUUsage();
                    Console.WriteLine(cu.GetCPUUsage());
                    Console.WriteLine(cu.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("CPU használat meghatározás");
            CPUUsagePrinting();

            // VMI Temperature
            Console.WriteLine("\nWMI hőmérésklet meghatározás");
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

            Console.WriteLine("\nOHM hőmérésklet meghatározás");
            OHMTemperatureService ohmTS = new OHMTemperatureService();
            try
            {
                ohmTS.ReadTemperature();
                if (!ohmTS.CanGetWMITemperature)
                    Console.WriteLine("OHM hőmérésklet meghatározás nem működik");
                for (int i = 0; i < 5; i++)
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
