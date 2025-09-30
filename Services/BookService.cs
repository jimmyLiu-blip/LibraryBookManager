using Domain;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BookService:IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        private void ValidateBookParameters(string title, string author, string isbn, int quantity, int availableQuantity)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentNullException(nameof(title), "書名(title)不可為空");
            }

            if (string.IsNullOrWhiteSpace(author))
            {
                throw new ArgumentNullException(nameof(author),"作者(Author)不可為空");
            }

            if (string.IsNullOrWhiteSpace(isbn))
            {
                throw new ArgumentNullException(nameof(isbn),"ISBN不可為空");
            }

            if (quantity < 0)
            {
                throw new ArgumentException("書本數量不可以 <= 0",nameof(quantity));
            }
        }

        public  Book GetBookByISBN(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
            { 
                throw new ArgumentNullException(nameof(isbn),"ISBN不可為空");
            }

            return GetAllBooks()
                .FirstOrDefault(b => string.Equals(b.ISBN, isbn, StringComparison.OrdinalIgnoreCase));
        }

        public  List<Book> GetAllBooks()
        {
            return _bookRepository.GetAllBooksFromList();
        }

        public void AddBook(string title, string author, string isbn, int quantity, int availableQuantity)
        {
            ValidateBookParameters(title, author, isbn, quantity);

            if (GetBookByISBN(isbn) != null )
            {
                throw new InvalidOperationException($"此{isbn}書籍已存在列表清單中");
            }

            var newBook = new Book(title, author, isbn, quantity, availableQuantity);

            _bookRepository.AddBookToList(newBook);

        }

        public List<Book> FindBooks(string isbn, string title)
        {
            if (string.IsNullOrWhiteSpace(isbn) && string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentNullException("isbn和title不可同時都不提供");
            }

            var allBooks = GetAllBooks();

            var result = allBooks.AsQueryable();

            if (!string.IsNullOrWhiteSpace(isbn))
            {
                result = result.Where(b => b.ISBN.ToLower().Equals(isbn.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(title))
            {
                result = result.Where(b => b.Title.ToLower().Contains(title.ToLower()));
            }

            return result.ToList();
        }

        public void UpdateQuantity(string isbn, int quantity)
        {
            if (string.IsNullOrWhiteSpace(isbn))
            {
                throw new ArgumentNullException(nameof(isbn),"ISBN不可為空");
            }

            if (quantity < 0)
            {
                throw new ArgumentException("書本數量不可以 <= 0",nameof(quantity));
            }
            
            var book = GetBookByISBN(isbn);

            if (book == null)
            {
                throw new InvalidOperationException($"此{isbn}書籍不存在清單列表中，無法修改");
            }

            book.Quantity = quantity;

        }

        public void DeleteBook(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
            {
                throw new ArgumentNullException(nameof(isbn), "ISBN不可為空");
            }

            var book = GetBookByISBN(isbn);

            if (book == null)
            {
                throw new InvalidOperationException($"此{isbn}書籍不存在清單列表中，無法刪除");
            }

            _bookRepository.DeleteBookFromList(book);
        }
    }
}
