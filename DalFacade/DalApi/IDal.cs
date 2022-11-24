
namespace DalApi;

/// <summary>
/// MAIN Interface. Combines all Ientities together when a property for each Ientity is made
/// </summary>
public interface IDal
{
    /// <summary>
    /// IProduct getter. gives access to all IProduct's methods
    /// </summary>
    IProduct Product { get; }
    /// <summary>
    /// IOrder getter. gives access to all IOrder's methods
    /// </summary>
    IOrder Order { get; }
    /// <summary>
    /// IOrderItem getter. gives access to all IOrderItem's methods
    /// </summary>
    IOrderItem OrderItem { get; }
}
