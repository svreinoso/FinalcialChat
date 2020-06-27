using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalcialChat.Models
{
    public class UserChatroom
    {
        public int ChatroomId { get; set; }
        public Chatroom Chatroom { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}