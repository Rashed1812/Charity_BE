using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOS.UserDTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<string> Roles { get; set; }
    }

    public class UpdateUserDTO
    {
        [StringLength(50)]
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(200)]
        public string Address { get; set; }
    }
}
