using System.Xml.Linq;
using System.Reflection;

public static class XmlConverter
{
    public static XDocument ObjectToXml<T>(T obj)
    {
        Type type = typeof(T);
        XElement objectElement = new XElement(type.Name);

        PropertyInfo[] properties = type.GetProperties();

        foreach (PropertyInfo property in properties)
        {
            object value = property.GetValue(obj);
            XElement propertyElement = new XElement(property.Name, value);
            objectElement.Add(propertyElement);
        }

        XDocument doc = new XDocument(objectElement);

        return doc;
    }
}

