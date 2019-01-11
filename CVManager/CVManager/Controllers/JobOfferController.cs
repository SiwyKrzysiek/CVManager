using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CVManager.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using CVManager.Models;
using Microsoft.EntityFrameworkCore;

namespace CVManager.Controllers
{
    [Route("[controller]/[action]")]
    public class JobOfferController : Controller
    {
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

        public async Task<IActionResult> Details(int id)
        {
            var offer = await _context.JobOffers.Include(x => x.Company).Include(x => x.JobApplications).FirstOrDefaultAsync(o => o.Id == id);
            if (offer == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
           
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
