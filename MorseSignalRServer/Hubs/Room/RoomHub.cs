using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using MorseSignalRServer.Model;

namespace MorseSignalRServer.Hubs.Room
{
    public static class UserHandler
    {
        public static Dictionary<string, string> RoomConnectedIds = new Dictionary<string, string>();
    }

    public class RoomHub : Hub<IRoomHum>
    {
        [HubMethodName("Send")]
        public async Task SendRoom(RoomMessageDto roomMessageDto)
        {
            var result = UserHandler.RoomConnectedIds.TryGetValue(Context.ConnectionId, out var roomName);
            if (result)
                await Clients.OthersInGroup(roomName).ReceiveMessage(roomMessageDto);
        }

        [HubMethodName("Join")]
        public async Task JoinRoom(RoomDto roomDto)
        {
            if (!UserHandler.RoomConnectedIds.ContainsKey(Context.ConnectionId))
                UserHandler.RoomConnectedIds.Add(Context.ConnectionId, roomDto.Name);
            roomDto.Id = Context.ConnectionId;
            await Groups.AddToGroupAsync(Context.ConnectionId, roomDto.Name);
            await Clients.OthersInGroup(roomDto.Id).UserJoinedRoom(roomDto);
        }

        [HubMethodName("Leave")]
        public async Task LeaveRoom(RoomDto roomDto)
        {
            if (UserHandler.RoomConnectedIds.ContainsKey(Context.ConnectionId))
                UserHandler.RoomConnectedIds.Remove(Context.ConnectionId);
            roomDto.Id = Context.ConnectionId;
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
                UserHandler.RoomConnectedIds.Remove(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}