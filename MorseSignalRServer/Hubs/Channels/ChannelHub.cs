using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using MorseSignalRServer.Models;

namespace MorseSignalRServer.Hubs.Channels
{
    public class ChannelHub : Hub<IChannelClient>
    {
        [HubMethodName("Send")]
        public async Task SendMessage(ChannelMessageDto channelDto)
        {
            var result = ChannelHandler.ChannelDictionary.TryGetValue(Context.ConnectionId, out var channelName);
            if (result)
            {
                await Clients.OthersInGroup(channelName)
                    .ReceiveMessage(channelDto);
            }
        }

        [HubMethodName("Join")]
        public async Task JoinChannel(ChannelDto channelDto)
        {
            ChannelHandler.ChannelDictionary.Add(Context.ConnectionId, channelDto.ChannelName);

            await Groups.AddToGroupAsync(Context.ConnectionId, channelDto.ChannelName);

            await Clients.OthersInGroup(channelDto.ChannelName).UserJoinedChannel(new UserJoinedChannelDto(channelDto.ChannelName, channelDto.Id));

            await Clients.Others.UsersInGroup(ChannelHandler.NumberOfUsersInChannel(channelDto.ChannelName));
        }

        [HubMethodName("Leave")]
        public async Task LeaveChannel(ChannelDto channelDto)
        {
            ChannelHandler.ChannelDictionary.Remove(Context.ConnectionId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, channelDto.ChannelName);
            await Clients.OthersInGroup(Context.ConnectionId).UserLeftChannel(new UserLeftChannelDto(channelDto.ChannelName, Context.ConnectionId));
            await Clients.Others.UsersInGroup(ChannelHandler.NumberOfUsersInChannel(channelDto.ChannelName));
        }

        public override async Task OnConnectedAsync()
        {
            var result = ChannelHandler.ChannelDictionary.TryGetValue(Context.ConnectionId, out var roomName);
            if (result)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, roomName);

                await Clients.Caller.UserJoinedChannel(new UserJoinedChannelDto(roomName,
                                                                                Context.ConnectionId));

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
                await Clients.Others.UserLeftChannel(new UserLeftChannelDto(roomName, Context.ConnectionId));
                await Clients.Others.UsersInGroup(ChannelHandler.NumberOfUsersInChannel(roomName));
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}

