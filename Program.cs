// See https://aka.ms/new-console-template for more information
using Bib.Core;
using Bib.Core.Entities;
using Bib.Core.Interfaces;
using Bib.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

//Console.WriteLine("Hello, World!");

var b1 = new Book();

var serviceProvider = new ServiceCollection().AddBib().BuildServiceProvider();

IRepository<Book, int> repo = serviceProvider.GetService<IRepository<Book, int>>();

var browser = serviceProvider.GetService<BookBrowser>();

var exit = false;
CreateSampleBooks().ForEach(b => browser?.Register(b)); // Ensure browser isn't null

var printBooks = () =>
{
    var books = browser.GetAllBooks();
    Console.Clear();
    Console.WriteLine("=== List of Books ===");
    foreach (var book in books)
    {
        Console.WriteLine($"Id: {book.Id}, Title: {book.Title}, Author: {book.Author}, Summary: {book.Summary}, ISBN: {book.ISBN}");
    }
    return false;
};

var printPeople = () =>
{
    var persons = browser.GetAllPersons();
    Console.Clear();
    Console.WriteLine("=== List of People ===");
    foreach (var person in persons)
    {
        Console.WriteLine($"Id: {person.Id}, Name: {person.Name}");
    }
    return false;

};

var quitApp = () =>
{
    exit = true;
    return false;

};

var invalidInput = () =>
{
    Console.WriteLine("Invalid input");
    return false;

};

var addPeople = () =>
{
    Console.Clear();
    Console.WriteLine("=== Add a New Person ===");
    Console.Write("Set ID: ");
    int id = int.Parse(Console.ReadLine());
    Console.Write("Set Name: ");
    string name = Console.ReadLine();
    browser.Register(new Person { Id = id, Name = name, Books = new List<Book>() });
    Console.WriteLine("Person added successfully.");
    return false;
};
var addBooks = () =>
{
    Console.Clear();
    Console.WriteLine("=== Add a New Book ===");
    Console.Write("Set ID: ");
    int id = int.Parse(Console.ReadLine());
    Console.Write("Set Title: ");
    string title = Console.ReadLine();
    Console.Write("Set Author: ");
    string author = Console.ReadLine();
    Console.Write("Set ISBN: ");
    string isbn = Console.ReadLine();
    Console.Write("Set Summary: ");
    string summary = Console.ReadLine();
    browser.Register(new Book
    {
        Id = id,
        Title = title,
        Author = author,
        ISBN = isbn,
        Summary = summary,
    });
    Console.WriteLine("Book added successfully.");
    return false;
};

var borowBook = () =>
{
    Console.Clear();
    Console.WriteLine("=== Borrow a Book ===");
    Console.Write("ID of Person: ");
    int personID = int.Parse(Console.ReadLine());
    Console.Write("ID of Book: ");
    int bookID = int.Parse(Console.ReadLine());

    Book book = browser?.GetAllBooks().FirstOrDefault(a => a.Id == bookID);

    var person = browser.GetAllPersons().FirstOrDefault(a => a.Id == personID);

    if (book != null && person != null)
    {
        browser.Lend(book, person);
        Console.WriteLine("Book borrowed successfully.");
    }
    else
    {
        Console.WriteLine("Invalid person or book ID.");
    }

    return false;
};

var returnBook = () =>
{
    Console.Clear();
    Console.WriteLine("=== Return a Book ===");
    Console.Write("ID of Person: ");
    int personID = int.Parse(Console.ReadLine());
    Console.Write("ID of Book: ");
    int bookID = int.Parse(Console.ReadLine());

    var book = browser.GetAllBooks().FirstOrDefault(a => a.Id == bookID);
    var person = browser.GetAllPersons().FirstOrDefault(a => a.Id == personID);

    if (book != null && person != null)
    {
        browser.GiveBack(book, person);
        Console.WriteLine("Book returned successfully.");
    }
    else
    {
        Console.WriteLine("Invalid person or book ID.");
    }

    return false;
};


var printMenu = () =>
{
    Console.WriteLine("q: quit");
    Console.WriteLine("b: list books");
    Console.WriteLine("p: list people");
    Console.WriteLine("x: addPeople");
    Console.WriteLine("y: addBook");
    Console.WriteLine("l: lendBook");
    Console.WriteLine("r: returnBook");
    var choice = Console.ReadKey();

    return choice.KeyChar switch
    {
        'q' => quitApp(),
        'b' => printBooks(),
        'p' => printPeople(),
        'x' => addPeople(),
        'y' => addBooks(),
        'l' => borowBook(),
        'r' => returnBook(),
        _ => invalidInput()
    };

};

while (!exit)
{
   exit = printMenu();
}
static List<Book> CreateSampleBooks() => new()
    {
        // Classic Science Fiction Novel
        new Book
        {
            Id = 1,
            Title = "Dune",
            Author = "Frank Herbert",
            ISBN = "978-0441172719",
            Summary = "A complex saga of politics, religion, and ecology set on a desert planet where water is the most precious resource.",
        },

        // Contemporary Literary Fiction
        new Book
        {
            Id = 2,
            Title = "A Gentleman in Moscow",
            Author = "Amor Towles",
            ISBN = "978-0670026197",
            Summary = "A Russian aristocrat is sentenced to house arrest in a luxury hotel, witnessing decades of Soviet history from a single location.",
        },

        // Technical Programming Book
        new Book
        {
            Id = 3,
            Title = "Clean Code: A Handbook of Agile Software Craftsmanship",
            Author = "Robert C. Martin",
            ISBN = "978-0132350884",
            Summary = "A comprehensive guide to writing readable, maintainable, and efficient code for professional software developers.",
        },

        // Popular Science
        new Book
        {
            Id = 4,
            Title = "A Brief History of Time",
            Author = "Stephen Hawking",
            ISBN = "978-0553380163",
            Summary = "An accessible exploration of cosmology, black holes, space-time, and the fundamental nature of the universe.",
        },

        // Historical Non-Fiction
        new Book
        {
            Id = 5,
            Title = "Sapiens: A Brief History of Humankind",
            Author = "Yuval Noah Harari",
            ISBN = "978-0062316110",
            Summary = "A provocative journey through the evolution of human species, exploring how Homo sapiens came to dominate the Earth.",
        } 
};

