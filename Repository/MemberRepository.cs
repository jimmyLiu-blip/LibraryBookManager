using Domain;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Repository
{
    public class MemberRepository:IMemberRepository
    {
        private readonly List<Member> _members;

        public MemberRepository()
        {
            _members = new List<Member>
            {
                new Member("N001","李東海",MemberType.Regular),
                new Member("N002","曹圭賢",MemberType.Premium),
                new Member("N003","曹政奭",MemberType.NonMember)
            };
        }

        public void AddMember(Member member)
        {
            _members.Add(member);
        }

        public Member GetMemberById(string memberId)
        {
            return _members.FirstOrDefault(m => m.MemberId == memberId);
        }

        public List<Member> GetAllMembers()
        { 
            return _members; 
        }
        public void UpdateMember(Member member)
        { 
            var existing = GetMemberById(member.MemberId);
            if (existing != null)
            {
                existing.Name = member.Name;
                existing.CurrentBorrowCount = member.CurrentBorrowCount;
            }
        }

    }
}
