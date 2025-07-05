using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOS.AuthDTO
{
    public class RegisterAdvisorDTO
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string Specialty { get; set; }

        [Required]
        public string Description { get; set; }

        public string? ZoomRoomUrl { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
