﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CVManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace CVManager.Controllers
{
    [Route("[controller]/[action]")]
    public class ApplicationController : Controller
    {
        //ToDO: Use DB
        public static readonly List<JobApplication> _applications = new List<JobApplication>();

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //ToDO: Use DB
            var application = _applications.FirstOrDefault(a => a.Id == id);
            if (application == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            return View(application);
        }

        [HttpGet]
        public IActionResult Apply(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //ToDO: Use DB
            var offer = JobOfferController._jobOffers.Find(o => o.Id == id);
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

            //ToDO: Use DB
            var id = (_applications.Count == 0) ? 1 : _applications.Max(a => a.Id) + 1; //Generate new id

            //ToDO: Use DB
            _applications.Add(new JobApplication()
            {
                Id = id,
                OfferId = model.OfferId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                EmailAddress = model.EmailAddress,
                ContactAgreement = model.ContactAgreement,
                CvUrl = model.CvUrl,
                DateOfBirth = model.DateOfBirth,
                Description = model.Description
            });

            return RedirectToAction("Details", "JobOffer", new {id = model.OfferId});
        }
    }
}