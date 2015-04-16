
namespace WebTest
{

    public class BetterDictionary<TKey, TValue> : System.Collections.Generic.Dictionary<TKey, TValue>
    {



        protected cDictionary SerializationHelper;

        public void Store(ref TKey key, TValue value)
        {
            if (this.ContainsKey(key))
            {
                this[key] = value;
            }
            else
            {
                this.Add(key, value);
            }
        }
        // Store




        [System.Xml.Serialization.XmlRoot("dictionary")]
        public class cDictionary
        {

            [System.Xml.Serialization.XmlElement("keys")]

            public System.Collections.Generic.List<string> keys = new System.Collections.Generic.List<string>();
            [System.Xml.Serialization.XmlElement("values")]
            public System.Collections.Generic.List<string> values = new System.Collections.Generic.List<string>();
        }
        // cDictionary


        protected void Serialize()
        {
            foreach (System.Collections.Generic.KeyValuePair<TKey, TValue> ThisDictionaryEntry in this)
            {
                SerializationHelper = new cDictionary();
                SerializationHelper.keys.Add(ThisDictionaryEntry.Key.ToString());
                SerializationHelper.values.Add(ThisDictionaryEntry.Value.ToString());
            }
        } // Serialize


        public void SerializeToXML(ref string strFileNameAndPath)
        {
            Serialize();
            cDictionary ThisFacility = SerializationHelper;
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(cDictionary));

            System.Xml.XmlTextWriter xtwXMLtextWriter = null;
            try
            {
                //xtwXMLtextWriter = New System.Xml.XmlTextWriter(strFileNameAndPath, System.Text.Encoding.UTF8)
                //xtwXMLtextWriter = New XmlTextWriterIndentedStandaloneNo("C:\Users\stefan.steiger\Desktop\furniture.xml", System.Text.Encoding.UTF8)

                //xtwXMLtextWriter.Formatting = System.Xml.Formatting.Indented

                System.Xml.Serialization.XmlSerializerNamespaces ns = new System.Xml.Serialization.XmlSerializerNamespaces();
                ns.Add("", "");

                serializer.Serialize(System.Web.HttpContext.Current.Response.OutputStream, ThisFacility, ns);
                //serializer.Serialize(xtwXMLtextWriter, MyAppConfig)

                //xtwXMLtextWriter.Flush()
                //xtwXMLtextWriter.Close() 'Write the XML to file and close the writer
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("Encountered Exception in COR.XMLserialization.SerializeToXML()\r\nDetails:\r\n " + ex.Message);
            }


            //Dim swEncodingWriter As System.IO.StreamWriter = New System.IO.StreamWriter("C:\Users\stefan.steiger\Desktop\furniture.xml", False, System.Text.Encoding.UTF8)
            //serializer.Serialize(swEncodingWriter, MyAppConfig)
            //swEncodingWriter.Close()
            //swEncodingWriter.Dispose()
        } // SerializeToXML


    } // BetterDictionary

}
