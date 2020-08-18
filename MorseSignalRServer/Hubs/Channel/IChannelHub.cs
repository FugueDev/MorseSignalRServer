using System.Threading.Tasks;
using MorseSignalRServer.Models;

namespace MorseSignalRServer.Hubs.Room
{
    public interface IRoomHub
    {
        Task ReceiveMessage(RoomMessageDto roomMessage);
        Task UserLeftRoom(UserLeftRoomDto room);
        Task UserJoinedRoom(UserJoinedRoomDto room);
    
    }
}
