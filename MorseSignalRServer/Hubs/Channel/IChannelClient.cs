using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MorseSignalRServer.Model;

namespace MorseSignalRServer.Hubs.Interfaces
{
    public interface IChannelClient
    {
        Task ReceiveMessage(string user, string message);

        Task UserJoinedGroup(ChannelDto group);

        Task UserLeftGroup(ChannelDto group);
    }
}
