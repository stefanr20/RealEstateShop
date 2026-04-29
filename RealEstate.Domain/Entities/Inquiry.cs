using System;
using System.ComponentModel.DataAnnotations;

namespace RealEstate.Domain.Entities
{
    public class Inquiry
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        public string Phone { get; set; }

        [Required(ErrorMessage = "Message is required")]
        public string Message { get; set; }

        public int? PropertyId { get; set; }
        public Property? Property { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? AdminReply { get; set; }
        public DateTime? RepliedAt { get; set; }
    }
}