using System.Threading.Tasks;
using MorseSignalRServer.Models;

namespace MorseSignalRServer.Hubs.Channel
{
    public interface IChannelHub
    {
        Task ReceiveMessage(RoomMessageDto roomMessage);
        Task UserLeftRoom(UserLeftRoomDto room);
        Task UserJoinedRoom(UserJoinedRoomDto room);

        Task UsersInRoom(UsersInChannelDto usersDto);

    }
}
