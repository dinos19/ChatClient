using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChatClient.Models
{
    [Table("Account")]
    public class Account
    {
        [Key]
        public int AccountId { get; set; }

        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int GroupId { get; set; }
        public bool IsFriend { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        [NotMapped]
        public bool IsOnline { get; set; }



        public override bool Equals(object? obj1)
        {
            if(obj1 == null || this == null) return false;

            if (((Account)obj1).AccountId > 0 && this.AccountId > 0)
                if (((Account)obj1).AccountId == this.AccountId)
                    return true;
                else
                    return false;
            else if(!string.IsNullOrEmpty(((Account)obj1).Email) && !string.IsNullOrEmpty(this.Email))
                if(((Account)obj1).Email == this.Email && ((Account)obj1).Name == this.Name)
                    return true;
                else
                    return false;

            return false;
        }
    }
}