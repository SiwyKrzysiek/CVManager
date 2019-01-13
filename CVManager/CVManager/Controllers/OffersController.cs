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

        private List<JobOffer> LoadJobOffers()
        {
            var jobOffers = _context.JobOffers.ToList();
            var companies = _context.Companies.ToList();
            var applications = _context.JobApplications.ToList();

            foreach (var offer in jobOffers)
            {
                offer.Company = companies.FirstOrDefault(c => c.Id == offer.CompanyId);
                offer.JobApplications = applications.FindAll(a => a.OfferId == offer.Id);
            }

            return jobOffers;
        }


        /// <summary>
        /// Get all job offers that have selected text inside.
        /// If searchString is empty then it returns all offers.
        /// </summary>
        /// <param name="searchString">Case sensitive fragment of job offer title</param>
        /// <returns>All job offers that match search</returns>
        [HttpGet]
        public IActionResult Offers([FromQuery(Name = "search")] string searchString = "")
        {
            var offers = LoadJobOffers();
            if (!string.IsNullOrEmpty(searchString))
                offers = offers.Where(o => o.JobTitle.Contains(searchString)).ToList();

            return Ok(offers);
        }

        /// <summary>
        /// Get job offer with given id
        /// </summary>
        /// <param name="id">Id of job offer</param>
        /// <returns>Job offer with matching id</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Offers([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var offer = await _context.JobOffers.FindAsync(id);

            if (offer == null)
            {
                return NotFound();
            }

            return Ok(offer);
        }

        /// <summary>
        /// Get job offer with given id
        /// </summary>
        /// <param name="id">Json with id parameter specifying job offer</param>
        /// <returns>Job offer with matching id</returns>
        [HttpPost]
        public IActionResult Post([FromBody] ItemId id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var offer = _context.JobOffers.FirstOrDefault(o => o.Id == id.Id);

            if (offer == null)
            {
                return NotFound();
            }

            return Ok(offer);
        }
    }
}