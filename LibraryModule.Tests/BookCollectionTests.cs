using FluentAssertions;
using LibraryModule.Entites;
using LibraryModule.Extensions;
using System.Xml.Linq;

namespace LibraryModule.Tests
{
    public class BookCollectionTests
    {
        private string TestXmlFilePath = Path.Combine(AppContext.BaseDirectory, "Assets", "testdata.xml");

        [Fact]
        public void LoadFromXml_Works()
        {
            var library = new BooksCollection();

            library.LoadFromXml(TestXmlFilePath);

            library.Books.Should().NotBeNullOrEmpty();
            library.Books.Should().HaveCount(3);
        }

        [Fact]
        public void AddBook_Works()
        {
            var bookToAdd = new Book
            {
                Title = "Clean Code",
                Author = "Martin Robert",
                Pages = 464
            };

            var library = new BooksCollection();
            library.AddBook(bookToAdd);

            library.Books.Should().NotBeNullOrEmpty();
            library.Books.Should().HaveCount(1);
            library.Books.Single().Should().BeEquivalentTo(bookToAdd);
        }

        [Fact]
        public void SearchByTitle_ShouldReturnMatchingBooks()
        {
            var library = new BooksCollection();
            library.AddBook(new Book { Title = "Clean Code", Author = "Martin Robert", Pages = 464 });
            library.AddBook(new Book { Title = "Code Complete", Author = "McConnell Steve", Pages = 960 });
            library.AddBook(new Book { Title = "The Pragmatic Programmer", Author = "Hunt Andrew", Pages = 352 });

            List<Book> results = library.SearchByTitle("Code");

            results.Should().HaveCount(2)
                .And.Contain(book => book.Title == "Clean Code")
                .And.Contain(book => book.Title == "Code Complete");
        }

        [Fact]
        public void SearchByTitle_ShouldReturnEmptyList_WhenNoMatch()
        {
            var library = new BooksCollection();
            library.AddBook(new Book { Title = "The Pragmatic Programmer", Author = "Hunt Andrew", Pages = 352 });

            List<Book> results = library.SearchByTitle("Code");

            results.Should().BeEmpty();
        }

        [Fact]
        public void SortInAlphabeticalOrder_ShouldCreateTheCorectBooksOrder()
        {
            var library = new BooksCollection();
            library.AddBook(new Book { Title = "The Ugly Duckling", Author = "Andersen Hans", Pages = 100 });
            library.AddBook(new Book { Title = "The Little Mermaid", Author = "Andersen Hans", Pages = 120 });
            library.AddBook(new Book { Title = "It", Author = "King Stephen", Pages = 1138 });
            library.AddBook(new Book { Title = "The Shining", Author = "King Stephen", Pages = 659 });

            library.SortInAlphabeticalOrder();

            library.Books
                .Select(b => b.Title)
                .Should()
                .ContainInConsecutiveOrder("The Little Mermaid", "The Ugly Duckling", "It", "The Shining");
        }

        [Fact]
        public void SaveToXml_ShouldCreateValidXmlFile()
        {
            // Arrange
            var library = new BooksCollection();
            var book = new Book { Title = "The Little Mermaid", Author = "Andersen Hans", Pages = 120 };
            library.AddBook(book);

            string filePath = "savedbooks.xml";

            // Act
            library.SaveToXml(filePath);

            // Assert
            File.Exists(filePath).Should().BeTrue();

            var xDocument = XDocument.Load(filePath);
            var books = xDocument.Descendants("Book")
                .Select(x => x.ToBookDomain())
                .ToList();

            books.Should().NotBeNullOrEmpty();
            books.Single().Should().BeEquivalentTo(book);

            File.Delete(filePath);
        }
    }
}