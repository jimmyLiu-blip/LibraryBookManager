using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Services
{
    public interface IBookService
    {
        void AddBook(string title, string author, string isbn, int quantity);

        List<Book> FindBooks(string isbn, string title);

        void UpdateQuantity(string isbn, int quantity);

        void DeleteBook(string isbn);
    }
}
