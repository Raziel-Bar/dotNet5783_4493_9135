using DO;

namespace Dal;
public class DalOrderItem
{
    public int addNewOrderItem(OrderItem newOrderItem)
    {
        if (Array.Exists(DataSource._orderItems, p => p.OrderItemID == newOrderItem.OrderItemID))
            throw new Exception("The order you want to add is already exist");

        newOrderItem.OrderItemID = DataSource.Config.getRunNumberOrderItemID;

        DataSource._orderItems[DataSource.Config._orderItemCounter++] = newOrderItem;

        return newOrderItem.OrderItemID;
    }
    public OrderItem searchProductItem(int orderId, int productId)
    {
        int index = Array.FindIndex(DataSource._orderItems, p => p.OrderID == orderId && p.ProductID == productId);
        if (index == -1)
            throw new Exception("The item product you want is not exist");

        return DataSource._orderItems[index];
    }

    public OrderItem searchOrderItem(int orderItemId)
    {
        int index = Array.FindIndex(DataSource._orderItems, p => p.OrderItemID == orderItemId);
        if (index == -1)
            throw new Exception("The order item you want is not exist");

        return DataSource._orderItems[index];
    }

    public List<OrderItem> ordersList(int orderId)
    {
        List<OrderItem> list = new List<OrderItem>();
        foreach (OrderItem item in DataSource._orderItems)
        {
            if (item.OrderID == orderId)
                list.Add(item);
        }

        return list;
    }/////////////////////////

    public List<OrderItem> orderItemList()
    {
        List<OrderItem> list = new List<OrderItem>();

        foreach (OrderItem item in DataSource._orderItems)
            list.Add(item);

        return list;
    }////////////

    public void deleteOrderItem(int orderItemId)
    {
        int index = Array.FindIndex(DataSource._orderItems, p => p.OrderItemID == orderItemId);

        if (index == -1)
            throw new Exception("The order item you want to delete is not exist");

        int last = (--DataSource.Config._orderItemCounter);

        DataSource._orderItems[index] = DataSource._orderItems[last];

        Array.Clear(DataSource._orderItems, last, last);

    }

    public void UpdateOrderItem(OrderItem updateOrderItem)
    {
        int index = Array.FindIndex(DataSource._orderItems, p => p.OrderItemID == updateOrderItem.OrderItemID);

        if (index == -1)
            throw new Exception("The order item you want to update is not exist");

        DataSource._orderItems[index] = updateOrderItem;    
    }
}
