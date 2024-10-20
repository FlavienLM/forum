using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace forum.Models
{
    [Table("friend_message")]
    public class FriendMessage
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("sender_id")]
        public int SenderId { get; set; }

        [ForeignKey("SenderId")]
        public virtual User? Sender { get; set; }

        [Column("receiver_id")]
        public int ReceiverId { get; set; }

        [ForeignKey("ReceiverId")]
        public virtual User? Receiver { get; set; }

        [Column("content")]
        public string? Content { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("state")]
        public int State {get; set;} = 0;

        [Column("last_modified")]
        public DateTime? LastModified {get; set;} = null;

    }
}
