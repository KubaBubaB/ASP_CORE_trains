namespace DotNET_ASP_App.DTOs.Responses;

public class GetAllWalletsBalance
{
    public class WalletBalance
    {
        public int Id { get; set; }
        public double Balance { get; set; }
    }
    
    public List<WalletBalance> Balances { get; set; }
}