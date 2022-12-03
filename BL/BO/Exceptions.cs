namespace BO;
/// <summary>
/// BL exceptions. Here we gather all possible errors that can happen when using the BL
/// </summary>

/// <summary>
/// Exception for cases when an object from a given entity is not found in the database
/// </summary>

[Serializable]
public class NotFoundInDalException : Exception
{
    public NotFoundInDalException(string type, Exception ex) : base($"ERROR dotNet5783_BL_ERROR_01: {type} not found in data", ex) { }
    public NotFoundInDalException(string type) : base($"ERROR dotNet5783_BL_ERROR_01: {type} not found in data") { } /////////

}
/// <summary>
/// Exception for cases when an object from a given entity already exists in the database when it's not suppose to
/// </summary>
[Serializable]
public class AlreadyExistInDalException : Exception
{
    public AlreadyExistInDalException(string type, Exception ex) : base($"ERROR dotNet5783_BL_ERROR_02: {type} already exists in data", ex) { }
}
/// <summary>
/// Exception for cases when an object's input data (ID and stuff) is either invalid or incorrect when compared to the data in Dal
/// </summary>

[Serializable]
public class InvalidDataException : Exception
{
    public InvalidDataException(string type) : base($"ERROR dotNet5783_BL_ERROR_03: {type}'s data is invalid") { }
}
/// <summary>
/// Exception for cases when we try to remove a product that exists in an existing order
/// </summary>

[Serializable]
public class RemoveProductThatIsInOrdersException : Exception
{
    public RemoveProductThatIsInOrdersException() : base("ERROR dotNet5783_BL_ERROR_04: Cannot remove a product from data because there are orders that includes that product!") { }
}
/// <summary>
/// Exception for cases when we try to add more items to order or cart and we dont have enough in stock or when stock is actually empty
/// </summary>

[Serializable]
public class StockNotEnoughtOrEmptyException : Exception
{
    public StockNotEnoughtOrEmptyException() : base("ERROR dotNet5783_BL_ERROR_05: Not enough amount in stock") { }
}
/// <summary>
/// Exception for cases when we try to update an item in cart that does not exist in the cart
/// </summary>
[Serializable]
public class ProductNotFoundInCartException : Exception
{
    public ProductNotFoundInCartException() : base("ERROR dotNet5783_BL_ERROR_06: Product is not in cart") { }
}
/// <summary>
/// Exception for cases when we try to update or set dates that conflicts logical time line. includes description of the problem
/// </summary>

[Serializable]
public class DateException : Exception
{
    public DateException(string description) : base($"ERROR dotNet5783_BL_ERROR_07: {description}") { }
}
/// <summary>
/// Exception for cases when we tried to confirm a cart with non updated items.
/// </summary>

[Serializable]
public class ChangeInCartItemsDetailsException : Exception
{
    public ChangeInCartItemsDetailsException() : base(@"ERROR dotNet5783_BL_ERROR_08: Failed to confirm order because 1 or more items in the order's details has been changed.
Please note that your cart has been UPDATED! Please recheck your cart before you wish to confirm!") { }
}
/// <summary>
/// Exception for cases when something that shouldn't happen happened. FOR DEVELOPERS USE ONLY
/// </summary>
[Serializable]
public class UnexpectedException : Exception
{
    public UnexpectedException() : base("ALERT UNEXPECTED ECEPTION!!!!") { }
}
