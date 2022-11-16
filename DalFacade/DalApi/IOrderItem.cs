using DO;
namespace DalApi;

public interface IOrderItem : ICrud<OrderItem> 
{
    OrderItem GetByOrderAndProcuctIDs(int orderID, int productID);
    IEnumerable<OrderItem> GetItemsInOrder(int orderID);
}
