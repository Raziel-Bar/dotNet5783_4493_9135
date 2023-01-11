using DalApi;
using System.Diagnostics;

namespace Dal;

sealed internal class DalXml : IDal
{
    public static IDal Instance { get; } = new DalXml();

    private DalXml() { }

    public IProduct Product { get; } = new Dal.DalProduct();

    public IOrder Order { get; } = new Dal.DalOrder();

    public IOrderItem OrderItem { get; } = new Dal.DalOrderItem();
}
