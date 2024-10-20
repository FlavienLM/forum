using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace forum.Models
{
    public class MessageDTO
    {
        
        public int Id { get; set; }
        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public virtual User? Author { get; set; }
        public int DiscussionId { get; set; }
        [ForeignKey("DiscussionId")]
        public virtual Discussion? Discussion { get; set; }
        public string? Content { get; set; }
        public DateTime Date { get; set; }
        public int Status {get; set;}
        public DateTime? LastModified {get; set;} = null;

        public string? Pseudo {get; set;} = null;       
    }
}
