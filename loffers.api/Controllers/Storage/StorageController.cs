using loffers.api.Utils;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
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
            string fileName;
            if (httpRequest.Files.Count > 0)
            {
                var postedFile = httpRequest.Files[0];
                string uniqueIdentifier = DateTime.Now.ToUniversalTime().ToString("yyyyMMdd\\THHmmssfff");

                byte[] Array = new byte[postedFile.ContentLength];
                postedFile.InputStream.Read(Array, 0, Array.Length);

                bool isSaved = false;
                var largeImage = ResizeFile(Array, 800, ref isSaved);
                fileName = this.GenerateFileName(postedFile.FileName, uniqueIdentifier);
                var filePath = HttpContext.Current.Server.MapPath("~/offers/" + fileName);
                File.WriteAllBytes(filePath, largeImage);

                var smallImage = ResizeFile(Array, 200, ref isSaved);
                var smallFileName = this.GenerateSmallFileName(postedFile.FileName, uniqueIdentifier);
                filePath = HttpContext.Current.Server.MapPath("~/offers/" + smallFileName);
                File.WriteAllBytes(filePath, smallImage);

                return Ok(new { url = BaseUrlToSaveImages + "offers/" + smallFileName, originalUrl= BaseUrlToSaveImages + "offers/" + fileName });
            }
            return BadRequest("No file is posted to save.");
        }

        private string GenerateFileName(string fileName, string uniqueString)
        {
            return uniqueString + "_" + fileName.Split('.')[0] + "." + fileName.Split('.')[1];
        }

        private string GenerateSmallFileName(string fileName, string uniqueString)
        {
            return uniqueString + "_s_" + fileName.Split('.')[0] + "." + fileName.Split('.')[1];
        }

        public static byte[] ResizeFile
                            (
                                byte[] bytFileContent,
                                int intTargetSize,
                                ref bool blSavedAsPng
                            )
        {
            if (bytFileContent == null)
                return null;

            blSavedAsPng = false;

            using (
                System.Drawing.Image imgOriginal = System.Drawing.Image.FromStream(new MemoryStream(bytFileContent)))
            {
                /// Calculate and get the new dimension of image as per the new resolution passed.
                Size newSize = Compute.Dimensions(imgOriginal.Size, intTargetSize);

                using (Bitmap imgNew = new Bitmap(newSize.Width, newSize.Height))
                {
                    using (Graphics canvas = Graphics.FromImage(imgNew))
                    {
                        canvas.SmoothingMode = SmoothingMode.HighQuality;
                        canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        canvas.CompositingQuality = CompositingQuality.HighQuality;
                        canvas.DrawImage(imgOriginal, new Rectangle(0, 0, newSize.Width, newSize.Height));

                        MemoryStream m = new MemoryStream();

                        if (ImageFormat.Tiff.Equals(imgOriginal.RawFormat))
                        {
                            FrameDimension frameDimensions = new FrameDimension(imgOriginal.FrameDimensionsList[0]);

                            // Selects first frame and save as jpeg. 
                            imgNew.SelectActiveFrame(frameDimensions, 0);

                            imgNew.Save(m, ImageFormat.Jpeg);
                        }
                        else if (ImageFormat.Png.Equals(imgOriginal.RawFormat) || ImageFormat.Gif.Equals(imgOriginal.RawFormat))
                        {
                            if (true)
                            {
                                blSavedAsPng = true;
                                imgNew.Save(m, ImageFormat.Png);
                            }
                            else
                            {
                                imgNew.Save(m, ImageFormat.Jpeg);
                            }
                        }
                        else
                            imgNew.Save(m, ImageFormat.Jpeg);

                        return m.ToArray();
                    }
                }
            }
        }
    }
}
