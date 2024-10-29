using System.Threading.Tasks;
using HW3_23743094.Models; 
using System.Data.Entity;
using System.Web.Mvc;

namespace HW3_23743094.Controllers
{
    public class HomeController : Controller

    {    // Initialize the database context to access your database
        private LibraryContext db = new LibraryContext();

    
        public async Task<ActionResult> Index()
        {
            // Fetch students, books, authors, and types from the database
            var students = await db.Students.Include(s => s.Borrows).ToListAsync();
            var books = await db.Books.Include(b => b.Author).Include(b => b.Type).Include(b => b.Borrows).ToListAsync();
            var authors = await db.Authors.ToListAsync();
            var types = await db.Types.ToListAsync(); 

            // Pass the data to the view using HomeViewModel
            var viewModel = new HomeViewModel
            {
                Students = students,
                Books = books,
                Authors = authors,
                Types = types  
            };

            return View(viewModel);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateStudent(Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student); // Add the new student to the Students table
                await db.SaveChangesAsync(); 
                return RedirectToAction("Index"); 
            }

            return View(student); 
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateBook(Book book)
        {
            if (ModelState.IsValid)
            {
                db.Books.Add(book); // Add the new book to the Books table
                await db.SaveChangesAsync(); 
                return RedirectToAction("Index"); 
            }

            // Repopulate dropdowns in case of validation failure
            ViewBag.AuthorId = new SelectList(db.Authors, "AuthorId", "Name", book.AuthorId);
            ViewBag.TypeId = new SelectList(db.Types, "TypeId", "Name", book.TypeId);
            return View(book); 
        }
    }
}
