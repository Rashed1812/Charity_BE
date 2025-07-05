using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DAL.Data.Models.IdentityModels
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string FullName { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters.")]
        public string Address { get; set; }

        // Navigation properties
        public Advisor Advisor { get; set; }
        public Admin Admin { get; set; }
        public ICollection<AdviceRequest> AdviceRequests { get; set; }
        public ICollection<Complaint> Complaints { get; set; }
    }
}
