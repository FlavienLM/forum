using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace forum.Models
{
    [Table("message")]
    public class Message
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("author_id")]
        public int AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public virtual User? Author { get; set; }

        [Column("discussion_id")]
        public int DiscussionId { get; set; }

        [ForeignKey("DiscussionId")]
        public virtual Discussion? Discussion { get; set; }

        [Column("content")]
        public string? Content { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("status")]
        public int Status {get; set;}

        [Column("last_modified")]
        public DateTime? LastModified {get; set;} = null;
        
    }
}
