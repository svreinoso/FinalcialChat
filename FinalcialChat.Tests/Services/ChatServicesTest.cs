using System;
using FinalcialChat.Dtos;
using FinalcialChat.Interfaces;
using FinalcialChat.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FinalcialChat.Tests.Services
{
    [TestClass]
    public class ChatServicesTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var httpClientManager = new Mock<IHttpClientManager>();

            var chatServices = new ChatServices(httpClientManager.Object);
            var chatDto = chatServices.GetChats("123", 1, 1);

            Assert.IsNotNull(chatDto);
        }
    }
}
