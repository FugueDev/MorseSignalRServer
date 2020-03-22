namespace MorseSignalRServer.Models
{
    public class UserLeftChannelDto
    {
        public UserLeftChannelDto(string channelName, string id)
        {
            ChannelName = channelName;
            Id = id;
        }

        public string ChannelName { get; }
        public string Id { get; }
    }
}