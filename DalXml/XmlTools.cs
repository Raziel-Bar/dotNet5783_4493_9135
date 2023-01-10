using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace Dal;

public static class XmlTools
{
    const string S_DIR = @"..\xml\";

    static XmlTools()
    {
        if (!Directory.Exists(S_DIR)) Directory.CreateDirectory(S_DIR);
    }

    #region Extension Functions EMPTY
    #endregion

    #region Save\Load with XElement 



    #endregion

    #region Save\Load with Xml Serialier

    public static void SaveListToXMLSerializer<T>(List<T> list, string entity, string rootName)
    {
        string filePath = $"{S_DIR + entity}.xml";
        try
        {
            using FileStream file = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None);

            using XmlWriter xmlWriter = XmlWriter.Create(file, new XmlWriterSettings { Indent = true });

            XmlSerializer serializer = new(typeof(List<T>), new XmlRootAttribute(rootName));

            serializer.Serialize(xmlWriter, list);
        }
        catch (Exception ex)
        {
            throw new DO.XMLFileLoadCreateException(filePath, ex);
        }
    }

    public static List<T> LoadListToXMLSerializer<T>(string entity)
    {
        string filePath = $"{S_DIR + entity}.xml";
        try
        {
            if (!File.Exists(filePath)) return new();

            using FileStream file = new(filePath, FileMode.Open);

            XmlSerializer serializer = new(typeof(List<T>));

            return serializer.Deserialize(file) as List<T> ?? new();
        }
        catch (Exception ex)
        {
            throw new DO.XMLFileLoadCreateException(filePath, ex);
        }
    }

    #endregion

}
