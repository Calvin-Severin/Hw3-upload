using HW3_23743094.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HW3_23743094.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Name { get; set; }

        // Foreign key for Author
        public int AuthorId { get; set; }
        public Author Author { get; set; }

        // Foreign key for Type
  
        public Type Type { get; set; }

        [Required]
        public int TypeId { get; set; }


        public ICollection<Borrow> Borrows { get; set; }
    }

}


