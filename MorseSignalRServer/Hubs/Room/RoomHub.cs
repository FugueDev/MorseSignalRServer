using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using MorseSignalRServer.Models;

namespace MorseSignalRServer.Hubs.Room
{
    public static class UserHandler
    {
        public static Dictionary<string, string> RoomConnectedIds = new Dictionary<string, string>();
    }

    public class RoomHub : Hub<IRoomHub>
    {
        [HubMethodName("Send")]
        public async Task SendRoom(RoomMessageDto roomMessageDto)
        {
            var result = UserHandler.RoomConnectedIds.TryGetValue(Context.ConnectionId, out var roomName);
            if (result)
                await Clients.OthersInGroup(roomName).ReceiveMessage(roomMessageDto);
        }

        [HubMethodName("Join")]
        public async Task JoinRoom(UserJoinedRoomDto roomDto)
        {
            if (!UserHandler.RoomConnectedIds.ContainsKey(Context.ConnectionId))
                UserHandler.RoomConnectedIds.Add(Context.ConnectionId, roomDto.Name);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomDto.Name);
            await Clients.OthersInGroup(roomDto.Id).UserJoinedRoom(roomDto);
        }

        [HubMethodName("Leave")]
        public async Task LeaveRoom(UserLeftRoomDto roomDto)
        {
            if (UserHandler.RoomConnectedIds.ContainsKey(Context.ConnectionId))
                UserHandler.RoomConnectedIds.Remove(Context.ConnectionId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomDto.Name);
            await Clients.OthersInGroup(roomDto.Id).UserLeftRoom(roomDto);
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (UserHandler.RoomConnectedIds.ContainsKey(Context.ConnectionId))
            {
                var isInRoom = UserHandler.RoomConnectedIds.TryGetValue(Context.ConnectionId, out var roomName);
                if (isInRoom)
                    await Clients.OthersInGroup(roomName).UserLeftRoom(null);
                UserHandler.RoomConnectedIds.Remove(Context.ConnectionId);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}