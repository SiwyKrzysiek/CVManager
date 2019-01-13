using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CVManager.Models
{
    public class JobOfferCreate
    {
        [Display(Name = "Job title")]
        [Required]
        public string JobTitle { get; set; }
        public int CompanyId { get; set; }
        [Display(Name = "Salary from")]
        public decimal? SalaryFrom { get; set; }
        [Display(Name = "Salary to")]
        public decimal? SalaryTo { get; set; }
        public string Location { get; set; }
        [Required]
        [MinLength(50, ErrorMessage = "Description must be at lest 50 characters")]
        public string Description { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        [Display(Name = "Valid until")]
        public DateTime? ValidUntil { get; set; }
        public List<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
    }
}
