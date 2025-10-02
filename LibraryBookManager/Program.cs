using Domain;
using Repository;
using Services;
using System;
using System.Linq;

namespace LibraryBookManager
{
    public class Program
    {
        private static readonly BookRepository _bookRepository = new BookRepository();
        private static readonly MemberRepository _memberRepository = new MemberRepository();
        private static readonly BorrowRecordRepository _borrowRecordRepository = new BorrowRecordRepository();

        private static readonly BookService _bookService = new BookService(_bookRepository);
        private static readonly MemberService _memberService = new MemberService(_memberRepository);
        private static readonly BorrowService _borrowService = new BorrowService(_bookRepository, _memberRepository, _borrowRecordRepository);
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
                            DeleteMember();
                            break;
                        case "8":
                            UpgradeMembership();
                            break;
                        case "9":
                            GetAllMembers();
                            break;
                        case "10":
                            GetMember();
                            break;
                        case "11":
                            BorrowBook();
                            break;
                        case "12":
                            ReturnBook();
                            break;
                        case "13":
                            GetMemberBorrowHistory();
                            break;
                        case "14":
                            GetActiveBorrows();
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
            Console.WriteLine("輸入 5，顯示所有書籍清單");
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
                var inputMemberType = Console.ReadLine();

                if (inputMemberType == "1")
                {
                    memberType = MemberType.NonMember;
                    break;
                }
                else if (inputMemberType == "2")
                {
                    memberType = MemberType.Regular;
                    break;
                }
                else if (inputMemberType == "3")
                {
                    memberType = MemberType.Premium;
                    break;
                }
                else
                {
                    Console.WriteLine("輸入錯誤，請重新輸入");
                    continue;
                }
            }
            _memberService.RegisterMember(memberId, name, memberType);
            Console.Clear();
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

        private static void DeleteMember()
        {
            Console.WriteLine("===註銷會員===");

            string memberId;

            while (true)
            {
                Console.Write("請輸入你的會員ID：");
                var inputmemberId = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(inputmemberId))
                {
                    Console.WriteLine("不可以沒有輸入會員ID");
                    continue;
                }
                memberId = inputmemberId;
                break;
            }
            var member = _memberService.GetMember(memberId);
            if (member == null)
            {
                Console.WriteLine($"此會員ID：{memberId}不存在");
                return;
            }
            _memberService.DeleteMember(memberId);

            Console.Clear();
            Console.WriteLine($"註銷會員成功，已註銷會員ID為：{memberId}");
        }

        private static void UpgradeMembership()
        {
            Console.WriteLine("===更新會員===");

            string memberId;

            while (true)
            {
                Console.Write("請輸入你的會員ID：");
                var inputmemberId = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(inputmemberId))
                {
                    Console.WriteLine($"不可沒有輸入會員ID");
                    continue;
                }
                memberId = inputmemberId;
                break ;
            }
            var member = _memberService.GetMember(memberId);
            if (member == null)
            {
                Console.WriteLine($"此會員ID：{memberId}不存在");
                return;
            }

            Console.WriteLine($"目前的會員ID：{memberId}，目前類型：{GetMemberTypeText(member.MemberType)}");
            Console.WriteLine("請選擇新的會員類型：");
            Console.WriteLine("1.非會員(可借2本書)");
            Console.WriteLine("2 一般會員(可借5本書)");
            Console.WriteLine("3 高級會員(可借10本書)");
            Console.Write("請輸入選項(1~3)：");

            MemberType newMemberType;
            string inputMemberType;

            while (true)
            {
                inputMemberType = Console.ReadLine();
                if (inputMemberType == "1")
                {
                    newMemberType = MemberType.NonMember;
                    break;
                }
                else if (inputMemberType == "2")
                {
                    newMemberType = MemberType.Regular;
                    break;
                }
                else if (inputMemberType == "3")
                {
                    newMemberType = MemberType.Premium;
                    break;
                }
                else
                {
                    Console.WriteLine("輸入未知類型，請重新輸入");
                    Console.Write("請重新輸入選項(1~3)：");
                    continue;
                }
            }

            if (member.MemberType == newMemberType)
            {
                Console.WriteLine($"會員類型未變動，目前已是{GetMemberTypeText(newMemberType)}");
                return;
            }

            _memberService.UpgradeMembership(memberId, newMemberType);
            Console.Clear();
            Console.WriteLine($"會員更新成功!!會員ID：{memberId}，新會員類型：{GetMemberTypeText(newMemberType)}");
        }

        private static void GetAllMembers()
        {
            Console.WriteLine("===取得所有會員===");

            var allMembers = _memberService.GetAllMembers();
            if (allMembers == null || !allMembers.Any())
            {
                Console.WriteLine("目前無任何會員存在");
                return;
            }
            foreach (var member in allMembers)
            {
                Console.WriteLine($"會員ID：{member.MemberId}，會員姓名：{member.Name}，會員類型：{GetMemberTypeText(member.MemberType)}，目前借閱數：{member.CurrentBorrowCount}/{member.MaxBorrowLimit}");
            }
        
        }

        private static void GetMember()
        {
            Console.WriteLine("===取得會員資料===");

            string memberId;

            while (true)
            {
                Console.Write("請輸入你的會員ID：");
                var inputmemberId = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(inputmemberId))
                {
                    Console.WriteLine("不可沒有輸入會員ID");
                    continue;
                }
                memberId = inputmemberId;
                break;
            }

            var member = _memberService.GetMember(memberId);
            if (member == null)
            {
                Console.WriteLine($"此會員ID：{memberId}不存在");
                return ;
            }
            Console.Clear();
            Console.WriteLine($"會員ID：{memberId}，會員姓名：{member.Name}，會員類型：{GetMemberTypeText(member.MemberType)}，借閱數：{member.CurrentBorrowCount}/{member.MaxBorrowLimit}");
        }

        private static void BorrowBook()
        {
            Console.WriteLine("===借書服務===");

            string memberId;
            string isbn;
            Member member = null;
            Book book = null;

            while (true)
            {
                Console.Write("請輸入你的會員ID：");
                var inputmemberId = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(inputmemberId))
                {
                    Console.WriteLine("不可沒有輸入會員Id");
                    continue;
                }
                memberId = inputmemberId;
                member = _memberService.GetMember(memberId);

                if (member == null)
                {
                    Console.WriteLine($"不存在此會員ID：{memberId}");
                    continue;
                }

                break;
            }

            while (true)
            {
                Console.Write("請輸入要借閱書籍的ISBN：");
                var inputisbn = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(inputisbn))
                {
                    Console.WriteLine("不可沒有輸入借閱書籍的ISBN");
                    continue;
                }
                isbn = inputisbn;
                book = _bookService.GetBookByISBN(isbn);
                if (book == null)
                {
                    Console.WriteLine($"此書籍ISBN：{isbn}不存在");
                    continue;
                }
                break;
            }

            if (book.AvailableQuantity <= 0)
            {
                Console.WriteLine($"此書籍ISBN：{isbn}目前沒有庫存可以出借");
                return;
            }
            if (!member.CanBorrow(1))
            {
                Console.WriteLine($"此會員：{member.Name}已達可借閱上限");
                return;
            }

            try
            {
                _borrowService.BorrowBook(memberId, isbn);
                Console.WriteLine($"借閱書籍成功!會員姓名：{member.Name},成功借閱書籍：{book.Title},ISBN：{isbn}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"借閱失敗!原因：{ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"發生未知錯誤造成借閱失敗!原因：{ex.Message}");
            }

        }

        private static void ReturnBook()
        {
            Console.WriteLine("===還書服務===");

            string memberId;
            string isbn;
            Member member = null;
            Book book = null;

            while (true)
            {
                Console.Write("請輸入你的會員ID：");
                var inputmemberId = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(inputmemberId))
                {
                    Console.WriteLine("不可沒有輸入會員ID");
                    continue;
                }
                memberId = inputmemberId;

                member = _memberService.GetMember(memberId);
                if (member == null)
                {
                    Console.WriteLine($"此會員ID:{memberId}不存在");
                    continue;
                }
                break;
            }

            while (true)
            {
                Console.Write("請輸入你要歸還書籍的ISBN：");
                var inputisbn = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(inputisbn))
                {
                    Console.WriteLine("不可沒有輸入歸還書籍的ISBN");
                    continue;
                }
                isbn = inputisbn;

                book = _bookService.GetBookByISBN(isbn);
                if (book == null)
                {
                    Console.WriteLine($"此ISBN：{isbn}書籍不存在");
                    continue;
                }
                break;
            }
            try
            {
                _borrowService.ReturnBook(memberId, isbn);
                Console.WriteLine($"還書成功!會員姓名：{member.Name},還書書名：{book.Title},ISBN：{isbn}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"還書失敗!原因：{ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"發生未知錯誤造成還書失敗!原因：{ex.Message}");
            }
            
        }

        private static void GetMemberBorrowHistory()
        {
            Console.WriteLine("===取得會員歷史借閱紀錄===");

            string memberId;
            Member member;

            while (true)
            {
                Console.Write("請輸入你的會員ID：");
                var inputmemberId = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(inputmemberId))
                {
                    Console.WriteLine("不可沒有輸入會員ID");
                    continue;
                }
                memberId = inputmemberId;

                member = _memberService.GetMember(memberId);
                if (member == null)
                {
                    Console.WriteLine($"此會員ID：{memberId}不存在");
                    continue;
                }
                break;
            }
            
            var allBorrowsHistory = _borrowService.GetMemberBorrowHistory(memberId);
            if (allBorrowsHistory == null ||!allBorrowsHistory.Any())
            {
                Console.WriteLine($"目前{member.Name}沒有任何借書紀錄");
                return;
            }
            foreach (var borrow in allBorrowsHistory)
            {
                Console.WriteLine($"借閱ID：{borrow.RecordId}，會員ID：{memberId}，借閱書籍ISBN：{borrow.ISBN}，借閱時間：{borrow.BorrowDate}，歸還時間：{borrow.ReturnDate}，歸還狀態：{(borrow.IsReturned ? "已歸還":"未歸還")}");
            }
        }

        private static void GetActiveBorrows()
        {
            Console.WriteLine("===取得會員尚未歸還書籍紀錄===");

            string memberId;
            Member member;
            while (true)
            {
                Console.Write("請輸入你的會員ID：");
                var inputmemberId = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(inputmemberId))
                {
                    Console.WriteLine("不可沒有輸入會員ID");
                    continue;
                }
                memberId = inputmemberId;

                member = _memberService.GetMember(memberId);
                if (member == null)
                {
                    Console.WriteLine($"此會員ID：{memberId}不存在會員");
                }
                break;
            }
            var allBorrows = _borrowService.GetActiveBorrows(memberId);
            if (allBorrows == null || !allBorrows.Any())
            {
                Console.WriteLine($"會員姓名：{member.Name}目前沒有尚未歸還的書籍");
                return;
            }
            foreach (var borrow in allBorrows)
            {
                Console.WriteLine($"借閱ID：{borrow.RecordId}，會員ID：{memberId}，借閱書籍ISBN：{borrow.ISBN}，借閱時間：{borrow.BorrowDate}");
            }

        }
    }
}
