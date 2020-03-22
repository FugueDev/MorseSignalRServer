using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MorseSignalRServer.Models;

namespace MorseSignalRServer.Hubs.Channels
{
    public interface IChannelClient
    {
        Task ReceiveMessage(string user, string message);
        Task UserJoinedChannel(UserJoinedChannelDto dto);
        Task UserLeftChannel(UserLeftChannelDto dto);
        Task UsersInGroup(int numberOfUsers);
    }
}
