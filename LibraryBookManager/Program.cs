using Repository;
using Services;
using System;

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

                    switch (choice)
                    {
                        case "1":
                            AddBook();
                            break;
                        case "2":
                            FindBooks();
                            break;
                        case "3":
                            //UpdateQuantity();
                            break;
                        case "4":
                            DeleteBook();
                            break;
                        case "5":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("輸入錯誤，請按任意鍵離開");
                            break;

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
            Console.WriteLine("輸入5，離開");
            Console.WriteLine("請輸入數字1~5：");
        }

        private static void AddBook()
        {
            Console.Clear();
            Console.WriteLine("===新增書籍===");

            Console.Write("請輸入書籍的書名：");
            string title = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("不可沒有輸入書名");
                return;
            }

            Console.Write("請輸入書籍的作者：");
            string author = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(author))
            {
                Console.WriteLine("不可沒有輸入作者");
                return;
            }

            Console.Write("請輸入書籍的ISBN：");
            string isbn = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(isbn))
            {
                Console.WriteLine("不可沒有輸入ISBN");
                return;
            }

            int quantity;
            while(true)
            {
                Console.Write("請輸入書籍的數量：");
                string input = Console.ReadLine();
                if (!int.TryParse(input, out quantity))
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

            Console.WriteLine($"新增書籍成功，加入書名：{title}，作者：{author}，ISBN：{isbn}，數量：{quantity}");
            Console.WriteLine("按任意鍵回到目錄頁面");
            Console.ReadLine();
        }

        private static void DeleteBook()
        {
            Console.Clear();
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
            Console.WriteLine("按任意鍵回到目錄頁面");
            Console.ReadLine();
        }

        private static void FindBooks()
        {
            Console.Clear();
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

            Console.WriteLine($"尋找到的書籍為{allBooks}");
            Console.WriteLine("按任意鍵回到目錄頁面");
            Console.ReadLine();
        }

        private static void UpdateQuantity()
        { 
            Console.Clear();
            Console.WriteLine("修改書籍數量");

            Console.Write("請輸入想要修改書籍數量的ISBN碼：");
            string isbn;
            while (true)
            { 
                isbn = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(isbn))
                {
                    Console.Write("ISBN碼不得沒有輸入，");
                    continue;
                }
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
                if (string.IsNullOrWhiteSpace(isbn))
                {
                    Console.Write("數量不得沒有輸入，");
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
            Console.WriteLine("按任意鍵回到目錄頁面");
            Console.ReadLine();
        }
    }
}
