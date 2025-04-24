using System;
using System.Collections.Generic;

public abstract class AbstractPerson
{
    public int Id { get; set; }
    public string Name { get; set; }

    public AbstractPerson(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public abstract void DisplayInfo();
}

public class Member : AbstractPerson
{
    public List<Book> BorrowedBooks { get; set; }

    public Member(int id, string name) : base(id, name)
    {
        BorrowedBooks = new List<Book>();
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"Member ID: {Id}, Name: {Name}");
    }

    public void BorrowBook(Book book)
    {
        if (book == null)
        {
            Console.WriteLine("Book does not exist.");
            return;
        }

        if (book.IsAvailable)
        {
            BorrowedBooks.Add(book);
            book.IsAvailable = false;
            Console.WriteLine($"Book '{book.Title}' borrowed successfully by {Name}.");
        }
        else
        {
            Console.WriteLine($"Book '{book.Title}' is not available.");
        }
    }

    public void ReturnBook(Book book)
    {
        if (book == null)
        {
            Console.WriteLine("Book does not exist.");
            return;
        }

        if (BorrowedBooks.Contains(book))
        {
            BorrowedBooks.Remove(book);
            book.IsAvailable = true;
            Console.WriteLine($"Book '{book.Title}' returned successfully by {Name}.");
        }
        else
        {
            Console.WriteLine($"Book '{book.Title}' was not borrowed by {Name}.");
        }
    }

    public void DisplayBorrowedBooks()
    {
        if (BorrowedBooks.Count == 0)
        {
            Console.WriteLine($"{Name} has no borrowed books.");
            return;
        }

        Console.WriteLine($"Books borrowed by {Name}:");
        foreach (var book in BorrowedBooks)
        {
            book.DisplayInfo();
        }
    }
}

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string AuthorName { get; set; }
    public bool IsAvailable { get; set; }

    public Book(int id, string title, string authorName)
    {
        Id = id;
        Title = title;
        AuthorName = authorName;
        IsAvailable = true; // Book is available by default
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Book ID: {Id}, Title: {Title}, Author: {AuthorName}, Available: {(IsAvailable ? "Yes" : "No")}");
    }
}

public class Library
{
    private List<Book> Books { get; set; }
    private List<AbstractPerson> Members { get; set; }

    public Library()
    {
        Books = new List<Book>();
        Members = new List<AbstractPerson>();
    }

    public void AddBook(Book book)
    {
        Books.Add(book);
        Console.WriteLine($"Book '{book.Title}' added to the library.");
    }

    public void AddMember(AbstractPerson member)
    {
        Members.Add(member);
        Console.WriteLine($"Member '{member.Name}' added to the library.");
    }

    public Book FindBookById(int id)
    {
        return Books.Find(book => book.Id == id);
    }

    public AbstractPerson FindMemberById(int id)
    {
        return Members.Find(member => member.Id == id);
    }

    public void DisplayAllBooks()
    {
        if (Books.Count == 0)
        {
            Console.WriteLine("No books in the library.");
            return;
        }

        Console.WriteLine("All Books:");
        foreach (var book in Books)
        {
            book.DisplayInfo();
        }
    }

    public void DisplayAllMembers()
    {
        if (Members.Count == 0)
        {
            Console.WriteLine("No members in the library.");
            return;
        }

        Console.WriteLine("All Members:");
        foreach (var member in Members)
        {
            member.DisplayInfo();
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Library library = new Library();

        // إضافة كتب
        library.AddBook(new Book(1, "1984", "George Orwell"));
        library.AddBook(new Book(2, "The Hobbit", "J.R.R. Tolkien"));
        library.AddBook(new Book(3, "Clean Code", "Robert C. Martin"));

        // إضافة أعضاء
        library.AddMember(new Member(1, "Ali"));
        library.AddMember(new Member(2, "Sara"));

        while (true)
        {
            Console.WriteLine("\n--- Library Menu ---");
            Console.WriteLine("1. Show all books");
            Console.WriteLine("2. Show all members");
            Console.WriteLine("3. Borrow book");
            Console.WriteLine("4. Return book");
            Console.WriteLine("5. Show member borrowed books");
            Console.WriteLine("0. Exit");
            Console.Write("Choose option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    library.DisplayAllBooks();
                    break;

                case "2":
                    library.DisplayAllMembers();
                    break;

                case "3":
                    Console.Write("Enter member ID: ");
                    int mid = int.Parse(Console.ReadLine());
                    Console.Write("Enter book ID: ");
                    int bid = int.Parse(Console.ReadLine());

                    AbstractPerson absMember = library.FindMemberById(mid);
                    Book bookToBorrow = library.FindBookById(bid);

                    if (absMember is Member m1 && bookToBorrow != null)
                        m1.BorrowBook(bookToBorrow);
                    else
                        Console.WriteLine("Invalid member or book ID.");
                    break;

                case "4":
                    Console.Write("Enter member ID: ");
                    mid = int.Parse(Console.ReadLine());
                    Console.Write("Enter book ID: ");
                    bid = int.Parse(Console.ReadLine());

                    absMember = library.FindMemberById(mid);
                    Book bookToReturn = library.FindBookById(bid);

                    if (absMember is Member m2 && bookToReturn != null)
                        m2.ReturnBook(bookToReturn);
                    else
                        Console.WriteLine("Invalid member or book ID.");
                    break;

                case "5":
                    Console.Write("Enter member ID: ");
                    mid = int.Parse(Console.ReadLine());
                    absMember = library.FindMemberById(mid);
                    if (absMember is Member m3)
                        m3.DisplayBorrowedBooks();
                    else
                        Console.WriteLine("Invalid member ID.");
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }
}