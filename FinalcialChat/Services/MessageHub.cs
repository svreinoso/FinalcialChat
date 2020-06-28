using FinalcialChat.Interfaces;
using FinalcialChat.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FinalcialChat.Services
{
    [Authorize]
    public class MessageHub : Hub
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();
        private ChatServices _chatService;

        public MessageHub(ChatServices chatServices)
        {
            this._chatService = chatServices;
        }

        public void Send(Message message)
        {
            var currentUserId = Context.User.Identity.GetUserId();
            message.CreatedBy = currentUserId;
            var messageDto = _chatService.AddMessage(message);

            var users = _chatService.GetUsersInRoom(message.ChatroomId);
            var connections = _dbContext.ChatConnections.Where(x => x.Connected && users.Contains(x.UserAgent))
                .Select(x => x.ConnectionID).ToList();

            foreach (var connectionId in connections)
            {
                Clients.Client(connectionId).addChatMessage(messageDto);
            }
        }

        public override Task OnConnected()
        {
            var currentUserId = Context.User.Identity.GetUserId();
            var name = Context.User.Identity.Name;
            var user = _dbContext.Users
                .Include(u => u.Connections)
                .FirstOrDefault(u => u.Id == currentUserId);

            user.Connections.Add(new ChatConnection
            {
                ConnectionID = Context.ConnectionId,
                UserAgent = currentUserId,
                Connected = true
            });
            _dbContext.SaveChanges();
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var connection = _dbContext.ChatConnections.FirstOrDefault(x => x.ConnectionID == Context.ConnectionId);
            connection.Connected = false;
            _dbContext.SaveChanges();
            return base.OnDisconnected(stopCalled);
        }

    }
}
