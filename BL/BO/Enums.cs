namespace BO;
/// <summary>
/// Here we have all the enums that are needed for our Bl
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
    // for stage 2 we will stay with 5 categories//===
    //================================================

    /// <summary>
    /// ELSE is the category for non wine bottle products.
    /// </summary>
    // ELSE
}

/// <summary> 8888888888888888888888888888888888888888888888888888888888
/// enum for the first menu in DalTest\Program.cs
/// </summary>
public enum ENTITIES { EXIT, PRODUCT, ORDER, CART }; // to be check if used! ^&^&^&^&^&^&^&^&^&^&^&^&^

/// <summary> 8888888888888888888888888888888888888888888888888888888888
/// enum for the second menu for each selction from menu_1 in DalTest\Program.cs
/// </summary>
public enum OPTIONS { EXIT = 0, ADD = 1, UPDATE, DELETE, SEARCH, GET_LIST, ORDER_ITEM_LIST, ORDER_ITEM_SEARCH }; // to be upddated when BlTest is built &*&*&*&*&*&*&*&*&*&*&*&*&

/// <summary>
/// enum for an order's status
/// </summary>
public enum ORDER_STATUS { PENDING = 0, SHIPPED, DELIVERED };