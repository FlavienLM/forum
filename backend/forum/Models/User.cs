using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace forum.Models
{
    [Table("user")]
    public class User
    {

        [Column("id")]
        public int Id { get; set; }

        [Column("account_id")]
        public string? AccountId { get; set; }

        [Column("pseudo")]
        public string? Pseudo { get; set; }

        [Column("password")]
        public string? Password { get; set; }

        [Column("mail")]
        public string? Mail { get; set; }

        [Column("profile")]
        public string? Profile { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }
    }


}
