﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasuringDevice.Service
{
    public class TemperatureException : Exception
    {
        public TemperatureException(string message)
            :base(message)
            { }
    }
}
