
using Gavre_Natanael_Lab2.Models;

namespace Gavre_Natanael_Lab2.ViewModels
{
    public class CategoryIndexData
    {
        public IEnumerable<Category> Categories { get; set; }
            = new List<Category>();

        public IEnumerable<Book> Books { get; set; }
            = new List<Book>();
    }
}
