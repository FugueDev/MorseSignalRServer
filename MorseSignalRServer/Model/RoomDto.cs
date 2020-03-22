using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MorseSignalRServer.Model
{
    public class RoomDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class RoomMessageDto
    {
        public string Message { get; set; }
        public string SenderName { get; set; }
    }
}
