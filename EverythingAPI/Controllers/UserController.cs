using EverythingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using EverythingAPI.DAL;
using Microsoft.AspNetCore.RateLimiting;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("fixed")]
public class UserController : ControllerBase
{
    private readonly UserDAL UserDAL;

    private readonly List<User> users = new List<User>(); 

    public UserController(UserDAL userDAL)
    {
        UserDAL = userDAL;
    }

    [HttpGet]
    public async Task<IEnumerable<User>> GetUsers() 
    {
        users.AddRange(await UserDAL.RetrieveAllUsers());
        return users; 
    }

    [HttpGet("{email}")]
    public async Task<ActionResult<User>> GetUsersByEmailFull(string email)
    {
        List<User> user = await UserDAL.RetrieveSpecificUserAllData(email);


        return Ok(user);
    }


    [HttpPost]
    public async Task<IActionResult> CreateNewUser(string userName, string userEmail)
    {
        try
        {
            await UserDAL.CreateNewUser(userName, userEmail);
            return Ok("User created successfully.");
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

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        try
        {
            await UserDAL.DeleteUser(userId);
            return Ok("User deleted successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

}
