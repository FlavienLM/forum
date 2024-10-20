using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace forum.Models.Notification
{
    [Table("discussion_notification")]
    public class DiscussionNotification
    {

        [Column("id")]
        public int Id { get; set; }

        [Column("receiver_id")]
        public int ReceiverId { get; set; }

        [ForeignKey("ReceiverId")]
        public virtual User? Receiver { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("is_read")]
        public bool IsRead { get; set; }

        [Column("discussion_id")]
        public int DiscussionId { get; set; }

        [ForeignKey("DiscussionId")]
        public virtual Discussion? Discussion { get; set; }
    }
}
