using Repository;
using Services;
using System;
using System.Linq;

namespace LibraryBookManager
{
    public class Program
    {
        private static BookService _bookService = new BookService(new BookRepository());
        static void Main(string[] args)
        {
            bool exit = false;

            while (!exit)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("   ===歡迎使用小型圖書管理系統===   ");
                    Showmenu();
                    string choice = Console.ReadLine();
                    Console.Clear();

                    switch (choice)
                    {
                        case "1":
                            AddBook();
                            break;
                        case "2":
                            FindBooks();
                            break;
                        case "3":
                            UpdateQuantity();
                            break;
                        case "4":
                            DeleteBook();
                            break;
                        case "5":
                            ShowAllBooks();
                            break;
                        case "6":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("輸入錯誤，請按任意鍵離開");
                            break;
                    }
                    if (!exit)
                    {
                        Console.WriteLine("按任意鍵回到目錄頁面");
                        Console.ReadLine();
                    }
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"出現操作錯誤{ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"出現異常錯誤{ex.Message}");
                }
            }
        }

        private static void Showmenu()
        {
            Console.WriteLine("輸入1，新增書籍");
            Console.WriteLine("輸入2，查詢書籍");
            Console.WriteLine("輸入3，修改書籍數量");
            Console.WriteLine("輸入4，刪除書籍");
            Console.WriteLine("輸入5，所有書籍清單");
            Console.WriteLine("輸入6，離開");
            Console.Write("請輸入數字1~6：");
        }

        private static void AddBook()
        {
            Console.WriteLine("===新增書籍===");

            string title;
            string author;
            string isbn;
            int quantity;

            while (true)
            {
                Console.Write("請輸入書籍的書名：");
                var input1 = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input1))
                {
                    Console.WriteLine("不可沒有輸入書名");
                    continue;
                }
                title = input1;

                Console.Write("請輸入書籍的作者：");
                var input2 = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input2))
                {
                    Console.WriteLine("不可沒有輸入作者");
                    continue;
                }
                author = input2;

                Console.Write("請輸入書籍的ISBN：");
                string input3 = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input3))
                {
                    Console.WriteLine("不可沒有輸入ISBN");
                    continue;
                }
                isbn = input3;
                
                Console.Write("請輸入書籍的數量：");
                string input4 = Console.ReadLine();
                if (!int.TryParse(input4, out quantity))
                {
                    Console.WriteLine($"輸入錯誤，請輸入正確的數字");
                    continue;
                }
                if (quantity < 0)
                {
                    Console.WriteLine("書籍數量不可為負數");
                    continue;
                }
                break;
            }

            _bookService.AddBook(title, author, isbn, quantity);

            Console.WriteLine($"新增書籍成功，加入書名：{title,-20}，作者：{author,-10}，ISBN：{isbn,-10}，數量：{quantity,3}");
        }

        private static void DeleteBook()
        {
            Console.WriteLine("===刪除書籍===");

            string isbn;
            Console.Write("請輸入想要刪除書籍的ISBN碼：");
            while (true) 
            {
                isbn = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(isbn))
                {
                    Console.Write("ISBN碼不能沒有輸入，");
                    continue;
                }
                break ;
            }

            _bookService.DeleteBook(isbn);
            Console.WriteLine($"成功刪除ISBN為{isbn}的書籍");
        }

        private static void FindBooks()
        {
            Console.WriteLine("尋找書籍");

            string isbn;
            string title;
            while (true)
            {
                Console.Write("請輸入想要尋找書籍的ISBN碼：");
                isbn = Console.ReadLine();

                Console.Write("請輸入想要尋找書籍的關鍵字：");
                title = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(isbn))
                {
                    Console.WriteLine("不可以同時沒有輸入ISBN碼和書名關鍵字，");
                    continue;
                }
                break;
            }

            var allBooks = _bookService.FindBooks(isbn, title);

            if (!allBooks.Any())
            {
                Console.WriteLine("沒有找到符合條件的書籍");
            }
            else
            {
                Console.WriteLine("找到以下的書籍：");
                foreach (var book in allBooks)
                {
                    Console.WriteLine(book);
                }
            }
        }

        private static void UpdateQuantity()
        { 
            Console.WriteLine("===修改書籍數量===");

            Console.Write("請輸入想要修改書籍數量的ISBN碼：");
            string isbn;
            while (true)
            { 
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.Write("ISBN碼不得沒有輸入，");
                    continue;
                }
                isbn = input;
                break;
            }

            Console.Write($"請輸入{isbn}的書籍數量：");
            int quantity;
            while (true)
            {
                var input = Console.ReadLine();
                if (!int.TryParse(input, out quantity))
                {
                    Console.Write("輸入錯誤，請重新輸入數字，");
                    continue;
                }
                if (quantity < 0)
                {
                    Console.Write("數量不得為負數，");
                    continue;
                }
                break;
            }

            _bookService.UpdateQuantity(isbn, quantity);
            Console.WriteLine($"更新ISBN：{isbn}的數量：{quantity}");
        }

        private static void ShowAllBooks()
        {
            Console.WriteLine("===所有書籍清單===");

            var allBooks = _bookService.GetAllBooks();

            foreach ( var book in allBooks )
            {
                Console.WriteLine(book);
            }
        }
    }
}
