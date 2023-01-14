using DalApi;
using System.Diagnostics;

namespace Dal;

/// <summary>
/// Sealed class that implements IDal interface, providing data access layer functionality for XML files.
/// </summary>
sealed internal class DalXml : IDal
{
    /// <summary>
    /// The singleton instance of the DalXml class
    /// </summary>
    public static IDal Instance { get; } = new DalXml();

    /// <summary>
    /// private constructor to ensure only one instance of the class is created
    /// </summary>
    private DalXml() { }

    /// <summary>
    /// Property that gets the DalProduct object 
    /// </summary>
    public IProduct Product { get; } = new Dal.DalProduct();

    /// <summary>
    /// Property that gets the DalOrder object
    /// </summary>
    public IOrder Order { get; } = new Dal.DalOrder();

    /// <summary>
    /// Property that gets the DalOrderItem object
    /// </summary>
    public IOrderItem OrderItem { get; } = new Dal.DalOrderItem();
}
