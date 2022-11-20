using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MeasureDeviceProject.Model.DataSendingElements
{
    public interface IDataSendingSubDevice
    {
        public Task<bool> ContactingToTheServer();
    }
}
