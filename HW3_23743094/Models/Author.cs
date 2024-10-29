using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HW3_23743094.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        // Author can have many books
        public ICollection<Book> Books { get; set; }
    }

    public class Type
    {
        public int TypeId { get; set; }
        public string Name { get; set; }

        // Type can have many books
        public ICollection<Book> Books { get; set; }
    }

}