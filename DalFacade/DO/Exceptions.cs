using DO;
using System;

namespace DO;



public class NotFoundException : Exception
{
    public NotFoundException(string type) : base($"{type} was not found") { }
    //public void throwNotFound(string type) { throw new Exception($"ERROR dotNet5783_01: The {type} Does not exist"); } // is this right?
}

public class AlreadyExistException : Exception
{
    public AlreadyExistException(string type) : base($"{type} already exists") { }

    //public void throwAlreadyExist(string type) { throw new Exception($"ERROR dotNet5783_02: The {type} Already exists"); } // is this right?
}

