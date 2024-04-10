﻿using jihadkhawaja.chat.client.Core;
using jihadkhawaja.chat.shared.Interfaces;
using jihadkhawaja.chat.shared.Models;
using jihadkhawaja.chat.client.Services;
using System.Text.Json;

namespace Egroo.Server.Test
{
    [TestClass]
    public class ChannelTest
    {
        private IChatAuth ChatAuthService { get; set; } = null!;
        private IChatChannel ChatChannelService { get; set; } = null!;

        private static Channel Channel { get; set; }

        [TestInitialize]
        public async Task Initialize()
        {
            ChatAuthService = new ChatAuthService();
            ChatChannelService = new ChatChannelService();

            MobileChatSignalR.Initialize(TestConfig.HubConnectionUrl);
            await MobileChatSignalR.HubConnection.StartAsync();

            dynamic? dynamicObj = await ChatAuthService.SignIn("test", "HvrnS4Q4zJ$xaW!3");
            Dictionary<string, object>? result = null;
            if (dynamicObj is not null)
            {
                result = JsonSerializer.Deserialize<Dictionary<string, object>>(dynamicObj);
            }

            //check user
            if (result is not null)
            {
                string Token = result["token"].ToString();

                MobileChatSignalR.Initialize(TestConfig.HubConnectionUrl, Token);
                await MobileChatSignalR.HubConnection.StartAsync();
            }
            else
            {
                MobileChatSignalR.Initialize(TestConfig.HubConnectionUrl);
                await MobileChatSignalR.HubConnection.StartAsync();
            }
        }
        [TestMethod]
        public async Task CreateChannelTest2()
        {
            Channel = await ChatChannelService.CreateChannel("test");

            Assert.IsNotNull(Channel);
        }

        [TestMethod]
        public async Task DeleteChannelTest1()
        {
            Assert.IsTrue(await ChatChannelService.DeleteChannel(Channel.Id));
        }

        [TestCleanup]
        public async Task Cleanup()
        {
        }
    }
}
