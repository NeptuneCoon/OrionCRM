using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Orion.CRM.Toolkit
{
    public class XmlSerializeHelper
    {
        /// <summary>
        /// 将对象序列化为xml对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns></returns>
        public static string XmlSerialize(object obj, Encoding encoding)
        {
            using (MemoryStream stream = new MemoryStream()) {
                XmlSerializer serializer = new XmlSerializer(obj.GetType());

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.NewLineChars = "\r\n";
                settings.Encoding = encoding;
                settings.IndentChars = " ";

                using (XmlWriter writer = XmlWriter.Create(stream, settings)) {
                    serializer.Serialize(writer, obj);
                    writer.Dispose();
                }

                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream, encoding)) {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 从XML字符串中反序列化对象
        /// </summary>
        /// <typeparam name="T">结果对象类型</typeparam>
        /// <param name="xml">XML字符串</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>反序列化得到的对象</returns>
        public static T XmlDeserialize<T>(string xml, Encoding encoding)
        {
            if (string.IsNullOrEmpty(xml)) throw new ArgumentNullException("xml");
            if (encoding == null) throw new ArgumentNullException("encoding");

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(encoding.GetBytes(xml))) {
                using (StreamReader sr = new StreamReader(ms, encoding)) {
                    return (T)serializer.Deserialize(sr);
                }
            }
        }
    }
}
