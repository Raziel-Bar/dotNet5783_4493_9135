using DO;
namespace DalApi;

public class NotFound : System.Exception
{
    public void throwNotFound(string type) { throw new Exception($"ERROR dotNet5783_01: The {type} Does not exist"); } // is this right?
}

public class AlreadyExist
{
    public void throwAlreadyExist(string type) { throw new Exception($"ERROR dotNet5783_02: The {type} Already exists"); } // is this right?
}

