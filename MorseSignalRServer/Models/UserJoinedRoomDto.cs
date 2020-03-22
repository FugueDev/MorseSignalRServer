namespace MorseSignalRServer.Models
{
    public class UserJoinedRoomDto
    {
        public UserJoinedRoomDto(string roomName, string id)
        {
            Name = roomName;
            Id = id;
        }

        public string Name { get; }
        public string Id { get; }
    }
}