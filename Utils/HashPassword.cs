namespace bidding_zone_api.Utils;

public class HashPassword
{
    public static string HashingPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}