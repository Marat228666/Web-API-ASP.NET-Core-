

namespace WebAPI_ASPNET_Core.HttpControllers;

[ApiController]
[Route("api/[controller]")]
public class HttpGetController:ControllerBase
{
    private readonly ApplicationDbContext _context;
    
    public HttpGetController(ApplicationDbContext context)
    {
        _context = context;
    }
        
    [HttpGet("get-all-users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users= await _context.GetAllUsers();

        if (users.Count() != 0)
        {
            return Ok(users);
            
        }

        return NoContent();
    }
    
    [HttpGet("get-all-paging-users/{pageNumber:int:min(1)}/{pageSize:int:min(1)}")]
    [HttpGet("get-all-paging-users/{pageNumber:int:min(1)}&{pageSize:int:min(1)}")]
    [HttpGet("get-all-paging-users")]
    public async Task<IActionResult> GetAllPagingUsers(int? pageNumber, int? pageSize)
    {
        int CurrentPageNumber = pageNumber ?? 1;
        int CurrentPageSize = pageSize ?? 5;
        var users= await _context.GetAllUsers();

        if (users.Count() != 0)
        {
            return Ok(users.Skip((CurrentPageNumber-1)*CurrentPageSize).Take(CurrentPageSize));
            
        }

        return NoContent();
    }
    
    [HttpGet("get-user-by-id/{id:int:min(1)}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _context.GetUserById(id);
        if (user is UserModel)
        {
            return Ok(user);
        }

        return BadRequest( new NullReferenceException($"User({{\"id\":{id}}}) is not found").Message);




    }
    
    [HttpGet("get-users-by-property/{propertyName}/{propertyValue}/")]
    [HttpGet("get-users-by-property/{propertyName}&{propertyValue}")]
    public async Task<IActionResult> GetUsersByProperty(string propertyName, string propertyValue)
    {
        try
        {
            var usersByProperty = await _context.GetUsersByProperty(propertyName, propertyValue);
            return Ok(usersByProperty);
        }
        catch(ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    
    [HttpGet("get-paging-users-by-property/{propertyName}/{propertyValue}/{pageNumber:int:min(1)}/{pageSize:int:min(1)}")]
    [HttpGet("get-paging-users-by-property/{propertyName}&{propertyValue}/{pageNumber:int:min(1)}&{pageSize:int:min(1)}")]
    public async Task<IActionResult> GetPagingUsersByProperty(string propertyName, string propertyValue,int? pageNumber, int? pageSize)
    {
        int CurrentPageNumber = pageNumber ?? 1;
        int CurrentPageSize = pageSize ?? 5;
        try
        {
            var usersByProperty = await _context.GetUsersByProperty(propertyName, propertyValue);
            return Ok(usersByProperty.Skip((CurrentPageNumber-1)*CurrentPageSize).Take(CurrentPageSize));
        }
        catch(ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }
    
}

