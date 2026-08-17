using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using bidding_zone_api.Configuration;
using bidding_zone_api.Dtos.Request;
using bidding_zone_api.Dtos.Response;
using bidding_zone_api.Interfaces;
using bidding_zone_api.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
// using Microsoft.IdentityModel.JsonWebTokens;
// using Microsoft.IdentityModel.Tokens;

namespace bidding_zone_api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UsersController: ControllerBase
{
    private readonly IUsersService _usersService;
    private readonly IValidator<UpdateUserDto> _updateUserValidator;
    private readonly IValidator<AddressDto> _updateAddressValidator;
    private readonly IValidator<CreateUserDto> _createUserValidator;
    private readonly IValidator<LoginDto> _loginValidator;
    private readonly IConfiguration _configuration;

    private readonly IAuthorizationService _authorizationService;

    private readonly ILogger<UsersController> _logger;
    private readonly JwtOptions _jwtOptions;

    public UsersController(IUsersService service,
    IAuthorizationService authorizationService,
     IValidator<UpdateUserDto> updateUserValidator,
     ILogger<UsersController> logger,
     IValidator<AddressDto> addressValidator,
     IValidator<CreateUserDto> createUserValidator,
     IValidator<LoginDto> loginValidator,
     IConfiguration configuration,
     IOptions<JwtOptions> options
     )
    {
        _usersService = service;
        _updateUserValidator = updateUserValidator;
        _logger = logger;
        _updateAddressValidator = addressValidator;
        _createUserValidator = createUserValidator;
        _loginValidator = loginValidator;
        _configuration = configuration;
        _jwtOptions = options.Value;
        _authorizationService = authorizationService;
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto data)
    {
        _logger.LogWarning("Login validation failed for request.");
        ValidationResult validationResult = await _loginValidator.ValidateAsync(data);
        if(!validationResult.IsValid)
        {
            _logger.LogWarning("data for login request validation failed");
            return BadRequest(validationResult.ToDictionary());
        }

        _logger.LogInformation("checking if user exists in the database");
        var user = await _usersService.Login(data.Email, data.Password);
        if(user == null)
        {
            _logger.LogWarning("Failed login attempt for email: {Email}", data.Email);
            return Unauthorized(new { message = "Invalid email or password." });
        }

        List<Claim> claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(20),
            signingCredentials: credentials
        );
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        _logger.LogInformation("user has succesfully logged in");
        return Ok(new LoginResponseDto{Token = jwt});
    }


    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserResponseDto>> GetMe()
    {
        var userId = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sub);
        if(userId == null) return NotFound();

        if(!Guid.TryParse(userId.Value, out var parsedId))
        {
            return BadRequest();
        }
        var result = await _usersService.GetUser(parsedId);
        if(result == null)
        {
            return NotFound();
        }
        return Ok(result);

    }


    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<UserResponseDto>>> GetUsers([FromQuery] int page)
    {
        _logger.LogInformation("fetching all users");
        var result = await _usersService.GetUsers(page);
        return Ok(result);
    }

    [HttpGet("test")]
    [Authorize]
    public async Task<ActionResult> TestAuthorize()
    {
        return Ok("working!");
    }


    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResponseDto>> GetUser([FromRoute] Guid id)
    {
        _logger.LogInformation("fetching user {UserId}", id);
        var result = await _usersService.GetUser(id);
        if(result == null)
        {
            _logger.LogWarning("user {UserId} not found", id);
            return NotFound();
        }
        return Ok(result);
    }


    [HttpPut("{id}/update")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserResponseDto>> UpdateUser([FromBody] UpdateUserDto data, [FromRoute] Guid id)
    {
        var authResult = await _authorizationService.AuthorizeAsync(User, id, "IsOwnerPolicy");
        if (!authResult.Succeeded)
        {
            return Forbid();
        }

        _logger.LogInformation("updating user {UserId}", id);
        ValidationResult validationResult = await _updateUserValidator.ValidateAsync(data);
        if(!validationResult.IsValid)
        {
            _logger.LogWarning("invalid data for updating user {UserId}", id);
            return BadRequest();
        }
        try
        {
            var result = await _usersService.UpdateUser(data, id);
            if(result == null)
            {
                _logger.LogWarning("user {UserId} not found for update", id);
                return NotFound();
            }
            _logger.LogInformation("user {UserId} updated successfully", id);
            return NoContent();
        }catch(BadHttpRequestException ex)
        {
            _logger.LogWarning(ex, "invalid data for updating a user");
            return BadRequest();
        }
    }


    [HttpPut("{id}/update/address")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserResponseDto>> UpdateUserAddress([FromBody] AddressDto data, [FromRoute] Guid id)
    {
        var authResult = await _authorizationService.AuthorizeAsync(User, id, "IsOwnerPolicy");
        if (!authResult.Succeeded)
        {
            return Forbid();
        }

        _logger.LogInformation("updating address for user {UserId}", id);
        ValidationResult validationResult = await _updateAddressValidator.ValidateAsync(data);
        if(!validationResult.IsValid)
        {
            _logger.LogWarning("invalid address data for user {UserId}", id);
            return BadRequest();
        }
        try
        {
            var result = await _usersService.UpdateAddress(data, id);
            if(result == null)
            {
                _logger.LogWarning("user {UserId} not found for address update", id);
                return NotFound();
            }
            _logger.LogInformation("address for user {UserId} updated successfully", id);
            return NoContent();
        }catch(BadHttpRequestException ex)
        {
            _logger.LogWarning(ex, "invalid data for updating a user's address");
            return BadRequest();
        }
    }


    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteUser([FromRoute] Guid id)
    {
        _logger.LogInformation("Attempting to delete user {UserId}", id);
        var result = await _usersService.DeleteUser(id);
        if (result == null)
        {
            _logger.LogWarning("user {UserId} not found for deletion", id);
            return NotFound();
        }
        return NoContent();
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> Signup([FromBody] CreateUserDto data)
    {
        ValidationResult validationResult = await _createUserValidator.ValidateAsync(data);
        if(!validationResult.IsValid)
        {
            _logger.LogWarning("invalid data for adding a user");
            return BadRequest();
        }
        _logger.LogInformation("adding new user {Gender} {FirstName} {Email}", data.Gender, data.FirstName, data.Email);
        try
        {
            var result = await _usersService.AddUser(data);
            _logger.LogInformation("user {UserId} created successfully", result.Id);
            return CreatedAtAction(nameof(GetUser), new {id = result.Id}, result);
        }catch(BadHttpRequestException ex)
        {
            _logger.LogWarning(ex, "invalid data for adding a user");
            return BadRequest();
        }
    }
}