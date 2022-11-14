using DO;
namespace DalApi;

public interface IOrderItem : ICrud<OrderItem> 
{
    public OrderItem GetByOrderAndProcuctIDs(int orderID, int productID);
    public IEnumerable<OrderItem> PrintItemsInOrder(int orderID);
}
