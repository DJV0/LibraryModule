using LibraryModule.Entites;
using System.Xml.Linq;

namespace LibraryModule.Extensions
{
    public static class BookExtensions
    {
        public static Book ToBookDomain(this XElement xBook)
        {
            if (xBook == null)
                throw new ArgumentNullException(nameof(xBook), "XBook cannot be null.");

            return new Book
            {
                Title = xBook.Element("Title")?.Value ?? string.Empty,
                Author =  xBook.Element("Author")?.Value ?? string.Empty,
                Pages = int.TryParse(xBook.Element("Pages")?.Value, out int pages) ? pages : 0
            };
        }

        public static XElement ToXmlBook(this Book book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book), "Book cannot be null.");

            return new XElement("Book",
                new XElement("Title", book.Title),
                new XElement("Author", book.Author),
                new XElement("Pages", book.Pages)
            );
        }
    }
}
