using System.Collections;
using System.Net;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("elencoLocali")]
public class Locali
{
    [XmlArray("CollezioneLocali")]
    [XmlArrayItem("Item")]
    public List<Locale> CollezioneLocali = new List<Locale>();

    public static Locali Load(string url)
    {
        WebClient client = new WebClient();

        string data = Encoding.Default.GetString(client.DownloadData(url));

        Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(data));

        StreamReader reader = new StreamReader(stream);

        XmlSerializer ser = new XmlSerializer(typeof(Locali));

        Locali CollezioneLocali = ser.Deserialize(reader) as Locali;

        reader.Close();

        return CollezioneLocali;
    }
}
