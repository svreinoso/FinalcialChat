using FinalcialChat.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinalcialChat.Models
{
    public class Chatroom : IAuditableEntity
    {
        public Chatroom()
        {
            Users = new List<ApplicationUser>();
            Messages = new List<Message>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Message> Messages { get; set; }

        public string CreatedBy { get ; set ; }
        public string ModifiedBy { get ; set ; }
        public DateTimeOffset CreatedDate { get ; set ; }
        public DateTimeOffset ModifiedDate { get ; set ; }
    }
}
