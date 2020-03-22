using System.Threading.Tasks;
using MorseSignalRServer.Model;

namespace MorseSignalRServer.Hubs.Room
{
    public interface IRoomHum
    {
        Task ReceiveMessage(RoomMessageDto roomMessage);

        Task UserLeftRoom(RoomDto room);
        Task UserJoinedRoom(RoomDto room);
    
    }
}
