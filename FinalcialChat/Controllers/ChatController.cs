using FinalcialChat.Dtos;
using FinalcialChat.Interfaces;
using FinalcialChat.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FinalcialChat.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IChatServices _chatService;
        private readonly IRoomServices _roomServices;
        public Func<string> GetUserId;
        public ChatController(IChatServices chatService, IRoomServices roomServices)
        {
            _chatService = chatService;
            _roomServices = roomServices;
            GetUserId = () => User.Identity.GetUserId();
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

    }
}
