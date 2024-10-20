using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace forum.Models.Notification
{
    [Table("user_notification")]
    public class UserNotification
    {

        [Column("id")]
        public int Id { get; set; }

        [Column("receiver_id")]
        public int ReceiverId { get; set; }

        [ForeignKey("ReceiverId")]
        public virtual User? Receiver { get; set; }

        [Column("reference_id")]
        public int ReferenceId { get; set; }
        [Column("reference_string")]
        public string? ReferenceString { get; set; } = null;

        [Column("notification_type")] // 0 = Friend, 1 = Discussion, 2 = Request
        public int NotificationType { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("is_read")]
        public bool IsRead { get; set; } = false;

        [Column("friend_request_id")]
        public int? FriendRequestId { get; set; }

        [ForeignKey("FriendRequestId")]
        public virtual Friendship? FriendRequest { get; set; }

    }


}
