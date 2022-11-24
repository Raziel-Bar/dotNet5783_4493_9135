namespace BO;
/// <summary>
/// BL exceptions. Here we gather all possible errors that can happen when using the BL
/// </summary>

/// <summary>
/// Exception for cases when an object from a given entity is not found in the database
/// </summary>

public class NotFoundInDalException : Exception
{
    public NotFoundInDalException(string type, Exception ex) : base($"ERROR dotNet5783_BL_ERROR_01: {type} not found in data", ex) { }
}

/// <summary>
/// Exception for cases when an object from a given entity already exists in the database when it's not suppose to
/// </summary>
public class AlreadyExistInDalException : Exception
{
    public AlreadyExistInDalException(string type, Exception ex) : base($"ERROR dotNet5783_BL_ERROR_02: {type} already exists in data", ex) { }
}

/// <summary>
/// Exception for cases when an object's input data (ID and stuff) is either invalid or incorrect when compared to the data in Dal
/// </summary>
public class InvalidDataException : Exception
{
    public InvalidDataException(string type) : base($"ERROR dotNet5783_BL_ERROR_03: {type}'s data is invalid") { }
}

public class RemoveProductThatIsInOrdersException : Exception
{
    public RemoveProductThatIsInOrdersException() : base("ERROR dotNet5783_BL_ERROR_04: Cannot remove a product from data because there are orders that includes that product!") { }
}

public class StockNotEnoughtOrEmptyException : Exception // לא חושב שצריך מאחר ויש סטטוס של לא זמין
{
    public StockNotEnoughtOrEmptyException() : base("ERROR dotNet5783_BL_ERROR_05: Not enough amount in stock") { }
}
public class ProductNotFoundInCartException : Exception
{
    public ProductNotFoundInCartException() : base("ERROR dotNet5783_BL_ERROR_06: Product is not in cart") { }
}

public class DateException : Exception
{
    public DateException() : base($"ERROR dotNet5783_BL_ERROR_07: your order's tracking dates conflicts your actions") { }
}

public class UnexpectedException : Exception
{
    public UnexpectedException() : base("ALERT UNEXPECTED ECEPTION!!!!") { }
}
