using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace forum.Models
{
    [Table("discussion")]
    public class Discussion
    {

        [Column("id")]
        public int Id { get; set; }

        [Column("creator_id")]
        public int CreatorId { get; set; }
        public virtual User? Creator { get; set; }

        [Column("title")]
        public string? Title { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("lastMessage")]
        public string? LastMessage { get; set; } = null;

    }




}
