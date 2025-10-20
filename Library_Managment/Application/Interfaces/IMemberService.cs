using Library_Managment.Application.DTOs;

namespace Library_Managment.Application.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberDto>> GetAllAsync();
        Task<MemberDto> CreateAsync(CreateMemberDto dto);
        Task<MemberDto?> GetByIdAsync(int id);
    }

}
