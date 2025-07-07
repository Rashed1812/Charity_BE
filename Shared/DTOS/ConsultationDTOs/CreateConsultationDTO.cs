using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.ConsultationDTOs
{
    public class CreateConsultationDTO
    {
        [Required(ErrorMessage = "Consultation name is required.")]
        [StringLength(100, ErrorMessage = "Consultation name must be less than 100 characters.")]
        public string ConsultationName { get; set; }
    }
}
