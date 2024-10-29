using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using System.Web.Mvc;
using HW3_23743094.Models;

namespace HW3_23743094.Controllers
{
    public class MaintainController : Controller
    {
        private readonly LibraryContext db = new LibraryContext(); // Database context

        // GET: Maintain
        public async Task<ActionResult> Index()
        {
            // get borrow records 
            var borrows = await db.Borrows
                .Include(b => b.Student)
                .Include(b => b.Book)
                .OrderByDescending(b => b.BorrowId)
                .Take(10)
                .ToListAsync();

            var viewModel = new HomeViewModel
            {
                Students = await db.Students.ToListAsync(),
                Books = await db.Books.Include(b => b.Author).Include(b => b.Type).ToListAsync(),
                Authors = await db.Authors.ToListAsync(),
                Types = await db.Types.ToListAsync(),
                BorrowList = borrows 
            };


            return View(viewModel);
        }



        // Create Author
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAuthor(Author author)
        {
            if (ModelState.IsValid)
            {
                db.Authors.Add(author);  
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(author); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAuthor(Author author)
        {
            if (ModelState.IsValid)
            {
                db.Entry(author).State = EntityState.Modified; 
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(author); 
        }


        [HttpPost]
        public async Task<ActionResult> DeleteAuthor(int id)
        {
            var author = await db.Authors.FindAsync(id);
            if (author != null)
            {
                // Find all books related to this author
                var books = db.Books.Where(b => b.AuthorId == id).ToList();

                // Find and delete all borrow records associated with these books
                foreach (var book in books)
                {
                    var borrows = db.Borrows.Where(b => b.BookId == book.BookId);
                    db.Borrows.RemoveRange(borrows);  
                }

                // Now delete the books related to this author
                db.Books.RemoveRange(books);

                // delete the author
                db.Authors.Remove(author);

                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        // Create Type
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateType(HW3_23743094.Models.Type type)
        {
            if (ModelState.IsValid)
            {
                db.Types.Add(type);  
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(type); 
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditType(HW3_23743094.Models.Type type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(type).State = EntityState.Modified; // Update existing type
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(type); 
        }

        // Delete Type
        [HttpPost]
        public async Task<ActionResult> DeleteType(int id)
        {
            var type = await db.Types.FindAsync(id);
            if (type != null)
            {
                db.Types.Remove(type);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        public async Task<JsonResult> GetCreateBorrowData()
        {
            var students = await db.Students.Select(s => new { s.StudentId, s.Name }).ToListAsync();
            var books = await db.Books.Select(b => new { b.BookId, b.Name }).ToListAsync();

            return Json(new { students, books }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateBorrow(Borrow borrow)
        {
            if (ModelState.IsValid)
            {
                if (borrow.TakenDate == DateTime.MinValue)
                {
                    borrow.TakenDate = DateTime.Now;
                }

                db.Borrows.Add(borrow);
                await db.SaveChangesAsync();
                return Json(new { success = true });
            }

            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });
        }


        public async Task<JsonResult> GetBorrow(int id)
        {
            var borrow = await db.Borrows
                .Include(b => b.Student)
                .Include(b => b.Book)
                .FirstOrDefaultAsync(b => b.BorrowId == id);

            if (borrow == null) return Json(null, JsonRequestBehavior.AllowGet);

            var students = await db.Students.Select(s => new { id = s.StudentId, name = s.Name }).ToListAsync();
            var books = await db.Books.Select(b => new { id = b.BookId, name = b.Name }).ToListAsync();

            return Json(new
            {
                BorrowId = borrow.BorrowId,
                StudentId = borrow.StudentId,
                BookId = borrow.BookId,
                TakenDate = borrow.TakenDate.ToString("yyyy-MM-dd"),
                students,
                books
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditBorrow(Borrow borrow)
        {
            if (ModelState.IsValid)
            {
                db.Entry(borrow).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteBorrow(int id)
        {
            var borrow = await db.Borrows.FindAsync(id);
            if (borrow != null)
            {
                db.Borrows.Remove(borrow);
                await db.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false, error = "Borrow not found" });
        }

         

    }
}

