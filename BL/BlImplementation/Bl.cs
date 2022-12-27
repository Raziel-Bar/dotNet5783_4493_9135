
using BlApi;

namespace BlImplementation;

/// <summary>
/// Main BL Class. Inherits IBAL thus makes a combined object off all entities's methods implementations
/// </summary>
sealed internal class Bl : IBl
{
    /// <summary>
    ///  ICart. has all cart methods
    /// </summary
    public ICart Cart { get; } = new Cart();
    /// <summary>
    ///  IOrder. has all order methods
    /// </summary>
    public IOrder Order { get; } = new Order();
    /// <summary>
    ///  IProduct. has all Product methods
    /// </summary>
    public IProduct Product { get; } = new Product();
}
