
using BlApi;

namespace BlImplementation;

/// <summary>
/// Main BL Class. Inherits IBAL thus makes a combined object off all entities's methods implementations
/// </summary>
sealed public class Bl : IBl
{

    /// <summary>
    ///  ICart. has all cart methods
    /// </summary
    public ICart Cart => new Cart();


    /// <summary>
    ///  IOrder. has all order methods
    /// </summary>
    public IOrder Order =>  new Order();



    /// <summary>
    ///  IProduct. has all Product methods
    /// </summary>
    public IProduct Product =>  new Product();
}
