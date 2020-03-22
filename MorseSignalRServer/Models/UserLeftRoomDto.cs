namespace MorseSignalRServer.Models
{
    public class UserLeftRoomDto
    {
        public UserLeftRoomDto(string roomName, string id)
        {
            Name = roomName;
            Id = id;
        }

        public string Name { get; }
        public string Id { get; }
    }
}