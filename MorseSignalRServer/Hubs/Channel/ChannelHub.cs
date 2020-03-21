using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using MorseSignalRServer.Hubs.Interfaces;
using MorseSignalRServer.Model;

namespace MorseSignalRServer.Hubs
{
    public static class UserHandler
    {
        public static Dictionary<string, string> RoomDictionary = new Dictionary<string, string>();
    }

    public class ChannelHub : Hub<IChannelClient>
    {
        public async Task SendMessage(ChannelMessageDto channelDto)
        {
            await Clients.OthersInGroup(channelDto.ChannelName)
                .ReceiveMessage(channelDto.SenderName, channelDto.Message);
        }


        public async Task JoinGroup(ChannelDto channelDto)
        {
            channelDto.Id = Context.ConnectionId;
            UserHandler.RoomDictionary.Add(channelDto.Id, channelDto.ChannelName);
            await Groups.AddToGroupAsync(Context.ConnectionId, channelDto.ChannelName);
            await Clients.OthersInGroup(channelDto.ChannelName).UserJoinedGroup(channelDto);
        }

        public async Task LeaveGroup(ChannelDto channelDto)
        {
            channelDto.Id = Context.ConnectionId;
            UserHandler.RoomDictionary.Remove(channelDto.Id);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, channelDto.ChannelName);
            await Clients.OthersInGroup(channelDto.Id).UserLeftGroup(channelDto);
        }

        public override async Task OnConnectedAsync()
        {
            var result = UserHandler.RoomDictionary.TryGetValue(Context.ConnectionId, out var roomName);
            if (result)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                await Clients.Caller.UserJoinedGroup(new ChannelDto
                    {ChannelName = roomName, Id = Context.ConnectionId});
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var result = UserHandler.RoomDictionary.TryGetValue(Context.ConnectionId, out var roomName);
            if (result)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
                await Clients.Caller.UserLeftGroup(new ChannelDto
                    {ChannelName = roomName, Id = Context.ConnectionId});
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}