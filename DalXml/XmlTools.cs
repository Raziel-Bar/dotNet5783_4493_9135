using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Dal;

public static class XmlTools
{
    const string S_DIR = @"..\xml\";

    static XmlTools()
    {
        if (!Directory.Exists(S_DIR)) Directory.CreateDirectory(S_DIR);
    }

    #region Save\Load with XElement 

    /// <summary>
    /// Saves a list represented by a XElement to an XML file with a specific name located in the S_DIR directory.
    /// </summary>
    /// <param name="rootElem">The XElement representing the list to save.</param>
    /// <param name="entity">The name of the file to save the list to.</param>
    public static void saveListToXMLElment(XElement rootElem, string entity)
    {
        string filePath = $"{S_DIR + entity}.xml";// .xml
        try
        {
            rootElem.Save(filePath);
        }
        catch (Exception ex)
        {
            // DO.XMLFileLoadCreateException(filePath, $"fail to creat xml file: {filePath}", ex)
            throw new Exception($"fail to creat xml file: {filePath}", ex);
        }
    }

    /// <summary>
    /// Loads a list from an XML file with a specific name located in the S_DIR directory, represented by a XElement
    /// </summary>
    /// <param name="entity">The name of the file to load the list from.</param>
    /// <returns>The XElement representing the list read from the XML file.</returns>
    public static XElement LoadListFromXMLElment(string entity)
    {
        string filePath = $"{S_DIR + entity}.xml";
        try
        {
            if (File.Exists(filePath))
                return XElement.Load(filePath);
            XElement rootElem = new(entity);
            rootElem.Save(filePath);
            return rootElem;
        }
        catch (Exception ex)
        {
            // DO.XMLFileLoadCreateException(filePath, $"fail to load xml file: {filePath}", ex)
            throw new Exception($"fail to load xml file: {filePath}", ex);
        }

    }
    #endregion

    #region Save\Load with Xml Serialier

    /// <summary>
    /// Saves a list of objects to an XML file using the XmlSerializer class, using the provided root name and file name.
    /// </summary>
    /// <typeparam name="T">The type of the objects in the list.</typeparam>
    /// <param name="list">The list of objects to save.</param>
    /// <param name="rootName">The root name of the XML file.</param>
    /// <param name="entity">The name of the file to save the list to, located in the S_DIR directory.</param>
    public static void SaveListToXMLSerializer<T>(List<T> list, string rootName, string entity)
    {
        string filePath = $"{S_DIR + entity}.xml";
        try
        {
            using FileStream file = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None);



            XmlSerializer serializer = new(list.GetType());

            serializer.Serialize(file, list);
        }
        catch (Exception ex)
        {
            throw new DO.XMLFileLoadCreateException(filePath, ex);
        }
    }

    /// <summary>
    /// Loads a list of objects from an XML file using the XmlSerializer class, using the provided file name.
    /// </summary>
    /// <typeparam name="T">The type of the objects in the list.</typeparam>
    /// <param name="entity">The name of the file to load the list from, located in the S_DIR directory.</param>
    /// <returns>The list of objects read from the XML file.</returns>
    public static List<T> LoadListFromXMLSerializer<T>(string entity)
    {
        string filePath = $"{S_DIR + entity}.xml";
        try
        {
            if (!File.Exists(filePath)) return new();

            using FileStream file = new(filePath, FileMode.Open);

            XmlSerializer serializer = new(typeof(List<T>));
            List<T> list = new List<T>();
            list = (List<T>)serializer.Deserialize(file);

            return list;
        }
        catch (Exception ex)
        {
            throw new DO.XMLFileLoadCreateException(filePath, ex);
        }
    }


    #endregion

    //================================= Bonus ==================================

    #region xmlConvertor

    /// <summary>
    /// Converts an object of any type to an XElement with a specified name
    /// </summary>
    /// <typeparam name="Item">The type of the object to convert</typeparam>
    /// <param name="item">The object to convert</param>
    /// <param name="name">The name of the resulting XElement</param>
    /// <returns>An XElement representation of the input object</returns>
    internal static XElement itemToXelement<Item>(Item item, string name)
    {
        IEnumerable<PropertyInfo> items = item!.GetType().GetProperties();

        IEnumerable<XElement> xElements = from propInfo in items
                                          select new XElement(propInfo.Name, propInfo.GetValue(item)!.ToString());

        return new XElement(name, xElements);
    }

    /// <summary>
    /// Converts an XElement to an object of a specified type
    /// </summary>
    /// <typeparam name="Item">The type of the object to create</typeparam>
    /// <param name="xElement">The XElement to convert</param>
    /// <returns>An object of the specified type, populated with the data from the XElement</returns>
    internal static Item xelementToItem<Item>(XElement xElement) where Item : new()
    {
        Item newItem = new Item();
        object obj = newItem;
        IEnumerable<XElement> elements = xElement.Elements();

        Dictionary<string, PropertyInfo> items = obj.GetType().GetProperties().ToDictionary(k => k.Name, v => v);

        foreach (var item in elements)
        {
            items[item.Name.LocalName].SetValue(obj, Convert.ChangeType(item.Value, items[item.Name.LocalName].PropertyType));
        }

        return (Item)obj;
    }

    /// <summary>
    /// Converts an XElement with multiple child elements to a collection of objects of a specified type
    /// </summary>
    /// <typeparam name="Item">The type of the objects to create</typeparam>
    /// <param name="xElement">The XElement to convert</param>
    /// <returns>A collection of objects of the specified type, populated with the data from the child elements of the XElement</returns>
    internal static IEnumerable<Item> xelementToItems<Item>(XElement xElement) where Item : new()
    {
        return from element in xElement.Elements()
               select xelementToItem<Item>(element);
    }

    #endregion

    #region RunNumber

    /// <summary>
    /// Retrieves the run number of a specific entity from an XML file "config.xml" located in the S_DIR directory.
    /// </summary>
    /// <param name="entityName">The name of the entity to retrieve the run number for.</param>
    /// <returns>The run number of the specified entity.</returns>
    internal static int getRunNumber(string entityName)
    {

        XElement dalConfig = XElement.Load(S_DIR + "config.xml");

        return int.Parse(dalConfig.Element(entityName)!.Value!) + 1;
    }

    /// <summary>
    /// Saves the run number of a specific entity to an XML file "config.xml" located in the S_DIR directory.
    /// </summary>
    /// <param name="entityName">The name of the entity to save the run number for.</param>
    /// <param name="runNumber">The run number to save for the specified entity.</param>
    internal static void saveRunNumber(string entityName, int runNumber)
    {

        XElement dalConfig = XElement.Load(S_DIR + "config.xml");

        dalConfig.Element(entityName)!.Value = runNumber.ToString();

        dalConfig.Save(S_DIR + "config.xml");
    }

    #endregion


}
