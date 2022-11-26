namespace BO;
/// <summary>
/// Here we have all the enums that are needed for our Bl
/// </summary>

/// <summary>
/// The category of the product, in our case, wine - which winery is the product from.
/// Needed in Bl too for the BO entities
/// </summary>
public enum WINERYS
{
    GOLAN,
    DALTON,
    TEPERBERG,
    CARMEL,
    BARKAN,

    //================================================
    // for stage 2 we will stay with 5 categories//===
    //================================================

    /// <summary>
    /// ELSE is the category for non wine bottle products.
    /// </summary>
    // ELSE
}

/// <summary>
/// enum for an order's status
/// </summary>
public enum ORDER_STATUS { PENDING, SHIPPED, DELIVERED }
/// <summary>
/// Enum for a product's stock's state. Either empty stock => Product's Unavailable, or not empty => Product's available
/// </summary>
public enum Available { Available, Unavailable };