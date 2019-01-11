using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CVManager.Models
{
    public class JobOffer //Some of the fields are virtual in example but I don't know why. Let's see what happens if they are not :D
    {
        public int Id { get; set; }
        [Display(Name = "Job title")]
        [Required]
        public string JobTitle { get; set; }
        //[ForeignKey("CompanyId")] //This is how you used to mark foren keys. Now it's no longer needed
        public Company Company { get; set; }
        public int CompanyId { get; set; }
        [Display(Name = "Salary from")]
        public decimal? SalaryFrom { get; set; }
        [Display(Name = "Salary to")]
        public decimal? SalaryTo { get; set; }
        public DateTime Created { get; set; }
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
