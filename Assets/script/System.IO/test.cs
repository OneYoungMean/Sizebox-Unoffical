using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace KeyboardSwitcher
{
    public static class XmlPersister
    {
        public static void SerializeObject<T>(T serializableObject, string fileName)//OYM：序列化
        {
            if (serializableObject == null) { return; }

            try
            {
                XmlDocument xmlDocument = new XmlDocument();//OYM：创建一个xml
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());//OYM：实例化

                using (MemoryStream stream = new MemoryStream())//OYM：建立流
                {
                    serializer.Serialize(stream, serializableObject);//OYM：序列化
                    stream.Position = 0;//OYM：位置，具体意义不明
                    xmlDocument.Load(stream);//OYM：加载流
                    xmlDocument.Save(fileName);//OYM：保存为blabla文件
                    stream.Close();//OYM：关闭
                }
            }
            catch (Exception)
            {
                throw;//OYM：报错
            }
        }

        public static T DeSerializeObject<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return default(T); }

            T objectOut = default(T);//OYM：default是默认值的意思

            try
            {
                string attributeXml = string.Empty;

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);
                string xmlString = xmlDocument.OuterXml;//OYM：输出xml，这里是干啥用的？

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(T);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (T)serializer.Deserialize(reader);//OYM：反序列化
                        reader.Close();
                    }

                    read.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return objectOut;
        }
    }
}