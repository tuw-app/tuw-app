using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.MDDataModel
{
    public class MDSendedDataFromDeviceToServer
    {
        private string data;
        public string Data { get => data; set => data = value; }

        public MDSendedDataFromDeviceToServer()
        {
        }

        public MDSendedDataFromDeviceToServer(string data)
        {
            this.data = data;
        }

        public override string ToString()
        {
            return data; 
        }


    }
}
