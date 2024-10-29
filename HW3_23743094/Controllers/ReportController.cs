using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using HW3_23743094.Models;
using System.Data.Entity;

namespace HW3_23743094.Controllers
{
    public class ReportController : Controller
    {
        private readonly LibraryContext db = new LibraryContext(); // Database context

        // Action to display the Current Loans Report
        public async Task<ActionResult> Index()
        {
            var viewModel = new HomeViewModel
            {
                BorrowList = await db.Borrows
                    .Include(b => b.Student)
                    .Include(b => b.Book)
                    .Where(b => b.BroughtDate == null) // Only get borrows that are not returned
                    .ToListAsync()
            };

            return View(viewModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
