using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gavre_Natanael_Lab2.Data;
using Gavre_Natanael_Lab2.Models;

namespace Gavre_Natnael_Lab2.Pages.Borrowings
{
    public class EditModel : PageModel
    {
        private readonly Gavre_Natanael_Lab2.Data.Gavre_Natanael_Lab2Context _context;

        public EditModel(Gavre_Natanael_Lab2.Data.Gavre_Natanael_Lab2Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Borrowing Borrowing { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowing =  await _context.Borrowing.FirstOrDefaultAsync(m => m.ID == id);
            if (borrowing == null)
            {
                return NotFound();
            }
            Borrowing = borrowing;
           PopulateDropdowns(borrowing.BookID, borrowing.MemberID);
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PopulateDropdowns(Borrowing.BookID, Borrowing.MemberID);
                return Page();
            }

            _context.Attach(Borrowing).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BorrowingExists(Borrowing.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool BorrowingExists(int id)
        {
            return _context.Borrowing.Any(e => e.ID == id);
        }


        private void PopulateDropdowns(int? bookID, int? memberID)
        {
            var bookList = _context.Book
                .Select(b => new
                {
                    b.ID,
                    BookFullName = b.Title + " - " +
                        (b.Author != null
                            ? b.Author.LastName + " " + b.Author.FirstName
                            : "Fara autor")
                })
                .ToList();

            var memberList = _context.Member
                .Select(m => new
                {
                    m.ID,
                    FullName = m.FirstName + " " + m.LastName
                })
                .ToList();

            ViewData["BookID"] = new SelectList(
                bookList, "ID", "BookFullName", bookID);

            ViewData["MemberID"] = new SelectList(
                memberList, "ID", "FullName", memberID);
        }

    }
}
