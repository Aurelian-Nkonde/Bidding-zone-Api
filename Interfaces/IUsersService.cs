using bidding_zone_api.Dtos.Request;
using bidding_zone_api.Dtos.Response;

namespace  bidding_zone_api.Interfaces;

public interface IUsersService
{
    Task<UserResponseDto?> GetUser(Guid id);
    Task<IEnumerable<UserResponseDto>> GetUsers();
    Task<UserResponseDto> AddUser(CreateUserDto data);
    Task<UserResponseDto?> UpdateUser(UpdateUserDto data, Guid id);
    Task<UserResponseDto?> UpdateAddress(AddressDto data, Guid id);
    Task<UserResponseDto?> Login(string email, string password);
}