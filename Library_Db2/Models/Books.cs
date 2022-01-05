using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_Db2.Models
{
    public class Books
    {
        public enum CategoryType
        {
            Adventure,
            Drama,
            Historical,
            Horror,
            Romantic
        }
        [Key]
        public int BookId { get; set; }
        public string BookName { get; set; }
        public CategoryType Category { get; set; }
        public decimal Price { get; set; }
        public int Pages { get; set; }
        [DisplayName("Author")]
        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public Authors Authors { get; set; }
        //public ICollection<Authors> Author { get; set; }

    }
}
