namespace MorseSignalRServer.Models
{
    public class UserJoinedChannelDto
    {
        public UserJoinedChannelDto(string channelName, string id)
        {
            ChannelName = channelName;
            Id = id;
        }

        public string ChannelName { get; }
        public string Id { get; }
    }
}