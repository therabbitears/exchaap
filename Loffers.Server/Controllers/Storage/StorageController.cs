using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Loffers.Server.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [ApiController]
    [Route("api/storage")]
    public class StorageController : ControllerBase
    {
        string accessKey = string.Empty;

        [Obsolete]
        private readonly IHostingEnvironment _environment;
        public StorageController(IHostingEnvironment environment)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            this.accessKey = "DefaultEndpointsProtocol=https;AccountName=loffers;AccountKey=5nMj9eQP0e205w+uxVC5DPqyov9NzHZpBgYQge2rjGyMQELJTdnaFfXmPlXzD0473akmi5rdvfh+/TqHVvjDYw==;EndpointSuffix=core.windows.net";

        }

        [Route("upload")]
        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file, string fileName = "placeholder_name.png")
        {
            if (file != null && file.Length > 0)
            {
                try
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        var _task = await this.UploadFileToBlobAsync(fileName, fileBytes, file.ContentType);
                        return Ok(new { url = _task });
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }

            return Unauthorized(new { url = "https://s0.2mdn.net/8017295/Value_Prop_-_Shop_More_Confidently_-_Nov_2019_-_300X600.jpg" });

        }

        private async Task<string> UploadFileToBlobAsync(string strFileName, byte[] fileData, string fileMimeType)
        {
            try
            {
                var token = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                var cloudStorageAccount = CloudStorageAccount.Parse(accessKey);
                var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                string strContainerName = "uploads";
                var cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);
                string fileName = this.GenerateFileName(strFileName);

                if (await cloudBlobContainer.CreateIfNotExistsAsync())
                {
                    await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                }

                if (fileName != null && fileData != null)
                {
                    var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
                    cloudBlockBlob.Properties.ContentType = fileMimeType;
                    cloudBlockBlob.Metadata.Add("Owner", token);
                    await cloudBlockBlob.UploadFromByteArrayAsync(fileData, 0, fileData.Length);
                    return cloudBlockBlob.Uri.AbsoluteUri;
                }
                return "";
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private string GenerateFileName(string fileName)
        {
            return DateTime.Now.ToUniversalTime().ToString("yyyyMMdd\\THHmmssfff") + "_" + fileName.Split('.')[0] + "." + fileName.Split('.')[1];
        }
    }
}
