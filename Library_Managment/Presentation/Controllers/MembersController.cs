using Library_Managment.Domain.Entities;
using Library_Managment.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Library_Managment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly IGenericRepository<Member> _memberRepository;

        public MembersController(IGenericRepository<Member> memberRepository)
        {
            _memberRepository = memberRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetMembers()
        {
            var members = await _memberRepository.GetAllAsync();
            return Ok(members);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMember(int id)
        {
            var member = await _memberRepository.GetByIdAsync(id);
            if (member == null) return NotFound();
            return Ok(member);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMember([FromBody] Member member)
        {
            await _memberRepository.AddAsync(member);
            return CreatedAtAction(nameof(GetMember), new { id = member.Id }, member);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMember(int id, [FromBody] Member updatedMember)
        {
            var member = await _memberRepository.GetByIdAsync(id);
            if (member == null) return NotFound();

            member.Name = updatedMember.Name;
            member.Email = updatedMember.Email;

            await _memberRepository.UpdateAsync(member);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var member = await _memberRepository.GetByIdAsync(id);
            if (member == null) return NotFound();

            await _memberRepository.DeleteAsync(member);
            return NoContent();
        }
    }
}
