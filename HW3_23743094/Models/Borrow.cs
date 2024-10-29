using HW3_23743094.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HW3_23743094.Models
{
    public class Borrow
    {
        public int BorrowId { get; set; }
        public DateTime TakenDate { get; set; }
        public DateTime? BroughtDate { get; set; } // Nullable for not yet returned books

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}

