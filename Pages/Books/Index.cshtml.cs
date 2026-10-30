using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Gavre_Natanael_Lab2.Data;
using Gavre_Natanael_Lab2.Models;

namespace Gavre_Natnael_Lab2.Pages.Books
{
    public class IndexModel : PageModel
    {

        public BookData BookD { get; set; } = new BookData();

        public int BookID { get; set; }

        public int CategoryID { get; set; }

        public string TitleSort { get; set; }

        public string AuthorSort { get; set; }

        public string CurrentFilter { get; set; }
        private readonly Gavre_Natanael_Lab2.Data.Gavre_Natanael_Lab2Context _context;

        public IndexModel(Gavre_Natanael_Lab2.Data.Gavre_Natanael_Lab2Context context)
        {
            _context = context;
        }

        public IList<Book> Book { get;set; } = default!;

        
        public async Task OnGetAsync(int? id, int? categoryID, string sortOrder, string searchString)
        {
            BookD = new BookData();

            TitleSort = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            AuthorSort = sortOrder == "author" ? "author_desc" : "author";

            CurrentFilter = searchString;

            BookD.Books = await _context.Book
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.BookCategories)
                    .ThenInclude(bc => bc.Category)
                .AsNoTracking()
                .OrderBy(b => b.Title)
                .ToListAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                BookD.Books = BookD.Books.Where(s => s.Author.FirstName.Contains(searchString) || s.Author.LastName.Contains(searchString) || s.Title.Contains(searchString));
            }

            // Pastram compatibilitatea cu pagina Index actuala
            Book = BookD.Books.ToList();

            if (id != null)
            {
                BookID = id.Value;

                var book = BookD.Books
                    .FirstOrDefault(b => b.ID == id.Value);

                if (book != null)
                {
                    BookD.Categories = book.BookCategories?
                        .Select(bc => bc.Category)
                        ?? Enumerable.Empty<Category>();
                }
            }

            switch (sortOrder)
            {
                case "title_desc" :
                    BookD.Books = BookD.Books.OrderByDescending(s =>s.Title);
                        break ;
                case "author_desc" :
                    BookD.Books = BookD.Books.OrderByDescending(s =>s.Author.FullName);
                        break ;
                case "author" :
                    BookD.Books = BookD.Books.OrderBy(s =>s.Author.FullName);
                        break ;
                default:
                    BookD.Books = BookD.Books.OrderBy (s => s.Title);
                        break;
            }
        }


    }
}
