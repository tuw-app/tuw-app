using System;
using System.Collections.Generic;
using System.Text;

namespace MeasureDeviceProject.Model
{
    public class MDSended
    {
        private string data;
        public string Data { get => data; set => data = value; }

        public MDSended()
        {
        }

        public MDSended(string data)
        {
            this.data = data;
        }

        public override string ToString()
        {
            return data; 
        }


    }
}
