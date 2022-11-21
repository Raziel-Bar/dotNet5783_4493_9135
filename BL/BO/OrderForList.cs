namespace BO;
public class OrderForList
{
    /// <summary>
    /// The order's unique ID (like "barcode")
    /// </summary>
    public int ID { get; set; }
    /// <summary>
    /// The name of the customer who placed the order.
    /// </summary>
    public string? CustomerName { get; set; }
    /// <summary>
    /// The current status of an existing order
    /// </summary>
    public ORDER_STATUS Status { get; set; }
    /// <summary>
    /// The amount of items in the order
    /// </summary>
    public int AmountOfItems { get; set; }
    /// <summary>
    /// The final price to pay for the whole order
    /// </summary>
    public double TotalPrice { get; set; }

    public override string ToString() //////@@@@@@@@@@@@@@
    {
        string toString = $@"
        Order ID: {ID}
        Customer name: {CustomerName}  Status: {Status}";

        toString += $@"
        Amount of items: {AmountOfItems}
        Total: {TotalPrice}";
        return toString;
    }
    //public override string ToString()
    //{
    //    string toString = $@"
    //    Order ID: {ID}
    //    Customer name: {CustomerName}";
    //    switch (Status)
    //    {
    //        case ORDER_STATUS.PENDING:
    //            toString += @"
    //    Status: Pending";
    //            break;
    //        case ORDER_STATUS.SHIPPED:
    //            toString += @"
    //    Status: Shipped";
    //            break;
    //        case ORDER_STATUS.DELIVERED:
    //            toString += @"
    //    Status: Delivered";
    //            break;
    //        default:
    //            break;
    //    }
    //    toString += $@"
    //    Amount of items: {AmountOfItems}
    //    Total: {TotalPrice}";
    //    return toString;
    //}
}
