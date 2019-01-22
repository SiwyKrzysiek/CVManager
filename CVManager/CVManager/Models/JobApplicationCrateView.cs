using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CVManager.Models
{
    public class JobApplicationCrateView : JobApplication
    {
        public string JobTitle { get; set; }
        [BindProperty]
        public IFormFile Photo { get; set; }
        [BindProperty]
        public IFormFile CV { get; set; }
    }
}
