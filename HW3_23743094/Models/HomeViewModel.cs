using System.Collections.Generic;
using System.Web.Mvc;

namespace HW3_23743094.Models
{
    public class HomeViewModel
    {
        // contains list of ojects 
        public List<Student> Students { get; set; }
        public List<Book> Books { get; set; }
        public List<Author> Authors { get; set; }
        public List<Type> Types { get; set; }

        public List<Borrow> BorrowList { get; set; } 
        public Borrow Borrow { get; set; }

       
        public IEnumerable<SelectListItem> StudentSelectList { get; set; }
        public IEnumerable<SelectListItem> BookSelectList { get; set; }
    }
}
