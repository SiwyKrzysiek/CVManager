﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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

        public static readonly List<JobOffer> _jobOffers = new List<JobOffer>
        {
            new JobOffer
            {
                Id = 1,
                JobTitle = "C# Programmer",
                Company = _companies.FirstOrDefault(c => c.Name == "Predica"),
                Created = DateTime.Now.AddDays(-5),
                SalaryFrom = 3500,
                SalaryTo = 6800,
                Location = "Cracow",
                Description = "Experienced C# developer with electronic background. The main task would be building smart devices software.",
                ValidUntil = DateTime.Now.AddDays(30)
            },
            new JobOffer{
                Id = 2,
                JobTitle = "Frontend Developer",
                Company = _companies.FirstOrDefault(c => c.Name =="Microsoft"),
                Created = DateTime.Now.AddDays(-2),
                Description = "Developing Office 365 front end interface. Working with SharePoint and graph API. Connecting with AAD and building ML for Mailbox smart assistant.",
                Location = "Poland",
                SalaryFrom = 2000,
                SalaryTo = 10000,
                ValidUntil = DateTime.Now.AddDays(20)
            },
            new JobOffer
            {
                Id = 3,
                JobTitle = "Baker",
                Company = _companies.FirstOrDefault(c => c.Name == "Sweet Home"),
                Created = DateTime.Now.AddHours(-8),
                SalaryFrom = 1500,
                SalaryTo = 4000,
                Location = "Warsaw",
                Description = "Baker in newly opened bakery. Baking tasty cakes and cokes. Experience with home made ice cram would be an additional benefit",
                ValidUntil = DateTime.Now.AddDays(10)
            }
        };

        [HttpGet]
        public IActionResult Index([FromQuery(Name = "search")] string searchString)
        {
            if (String.IsNullOrEmpty(searchString))
                return View(_jobOffers); //List all

            var searchResults = _jobOffers.FindAll(o => o.JobTitle.Contains(searchString));
            return View(searchResults);
        }

        public IActionResult Details(int id)
        {
            return View(_jobOffers.FirstOrDefault((offer) => offer.Id == id));
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var offer = _jobOffers.Find(o => o.Id == id);
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
            var offer = _jobOffers.Find(o => o.Id == model.Id);
            offer.JobTitle = model.JobTitle;
            offer.Description = model.Description;

            return RedirectToAction("Details", new {id = model.Id});
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            _jobOffers.RemoveAll(o => o.Id == id);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Create()
        {
            var model = new JobOfferCreateView
            {
                Companies = _companies //Load available companies list
            };
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<ActionResult> Create(JobOfferCreateView model)
        {
            if (!ModelState.IsValid)
            {
                model.Companies = _companies;
                return View(model);
            }

            var id = _jobOffers.Max(j => j.Id) + 1; //Generate new id

            _jobOffers.Add(new JobOffer
                {
                    Id = id,
                    CompanyId = model.CompanyId,
                    Company = _companies.FirstOrDefault(c => c.Id == model.CompanyId),
                    Description = model.Description,
                    JobTitle = model.JobTitle,
                    Location = model.Location,
                    SalaryFrom = model.SalaryFrom,
                    SalaryTo = model.SalaryTo,
                    ValidUntil = model.ValidUntil,
                    Created = DateTime.Now
                }
            );

            return RedirectToAction("Index");
        }
    }
}
