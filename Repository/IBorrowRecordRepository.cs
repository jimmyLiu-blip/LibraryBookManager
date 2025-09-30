using Domain;
using System.Collections.Generic;

namespace Repository
{
    public interface IBorrowRecordRepository
    {
        void AddRecord(BorrowRecord record);
        List<BorrowRecord> GetRecordsByMemberId(string memberId);
        List<BorrowRecord> GetRecordsByISBN(string isbn);
        List<BorrowRecord> GetActiveRecords();
        void UpdateRecord(BorrowRecord record);
        void DeleteRecord(BorrowRecord record);
    }
}
