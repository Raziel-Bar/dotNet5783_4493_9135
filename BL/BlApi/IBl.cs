
namespace BlApi;
/// <summary>
/// MAIN Interface. Combines all Ientities together when a property for each Ientity is made
/// </summary>
public interface IBl
{
    /// <summary>
    /// Icart getter. gives access to all ICart's methods
    /// </summary>
    public ICart Cart { get; }
    /// <summary>
    /// IOrder getter. gives access to all IOrder's methods
    /// </summary>
    public IOrder Order { get; }
    /// <summary>
    /// IProduct getter. gives access to all IProduct's methods
    /// </summary>
    public IProduct Product { get; }
}
