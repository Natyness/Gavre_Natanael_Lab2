using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Gavre_Natanael_Lab2.Data;
using Gavre_Natanael_Lab2.Models;
using Gavre_Natanael_Lab2.ViewModels;

namespace Gavre_Natnael_Lab2.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly Gavre_Natanael_Lab2.Data.Gavre_Natanael_Lab2Context _context;

        public IndexModel(Gavre_Natanael_Lab2.Data.Gavre_Natanael_Lab2Context context)
        {
            _context = context;
        }

        public IList<Category> Category { get;set; } = default!;

        public CategoryIndexData CategoryData { get; set; } = new();        //tema lab4

        public int CategoryID { get; set; }                                 //tema lab4

        
        public async Task OnGetAsync(int? id)
        {
            CategoryData = new CategoryIndexData();

            // Incarcam toate categoriile
            CategoryData.Categories = await _context.Category
                .OrderBy(c => c.CategoryName)
                .ToListAsync();

            // Pastram lista utilizata de pagina existenta
            Category = CategoryData.Categories.ToList();

            // Daca utilizatorul a selectat o categorie
            if (id != null)
            {
                CategoryID = id.Value;

                // Cautam cartile din categoria selectata
                CategoryData.Books = await _context.Book
                    .Include(b => b.Author)
                    .Where(b => b.BookCategories
                        .Any(bc => bc.CategoryID == id.Value))
                    .OrderBy(b => b.Title)
                    .AsNoTracking()
                    .ToListAsync();
            }
        }

    }
}
