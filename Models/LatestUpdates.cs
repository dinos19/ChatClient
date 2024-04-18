using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatClient.Models
{
    [Table("LatestUpdates")]

    public class LatestUpdates
    {
        [Key]

        public int LatestUpdatesId { get; set; }
        [NotMapped]
        public Account CurrentAccount { get; set; }

        public DateTime Account { get; set; }
        public DateTime ChatMessage { get; set; }
        public DateTime UserConnection { get; set; }
    }
}
