using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
        WHOISON,
        NOACTION
    }

    public enum ChatMessageStatus : int
    {
        INIT,
        SERVER_RECIEVED,
    }

    [Table("ChatMessage")]
    public class ChatMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ChatMessageId { get; set; }

        public ChatMessageAction Action { get; set; }
        public ChatMessageType Type { get; set; }
        public ChatMessageStatus Status { get; set; }
        public string Body { get; set; }
        public int FromAccountId { get; set; }

        [ForeignKey("FromAccountId")]
        public virtual Account FromAccount { get; set; }

        public int ToAccountId { get; set; }

        [ForeignKey("ToAccountId")]
        public virtual Account ToAccount { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime ReadDate { get; set; }

        [NotMapped]
        public ChatFile ChatFile { get; set; }
    }
}