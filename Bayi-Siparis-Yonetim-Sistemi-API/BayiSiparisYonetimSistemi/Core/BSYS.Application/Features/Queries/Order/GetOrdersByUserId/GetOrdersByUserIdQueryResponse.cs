
namespace BSYS.Application.Features.Queries.Order.GetOrdersByUserId;

public class GetOrdersByUserIdQueryResponse
{
    public int TotalOrderCount { get; set; }
    public object Orders { get; set; } 
    //   public List<OrderDetails> Orders { get; set; }
}
public class OrderDetails
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public string OrderCode { get; set; }
    public decimal TotalPrice { get; set; }
    public string UserName { get; set; }
    public bool Completed { get; set; }
}