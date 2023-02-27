namespace AzureIotDeviceOperations.Services
{
    public interface IManageIoTFileShareService
    {
            Task FileUploadAsync(IFormFile FileDetail);
            Task FileDownloadAsync(string fileShareName);
       
    }
}
