using FinalcialChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalcialChat.Dtos
{
    public class ChatDto
    {
        public List<ChatRoomDto> Chatrooms { get; set; }
        public List<MessageDto> Messages { get; set; }
        public int SelectedRoomId { get; set; }
    }
}