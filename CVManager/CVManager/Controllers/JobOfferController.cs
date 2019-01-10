using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CVManager.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using CVManager.Models;

namespace CVManager.Controllers
{
    [Route("[controller]/[action]")]
    public class JobOfferController : Controller
    {
        public static readonly List<Company> _companies = new List<Company>()
        {
            new Company() {Id = 1, Name = "Predica"},
            new Company() {Id = 2, Name = "Microsoft"},
            new Company() {Id = 3, Name = "Github"},
            new Company() {Id = 4, Name = "Havi Logistics"},
            new Company() {Id = 5, Name = "Sweet Home"}
        };

        //public static readonly List<JobOffer> _jobOffers = new List<JobOffer>
        //{
        //    new JobOffer
        //    {
        //        Id = 1,
        //        JobTitle = "C# Programmer",
        //        Company = _companies.FirstOrDefault(c => c.Name == "Predica"),
        //        Created = DateTime.Now.AddDays(-5),
        //        SalaryFrom = 3500,
        //        SalaryTo = 6800,
        //        Location = "Cracow",
        //        Description = "Experienced C# developer with electronic background. The main task would be building smart devices software.",
        //        ValidUntil = DateTime.Now.AddDays(30)
        //    },
        //    new JobOffer{
        //        Id = 2,
        //        JobTitle = "Frontend Developer",
        //        Company = _companies.FirstOrDefault(c => c.Name =="Microsoft"),
        //        Created = DateTime.Now.AddDays(-2),
        //        Description = "Developing Office 365 front end interface. Working with SharePoint and graph API. Connecting with AAD and building ML for Mailbox smart assistant.",
        //        Location = "Poland",
        //        SalaryFrom = 2000,
        //        SalaryTo = 10000,
        //        ValidUntil = DateTime.Now.AddDays(20)
        //    },
        //    new JobOffer
        //    {
        //        Id = 3,
        //        JobTitle = "Baker",
        //        Company = _companies.FirstOrDefault(c => c.Name == "Sweet Home"),
        //        Created = DateTime.Now.AddHours(-8),
        //        SalaryFrom = 1500,
        //        SalaryTo = 4000,
        //        Location = "Warsaw",
        //        Description = "Baker in newly opened bakery. Baking tasty cakes and cokes. Experience with home made ice cram would be an additional benefit",
        //        ValidUntil = DateTime.Now.AddDays(10)
        //    }
        //};

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

        private readonly DataContext _context;

        public JobOfferController(DataContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public IActionResult Index([FromQuery(Name = "search")] string searchString)
        {
            var jobOffers = LoadJobOffers();

            if (String.IsNullOrEmpty(searchString))
                return View(jobOffers); //List all

            var searchResults = jobOffers.FindAll(o => o.JobTitle.Contains(searchString));
            return View(searchResults);
        }

        public IActionResult Details(int id)
        {
            var offer = _context.JobOffers.ToList().FirstOrDefault((o) => o.Id == id);
            if (offer == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            offer.Company = _context.Companies.FirstOrDefault(c => c.Id == offer.CompanyId);

            var applications = _context.JobApplications.ToList().FindAll(a => a.OfferId == offer.Id);
            offer.JobApplications = applications;

            return View(offer);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var offer = LoadJobOffers().FirstOrDefault(o => o.Id == id);
            if (offer == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            return View(offer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(JobOffer model)
        {
            if (!ModelState.IsValid)
                return View();

            var offer = LoadJobOffers().FirstOrDefault(o => o.Id == model.Id);
            if (offer == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            offer.JobTitle = model.JobTitle;
            offer.Description = model.Description;

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new {id = model.Id});
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var offerToRemove = LoadJobOffers().FirstOrDefault(o => o.Id == id);
            if (offerToRemove == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            _context.JobOffers.Remove(offerToRemove);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Create()
        {
            var model = new JobOfferCreateView
            {
                Companies = _context.Companies.ToList() //Load companies from DB
            };
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<ActionResult> Create(JobOfferCreateView model)
        {
            if (!ModelState.IsValid)
            {
                model.Companies = _context.Companies.ToList();
                return View(model);
            }

            var newOffer = new JobOffer
            {
                CompanyId = model.CompanyId,
                Description = model.Description,
                JobTitle = model.JobTitle,
                Location = model.Location,
                SalaryFrom = model.SalaryFrom,
                SalaryTo = model.SalaryTo,
                ValidUntil = model.ValidUntil,
                Created = DateTime.Now
            };

            _context.JobOffers.Add(newOffer);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
