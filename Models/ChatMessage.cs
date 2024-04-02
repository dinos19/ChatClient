namespace ChatClient.Models
{
    public enum ChatMessageType : int
    {
        TEXT,
        IMAGE,
        AUDIO,
        VIDEO
    }

    public enum ChatMessageAction : int
    {
        ANNOUNCEMENTS,
        HELLO,
    }

    public class ChatMessage
    {
        public ChatMessageAction Action { get; set; }
        public ChatMessageType Type { get; set; }
        public string Body { get; set; }
        public string FromUser { get; set; }
        public string ToUser { get; set; }
    }
}