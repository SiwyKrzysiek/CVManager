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
        /// Add new offer to the data base
        /// </summary>
        /// <param name="offer">Json with job offer to be added</param>
        /// <returns>Response code and if successful redirection to created offer</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] JobOfferCreate offer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newOffer = new JobOffer()
            {
                CompanyId = offer.CompanyId,
                Description = offer.Description,
                JobTitle = offer.JobTitle,
                Location = offer.Location,
                SalaryFrom = offer.SalaryFrom,
                SalaryTo = offer.SalaryTo,
                ValidUntil = offer.ValidUntil,
                Created = DateTime.Now
            };

            _context.Add(newOffer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Offers", new {id = newOffer.Id}, offer);
        }

        /// <summary>
        /// Updates specified job offer
        /// </summary>
        /// <param name="id">Id of offer to be updated</param>
        /// <param name="offerData">Full details of update. This will replace old offer</param>
        /// <returns>Only response code</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Post([FromRoute] int id, [FromBody] JobOffer offerData)
        {
            var offer = await _context.JobOffers.FirstOrDefaultAsync(o => o.Id == id);
            if (offer == null)
                return NotFound();

            offer.JobTitle = offerData.JobTitle;
            offer.CompanyId = offerData.CompanyId;
            offer.Created = offer.Created;
            offer.Description = offer.Description;
            offer.JobApplications = offerData.JobApplications;
            offer.Location = offerData.Location;
            offer.SalaryFrom = offerData.SalaryFrom;
            offer.SalaryTo = offerData.SalaryTo;
            offer.ValidUntil = offerData.ValidUntil;

            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Deletes job offer with specified id
        /// </summary>
        /// <param name="id">Id of offer to be removed</param>
        /// <returns>Only response code</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var todoItem = await _context.JobOffers.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.JobOffers.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}