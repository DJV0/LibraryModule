using LibraryModule.Entites;
using LibraryModule.Extensions;
using LibraryModule.Helpers;
using System.Xml.Linq;

namespace LibraryModule
{
    public class BooksCollection
    {
        public List<Book> Books { get; private set; } = [];

        public void LoadFromXml(string filePath)
        {
            if(!File.Exists(filePath))
            {
                throw new FileNotFoundException("The file not found!", filePath);
            }

            var xmlFile = XDocument.Load(filePath);
            Books = xmlFile.Descendants("Book")
                .Select(book => book.ToBookDomain())
                .ToList();
        }

        public void AddBook(Book book)
        {
            if (book == null)
                throw new ArgumentNullException();

            Books.Add(book);
        }

        public List<Book> SearchByTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty.", nameof(title));

            return Books
                .Where(book => SearchingHelper.BoyerMooreSearch(book.Title, title) != -1)
                .ToList();
        }

        public void SortInAlphabeticalOrder()
        {
            SortingHelper.QuickSort(Books, (book1, book2) =>
            {
                int authorComparison = string.Compare(book1.Author, book2.Author, StringComparison.Ordinal);
                return authorComparison != 0 ? authorComparison : string.Compare(book1.Title, book2.Title, StringComparison.Ordinal);
            });
        }

        public void SaveToXml(string filePath)
        {
            var xBooks = new XElement("Library",
                Books.Select(book => book.ToXmlBook()));

            var xDocument = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), xBooks);
            xDocument.Save(filePath);
        }
    }
}
