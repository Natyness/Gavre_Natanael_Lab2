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

namespace Gavre_Natanael_Lab2.Pages.Books
{
    public class EditModel : BookCategoriesPageModel
    {
        private readonly Gavre_Natanael_Lab2Context _context;

        public EditModel(Gavre_Natanael_Lab2Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Book Book { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.BookCategories)
                    .ThenInclude(bc => bc.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.ID == id);

            if (book == null)
            {
                return NotFound();
            }

            Book = book;

            PopulateAssignedCategoryData(_context, Book);
            PopulateDropdowns(Book);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(
            int? id, string[]? selectedCategories)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookToUpdate = await _context.Book
                .Include(b => b.BookCategories)
                    .ThenInclude(bc => bc.Category)
                .FirstOrDefaultAsync(b => b.ID == id);

            if (bookToUpdate == null)
            {
                return NotFound();
            }

            ModelState.Remove("Book.BookCategories");

            if (!ModelState.IsValid)
            {
                PopulateAssignedCategoryData(_context, bookToUpdate);

                var selectedIds = new HashSet<string>(
                    selectedCategories ?? Array.Empty<string>());

                foreach (var category in AssignedCategoryDataList)
                {
                    category.Assigned = selectedIds.Contains(
                        category.CategoryID.ToString());
                }

                PopulateDropdowns(Book);

                return Page();
            }

            // Actualizam proprietatile cartii
            bookToUpdate.Title = Book.Title;
            bookToUpdate.AuthorID = Book.AuthorID;
            bookToUpdate.Prince = Book.Prince;
            bookToUpdate.PublishingDate = Book.PublishingDate;
            bookToUpdate.PublisherID = Book.PublisherID;

            // Actualizam categoriile selectate
            UpdateBookCategories(
                _context, selectedCategories, bookToUpdate);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private void PopulateDropdowns(Book book)
        {
            var authorList = _context.Author.Select(a => new
            {
                a.ID,
                FullName = a.LastName + " " + a.FirstName
            });

            ViewData["AuthorID"] = new SelectList(
                authorList,
                "ID",
                "FullName",
                book.AuthorID);

            ViewData["PublisherID"] = new SelectList(
                _context.Publisher,
                "ID",
                "PublisherName",
                book.PublisherID);
        }
    }
}

/*
namespace Gavre_Natnael_Lab2.Pages.Books
{
    public class EditModel : BookCategoriesPageModel
    {
        private readonly Gavre_Natanael_Lab2.Data.Gavre_Natanael_Lab2Context _context;

        public EditModel(Gavre_Natanael_Lab2.Data.Gavre_Natanael_Lab2Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Book Book { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.BookCategories)
                    .ThenInclude(bc => bc.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (book == null)
            {
                return NotFound();
            }

            Book = book;

            ViewData["PublisherID"] =
                new SelectList(_context.Set<Publisher>(), "ID", "PublisherName");

            ViewData["AuthorID"] =
                new SelectList(_context.Set<Author>(), "ID", "LastName");   

            PopulateAssignedCategoryData(_context, Book);

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        
        public async Task<IActionResult> OnPostAsync(
            int? id, string[] selectedCategories)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Incarcam cartea si relatiile existente
            var bookToUpdate = await _context.Book
                .Include(b => b.BookCategories)
                    .ThenInclude(bc => bc.Category)
                .FirstOrDefaultAsync(b => b.ID == id);

            if (bookToUpdate == null)
            {
                return NotFound();
            }

            // Actualizam proprietatile cartii
            if (await TryUpdateModelAsync<Book>(
            bookToUpdate,
            "Book",
            " ",
            b => b.Title,
            b => b.AuthorID,
            b => b.Price,
            b => b.PublishingDate,
            b => b.PublisherID))
            {
                // Actualizam categoriile selectate
                UpdateBookCategories(
                    _context, selectedCategories, bookToUpdate);

                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }

            // Daca formularul nu poate fi salvat,
            // reconstruim informatiile pentru afisare
            UpdateBookCategories(
                _context, selectedCategories, bookToUpdate);

            PopulateAssignedCategoryData(_context, bookToUpdate);

            Book = bookToUpdate;

            var authorList = _context.Author.Select(a => new
            {
                a.ID,
                FullName = a.LastName + " " + a.FirstName
            });

            ViewData["AuthorID"] = new SelectList(
                authorList, "ID", "FullName", bookToUpdate.AuthorID);

            ViewData["PublisherID"] = new SelectList(
                _context.Publisher,
                "ID",
                "PublisherName",
                bookToUpdate.PublisherID);

            return Page();
        }


        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.ID == id);
        }
    }
}
*/