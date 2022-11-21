namespace DO;
/// <summary>
/// Here we have all the enums that are needed for our Dal
/// </summary>


/// <summary>
/// The category of the product, in our case, wine - which winery is the product from.
/// </summary>
/// 
public enum WINERYS
{
    GOLAN,
    DALTON,
    TEPERBERG,
    CARMEL,
    BARKAN,

    //================================================
    // for stage 1 we will stay with 5 categories//===
    //================================================

    /// <summary>
    /// ELSE is the category for non wine bottle products.
    /// </summary>
    // ELSE
}

/// <summary>
/// enum for the first menu in DalTest\Program.cs
/// </summary>
public enum ENTITIES { EXIT, PRODUCT, ORDER, ORDER_ITEM, CART, };

/// <summary>
/// enum for the second menu for each selction from menu_1 in DalTest\Program.cs
/// </summary>
public enum OPTIONS { EXIT, ADD, UPDATE, DELETE, SEARCH, GET_LIST, ORDER_ITEM_LIST, ORDER_ITEM_SEARCH };