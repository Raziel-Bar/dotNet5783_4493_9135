using DO;

namespace BO;
public class OrderTracking// @@@@@@@@@@@@@@@@@@@@@@@
{
    /// <summary>
    /// The order's unique ID (like "barcode")
    /// </summary>
    public int ID { get; set; }
    /// <summary>
    /// The current status of an existing order
    /// </summary>
    public ORDER_STATUS Status { get; set; }

    /// <summary>
    /// a tracking journey. list of dates that tracks the order's process
    /// </summary>
    public List<Tuple<DateTime, ORDER_STATUS>> Tracker { get; set; }

    public override string ToString() => $@"

      ";
}
