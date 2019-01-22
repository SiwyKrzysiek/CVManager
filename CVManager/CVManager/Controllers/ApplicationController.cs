using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CVManager.EntityFramework;
using CVManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace CVManager.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly DataContext _context;

        public ApplicationController(DataContext context)
        {
            this._context = context;
        }

        private List<JobOffer> LoadJobOffers()
        {
            var jobOffers = _context.JobOffers.ToList();
            var companies = _context.Companies.ToList();
            foreach (var offer in jobOffers)
            {
                offer.Company = companies.FirstOrDefault(c => c.Id == offer.CompanyId);
            }

            return jobOffers;
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var application = _context.JobApplications.ToList().FirstOrDefault(a => a.Id == id);
            if (application == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            return View(application);
        }

        [HttpGet]
        public IActionResult Apply(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var offer = _context.JobOffers.ToList().FirstOrDefault(o => o.Id == id);
            if (offer == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            var application = new JobApplicationCrateView() {OfferId = offer.Id, JobTitle = offer.JobTitle};

            return View(application);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Apply(JobApplicationCrateView model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string photoURI = null;
            if (model.Photo != null) //User added his photo
            {
                var photoGUID = Guid.NewGuid().ToString(); //Create unique photo name
                var extension = Path.GetExtension(model.Photo.FileName);
                var photoName = photoGUID + extension;

                photoURI = await UploadPhotoToBlobStorageAsync(model.Photo, photoName);
            }

            string CVURI = null;
            if (model.CV != null) //User added CV
            {
                var CVGUID = Guid.NewGuid().ToString();
                var extension = Path.GetExtension(model.CV.FileName);
                var fileName = CVGUID + extension;

                CVURI = await UploadCVToBlobStorageAsync(model.CV, fileName);
            }

            var newApplication = new JobApplication()
            {
                OfferId = model.OfferId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                EmailAddress = model.EmailAddress,
                ContactAgreement = model.ContactAgreement,
                CvUrl = CVURI,
                DateOfBirth = model.DateOfBirth,
                Description = model.Description,
                PhotoFileName = photoURI,
            };

            _context.JobApplications.Add(newApplication);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "JobOffer", new {id = model.OfferId});
        }

        /// <summary>
        /// Uploads file received from form to Azure blob storage
        /// </summary>
        /// <param name="file">File data</param>
        /// <param name="fileName">Name that file should have in the storage</param>
        /// <returns>URI to uploaded file</returns>
        private static async Task<string> UploadFileToBlobStorageAsync(IFormFile file, string fileName, string containerName)
        {
            string connectionString = @"DefaultEndpointsProtocol=https;AccountName=jobofferstoragekd;AccountKey=1ERNYEI2u/olisE50l9VWia25IVhlGYIFZgbi24Y/KqwIJd1jWnb2Nm5G8beA5R5PN5aV4+W4Y6i5OvtjUXjMg==;EndpointSuffix=core.windows.net";

            if (CloudStorageAccount.TryParse(connectionString, out var storageAccount))
            {
                CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

                // Get reference to the blob container by passing the name by reading the value from the configuration (appsettings.json)
                CloudBlobContainer container = cloudBlobClient.GetContainerReference(containerName);
                //await container.CreateIfNotExistsAsync();

                // Get the reference to the block blob from the container
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

                using (var memoryStream = new MemoryStream())
                {
                    file.OpenReadStream().CopyTo(memoryStream); //Convert to byte[]
                    var bytes = memoryStream.ToArray();

                    await blockBlob.UploadFromByteArrayAsync(bytes, 0, bytes.Length); //Upload
                }

                return blockBlob.Uri.AbsoluteUri;
            }

            return null;
        }

        /// <summary>
        /// Uploads CV to blob storage
        /// </summary>
        /// <param name="modelCv">File received from form</param>
        /// <param name="fileName">Name that file should have on blob storage</param>
        /// <returns>URL to uploaded CV</returns>
        private static async Task<string> UploadCVToBlobStorageAsync(IFormFile modelCv, string fileName)
        {
            return await UploadFileToBlobStorageAsync(modelCv, fileName, "cvstorage");
        }

        /// <summary>
        /// Uploads file received from form to Azure blob storage
        /// </summary>
        /// <param name="file">File data</param>
        /// <param name="fileName">Name that file should have in the storage</param>
        /// <returns>URI to uploaded photo</returns>
        private static async Task<string> UploadPhotoToBlobStorageAsync(IFormFile file, string fileName)
        {
            return await UploadFileToBlobStorageAsync(file, fileName, "applications");   
        }

        private async Task<ActionResult> GetPicture(string name)
        {
            string connectionString = @"DefaultEndpointsProtocol=https;AccountName=jobofferstoragekd;AccountKey=1ERNYEI2u/olisE50l9VWia25IVhlGYIFZgbi24Y/KqwIJd1jWnb2Nm5G8beA5R5PN5aV4+W4Y6i5OvtjUXjMg==;EndpointSuffix=core.windows.net";

            if (CloudStorageAccount.TryParse(connectionString, out var storageAccount))
            {
                CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

                // Get reference to the blob container by passing the name by reading the value from the configuration (appsettings.json)
                CloudBlobContainer container = cloudBlobClient.GetContainerReference("applications");
                //await container.CreateIfNotExistsAsync();

                // Get the reference to the block blob from the container
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(name);
                if (!await blockBlob.ExistsAsync())
                    return NotFound();

                var url = blockBlob.StorageUri;
                var url2 = blockBlob.Uri;

                using (var stream = new MemoryStream())
                {
                    await blockBlob.DownloadToStreamAsync(stream);
                    return File(stream.ToArray(), "image/png"); //Getting MIME type of file would be better
                }
            }
            else
            {
                return NotFound();
            }
        }
    }
}