using System.Data.Entity;
using HW3_23743094.Models;

public class LibraryContext : DbContext
{
    // represents the different table in databse 
    public DbSet<Student> Students { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Borrow> Borrows { get; set; }
    public DbSet<Type> Types { get; set; } 

    //: Initializes the context using a database connection string
    public LibraryContext() : base("LibraryConnection")
    {
    }

    //Configures relationships between tables in the database
    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        // One-to-many relationship between Book and Borrow
        modelBuilder.Entity<Book>()
            .HasMany(b => b.Borrows)   // A book can have many borrow records
            .WithRequired(b => b.Book) // Each borrow record requires a book
            .HasForeignKey(b => b.BookId); // Foreign key from Borrow to Book

        // One-to-many relationship between Author and Book 
        modelBuilder.Entity<Book>()
            .HasRequired(b => b.Author)   // Each book must have an author
            .WithMany(a => a.Books)       // An author can have many books
            .HasForeignKey(b => b.AuthorId) // Foreign key from Book to Author
            .WillCascadeOnDelete(true);   

        base.OnModelCreating(modelBuilder);
    }

}
