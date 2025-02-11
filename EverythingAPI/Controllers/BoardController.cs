using EverythingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using EverythingAPI.DAL;
using Microsoft.AspNetCore.RateLimiting;

namespace EverythingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoardController : ControllerBase
    {

        private BoardDAL BoardDAL;

        public BoardController(BoardDAL boardDAL)
        {
            BoardDAL = boardDAL;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewBoard(string name, int userId)
        {
            try
            {
                await BoardDAL.CreateBoard(name, userId);
                return Ok("Board created successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpDelete("{boardId}")]
        public async Task<IActionResult> DeleteBoard(int boardId)
        {
            try
            {
                await BoardDAL.DeleteBoard(boardId);
                return Ok("Board deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
