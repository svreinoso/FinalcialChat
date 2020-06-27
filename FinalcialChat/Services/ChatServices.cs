using FinalcialChat.Dtos;
using FinalcialChat.Interfaces;
using FinalcialChat.Models;
using System.Collections.Generic;
using System.Linq;

namespace FinalcialChat.Services
{
    public class ChatServices : IChatServices
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();

        public MessageDto AddMessage(Message message)
        {
            _dbContext.Messages.Add(message);
            _dbContext.SaveChanges();
            var user = _dbContext.Users.Find(message.CreatedBy);
            return new MessageDto
            {
                Id = message.Id,
                ChatroomId = message.ChatroomId,
                Content = message.Content,
                CreatedBy = user.FirstName + " " + user.LastName,
                CreatedDate = message.CreatedDate
            };
        }

        public ChatDto GetChats(string currentUserId, int? chatRoomId)
        {
            var user = _dbContext.Users.Find(currentUserId);
            var messages = chatRoomId.HasValue ?
                _dbContext.Messages.Where(x => x.ChatroomId == chatRoomId.Value && x.CreatedBy == currentUserId)
                    .Select(x => new MessageDto 
                    { 
                        ChatroomId = x.ChatroomId,
                        Content = x.Content,
                        Id = x.Id,
                        CreatedDate = x.CreatedDate,
                        CreatedBy = x.User.FirstName + " " + x.User.LastName
                    }).ToList()
                : new List<MessageDto>();

            var chatRooms = user.Chatrooms != null && user.Chatrooms.Any() ?
                user.Chatrooms.Select(x => new ChatRoomDto { Id = x.Id, Name = x.Name }).ToList() :
                new List<ChatRoomDto>();

            return new ChatDto
            {
                Chatrooms = chatRooms,
                Messages = messages
            };
        }


    }
}
