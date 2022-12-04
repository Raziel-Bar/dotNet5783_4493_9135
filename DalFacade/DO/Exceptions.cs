namespace DO;
/// <summary>
/// DAL exceptions. Here we gather all possible errors that can happen when accessing the DAL
/// </summary>

/// <summary>
/// Exception for cases when an object from a given entity is not found in the database
/// </summary>
[Serializable]
public class NotFoundException : Exception
{
    public NotFoundException(string type) : base($"ERROR dotNet5783_DAL_ERROR_01: {type} not found") { }
}

/// <summary>
/// Exception for cases when an object from a given entity already exists in the database when it's not suppose to
/// </summary>
[Serializable]
public class AlreadyExistException : Exception
{
    public AlreadyExistException(string type) : base($"ERROR dotNet5783_DAL_ERROR_02:{type} already exists") { }
}