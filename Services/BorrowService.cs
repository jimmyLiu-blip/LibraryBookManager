using Domain;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class BorrowService:IBorrowService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly IBorrowRecordRepository _borrowRecordRepository;

        public BorrowService(IBookRepository bookRepository, IMemberRepository memberRepository, IBorrowRecordRepository borrowRecordRepository)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
            _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
            _borrowRecordRepository = borrowRecordRepository ??　throw new ArgumentNullException(nameof(borrowRecordRepository));
        }

        public void BorrowBook(string memberId, string isbn)
        {
            if (string.IsNullOrWhiteSpace(memberId))
            { 
                throw new ArgumentNullException(nameof(memberId));
            }
            if (string.IsNullOrWhiteSpace(isbn))
            {
                throw new ArgumentNullException(nameof(isbn));
            }

            var member = _memberRepository.GetMemberById(memberId);
            if (member == null)
            {
                throw new InvalidOperationException($"會員：{memberId}不存在");
            }

            var book = _bookRepository.GetAllBooksFromList().FirstOrDefault(b => b.ISBN == isbn);
            if (book == null)
            {
                throw new InvalidOperationException($"ISBN：{isbn}的書籍不存在");
            }

            if (book.AvailableQuantity <= 0)
            {
                throw new InvalidOperationException($"{book.Title}目前沒有庫存可以借閱");
            }

            if (!member.CanBorrow(1))
            {
                throw new InvalidOperationException($"{member.Name}已達借閱上限({member.CurrentBorrowCount}/{member.MaxBorrowLimit})");
            }

            book.AvailableQuantity--;
            member.CurrentBorrowCount++;

            var record = new BorrowRecord(memberId, isbn);
            _borrowRecordRepository.AddRecord(record);

            _memberRepository.UpdateMember(member);
        }
        public void ReturnBook(string memberId, string isbn)
        {
            if (string.IsNullOrWhiteSpace(memberId))
            {
                throw new ArgumentNullException(nameof(memberId));
            }
            if (string.IsNullOrWhiteSpace(isbn))
            {
                throw new ArgumentNullException(nameof(isbn));
            }

            var member = _memberRepository.GetMemberById(memberId);
            if (member == null)
            {
                throw new InvalidOperationException($"會員：{memberId}不存在");
            }

            var book = _bookRepository.GetAllBooksFromList().FirstOrDefault(b => b.ISBN == isbn);
            if (book == null)
            {
                throw new InvalidOperationException($"ISBN：{isbn}的書籍不存在");
            }

            var activeRecord = _borrowRecordRepository.GetRecordsByMemberId(memberId).FirstOrDefault(r => r.ISBN == isbn && !r.IsReturned);
            if (activeRecord == null)
            {
                throw new InvalidOperationException($"{member.Name}沒有借閱ISBN：{isbn}的紀錄");
            }

            activeRecord.Return();
            book.AvailableQuantity++;
            member.CurrentBorrowCount--;

            _memberRepository.UpdateMember(member);
            _borrowRecordRepository.UpdateRecord(activeRecord);
        }

        public List<BorrowRecord> GetMemberBorrowHistory(string memberId)
        {
            if (string.IsNullOrWhiteSpace(memberId))
            {
                throw new ArgumentNullException(nameof(memberId));
            }
            return _borrowRecordRepository.GetRecordsByMemberId(memberId);
        }

        public List<BorrowRecord> GetActiveBorrows(string memberId)
        {
            if (string.IsNullOrWhiteSpace(memberId))
            {
                throw new ArgumentNullException(nameof(memberId));
            }
            return _borrowRecordRepository.GetRecordsByMemberId(memberId)
                .Where(r => !r.IsReturned)
                .ToList();
        }
    }
}
