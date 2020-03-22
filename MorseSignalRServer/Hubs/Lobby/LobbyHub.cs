using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using MorseSignalRServer.Model;

namespace MorseSignalRServer.Hubs.Lobby
{
    public static class LobbyHandler
    {
        public static HashSet<string> SearchingIds = new HashSet<string>();
    }

    public class LobbyHub : Hub<ILobbyClient>
    {
        [HubMethodName("Find")]
        public async Task FindRandomRoom()
        {
            if (!LobbyHandler.SearchingIds.Contains(Context.ConnectionId))
                LobbyHandler.SearchingIds.Add(Context.ConnectionId);
            if (LobbyHandler.SearchingIds.Count >= 2)
            {
                var idsToPair = LobbyHandler.SearchingIds.Take(2).ToList();

                await Clients.Clients(idsToPair)
                    .RandomRoomFound(new LobbyDto {Name = Guid.NewGuid().ToString()});

                foreach (var id in idsToPair)
                {
                    if (LobbyHandler.SearchingIds.Contains(id))
                        LobbyHandler.SearchingIds.Remove(id);
                }
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (LobbyHandler.SearchingIds.Contains(Context.ConnectionId))
                LobbyHandler.SearchingIds.Remove(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}