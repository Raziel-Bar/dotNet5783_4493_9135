using DO;

namespace Dal;
static internal class DalDataSource
{
    //readonly??
    private static int productCounter = 0;
    private static int orderCounter = 0;
    private static int orderItemCOunter = 0;
    internal static Product[] _products = new Product[50];
    private static void addProduct(Product product)
    {
        _products[productCounter++] = product;
    }
    internal static Order[] _orders = new Order[100];
    private static void addOrder(Order order)
    {
        _orders[orderCounter++] = order;
    }
    internal static OrderItem[] _orderItems = new OrderItem[200];
    private static void addOrderItem(OrderItem orderItem)
    {
        _orderItems[orderItemCOunter++] = orderItem;
    }
}
