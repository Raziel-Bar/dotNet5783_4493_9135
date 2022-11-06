using DO;

namespace Dal;
public class DalOrder
{
    public int addNewOrder(Order newOrder)
    {
        if (Array.Exists(DataSource._orders, p => p.ID == newOrder.ID))
            throw new Exception("The order you want to add already exists");

        newOrder.ID = DataSource.Config.getRunNumberOrderID;

        DataSource._orders[DataSource.Config._orderCounter++] = newOrder;

        return newOrder.ID;
    }

    public  Order searchOrder(int orderId)
    {
        int index = Array.FindIndex(DataSource._orders, p => p.ID == orderId);

        if (index == -1)
            throw new Exception("The order you want is not exist");

        return DataSource._orders[index];
    }

    public Order[] listOfOrders()
    {
        Order[] newOrderlist = new Order[DataSource.Config._orderCounter];
        for (int i = 0; i < newOrderlist.Length; ++i)
            newOrderlist[i] = DataSource._orders[i];
        return newOrderlist;
    }
    
    public void deleteOrder(int orderId)
    {
        int index = Array.FindIndex(DataSource._orders, p => p.ID == orderId);

        if (index == -1)
            throw new Exception("The order you want to delete does not exist");

        int last = (--DataSource.Config._orderCounter);

        DataSource._orders[index] = DataSource._orders[last];

        Array.Clear(DataSource._orders, last, last);
    }

    public void updateOrder(Order orderUpdate)
    {
        int index = Array.FindIndex(DataSource._orders, p => p.ID == orderUpdate.ID);

        if (index == -1)
            throw new Exception("The order you want to update is not exist");

        DataSource._orders[index] = orderUpdate;
    }
}
//public  List<Order> listOfOrders()
//{
//    List<Order> ordersList = new List<Order>();
//    foreach (Order order in DataSource._orders)
//        ordersList.Add(order);

//    return ordersList;
//}