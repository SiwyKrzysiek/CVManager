using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CVManager.EntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CVManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private DataContext _context;

        public CompaniesController(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all companies that can post job offers
        /// </summary>
        /// <returns>List of companies</returns>
        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _context.Companies.ToListAsync();

            return Ok(companies);
        }
    }
}