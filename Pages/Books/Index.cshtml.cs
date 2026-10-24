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
        private readonly Gavre_Natanael_Lab2.Data.Gavre_Natanael_Lab2Context _context;

        public IndexModel(Gavre_Natanael_Lab2.Data.Gavre_Natanael_Lab2Context context)
        {
            _context = context;
        }

        public IList<Book> Book { get;set; } = default!;

        
        public async Task OnGetAsync(int? id, int? categoryID)
        {
            BookD = new BookData();

            BookD.Books = await _context.Book
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.BookCategories)
                    .ThenInclude(bc => bc.Category)
                .AsNoTracking()
                .OrderBy(b => b.Title)
                .ToListAsync();

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
        }

    }
}
