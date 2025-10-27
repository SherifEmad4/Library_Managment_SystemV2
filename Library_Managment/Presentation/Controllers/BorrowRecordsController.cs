using Library_Managment.Application.DTOs;
using Library_Managment.Domain.Entities;
using Library_Managment.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_Managment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowRecordsController : ControllerBase
    {
        private readonly IGenericRepository<BorrowRecord> _borrowRepository;
        private readonly IGenericRepository<Book> _bookRepository;

        public BorrowRecordsController(IGenericRepository<BorrowRecord> borrowRepository, IGenericRepository<Book> bookRepository)
        {
            _borrowRepository = borrowRepository;
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetBorrowRecords()
        {
            var records = (await _borrowRepository.Query()
                .Include(b => b.Book)
                .Include(m => m.Member)
                .ToListAsync())
                .Select(r => new BorrowRecordResponseDto
                {
                    Id = r.Id,
                    BookId = r.BookId,
                    BookTitle = r.Book.Title,
                    MemberId = r.MemberId,
                    MemberName = r.Member.Name,
                    BorrowDate = r.BorrowDate,
                    ReturnDate = r.ReturnDate ?? default
                })
                .ToList();

            return Ok(records);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetBorrowRecordById(int id)
        {
            // Query with Include to get Book and Member data
            var borrowRecord = await _borrowRepository.Query()
                .Include(r => r.Book)
                .Include(r => r.Member)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (borrowRecord == null)
                return NotFound("Borrow record not found.");

            var responseDto = new BorrowRecordResponseDto
            {
                Id = borrowRecord.Id,
                BookId = borrowRecord.BookId,
                BookTitle = borrowRecord.Book.Title,
                MemberId = borrowRecord.MemberId,
                MemberName = borrowRecord.Member.Name,
                BorrowDate = borrowRecord.BorrowDate,
                ReturnDate = borrowRecord.ReturnDate ?? default
            };

            return Ok(responseDto);
        }


        [HttpPost("borrow")]
        [Authorize]
        public async Task<IActionResult> BorrowBook([FromBody] BorrowRequestDto dto)
        {
            var memberId = int.Parse(User.FindFirst("id").Value);
            var book = await _bookRepository.GetByIdAsync(dto.BookId);

            if (book == null) return NotFound("Book not found.");
            if (!book.IsAvailable) return BadRequest("Book is currently not available.");

            var existingBorrow = await _borrowRepository.Query()
                .FirstOrDefaultAsync(br => br.BookId == dto.BookId && br.MemberId == memberId && br.ReturnDate == null);

            if (existingBorrow != null) return BadRequest("You have already borrowed this book.");

            var borrowRecord = new BorrowRecord
            {
                BookId = dto.BookId,
                MemberId = memberId,
                BorrowDate = DateTime.Now
            };

            book.IsAvailable = false;

            await _borrowRepository.AddAsync(borrowRecord);
            await _bookRepository.UpdateAsync(book);

            return Ok(new
            {
                message = "Book borrowed successfully",
                borrowRecord = new BorrowRecordDto
                {
                    Id = borrowRecord.Id,
                    BookId = borrowRecord.BookId,
                    BorrowDate = borrowRecord.BorrowDate
                }
            });
        }

        [HttpPost("return")]
        [Authorize]
        public async Task<IActionResult> ReturnBook([FromBody] BorrowRequestDto dto)
        {
            var memberId = int.Parse(User.FindFirst("id").Value);

            var borrowRecord = await _borrowRepository.Query()
                .Include(r => r.Book)
                .Include(r => r.Member)
                .Where(r => r.BookId == dto.BookId && r.MemberId == memberId && r.ReturnDate == null)
                .OrderByDescending(r => r.BorrowDate)
                .FirstOrDefaultAsync();

            if (borrowRecord == null)
                return BadRequest("No active borrow record found.");

            var book = borrowRecord.Book;
            if (book == null)
                return NotFound("Book not found.");

            borrowRecord.ReturnDate = DateTime.Now;
            book.IsAvailable = true;

            await _borrowRepository.UpdateAsync(borrowRecord);
            await _bookRepository.UpdateAsync(book);

            var responseDto = new BorrowRecordResponseDto
            {
                Id = borrowRecord.Id,
                BookId = borrowRecord.BookId,
                BookTitle = borrowRecord.Book.Title,
                MemberId = borrowRecord.MemberId,
                MemberName = borrowRecord.Member.Name,
                BorrowDate = borrowRecord.BorrowDate,
                ReturnDate = DateTime.Now
            };

            return Ok(new
            {
                message = "Book returned successfully",
                borrowRecord = responseDto
            });
        }

    }
}
