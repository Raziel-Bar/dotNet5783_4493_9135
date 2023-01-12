
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Dal;

public static class XmlTools
{
    const string S_DIR = @"..\xml\";
   static XElement dalConfig = XElement.Load(@"..\xml\dal-config.xml");
    static XmlTools()
    {
        if (!Directory.Exists(S_DIR)) Directory.CreateDirectory(S_DIR);
    }

    #region Extension Functions EMPTY
    #endregion

    #region Save\Load with XElement 

    public static void saveListToXMLElment(XElement rootElem, string entity)
    {
        string filePath = $"{S_DIR + entity}";
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

    public static void SaveListToXMLSerializer<T>(List<T> list, string rootName, string entity)
    {
        string filePath = $"{S_DIR + entity}.xml";
        try
        {
            using FileStream file = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None);

            //using XmlWriter xmlWriter = XmlWriter.Create(file, new XmlWriterSettings { Indent = true });

            XmlSerializer serializer = new(list.GetType());

            serializer.Serialize(file, list);
        }
        catch (Exception ex)
        {
            throw new DO.XMLFileLoadCreateException(filePath, ex);
        }
    }

    public static List<T> LoadListFromXMLSerializer<T>(string entity)
    {
        string filePath = $"{S_DIR + entity}.xml";
        try
        {
            if (!File.Exists(filePath)) return  new();

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

    #region xmlConvertor

    internal static XElement itemToXelement<Item>(Item item, string name)
    {
        IEnumerable<PropertyInfo> items = item!.GetType().GetProperties();

        IEnumerable<XElement> xElements = from propInfo in items
                                          select new XElement(propInfo.Name, propInfo.GetValue(item)!.ToString());

        return new XElement(name, xElements);
    }

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

    internal static IEnumerable<Item> xelementToItems<Item>(XElement xElement) where Item : new()
    {
        return from element in xElement.Elements()
               select xelementToItem<Item>(element);
    }

    internal static  int getRunNumber(string entityName)
    {
        
        return int.Parse(dalConfig.Element(entityName)!.Value!) + 1;
    }

    internal static void saveRunNumber(string entityName, int runNumber)
    {
           dalConfig.Element(entityName)!.Value = runNumber.ToString();
           saveListToXMLElment(dalConfig, entityName);
    }
    #endregion



}
