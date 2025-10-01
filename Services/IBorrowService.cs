using Domain;
using System.Collections.Generic;

namespace Services
{
    public interface IBorrowService
    {
        void BorrowBook(string memberId, string isbn);
        void ReturnBook(string memberId, string isbn);
        List<BorrowRecord> GetMemberBorrowHistory(string memberId);
        List<BorrowRecord> GetActiveBorrows(string memberId);

    }
}
