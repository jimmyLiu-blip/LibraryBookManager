using Domain;
using System.Collections.Generic;

namespace Services
{
    public interface IMemberService
    {
        void RegisterMember(string memberId, string name, MemberType memberType);
        Member GetMember(string memberId);
        List<Member> GetAllMembers();
        void UpgradeMembership(string memberId, MemberType newType);
        void DeleteMember(string memberId);
    }

}
