using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Repository;
using Services;
using System;
using System.Collections.Generic;
using Xunit;
using Assert = Xunit.Assert;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void AddBook_ShouldThrowArgumentNullException_WhenTitleIsInvalid(string title)
        { 
            var mockBookRepository = new Mock<IBookRepository>();

            var bookService = new BookService(mockBookRepository.Object);

            Assert.Throws<ArgumentNullException>(() => bookService.AddBook(title, "作者", "123", 5));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void AddBook_ShouldThrowArgumentNullException_WhenAuthorIsInvalid(string author)
        {
            var mockBookRepository = new Mock<IBookRepository>();

            var bookService = new BookService(mockBookRepository.Object);

            Assert.Throws<ArgumentNullException>(() => bookService.AddBook("title", author, "123", 5));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void AddBook_ShouldThrowArgumentNullException_WhenISBNIsInvalid(string isbn)
        {
            var mockBookRepository = new Mock<IBookRepository>();

            var bookService = new BookService(mockBookRepository.Object);

            Assert.Throws<ArgumentNullException>(() => bookService.AddBook("title", "author", isbn, 5));
        }

        [Fact]
        public void AddBook_ShouldThrowArgumentException_WhenQuantityIsNegative()
        {
            var mockBookRepository = new Mock<IBookRepository>();

            var bookService = new BookService(mockBookRepository.Object);

            Assert.Throws<ArgumentException>(() => bookService.AddBook("title", "author", "isbn", -5));
        }

        [Fact]
        public void AddBook_ShouldAddBook_WhenBookDoesNotExist()
        {
            // Arrange
            var mockBookRepository = new Mock<IBookRepository>();

            mockBookRepository.Setup(repo => repo.GetAllBooksFromList())
                              .Returns(new List<Book>());

            var bookService = new BookService(mockBookRepository.Object);

            string title = "航海王1";

            string author = "尾田榮一郎";

            string isbn = "123456789";

            int quantity = 1;

            // Act
            bookService.AddBook(title, author, isbn, quantity);

            // Assert
            mockBookRepository.Verify(repo => repo.AddBookToList(It.Is<Book>(b => 
            b.ISBN   == isbn   &&
            b.Title  == title  &&
            b.Author == author &&
            b.Quantity == quantity)), Times.Once);
        }

        [Fact]
        public void AddBook_ShouldThrowInvalidOperation_WhenBookDoesExist()
        {
            var mockBookRepository = new Mock<IBookRepository>();

            var existingBooks = new List<Book> { new Book("名偵探柯南","青山剛昌","123456777",1)};

            mockBookRepository.Setup(repo => repo.GetAllBooksFromList())
                              .Returns(existingBooks);

            var bookService = new BookService(mockBookRepository.Object);

            Assert.Throws<InvalidOperationException>( () => bookService.AddBook("名偵探柯南", "青山剛昌", "123456777", 1));

            mockBookRepository.Verify(repo => repo.AddBookToList(It.IsAny<Book>()), Times.Never);          
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void DeletBook_ShouldThrowArgumentNullException_WhenISBNIsInvalid(string isbn)
        {
            var mockBookRepository = new Mock<IBookRepository>();

            var bookService = new BookService(mockBookRepository.Object);

            Assert.Throws<ArgumentNullException>(() => bookService.DeleteBook(isbn));
        }

        [Fact]
        public void DeletBook_ShouldDeleteBook_WhenBookDoesExist()
        { 
            var mockBookRepository = new Mock<IBookRepository>();

            var bookToDelete = new Book("名偵探柯南", "青山剛昌", "123456777", 1);

            mockBookRepository.Setup(repo => repo.GetAllBooksFromList())
                              .Returns(new List<Book> { bookToDelete });

            var bookService = new BookService(mockBookRepository.Object);

            bookService.DeleteBook("123456777");

            mockBookRepository.Verify(repo => repo.DeleteBookFromList(It.IsAny<Book>()), Times.Once);
        }

        [Fact]
        public void DeletBook_ShouldThrowInvalidOperation_WhenBookDoesNotExist()
        {
            var mockBookRepository = new Mock<IBookRepository>();

            mockBookRepository.Setup(repo => repo.GetAllBooksFromList())
                              .Returns(new List<Book>());

            var bookService = new BookService(mockBookRepository.Object);

            Assert.Throws<InvalidOperationException>(() => bookService.DeleteBook("123456777"));

            mockBookRepository.Verify(repo => repo.DeleteBookFromList(It.IsAny<Book>()),Times.Never);
        }

        [Theory]
        [InlineData(null,null)]
        [InlineData("","")]
        [InlineData("   ", "   ")]
        public void FindBooks_ShouldThrowArgumentNullException_WhenISBNAndTitleAreBothInvalid(string isbn, string title)
        {
            var mockBookRepository = new Mock<IBookRepository>();

            var bookService = new BookService(mockBookRepository.Object);

            Assert.Throws<ArgumentNullException>(() => bookService.FindBooks(isbn, title));
        }

        [Fact]
        public void FinBooks_ShouldReturnMatchingBooks_WhenSearchByTitle()
        {
            var mockBookRepository = new Mock<IBookRepository>();

            var books = new List<Book>
            {
                new Book("名偵探柯南01", "青山剛昌", "123456789", 1),
                new Book("名偵探柯南02", "青山剛昌", "123456790", 1),
                new Book("名偵探柯南03", "青山剛昌", "123456791", 1)
            };

            mockBookRepository.Setup(repo => repo.GetAllBooksFromList())
                              .Returns(books);

            var bookService = new BookService(mockBookRepository.Object);

            var result = bookService.FindBooks(null, "名偵探");

            Assert.Equal(3,result.Count);
            Assert.All(result, book => Assert.Contains("名偵探", book.Title));
        }

        [Fact]
        public void FinBooks_ShouldReturnMatchingBooks_WhenSearchByISBN()
        {
            var mockBookRepository = new Mock<IBookRepository>();

            var books = new List<Book>
            {
                new Book("名偵探柯南01", "青山剛昌", "123456789", 1),
                new Book("名偵探柯南02", "青山剛昌", "123456790", 1),
                new Book("名偵探柯南03", "青山剛昌", "123456791", 1)
            };

            mockBookRepository.Setup(repo => repo.GetAllBooksFromList())
                              .Returns(books);

            var bookService = new BookService(mockBookRepository.Object);

            var result = bookService.FindBooks("123456789", null);

            Assert.Single(result);
            Assert.Equal("123456789", result[0].ISBN);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void UpdateQuantity_ShouldThrowArgument_WhenISBNIsInvalid(string isbn)
        { 
            var mockBookRepository = new Mock<IBookRepository>();

            var bookService = new BookService(mockBookRepository.Object);

            Assert.Throws<ArgumentNullException>(() => bookService.UpdateQuantity(isbn, 5));
        }

        [Fact]
        public void UpdateQuantity_ShouldThrowArgurement_WhenQuantityIsNegative()
        {
            var mockBookRepository = new Mock<IBookRepository>();

            var bookService = new BookService(mockBookRepository.Object);

            Assert.Throws<ArgumentException>(() => bookService.UpdateQuantity("isbn", -5));
        }

        [Fact]
        public void UpdateQuantity_ShouldUpdateQuantity_WhenBookDoesExis()
        {
            var mockBookRepository = new Mock<IBookRepository>();

            var book = new Book("名偵探柯南", "青山剛昌", "123456789", 5);

            mockBookRepository.Setup(repo => repo.GetAllBooksFromList())
                .Returns(new List<Book> { book });

            var bookService = new BookService(mockBookRepository.Object);

            bookService.UpdateQuantity("123456789", 1);

            Assert.Equal(1, book.Quantity);
        }

        [Fact]
        public void UpdateQuantity_ShouldThrowInvalidOperatiob_WhenBookDoesNotExis()
        {
            var mockBookRepository = new Mock<IBookRepository>();

            mockBookRepository.Setup(repo => repo.GetAllBooksFromList())
                .Returns(new List<Book>());

            var bookService = new BookService(mockBookRepository.Object);

            Assert.Throws<InvalidOperationException>(() => bookService.UpdateQuantity("123456789", 5));
        }
    }
}
