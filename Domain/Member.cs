namespace Domain
{
    public enum MemberType
    {
        NonMember,
        Regular,
        Premium
    }

    public class Member
    { 
        public string MemberId { get;}
        public string Name { get; set; }
        public MemberType MemberType { get; private set; }
        public int MaxBorrowLimit { get; private set; }
        public int CurrentBorrowCount { get; set; }

        public Member(string memberId, string name, MemberType memberType)
        {
            MemberId = memberId;
            Name = name;
            MemberType = memberType;
            CurrentBorrowCount = 0;

            if (memberType == MemberType.NonMember)
            {
                MaxBorrowLimit = 2;
            }
            else if (memberType == MemberType.Regular)
            {
                MaxBorrowLimit = 5;
            }
            else if (memberType == MemberType.Premium)
            {
                MaxBorrowLimit = 10;
            }
            else
            {
                MaxBorrowLimit = 0;
            }
        }

        public bool CanBorrow(int quantity = 1)
        {
            if (quantity < 0)
            { 
                return false;
            }
            return (CurrentBorrowCount +  quantity) <= MaxBorrowLimit;
        }

        public override string ToString()
        {
            return $"會員ID：{MemberId,-10} 名稱：{Name,-5} 類型：{MemberType,-10} 已借：{CurrentBorrowCount}/{MaxBorrowLimit}";
        }
    }
}
