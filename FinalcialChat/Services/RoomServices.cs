using FinalcialChat.Dtos;
using FinalcialChat.Interfaces;
using FinalcialChat.Models;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace FinalcialChat.Services
{
    public class RoomServices : IRoomServices
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();

        public void AddRooms(string currentUserId, List<int> roomIds)
        {
            var user = _dbContext.Users.Find(currentUserId);
            var rooms = _dbContext.Chatrooms.Where(x => roomIds.Contains(x.Id)).ToList();
            if (user.Chatrooms == null) user.Chatrooms = new List<Chatroom>();
            rooms.ForEach(room => user.Chatrooms.Add(room));
            _dbContext.SaveChanges();
        }

        public List<ChatRoomDto> GetRooms(string currentUserId)
        {
            var user = _dbContext.Users.Find(currentUserId);
            var roomIds = user.Chatrooms != null && user.Chatrooms.Any()
                ? user.Chatrooms.Select(x => x.Id).ToList()
                : new List<int>();

            var rooms = _dbContext.Chatrooms.Where(x => !roomIds.Contains(x.Id))
                .Select(x => new ChatRoomDto
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
            return rooms;
        }

    }
}
