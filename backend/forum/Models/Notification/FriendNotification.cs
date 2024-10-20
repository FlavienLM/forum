using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace forum.Models.Notification
{
    [Table("friend_notification")]
    public class FriendNotification
    {

        [Column("id")]
        public int Id { get; set; }

        [Column("receiver_id")]
        public int ReceiverId { get; set; }

        [ForeignKey("ReceiverId")]
        public required virtual User Receiver { get; set; }

        [Column("reference_id")]
        public int ReferenceId { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("is_read")]
        public bool IsRead { get; set; }

        [Column("sender_id")]
        public int SenderId { get; set; }

        [ForeignKey("SenderId")]
        public required virtual User Sender { get; set; }

    }


}
