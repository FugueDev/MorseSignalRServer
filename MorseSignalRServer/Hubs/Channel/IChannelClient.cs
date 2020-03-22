using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MorseSignalRServer.Model;

namespace MorseSignalRServer.Hubs.Interfaces
{
    public interface IChannelClient
    {
        Task ReceiveMessage(ChannelMessageDto channelMessage);

        Task UserJoinedChannel(ChannelDto group);

        Task UserLeftChannel(ChannelDto group);
        Task UsersInGroup(int numberOfUsers);
    }
}
