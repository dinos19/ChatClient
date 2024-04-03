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
        WHOISON
    }

    public class ChatMessage
    {
        public ChatMessageAction Action { get; set; }
        public ChatMessageType Type { get; set; }
        public string Body { get; set; }
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
    }
}