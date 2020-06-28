using FinalcialChat.Dtos;
using FinalcialChat.Interfaces;
using FinalcialChat.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using FinalcialChat.Enums;
using System;
using FileHelpers;

namespace FinalcialChat.Services
{
    public class ChatServices : IChatServices
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();
        private readonly IHttpClientManager _httpClientManager;

        public ChatServices(IHttpClientManager httpClientManager)
        {
            _httpClientManager = httpClientManager;
        }

        public List<MessageDto> AddMessage(Message message)
        {
            Message commandResultMessage = null;
            ApplicationUser chatBot = null;
            if(message.Content.StartsWith("/"))
            {
                message.MessageType = MessageType.Command;
                var temp = message.Content;
                temp = temp.Substring(1, temp.Length - 1);
                var messageParts = temp.Split('=');
                if(messageParts.Length != 2)
                {
                    message.Content += " (Command in wron format)";
                } else
                {
                    var command = messageParts[0];
                    var code = messageParts[1];
                    ChatCommand chatCommand;
                    var validCommand = Enum.TryParse(command, true, out chatCommand);
                    if(!validCommand || string.IsNullOrEmpty(code))
                    {
                        message.Content += " (Command code not found)";
                    } else
                    {
                        switch(chatCommand)
                        {
                            case ChatCommand.Stock:
                                try
                                {
                                    var csvCode = _httpClientManager.Get(code);
                                    var engine = new FileHelperEngine<CsvFields>();
                                    var csvFields = engine.ReadString(csvCode).ToList();
                                    var row = csvFields.FirstOrDefault();
                                    chatBot = _dbContext.Users.FirstOrDefault(x => 
                                        x.UserType == UserType.Bot && x.FirstName == "ChatBot1");
                                    commandResultMessage = new Message
                                    {
                                        Content = $"{row.Symbol} quote is ${row.Open} per share",
                                        CreatedBy = chatBot.Id,
                                        ChatroomId = message.ChatroomId,
                                        MessageType = MessageType.Text
                                    };
                                    message.Content += " (Command executed)";
                                }
                                catch (Exception ex)
                                {
                                    message.Content += " (Error reading the result of this code)";
                                }
                                break;
                        }
                    }
                }
            }

            if(message.MessageType != MessageType.Command)
            {
                _dbContext.Messages.Add(message);
            }
            if(commandResultMessage != null)
            {
                _dbContext.Messages.Add(commandResultMessage);
            }
            _dbContext.SaveChanges();
            var user = _dbContext.Users.Find(message.CreatedBy);
            var result = new List<MessageDto>()
            {
                new MessageDto
                {
                    Id = message.Id,
                    ChatroomId = message.ChatroomId,
                    Content = message.Content,
                    CreatedBy = user.FirstName + " " + user.LastName,
                    CreatedDate = message.CreatedDate != null ? message.CreatedDate : DateTimeOffset.Now,
                    MessageType = message.MessageType
                },
            };

            if(commandResultMessage != null)
            {
                result.Add(
                new MessageDto
                {
                    Id = commandResultMessage.Id,
                    ChatroomId = commandResultMessage.ChatroomId,
                    Content = commandResultMessage.Content,
                    CreatedBy = chatBot.FirstName,
                    CreatedDate = commandResultMessage.CreatedDate,
                    MessageType = commandResultMessage.MessageType
                });
            }

            return result;
        }

        public List<string> GetUsersInRoom(int roomId)
        {
            var users = _dbContext.Chatrooms.Include(x => x.Users)
                .Where(x => x.Id == roomId)
                .Select(x => x.Users.Select(u => u.Id)).FirstOrDefault();

            return users.ToList();
        }

        public ChatDto GetChats(string currentUserId, int? chatRoomId, int page)
        {
            var user = _dbContext.Users.Find(currentUserId);

            var chatRooms = user.Chatrooms != null && user.Chatrooms.Any() ?
                user.Chatrooms.Select(x => new ChatRoomDto { Id = x.Id, Name = x.Name }).ToList() :
                new List<ChatRoomDto>();

            const int pageSize = 50;
            var skip = (page -1) * pageSize;

            var messages = chatRoomId.HasValue && chatRooms.Any(x => x.Id == chatRoomId) ?
                _dbContext.Messages.Where(x => x.ChatroomId == chatRoomId.Value && 
                    (x.MessageType != MessageType.Command || (x.MessageType == MessageType.Command 
                    && x.CreatedBy == currentUserId)) )
                    .Select(x => new MessageDto 
                    { 
                        ChatroomId = x.ChatroomId,
                        Content = x.Content,
                        Id = x.Id,
                        CreatedDate = x.CreatedDate,
                        CreatedBy = x.User.FirstName + " " + x.User.LastName
                    }).OrderByDescending(x => x.CreatedDate).Skip(skip).Take(pageSize).ToList()
                : new List<MessageDto>();

            return new ChatDto
            {
                Chatrooms = chatRooms,
                Messages = messages.OrderBy(x => x.CreatedDate).ToList()
            };
        }

    }
}
