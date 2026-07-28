namespace bidding_zone_api.Dtos.Request;

public class UpdateItemWinnerDto
{
    public Guid? WinnerUserId {get;set;} = Guid.Empty;

}