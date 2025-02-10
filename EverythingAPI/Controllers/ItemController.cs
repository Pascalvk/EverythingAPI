using EverythingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using EverythingAPI.DAL;

namespace EverythingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private ItemDAL ItemDAL { get; set; }

        public ItemController(ItemDAL itemDAL)
        {
            ItemDAL = itemDAL;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewItem(string itemName, string itemDescription, int statusId, int boardId)
        {         
            try
            {
                await ItemDAL.CreateItem(itemName, itemDescription, statusId, boardId);
                return Ok("Item created successfully.");
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

        [HttpDelete("{itemId}")]
        public async Task<IActionResult> DeleteItem(int itemId)
        {
            try
            {
                await ItemDAL.DeleteItem(itemId);
                return Ok("Item deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> ChangeItemStatus(int itemId, int itemStatusId)
        {
            try
            {
                await ItemDAL.ChangeItemStatus(itemId, itemStatusId);
                return Ok("Item changed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
