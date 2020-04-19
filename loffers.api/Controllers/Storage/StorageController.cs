using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Loffers.Server.Controllers
{
    [Authorize]
    [RoutePrefix("api/storage")]
    public class StorageController : ParentController
    {
        string accessKey = string.Empty;

        //[Obsolete]
        //private readonly IHostingEnvironment _environment;
        //public StorageController(IHostingEnvironment environment)
        //{
        //    _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        //    this.accessKey = "DefaultEndpointsProtocol=https;AccountName=loffers;AccountKey=5nMj9eQP0e205w+uxVC5DPqyov9NzHZpBgYQge2rjGyMQELJTdnaFfXmPlXzD0473akmi5rdvfh+/TqHVvjDYw==;EndpointSuffix=core.windows.net";
        //}

        public StorageController()
        {
            this.accessKey = "DefaultEndpointsProtocol=https;AccountName=loffers;AccountKey=5nMj9eQP0e205w+uxVC5DPqyov9NzHZpBgYQge2rjGyMQELJTdnaFfXmPlXzD0473akmi5rdvfh+/TqHVvjDYw==;EndpointSuffix=core.windows.net";
        }

        public string BaseUrlToSaveImages
        {
            get
            {
                return ConfigurationManager.AppSettings["BaseUrlToSaveImages"];
            }
        }

        [Route("upload")]
        [HttpPost]
        public async Task<IHttpActionResult> Post()
        {
            var httpRequest = HttpContext.Current.Request;
            //if (!Request.Content.IsMimeMultipartContent())
            //    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            //var provider = new MultipartMemoryStreamProvider();
            //await Request.Content.ReadAsMultipartAsync(provider);
            //foreach (var file in provider.Contents)
            //{
            //    try
            //    {
            //        var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
            //        var contentType = file.Headers.ContentType;
            //        var fileBytes = await file.ReadAsByteArrayAsync();

            //        var _task = await this.UploadFileToBlobAsync(fileName, fileBytes, contentType.MediaType);
            //        return Ok(new { url = _task });
            //    }
            //    catch (Exception ex)
            //    {
            //        throw (ex);
            //    }
            //}

            //return Unauthorized();

            string fileName;
            if (httpRequest.Files.Count > 0)
            {
                var postedFile = httpRequest.Files[0];
                fileName = this.GenerateFileName(postedFile.FileName);
                var filePath = HttpContext.Current.Server.MapPath("~/offers/" + fileName);
                postedFile.SaveAs(filePath);
            }
            else
            {
                return BadRequest("No file is posted to save.");
            }

            return Ok(new { url = BaseUrlToSaveImages + "offers/" + fileName });
        }

        //private async Task<string> UploadFileToBlobAsync(string strFileName, byte[] fileData, string fileMimeType)
        //{
        //try
        //{
        //    //var token = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
        //    var token = User.Identity.Name;
        //    var cloudStorageAccount = CloudStorageAccount.Parse(accessKey);
        //    var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
        //    string strContainerName = "uploads";
        //    var cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);
        //    string fileName = this.GenerateFileName(strFileName);

        //    if (await cloudBlobContainer.CreateIfNotExistsAsync())
        //    {
        //        await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
        //    }

        //    if (fileName != null && fileData != null)
        //    {
        //        var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
        //        cloudBlockBlob.Properties.ContentType = fileMimeType;
        //        cloudBlockBlob.Metadata.Add("Owner", token);
        //        await cloudBlockBlob.UploadFromByteArrayAsync(fileData, 0, fileData.Length);
        //        return cloudBlockBlob.Uri.AbsoluteUri;
        //    }
        //    return "";
        //}
        //catch (Exception ex)
        //{
        //    throw (ex);
        //}
        //}

        private string GenerateFileName(string fileName)
        {
            return DateTime.Now.ToUniversalTime().ToString("yyyyMMdd\\THHmmssfff") + "_" + fileName.Split('.')[0] + "." + fileName.Split('.')[1];
        }
    }
}
