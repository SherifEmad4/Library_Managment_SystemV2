using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library_Managment.Infrastructure.Data;
using Library_Managment.Domain.Entities;

namespace Library_Managment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowRecordsController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BorrowRecordsController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: api/borrowrecords
        [HttpGet]
        public async Task<IActionResult> GetBorrowRecords()
        {
            var records = await _context.BorrowRecords
                .Include(b => b.Book)
                .Include(m => m.Member)
                .ToListAsync();

            return Ok(records);
        }

        // GET: api/borrowrecords/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBorrowRecord(int id)
        {
            var record = await _context.BorrowRecords
                .Include(b => b.Book)
                .Include(m => m.Member)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (record == null)
                return NotFound();

            return Ok(record);
        }

        // POST: api/borrowrecords
        [HttpPost]
        public async Task<IActionResult> CreateBorrowRecord([FromBody] BorrowRecord record)
        {
            _context.BorrowRecords.Add(record);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBorrowRecord), new { id = record.Id }, record);
        }

        // PUT: api/borrowrecords/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBorrowRecord(int id, [FromBody] BorrowRecord updatedRecord)
        {
            if (id != updatedRecord.Id)
                return BadRequest();

            _context.Entry(updatedRecord).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/borrowrecords/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBorrowRecord(int id)
        {
            var record = await _context.BorrowRecords.FindAsync(id);
            if (record == null)
                return NotFound();

            _context.BorrowRecords.Remove(record);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
