using FinalcialChat.Dtos;
using FinalcialChat.Interfaces;
using FinalcialChat.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FinalcialChat.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IChatServices _chatService;
        private readonly IRoomServices _roomServices;

        public ChatController(IChatServices chatService, IRoomServices roomServices)
        {
            _chatService = chatService;
            _roomServices = roomServices;
        }

        // GET: Chat
        public ActionResult Index(int? chatRoomId, int page = 1)
        {
            var currentUserId = User.Identity.GetUserId();
            var model = _chatService.GetChats(currentUserId, chatRoomId, page);
            model.SelectedRoomId = chatRoomId.HasValue ? chatRoomId.Value : 0;
            return View(model);
        }

        public ActionResult GetRooms()
        {
            var currentUserId = User.Identity.GetUserId();
            var rooms = _roomServices.GetRooms(currentUserId);
            return Json(rooms, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddRooms(List<int> roomIds)
        {
            var currentUserId = User.Identity.GetUserId();
            _roomServices.AddRooms(currentUserId, roomIds);
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddMessage(Message message)
        {
            MessageDto messageDto = _chatService.AddMessage(message);
            return Json(messageDto, JsonRequestBehavior.AllowGet);
        }
    }
}
