using System.ComponentModel.DataAnnotations;

namespace SMS.Models
{
    public class Chat
    {
        [Key]
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }


        public User Sender { get; set; }
        public User Receiver { get; set; }
    }
}
