using MeasureDeviceProject.Model.MeasureElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeasureDeviceProject.Model
{
    public  class MesuredCPUUsage
    {
		private CPUUsageResult cpuUsageResult;

		public CPUUsageResult CPUUsageResult
        {
			get { return cpuUsageResult; }
		}

		private DateTime measureTime;

        public DateTime MeasureTime
		{
			get { return measureTime; }
		}

        public MesuredCPUUsage(CPUUsageResult cpuUsageResult, DateTime measureTime)
        {
            this.cpuUsageResult = cpuUsageResult;
            this.measureTime = measureTime;
        }

        // Az időpont az ID-ben van
        public override string ToString()
        {
            return cpuUsageResult.ToString();
        }

    }
}
