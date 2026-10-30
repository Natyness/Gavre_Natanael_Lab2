using Gavre_Natanael_Lab2.Models;

namespace Gavre_Natanael_Lab2.ViewModels
{
    public class PublisherIndexData
    {
        public IEnumerable<Publisher> Publishers { get ; set; }
        public IEnumerable<Book> Books { get; set; }
    }
}