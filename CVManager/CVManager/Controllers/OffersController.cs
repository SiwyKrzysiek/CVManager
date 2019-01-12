using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CVManager.EntityFramework;
using CVManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CVManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffersController : ControllerBase
    {
        private readonly DataContext _context;

        public OffersController(DataContext context)
        {
            this._context = context;
        }

        private async Task<List<JobOffer>> LoadJobOffersAsync()
        {
            var jobOffers = await _context.JobOffers.ToListAsync();
            var companies = await _context.Companies.ToListAsync();
            var applications = await _context.JobApplications.ToListAsync();

            foreach (var offer in jobOffers)
            {
                offer.Company = companies.FirstOrDefault(c => c.Id == offer.CompanyId);
                offer.JobApplications = applications.FindAll(a => a.OfferId == offer.Id);
            }

            return jobOffers;
        }

        /// <summary>
        /// Get all job offers
        /// </summary>
        /// <returns>All job offers</returns>
        [HttpGet]
        public async Task<IEnumerable<JobOffer>> Offers()
        {
            var offers = await LoadJobOffersAsync();

            return offers;
        }
    }
}