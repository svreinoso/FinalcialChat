using FinalcialChat.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalcialChat.Interfaces
{
    public interface IRoomServices
    {
        List<ChatRoomDto> GetRooms(string currentUserId);
        void AddRooms(string currentUserId, List<int> roomIds);
    }
}