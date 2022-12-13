namespace BlImplementation;

/// <summary>
/// no object static class. made only for its methods that we will use in the other BlImplementation files
/// the class does a reflection copy from one object to another, copying all values that has same name and type in both objects
/// </summary>
/// <typeparam name="TFrom">The type of the object we copy from</typeparam>
/// <typeparam name="TTO">The type of the object we copy to</typeparam>
public static class PropertyCopier
{
    // dwarf for further development. please ignore //
    /*//    public static TTO CopyPropTo<TFrom, TTO>(this TFrom from, TTO to)
    //    {
    //        var fromProperties = from?.GetType().GetProperties();

    //        var toProperties = to?.GetType().GetProperties();

    //        Dictionary<string, PropertyInfo> properties = toProperties.ToDictionary(key => key.Name, value => value);

    //        if (fromProperties is null || toProperties is null)
    //        {
    //            throw new BO.UnexpectedException();
    //        }

    //        foreach (var fromProperty in fromProperties)
    //        {
    //            var value = fromProperty.GetValue(from);

    //            if (properties.ContainsKey(fromProperty.Name))
    //            {
    //                if (fromProperty.PropertyType == toProperty.PropertyType)
    //                    toProperty.SetValue(to, value);

    //                else
    //                {
    //                    Type typeNullAbleTo = Nullable.GetUnderlyingType(toProperty.PropertyType)!;

    //                    if (typeNullAbleTo is not null)
    //                        toProperty.SetValue(to, Enum.ToObject(typeNullAbleTo, value));
    //                }
    //            }
    //        }
    //        return to;
    //    }*/

    /// <summary>
    /// copier of simple objects
    /// </summary>
    /// <typeparam name="TFrom">The type of the object we copy from</typeparam>
    /// <typeparam name="TTO">The type of the object we copy yo</typeparam>
    /// <param name="from">The object we copy from</param>
    /// <param name="to">The object we copy to</param>
    /// <returns>the object we copied details to</returns>
    /// <exception cref="BO.UnexpectedException">exception for developers. NOT SUPPOSE TO HAPPEN</exception>
    public static TTO CopyPropTo<TFrom, TTO>(this TFrom from, TTO to)
    {
        var fromProperties = from?.GetType().GetProperties();

        var toProperties = to?.GetType().GetProperties();

        if (fromProperties is null || toProperties is null) throw new BO.UnexpectedException();

        foreach (var fromProperty in fromProperties)
        {
            var value = fromProperty.GetValue(from);

            foreach (var toProperty in toProperties)
            {
                if (fromProperty.Name == toProperty.Name) // we check names
                {

                    if (fromProperty.PropertyType == toProperty.PropertyType) // and types!
                        toProperty.SetValue(to, value);

                    else
                    {
                        Type typeNullAbleTo = Nullable.GetUnderlyingType(toProperty.PropertyType)!; // in case of enums conflicts (BO != DO)...

                        if (typeNullAbleTo is not null)
                            toProperty.SetValue(to, Enum.ToObject(typeNullAbleTo, value!));
                    }
                }
            }
        }

        return to;
    }

    /// <summary>
    /// object to STRUCT copier. takes care of structs as they are not exactly objects
    /// </summary>
    /// <typeparam name="TFrom">The type of the object we copy from</typeparam>
    /// <typeparam name="TTO">The type of the object we copy yo</typeparam>
    /// <param name="from">The object we copy from</param>
    /// <param name="to">The object we copy to</param>
    /// <returns>the struct we copied details to as object</returns>
    public static TTO CopyPropToStruct<TFrom, TTO>(this TFrom from, TTO to) where TTO : struct => (TTO)from.CopyPropTo(to as object);

    /// <summary>
    /// IEnumerable to IEnumerable copier
    /// </summary>
    /// <typeparam name="TFrom">The type of the object we copy from</typeparam>
    /// <typeparam name="TTO">The type of the object we copy yo</typeparam>
    /// <param name="froms">the collection we copy from</param>
    /// <returns>the new collection we made</returns>
    public static IEnumerable<TTO> CopyPropToList<TFrom, TTO>(this IEnumerable<TFrom> froms) where TTO : new() => from item in froms select item.CopyPropTo(new TTO());

}