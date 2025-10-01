using System;

namespace Domain
{
    public class BorrowRecord
    {
        public string RecordId { get; set; }
        public string MemberId { get; set; }
        public string ISBN { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned { get; set; }

        public BorrowRecord(string memberId, string isbn)
        { 
            RecordId = Guid.NewGuid().ToString();
            MemberId = memberId;
            ISBN = isbn;
            BorrowDate = DateTime.Now;
            ReturnDate = null;
            IsReturned = false;
        }

        public void Return()
        { 
            ReturnDate = DateTime.Now;
            IsReturned = true;
        }

        public override string ToString()
        {
            return $"借閱日期：{BorrowDate:yyyy-MM-dd} ISBN：{ISBN,-10}";
        }
    }
}
