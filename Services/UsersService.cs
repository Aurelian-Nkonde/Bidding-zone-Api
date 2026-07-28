using bidding_zone_api.AppContext;
using bidding_zone_api.Dtos.Request;
using bidding_zone_api.Dtos.Response;
using bidding_zone_api.Interfaces;
using bidding_zone_api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using Serilog;

namespace bidding_zone_api.Services;

public class UsersService: IUsersService
{
    private readonly ILogger<User> _logger;
    private readonly AppDbContext _context;
    public UsersService(AppDbContext appDbContenxt, ILogger<User> logger)
    {
        _context = appDbContenxt;
        _logger = logger;
    }

    public async Task<UserResponseDto?> GetUser(Guid id)
    {
        _logger.LogInformation("looking for a user with this id: {id}", id);
        var user = await _context.Users.FindAsync(id);
        if(user == null)
        {
            _logger.LogWarning("user with id: {id} is not found", id);
            return null;
        }
        return FormatUserResponse(user);
    }


    public async Task<IEnumerable<UserResponseDto>> GetUsers()
    {
        _logger.LogInformation("fetching all users");
        var users = await _context.Users.ToListAsync();
        return users.Select(user => FormatUserResponse(user));
    }


    public async Task<UserResponseDto> AddUser(CreateUserDto data)
    {
        _logger.LogInformation("checking if user exist");
        var userExist = await _context.Users.AnyAsync(user => user.Email == data.Email);
        if(userExist)
        {
            _logger.LogWarning("user already exist");
            throw new BadHttpRequestException("User already exist");
        }
        var hashedPasssword = BCrypt.Net.BCrypt.HashPassword(data.Password);
        Address? address = null;
        if(data.Address != null)
        {
            if(!Enum.TryParse<Province>(data.Address.Province, ignoreCase: true, out var parsedProvince))
            {
                throw new  BadHttpRequestException("Error parsing province");
            }
            address = new Address
            {
                HouseNumber = data.Address.HouseNumber,
                Surbub = data.Address.Surbub,
                StreetName = data.Address.StreetName,
                Province = parsedProvince
            };
        }
        if(!Enum.TryParse<Gender>(data.Gender, ignoreCase: true, out var parsedGender))
        {
            throw new BadHttpRequestException("Error parsing gender");
        }
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = data.FirstName,
            LastName = data.LastName,
            Gender = parsedGender,
            Email = data.Email,
            Address = address,
            Role = Role.User,
            Password = hashedPasssword,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        _logger.LogInformation("new user is successfully added");
        return FormatUserResponse(user);
    }


    public async Task<UserResponseDto?> UpdateUser(UpdateUserDto data, Guid id)
    {
        _logger.LogInformation("looking for a user with id: {id}", id);
        var user = await _context.Users.FindAsync(id);
        if(user == null)
        {
            _logger.LogWarning("user with id: {id} is not found", id);
            return null;
        }
        Address? address = null;
        if(data.Address != null)
        {
            if(!Enum.TryParse<Province>(data.Address.Province, ignoreCase: true, out var parsedProvince))
            {
                throw new BadHttpRequestException("Invalid province value provided");
            }
            address = new Address
            {
                HouseNumber = data.Address.HouseNumber,
                StreetName = data.Address.StreetName,
                Surbub = data.Address.Surbub,
                Province = parsedProvince
            };
        }
        _logger.LogInformation("Updating user data");

        if(!Enum.TryParse<Gender>(data.Gender, ignoreCase: true, out var parsedGender))
        {
            throw new BadHttpRequestException("Invalid value for gender provided");
        }

        user.FirstName = data.FirstName;
        user.LastName = data.LastName;
        user.Email = data.Email;
        user.Gender = parsedGender;
        user.Address = address;
        await _context.SaveChangesAsync();
        _logger.LogInformation("User data is updated");
        return FormatUserResponse(user);
    }


    public  async Task<UserResponseDto?> UpdateAddress(AddressDto data, Guid id)
    {
        _logger.LogInformation("Looking for a user with id: {id}", id);
        var user = await _context.Users.FindAsync(id);
        if(user == null)
        {
            _logger.LogWarning("user with id: {id} is not found", id);
            return null;
        }

        if(!Enum.TryParse<Province>(data.Province, ignoreCase: true, out var parsedProvince))
        {
            _logger.LogWarning("Parsed province value is not supported");
            throw new BadHttpRequestException("Invalid value parsed");
        }

        user.Address ??= new Address();
        user.Address.HouseNumber = data.HouseNumber;
        user.Address.StreetName = data.StreetName;
        user.Address.Surbub = data.Surbub;
        user.Address.Province = parsedProvince;
        await _context.SaveChangesAsync();
        _logger.LogInformation("user address is updated");
        return FormatUserResponse(user);
    }


    public async Task<UserResponseDto?> Login(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
        if(user == null) return null;
        if(!BCrypt.Net.BCrypt.Verify(password, user.Password)) return null;
        return FormatUserResponse(user);
    }


    public UserResponseDto FormatUserResponse(User value)
    {
        return new UserResponseDto
        {
            Id = value.Id,
            Email = value.Email,
            FirstName = value.FirstName,
            LastName = value.LastName,
            Gender = value.Gender,
            Address = value.Address,
            Role = value.Role
        };
    }
}