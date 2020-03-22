using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MorseSignalRServer.Model;

namespace MorseSignalRServer.Hubs.Lobby
{
   public interface ILobbyClient
    {
        Task RandomRoomFound(LobbyDto room);
    }
}
