using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gavre_Natanael_Lab2.Models
{
    public class Book
    {
        public int ID {get; set; }

        [Display(Name = "Book Titile")]
        [StringLength(30, MinimumLength = 3)]
        [Required(ErrorMessage = "Titlul cartii este obligatoriu")]
        public string Title {get; set; }

        public int? AuthorID { get; set; }
        public Author? Author { get; set; }

        [Column(TypeName = "decimal(6, 2)")]
        [Range(0.01, 500)]
        public decimal Prince { get; set; }

        [DataType(DataType.Date)]
        public DateTime PublishingDate { get; set; }

        public int? PublisherID { get; set; }
        public Publisher? Publisher { get; set; }

        public ICollection<Borrowing>? Borrowings { get; set;}

        public ICollection<BookCategory> BookCategories { get; set; }
    }
}