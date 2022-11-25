using AutoMapper;
using MeasureDeviceProject.Model;
using MeasureDeviceProject.Model.CPUUsageModel;
using MeasuringServer.Model;
using MeasuringServer.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MeasuringServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CPUUsageControllercs : Controller
    {
        private IRepositoryWrapper wrapper=null;


        public CPUUsageControllercs(IRepositoryWrapper wrapper)
        {
            this.wrapper = wrapper;
        }

        [HttpPost("api/cpuusage", Name = "Insert new cpu usage")]
        public async Task<IActionResult> InsertNewCpuUsage([FromBody] MDSended sendidDataText)
        {
            if (string.IsNullOrEmpty(sendidDataText.ToString()))
            {
                return BadRequest("Null data.");
            }
            CPUUsageEF data = null;
            try
            {
                data = new CPUUsageEF(sendidDataText);
            }
            catch { return BadRequest("Data is in wrong format."); }
            if (data == null || data.DataIsOk())
                return BadRequest("Data is in wrong format.");

            try
            {
                wrapper.CPUDatas.CreateCPUUsage(data);
            } 
            catch (Exception e)
            {
                return BadRequest($"Failed to insert {e.Message}");
            }

            return Ok();
        }
    }
}
