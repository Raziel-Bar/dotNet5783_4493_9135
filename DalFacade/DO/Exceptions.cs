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

/// <summary>
/// Exception for cases of failure to load the dal-config.xml or parts of it
/// </summary>
[Serializable]
public class DalConfigException : Exception
{
    public DalConfigException(string msg) : base(msg) { }
    public DalConfigException(string msg, Exception ex) : base(msg, ex) { }
}

/// <summary>
/// Exception for cases when something that shouldn't happen happened. FOR DEVELOPERS USE ONLY
/// </summary>
[Serializable]
public class UnexpectedException : Exception
{
    public UnexpectedException() : base("ALERT UNEXPECTED ECEPTION!!!!") { }
}
