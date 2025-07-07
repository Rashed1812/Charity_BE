//using System.ComponentModel.DataAnnotations;

//namespace Shared.DTOS.NewsDTOs
//{
//    public class ServiceOfferingDTO
//    {
//        public int Id { get; set; }
//        public string Title { get; set; }
//        public string Description { get; set; }
//        public string ImageUrl { get; set; }
//        public string RedirectUrl { get; set; }
//        public bool IsActive { get; set; }
//        public DateTime CreatedAt { get; set; }
//        public DateTime? UpdatedAt { get; set; }
//        public int ClickCount { get; set; }
//    }

//    public class CreateServiceOfferingDTO
//    {
//        [Required]
//        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
//        public string Title { get; set; }
        
//        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
//        public string Description { get; set; }
        
//        [Url(ErrorMessage = "Please provide a valid image URL")]
//        public string ImageUrl { get; set; }
        
//        [Url(ErrorMessage = "Please provide a valid redirect URL")]
//        public string RedirectUrl { get; set; }
        
//        public bool IsActive { get; set; } = true;
//    }

//    public class UpdateServiceOfferingDTO
//    {
//        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
//        public string Title { get; set; }
        
//        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
//        public string Description { get; set; }
        
//        [Url(ErrorMessage = "Please provide a valid image URL")]
//        public string ImageUrl { get; set; }
        
//        [Url(ErrorMessage = "Please provide a valid redirect URL")]
//        public string RedirectUrl { get; set; }
        
//        public bool? IsActive { get; set; }
//    }
//} 