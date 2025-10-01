using Domain;
using Repository;
using Services;
using System;
using System.Linq;

namespace LibraryBookManager
{
    public class Program
    {
        private static BookService _bookService = new BookService(new BookRepository());
        private static MemberService _memberService = new MemberService(new MemberRepository());
        private static BorrowService _borrowService = new BorrowService(new BookRepository(), new MemberRepository(), new BorrowRecordRepository());
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
                            RegisterMember();
                            break;
                        case "7":
                            //DeleteMember();
                            break;
                        case "8":
                            //UpgradeMembership();
                            break;
                        case "9":
                            //GetAllMembers();
                            break;
                        case "10":
                            //GetMember();
                            break;
                        case "11":
                            //BorrowBook();
                            break;
                        case "12":
                            //ReturnBook();
                            break;
                        case "13":
                            //GetMemberBorrowHistory();
                            break;
                        case "14":
                            //GetActiveBorrows();
                            break;
                        case "15":
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
                    Console.WriteLine("按任意鍵回到目錄頁面");
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"出現異常錯誤{ex.Message}");
                    Console.WriteLine("按任意鍵回到目錄頁面");
                    Console.ReadLine();
                }
            }
        }

        private static void Showmenu()
        {
            Console.WriteLine("輸入 1，新增書籍");
            Console.WriteLine("輸入 2，查詢書籍");
            Console.WriteLine("輸入 3，修改書籍數量");
            Console.WriteLine("輸入 4，刪除書籍");
            Console.WriteLine("輸入 5，所有書籍清單");
            Console.WriteLine("===============================");
            Console.WriteLine("輸入 6，註冊會員");
            Console.WriteLine("輸入 7，註銷會員");
            Console.WriteLine("輸入 8，更新會員");
            Console.WriteLine("輸入 9，顯示所有會員");
            Console.WriteLine("輸入10，取得會員資料");
            Console.WriteLine("===============================");
            Console.WriteLine("輸入11，借書服務");
            Console.WriteLine("輸入12，還書服務");
            Console.WriteLine("輸入13，取得會員借書紀錄");
            Console.WriteLine("輸入14，取得會員尚未歸還書籍的紀錄");
            Console.WriteLine("===============================");
            Console.WriteLine("輸入15，離開");
            Console.Write("請輸入數字1~15：");
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
                var inputTitle = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(inputTitle))
                {
                    Console.WriteLine("不可沒有輸入書名");
                    continue;
                }
                title = inputTitle;

                Console.Write("請輸入書籍的作者：");
                var inputAuthor = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(inputAuthor))
                {
                    Console.WriteLine("不可沒有輸入作者");
                    continue;
                }
                author = inputAuthor;

                Console.Write("請輸入書籍的ISBN：");
                string inputISBN = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(inputISBN))
                {
                    Console.WriteLine("不可沒有輸入ISBN");
                    continue;
                }
                isbn = inputISBN;
                
                Console.Write("請輸入書籍的數量：");
                string inputQuantity = Console.ReadLine();
                if (!int.TryParse(inputQuantity, out quantity))
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

            Console.WriteLine($"新增書籍成功，加入書名：{title,-20}，作者：{author,-10}，ISBN：{isbn,-10}，庫存總數量：{quantity,3}，可借數量：{quantity,3}");
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
        private static void RegisterMember()
        {
            Console.WriteLine("===註冊新會員===");

            string memberId;
            string name;
            MemberType memberType;

            while (true)
            {
                Console.Write("請輸入你的會員ID：");
                var inputmemberId = Console.ReadLine();
                if (string.IsNullOrEmpty(inputmemberId))
                {
                    Console.WriteLine("不可沒有輸入會員ID");
                    continue;
                }
                memberId = inputmemberId;

                Console.Write("請輸入你的名字：");
                var inputname = Console.ReadLine();
                if (string.IsNullOrEmpty(inputname))
                {
                    Console.WriteLine("不可沒有輸入姓名");
                    continue;
                }
                name = inputname;

                Console.WriteLine("請選擇會員類型");
                Console.WriteLine("1.非會員(可借2本)");
                Console.WriteLine("2.一般會員(可借5本)");
                Console.WriteLine("3.高級會員(可借10本)");
                Console.Write("請輸入選項(1~3)：");
                var inpurMemberType = Console.ReadLine();

                if (inpurMemberType == "1")
                {
                    memberType = MemberType.NonMember;
                    break;
                }
                else if (inpurMemberType == "2")
                {
                    memberType = MemberType.Regular;
                    break;
                }
                else if (inpurMemberType == "3")
                {
                    memberType = MemberType.Premium;
                }
                else
                {
                    Console.WriteLine("輸入錯誤，請重新輸入");
                    continue;
                }
            }
            _memberService.RegisterMember(memberId, name, memberType);
            Console.WriteLine($"註冊會員成功!!會員ID：{memberId}，會員姓名：{name}，會員類型:{GetMemberTypeText(memberType)}");
        }

        private static string GetMemberTypeText(MemberType type)
        {
            switch (type)
            { 
                case MemberType.NonMember:
                    return "非會員";
                case MemberType.Regular:
                    return "一般會員";
                case MemberType.Premium:
                    return "高級會員";
                default:
                    return "未知類型";
            }
        }
    }
}
