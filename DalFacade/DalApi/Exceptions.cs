using DO;
namespace DalApi;


//public class ExceptionFunctionThrow
//{
//    private const string exceptionNotFound = "you search for either does not exist or the order does not.";

//    private const string exceptionAlreadyExist = "you search for either does not exist or the order does not.";

//    public static void NotFoundException(string entityName, string message = "")
//    {
//        throw new NotFoundException($"The item " + entityName + " " + (message == string.Empty ? exceptionNotFound : message));
//    }

//    public static void AlreadyExistException(string entityName, string message = "")
//    {
//        throw new AlreadyExistException($"The item " + entityName + " " + (message == string.Empty ? exceptionAlreadyExist : message));
//    }

//}



public class NotFoundException: Exception
{
    public  NotFoundException(string message) : base(message) { }
    //public void throwNotFound(string type) { throw new Exception($"ERROR dotNet5783_01: The {type} Does not exist"); } // is this right?
}

public class AlreadyExistException: Exception
{
    public AlreadyExistException(string message) : base(message) { }

    public void throwAlreadyExist(string type) { throw new Exception($"ERROR dotNet5783_02: The {type} Already exists"); } // is this right?
}

