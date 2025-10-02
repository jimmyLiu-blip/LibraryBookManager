using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Repository
{
    public interface IBookRepository
    {
        void AddBookToList(Book book);

        List<Book> GetAllBooksFromList();

        void DeleteBookFromList(Book book);
        Book GetBookISBN(string isbn);

    }
}
