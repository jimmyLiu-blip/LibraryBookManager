using Domain;
using System.Collections.Generic;
using System.Linq;


namespace Repository
{
    public class BorrowRecordRepository:IBorrowRecordRepository
    {
        private readonly List<BorrowRecord> _records;

        public BorrowRecordRepository()
        {
            _records = new List<BorrowRecord>();
        }

        public void AddRecord(BorrowRecord record)
        {
            _records.Add(record);
        }

        public List<BorrowRecord> GetRecordsByMemberId(string memberId)
        {
            return _records.Where(r => r.MemberId == memberId).ToList();
        }

        public List<BorrowRecord> GetRecordsByISBN(string isbn)
        { 
            return _records.Where(r => r.ISBN == isbn).ToList();
        }

        public List<BorrowRecord> GetRecord()
        { 
            return _records.Where(r => !r.IsReturned).ToList();
        }

        public BorrowRecord GetRecordById(string recordid)
        {
            return _records.FirstOrDefault(r => r.RecordId == recordid);
        }

        public void UpdateRecord(BorrowRecord record)
        {
            var existing = GetRecordById(record.RecordId);
            if (existing != null)
            {
                existing.ReturnDate = record.ReturnDate;
                existing.IsReturned = record.IsReturned;
            }
        }

        public void DeleteRecord(BorrowRecord record)
        {
            var existing = GetRecordById(record.RecordId);
            if (existing != null)
            {
                _records.Remove(existing);
            }
        }

    }
}
