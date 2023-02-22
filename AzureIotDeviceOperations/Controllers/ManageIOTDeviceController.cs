using Microsoft.AspNetCore.Mvc;

using AzureIotDeviceOperations.Services;

namespace AzureIotDeviceOperations.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ManageIOTDeviceController : ControllerBase
    {

        private readonly ILogger<ManageIOTDeviceController> _logger;
        public ManageIOTDeviceController(ILogger<ManageIOTDeviceController> logger)
        {
            _logger = logger;

        }
        [HttpGet]
        [Route("~/[controller]/GetDevices")]
        public async Task<ActionResult> Get(int deviceCount = 10)
        {
            try
            {
                var devices = await ManageIoTDevicesService.GetDevicesAsync(deviceCount);
                return Ok(devices);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("~/[controller]/DeviceDetails")]
        public async Task<ActionResult> Details(string deviceId)
        {
            var device = await ManageIoTDevicesService.GetDevicesAsync(1);
            return Ok(device);
        }

        [HttpPost]
        [Route("~/[controller]/CreateDevice")]
        public ActionResult Create(string deviceName, bool isIoTEdge = false)
        {
            // Register Device with Azure
            var device = ManageIoTDevicesService.AddDevice(deviceName, isIoTEdge);

            return Ok(device);
        }

        [HttpDelete]
        [Route("~/[controller]/DeleteDevice")]
        public ActionResult Delete(string deviceId)
        {
            ManageIoTDevicesService.RemoveDevice(deviceId).Wait();
            return Ok();
        }
    }
}
