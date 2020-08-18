using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using MorseSignalRServer.Models;

namespace MorseSignalRServer.Hubs.Channel
{
    public static class ChannelHandler
    {
        public static Dictionary<string, string> RoomConnectedIds = new Dictionary<string, string>();
    }

    public class ChannelHub : Hub<IChannelHub>
    {
        [HubMethodName("Send")]
        public async Task SendRoom(RoomMessageDto roomMessageDto)
        {
            var result = ChannelHandler.RoomConnectedIds.TryGetValue(Context.ConnectionId, out var roomName);
            if (result)
                await Clients.OthersInGroup(roomName).ReceiveMessage(roomMessageDto);
        }

        [HubMethodName("Join")]
        public async Task JoinRoom(UserJoinedRoomDto roomDto)
        {
            if (!ChannelHandler.RoomConnectedIds.ContainsKey(Context.ConnectionId))
                ChannelHandler.RoomConnectedIds.Add(Context.ConnectionId, roomDto.Name);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomDto.Name);
            await Clients.OthersInGroup(roomDto.Id).UserJoinedRoom(roomDto);
            await Clients.Group(roomDto.Id).UsersInRoom(new UsersInChannelDto{NumberOfUsers =
                ChannelHandler.RoomConnectedIds.Count(x => x.Value == roomDto.Name) - 1});
        }

        [HubMethodName("Leave")]
        public async Task LeaveRoom(UserLeftRoomDto roomDto)
        {
            if (ChannelHandler.RoomConnectedIds.ContainsKey(Context.ConnectionId))
                ChannelHandler.RoomConnectedIds.Remove(Context.ConnectionId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomDto.Name);
            await Clients.OthersInGroup(roomDto.Id).UserLeftRoom(roomDto);
            await Clients.OthersInGroup(roomDto.Id).UsersInRoom(new UsersInChannelDto{NumberOfUsers =
                ChannelHandler.RoomConnectedIds.Count(x => x.Value == roomDto.Name) -1});
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (ChannelHandler.RoomConnectedIds.ContainsKey(Context.ConnectionId))
            {
                var isInRoom = ChannelHandler.RoomConnectedIds.TryGetValue(Context.ConnectionId, out var roomName);
                if (isInRoom)
                {
                    await Clients.OthersInGroup(roomName).UsersInRoom(new UsersInChannelDto
                    {
                        NumberOfUsers =
                            ChannelHandler.RoomConnectedIds.Count(x => x.Value == roomName)
                   -1 } );
                    await Clients.OthersInGroup(roomName).UserLeftRoom(null);
                    ChannelHandler.RoomConnectedIds.Remove(Context.ConnectionId);
                }
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}