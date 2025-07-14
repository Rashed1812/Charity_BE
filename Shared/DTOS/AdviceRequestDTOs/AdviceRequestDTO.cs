using System.ComponentModel.DataAnnotations;

namespace Shared.DTOS.AdviceRequestDTOs
{
    public class AdviceRequestDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int? AdvisorId { get; set; }
        public string AdvisorName { get; set; }
        public int ConsultationId { get; set; }
        public string ConsultationName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ConfirmedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string Response { get; set; }
        public int? Rating { get; set; }
        public string Review { get; set; }
    }

    public class CreateAdviceRequestDTO
    {
        [Required]
        public int ConsultationId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        public string Priority { get; set; }
<<<<<<< HEAD
=======
        
        public ConsultationType ConsultationType { get; set; } = ConsultationType.Online;

        [Required]
        public int AdvisorAvailabilityId { get; set; }
<<<<<<< Updated upstream
=======
>>>>>>> e8c8153619cc53aaad71f6042edd59cb485b2764
>>>>>>> Stashed changes
    }

    public class UpdateAdviceRequestDTO
    {
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        public string Priority { get; set; }
    }

    public class CompleteRequestDTO
    {
        [Required]
        [StringLength(2000)]
        public string Response { get; set; }
    }
} 