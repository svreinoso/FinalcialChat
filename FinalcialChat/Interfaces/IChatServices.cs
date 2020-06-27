using FinalcialChat.Dtos;
using FinalcialChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalcialChat.Interfaces
{
    public interface IChatServices
    {
        ChatDto GetChats(string currentUserId, int? chatRoomId);
        MessageDto AddMessage(Message message);
    }
}
