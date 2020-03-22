using System.Collections.Generic;
using System.Linq;

namespace MorseSignalRServer.Hubs.Channels
{
    internal static class ChannelHandler
    {
        public static Dictionary<string, string> ChannelDictionary = new Dictionary<string, string>();

        public static int NumberOfUsersInChannel(string channelName)
        {
            return ChannelDictionary.Count(x => x.Value == channelName);
        }
    }
}