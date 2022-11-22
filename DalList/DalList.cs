using DalApi;
namespace Dal;

/// <summary>
/// Main Dal Class. Inherits Idal thus makes a combined object off all entities's methods implementations
/// </summary>
sealed public class DalList : IDal
{
    /// <summary>
    /// DalProduct entity - "son" of IProduct. has all Product methods
    /// </summary>
    public IProduct Product => new DalProduct();
    /// <summary>
    /// DalOrder entity - "son" of IOrder. has all order methods
    /// </summary>
    public IOrder Order => new DalOrder();
    /// <summary>
    /// DalOrderItem entity - "son" of IOrderItem. has all OrderItem methods
    /// </summary>
    public IOrderItem OrderItem => new DalOrderItem();
}
