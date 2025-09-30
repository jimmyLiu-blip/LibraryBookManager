using Domain;
using System.Collections.Generic;

namespace Repository
{
    public interface IMemberRepository
    {
        void AddMember(Member member);
        Member GetMemberById(string memberId);
        List<Member> GetAllMembers();
        void UpdateMember(Member member);
        void DeleteMember(string memberId);
    }
}
