using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly List<Book> _bookList;

        public BookRepository()
        {
            _bookList = new List<Book>();

            _bookList.Add(new Book("航海王1", "尾田榮一郎", "123456788", 5));

            _bookList.Add(new Book("航海王2", "尾田榮一郎", "123456789", 2));

            _bookList.Add(new Book("火影忍者72", "岸本齊史", "987654321", 1));

            _bookList.Add(new Book("火影忍者71", "岸本齊史", "987654320", 1));
        }

        public void AddBookToList(Book book)
        {
                _bookList.Add(book);
        }

        public List<Book> GetAllBooksFromList()
        {
            return _bookList;
        }

        public void DeleteBookFromList(Book book)
        { 
            _bookList.Remove(book);
        }

        public Book GetBookISBN(string isbn)
        {
            return _bookList.FirstOrDefault(b => b.ISBN == isbn);
        }
    }
}
