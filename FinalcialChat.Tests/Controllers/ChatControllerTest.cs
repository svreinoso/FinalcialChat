using FinalcialChat.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using Moq;
using FinalcialChat.Interfaces;
using System.Security.Principal;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using FakeItEasy;
using System.Collections.Generic;
using FinalcialChat.Dtos;

namespace FinalcialChat.Tests.Controllers
{
    [TestClass]
    public class ChatControllerTest
    {
        [TestMethod]
        public void Index()
        {
            var chatServices = new Mock<IChatServices>();
            var roomServices = new Mock<IRoomServices>();

            var identity = new GenericIdentity("TestUsername");
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "123"));

            var fakePrincipal = A.Fake<IPrincipal>();
            A.CallTo(() => fakePrincipal.Identity).Returns(identity);

            var controller = new ChatController(chatServices.Object, roomServices.Object)
            {
                ControllerContext = A.Fake<ControllerContext>()
            };

            A.CallTo(() => controller.ControllerContext.HttpContext.User).Returns(fakePrincipal);

            chatServices.Setup(x => x.GetChats("123", 1, 1)).Returns(new ChatDto
            {
                Chatrooms = new List<ChatRoomDto>()
            });

            ViewResult result = controller.Index(1,1) as ViewResult;
           
            Assert.IsNotNull(result);
            var model = (ChatDto)result.ViewData.Model;
            Assert.AreEqual(0, model.Chatrooms.Count);

            var rooms = controller.GetRooms();
            Assert.IsNotNull(rooms);
        }
    }
}
