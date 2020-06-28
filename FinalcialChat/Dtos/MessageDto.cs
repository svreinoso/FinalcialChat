using FinalcialChat.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalcialChat.Dtos
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int ChatroomId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public MessageType MessageType { get; set; }
    }
}
