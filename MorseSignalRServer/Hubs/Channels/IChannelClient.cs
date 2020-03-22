using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MorseSignalRServer.Models;

namespace MorseSignalRServer.Hubs.Channels
{
    public interface IChannelClient
    {
        Task ReceiveMessage(ChannelMessageDto channelMessage);
        Task UserJoinedChannel(ChannelDto group);
        Task UserLeftChannel(ChannelDto group);
        Task UsersInGroup(int numberOfUsers);
        Task UserJoinedChannel(UserJoinedChannelDto dto);
        Task UserLeftChannel(UserLeftChannelDto dto);
    }
}
