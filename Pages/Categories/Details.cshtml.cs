using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Gavre_Natanael_Lab2.Data;
using Gavre_Natanael_Lab2.Models;

namespace Gavre_Natnael_Lab2.Pages.Categories
{
    public class DetailsModel : PageModel
    {
        private readonly Gavre_Natanael_Lab2.Data.Gavre_Natanael_Lab2Context _context;

        public DetailsModel(Gavre_Natanael_Lab2.Data.Gavre_Natanael_Lab2Context context)
        {
            _context = context;
        }

        public Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FirstOrDefaultAsync(m => m.ID == id);

            if (category is not null)
            {
                Category = category;

                return Page();
            }

            return NotFound();
        }
    }
}
