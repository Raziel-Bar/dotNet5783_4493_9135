﻿namespace BO;

/// <summary>
/// Presents an Order's status - current state in time.
/// </summary>
public class OrderTracking
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
    public List<Tuple<DateTime, ORDER_STATUS>> Tracker { get; set; } // '?' missing?
    public override string ToString() => $@"
        Order Number: {ID}
        Status: {Status}
        Tracking journey: 
            {string.Join(Environment.NewLine, Tracker)}"; // needed? if yes - NEEDS TESTING!!!!!!!!!!!!!!!!!!!!!
}