using DalApi;
using System.Security.Principal;

namespace Dal;

/// <summary>
/// Main Dal Class. Inherits Idal thus makes a combined object off all entities's methods implementations
/// The class creates 1 object only at most (Singleton)
/// </summary>
 internal sealed class DalList : IDal
{
    public static IDal Instance { get; } = new DalList();

    private DalList() { }

    /// <summary>
    /// DalProduct entity - "son" of IProduct. has all Product methods
    /// </summary>
    public IProduct Product { get; } = new DalProduct();

    /// <summary>
    /// DalOrder entity - "son" of IOrder. has all order methods
    /// </summary>
    public IOrder Order { get; } = new DalOrder();

    /// <summary>
    /// DalOrderItem entity - "son" of IOrderItem. has all OrderItem methods
    /// </summary>
    public IOrderItem OrderItem { get; } = new DalOrderItem();
}
