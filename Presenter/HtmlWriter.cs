using System.Xml;

namespace XmlCompare
{
    internal static class HtmlWriter
    {
        public static void Write(this XmlWriter writer, string text, string format)
        {
            WriteSimpleTag(writer, format, text);
        }

        public static void WriteSimpleTag(this XmlWriter writer,string tag, string text)
        {
            writer.WriteStartElement(tag);
            writer.WriteString(text);
            writer.WriteEndElement();
        }
    }
}
