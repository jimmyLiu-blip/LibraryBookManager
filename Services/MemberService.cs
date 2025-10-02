using Domain;
using Repository;
using System;
using System.Collections.Generic;

namespace Services
{
    public class MemberService:IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        public MemberService(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
        }
        public void RegisterMember(string memberId, string name, MemberType memberType)
        {
            if (string.IsNullOrWhiteSpace(memberId))
            {
                throw new ArgumentNullException(nameof(memberId), "會員ID不可為空");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), "姓名不可為空");
            }

            var existing = _memberRepository.GetMemberById(memberId);
            if (existing != null)
            {
                throw new InvalidOperationException($"會員ID：{memberId}已存在");
            }

            var Member = new Member(memberId, name, memberType);
            _memberRepository.AddMember(Member);
        }
        public Member GetMember(string memberId)
        {
            if (string.IsNullOrWhiteSpace(memberId))
            {
                throw new ArgumentException(nameof(memberId), "會員ID不可為空");
            }

            return _memberRepository.GetMemberById(memberId);
        }
        public List<Member> GetAllMembers()
        { 
            return _memberRepository.GetAllMembers();
        }

        public void UpgradeMembership(string memberId, MemberType newType)
        {
            if (string.IsNullOrWhiteSpace(memberId))
            {
                throw new ArgumentNullException(nameof(memberId), "會員ID不可為空");
            }

            var member = _memberRepository.GetMemberById(memberId);
            if (member == null)
            {
                throw new InvalidOperationException($"會員：{memberId}不存在");
            }

            var updateMember = new Member(member.MemberId, member.Name, newType)
            {
                CurrentBorrowCount = member.CurrentBorrowCount
            };
                
            _memberRepository.UpdateMember(updateMember);
        }

        public void DeleteMember(string memberId)
        {
            if (string.IsNullOrWhiteSpace(memberId))
            {
                throw new ArgumentNullException(nameof(memberId), "會員ID不存在");
            }

            var member = _memberRepository.GetMemberById(memberId);
            if (member == null)
            {
                throw new InvalidOperationException($"會員ID：{memberId}不存在，無法執行註銷");
            }

             _memberRepository.DeleteMember(memberId);
        }
    }
}
