using MeasuringDevice.Service;
using System;

namespace MeasuringDevice
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WMITemperatureService wmiTS=new WMITemperatureService();
            Console.WriteLine(wmiTS);
            OHMTemperatureService ohmTS = new OHMTemperatureService();
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Temp:"+ohmTS);
            }

            Console.ReadKey();
        }
    }
}
