using System.ComponentModel.DataAnnotations;

namespace Library_Db2.Models
{
    public class Authors
    {
        [Key]
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string Email { get; set; }
        public int Telnumber { get; set; }
    }
}
