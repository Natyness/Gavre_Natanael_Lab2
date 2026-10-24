using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Gavre_Natanael_Lab2.Data;
using Gavre_Natanael_Lab2.Models;

namespace Gavre_Natnael_Lab2.Pages.Books
{
    public class CreateModel : BookCategoriesPageModel
    {
        private readonly Gavre_Natanael_Lab2.Data.Gavre_Natanael_Lab2Context _context;

        public CreateModel(Gavre_Natanael_Lab2.Data.Gavre_Natanael_Lab2Context context)
        {
            _context = context;
        }

        
        public IActionResult OnGet()
        {
            var authorList = _context.Author.Select(a => new
            {
                a.ID,
                FullName = a.LastName + " " + a.FirstName
            });

            ViewData["AuthorID"] = new SelectList(
                authorList, "ID", "FullName");

            ViewData["PublisherID"] = new SelectList(
                _context.Publisher, "ID", "PublisherName");

            var book = new Book();
            book.BookCategories = new List<BookCategory>();

            PopulateAssignedCategoryData(_context, book);

            return Page();
        }


        [BindProperty]
        public Book Book { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        
        public async Task<IActionResult> OnPostAsync(
            string[]? selectedCategories)
        {
            // Construim lista categoriilor selectate
            Book.BookCategories = new List<BookCategory>();

            if (selectedCategories != null)
            {
                foreach (var cat in selectedCategories)
                {
                    Book.BookCategories.Add(new BookCategory
                    {
                        CategoryID = int.Parse(cat)
                    });
                }
            }

            // Categoriile sunt gestionate separat
            ModelState.Remove("Book.BookCategories");

            if (!ModelState.IsValid)
            {
                var authorList = _context.Author.Select(a => new
                {
                    a.ID,
                    FullName = a.LastName + " " + a.FirstName
                });

                ViewData["AuthorID"] = new SelectList(
                    authorList, "ID", "FullName", Book.AuthorID);

                ViewData["PublisherID"] = new SelectList(
                    _context.Publisher,
                    "ID",
                    "PublisherName",
                    Book.PublisherID);

                PopulateAssignedCategoryData(_context, Book);

                return Page();
            }

            _context.Book.Add(Book);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }
}
