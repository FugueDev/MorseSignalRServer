using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MorseSignalRServer.Model
{
    public class ChannelDto
    {
        public string Id { get; set; }
        public string ChannelName { get; set; }
    }
    public class ChannelMessageDto
    {
        public string Message { get; set; }
        public string SenderName { get; set; }
    }
}
