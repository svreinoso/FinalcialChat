using FinalcialChat.Enums;
using FinalcialChat.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalcialChat.Models
{
    public class Message : IAuditableEntity
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public MessageType MessageType { get; set; }
        public int ChatroomId { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        public virtual ApplicationUser User { get; set; }

        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
    }
}
