using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library_Db2.Data;
using Library_Db2.Models;

namespace Library_Db2.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public string Sortorder { get; private set; }

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            var applicationDbContext = _context.books.Include(b => b.Authors);
            Sortorder = string.IsNullOrEmpty(sortOrder) ? "" : sortOrder;
            ViewData["BookNameSort"] = String.IsNullOrEmpty(sortOrder) ? "booknamesort" : "";
            ViewData["PriceSort"] = sortOrder == "pricesort" ? "Price_desc" : "pricesort";
            ViewData["PagesSort"] = sortOrder == "pagessort" ? "pages_desc" : "pagessort";
            ViewData["CategorySort"] = sortOrder == "categorysort" ? "category_desc" : "categorysort";
            ViewData["AuthorSort"] = sortOrder == "authorsort" ? "author_desc" : "authorsort";
            ViewData["CurrentFilter"] = searchString;
            var Books = (from s in _context.books
                        select s).Include(b => b.Authors).ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                Books = Books.Where(s => s.BookName.ToLower().Contains(searchString.ToLower()) || 
                s.Authors.AuthorName.ToLower().Contains(searchString)).ToList();
            }
            switch (Sortorder)
            {
                case "booknamesort":
                    Books = Books.OrderByDescending(s => s.BookName).ToList();
                    break;
                case "pricesort":
                    Books = Books.OrderBy(s => s.Price).ToList();
                    break;
                case "Price_desc":
                    Books = Books.OrderByDescending(s => s.Price).ToList();
                    break;
                case "pagessort":
                    Books = Books.OrderBy(s => s.Pages).ToList();
                    break;
                case "pages_desc":
                    Books = Books.OrderByDescending(s => s.Pages).ToList();
                    break;
                case "authorsort":
                    Books = Books.OrderBy(s => s.Authors.AuthorName).ToList();
                    break;
                case "author_desc":
                    Books = Books.OrderByDescending(s => s.Authors.AuthorName).ToList();
                    break; 
                case "categorysort":
                    Books = Books.OrderBy(s => s.Category).ToList();
                    break;
                case "category_desc":
                    Books = Books.OrderByDescending(s => s.Category).ToList();
                    break;
                default:
                    Books = Books.OrderBy(s => s.BookName).ToList();
                    break;

            }
            return View(Books.ToList());
        }


        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var books = await _context.books
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (books == null)
            {
                return NotFound();
            }

            return View(books);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.authors, "AuthorId", "AuthorName");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,BookName,Category,Price,Pages,AuthorId")] Books books)
        {
            if (ModelState.IsValid)
            {
                _context.Add(books);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.authors, "AuthorId", "AuthorName", books.AuthorId);
            return View(books);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var books = await _context.books.FindAsync(id);
            if (books == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.authors, "AuthorId", "AuthorName", books.AuthorId);
            return View(books);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookId,BookName,Category,Price,Pages,AuthorId")] Books books)
        {
            if (id != books.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(books);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BooksExists(books.BookId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.authors, "AuthorId", "AuthorName", books.AuthorId);
            return View(books);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var books = await _context.books
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (books == null)
            {
                return NotFound();
            }

            return View(books);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var books = await _context.books.FindAsync(id);
            _context.books.Remove(books);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BooksExists(int id)
        {
            return _context.books.Any(e => e.BookId == id);
        }
    }
}
