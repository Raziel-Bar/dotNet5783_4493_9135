namespace DO;
/// <summary>
/// Here we have all the enums that are needed for our entities
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

public enum ENTITIES { EXIT = 0, PRODUCT, ORDER, ORDER_ITEM };
public  enum OPTIONS { EXIT = 0, ADD = 1, UPDATE, DELETE, SEARCH, GET_LIST, ORDER_ITEM_LIST, ORDER_ITEM_SEARCH };