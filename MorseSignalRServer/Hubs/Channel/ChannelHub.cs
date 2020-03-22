using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using MorseSignalRServer.Hubs.Interfaces;
using MorseSignalRServer.Model;

namespace MorseSignalRServer.Hubs
{
    internal static class ChannelHandler
    {
        public static Dictionary<string, string> ChannelDictionary = new Dictionary<string, string>();

        public static int NumberOfUsersInChannel(string channelName)
        {
            return ChannelDictionary.Count(x => x.Value == channelName);
        }
    }

    public class ChannelHub : Hub<IChannelClient>
    {
        [HubMethodName("Send")]
        public async Task SendMessage(ChannelMessageDto channelDto)
        {
            await Clients.OthersInGroup(channelDto.ChannelName)
                .ReceiveMessage(channelDto.SenderName, channelDto.Message);
        }

        [HubMethodName("Join")]
        public async Task JoinChannel(ChannelDto channelDto)
        {
            channelDto.Id = Context.ConnectionId;
            ChannelHandler.ChannelDictionary.Add(channelDto.Id, channelDto.ChannelName);
            await Groups.AddToGroupAsync(Context.ConnectionId, channelDto.ChannelName);
            await Clients.OthersInGroup(channelDto.ChannelName).UserJoinedChannel(channelDto);
            await Clients.Others.UsersInGroup(ChannelHandler.NumberOfUsersInChannel(channelDto.ChannelName));
        }
        [HubMethodName("Leave")]
        public async Task LeaveChannel(ChannelDto channelDto)
        {
            channelDto.Id = Context.ConnectionId;
            ChannelHandler.ChannelDictionary.Remove(channelDto.Id);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, channelDto.ChannelName);
            await Clients.OthersInGroup(channelDto.Id).UserLeftChannel(channelDto);
            await Clients.Others.UsersInGroup(ChannelHandler.NumberOfUsersInChannel(channelDto.ChannelName));
        }

        public override async Task OnConnectedAsync()
        {
            var result = ChannelHandler.ChannelDictionary.TryGetValue(Context.ConnectionId, out var roomName);
            if (result)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                await Clients.Caller.UserJoinedChannel(new ChannelDto
                    {ChannelName = roomName, Id = Context.ConnectionId});
                await Clients.Others.UsersInGroup(ChannelHandler.NumberOfUsersInChannel(roomName));
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var result = ChannelHandler.ChannelDictionary.TryGetValue(Context.ConnectionId, out var roomName);
            if (result)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
                await Clients.Others.UserLeftChannel(new ChannelDto
                    {ChannelName = roomName, Id = Context.ConnectionId});
                await Clients.Others.UsersInGroup(ChannelHandler.NumberOfUsersInChannel(roomName));
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}