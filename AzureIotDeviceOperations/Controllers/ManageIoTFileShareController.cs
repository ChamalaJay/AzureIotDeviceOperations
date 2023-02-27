using AzureIotDeviceOperations.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace AzureIotDeviceOperations.Controllers
{
    public class ManageIoTFileShareController : Controller
    {
        private readonly IManageIoTFileShareService manageIoTFileShareServicehare;

        public ManageIoTFileShareController(IManageIoTFileShareService manageIoTFileShareServicehare)
        {
            this.manageIoTFileShareServicehare = manageIoTFileShareServicehare;
        }

        /// <summary>
        /// upload file
        /// </summary>
        /// <param name="fileDetail"></param>
        /// <returns></returns>
        [HttpPost("Upload")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile fileDetail)
        {
            if (fileDetail != null)
            {
                await manageIoTFileShareServicehare.FileUploadAsync(fileDetail);
            }
            return Ok();
        }

        /// <summary>
        /// download file
        /// </summary>
        /// <param name="fileDetail"></param>
        /// <returns></returns>
        [HttpPost("Download")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            if (fileName != null)
            {
                await manageIoTFileShareServicehare.FileDownloadAsync(fileName);
            }
            return Ok();
        }
    }
}
}
