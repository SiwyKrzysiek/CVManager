﻿using System;
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
    public class JobOfferController : Controller
    {
        private readonly DataContext _context;

        public JobOfferController(DataContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var offer = await _context.JobOffers.Include(x => x.Company).Include(o => o.JobApplications).FirstOrDefaultAsync(o => o.Id == id);
            if (offer == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            return View(offer);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var offer = _context.JobOffers.FirstOrDefault(o => o.Id == id);
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

            var offer = await _context.JobOffers.FirstOrDefaultAsync(o => o.Id == model.Id);
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

            var offerToRemove = _context.JobOffers.FirstOrDefault(o => o.Id == id);
            if (offerToRemove == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            _context.JobOffers.Remove(offerToRemove);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View();
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
