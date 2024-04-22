using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChatClient.Models
{
    [Table("ChatFile")]
    public class ChatFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public int UploadResultId { get; set; }
        public string? UploadResultFileName { get; set; }
        public string? UploadResultStoredFileName { get; set; }
        public string? UploadResultContentType { get; set; }
        public byte[] FileContentArray { get; set; }
    }
}