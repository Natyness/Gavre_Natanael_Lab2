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

namespace Gavre_Natnael_Lab2.Pages.Publishers
{
    public class IndexModel : PageModel
    {
        private readonly Gavre_Natanael_Lab2.Data.Gavre_Natanael_Lab2Context _context;

        public IndexModel(Gavre_Natanael_Lab2.Data.Gavre_Natanael_Lab2Context context)
        {
            _context = context;
        }

        public IList<Publisher> Publisher { get;set; } = default!;

        public PublisherIndexData PublisherData { get; set; } = new();

        public int PublisherID { get; set; }

        public int BookID { get; set; }

        
        public async Task OnGetAsync(int? id, int? bookID)
        {
            PublisherData = new PublisherIndexData();

            PublisherData.Publishers = await _context.Publisher
                .Include(p => p.Books)
                    .ThenInclude(b => b.Author)
                .OrderBy(p => p.PublisherName)
                .ToListAsync();

            // Pastram compatibilitatea cu pagina actuala
            Publisher = PublisherData.Publishers.ToList();

            if (id != null)
            {
                PublisherID = id.Value;

                var publisher = PublisherData.Publishers
                    .FirstOrDefault(p => p.ID == id.Value);

                if (publisher != null)
                {
                    PublisherData.Books =
                        publisher.Books ?? new List<Book>();
                }
            }
        }

    }
}
