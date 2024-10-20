using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace forum.Models
{
    [Table("friend_request")]
    public class Friendship
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

        [Column("status")]
        public FriendshipStatus Status { get; set; } = FriendshipStatus.Pending;  // Using enum instead of a boolean

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("lastMessage")]
        public string? LastMessage { get; set; } = null;
    }

    // Enum for Friendship Status
    public enum FriendshipStatus
    {
        Pending,   // Request is sent but not yet accepted or rejected
        Accepted,  // Friendship request is accepted
        Rejected   // Friendship request is rejected
    }
}
