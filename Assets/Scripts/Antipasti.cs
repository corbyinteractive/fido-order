using System.Collections;
using System.Net;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

[XmlRoot("menuRistorante")]
public class Antipasti 
{
    [XmlArray("Area")]
    [XmlArrayItem("Item")]
    public List<Item> CollezioneAntipasti = new List<Item>();

    public static Antipasti Load(string url)
    {

        WebClient client = new WebClient();
        
        //TestSerializzation();

        string data = Encoding.Default.GetString(client.DownloadData(url));

        Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(data));

        StreamReader reader = new StreamReader(stream);

        XmlSerializer ser = new XmlSerializer(typeof(Antipasti));

        Antipasti CollezioneAntipasti = ser.Deserialize(reader) as Antipasti;

        reader.Close();




        return CollezioneAntipasti;

       
    }

    static void TestSerializzation()
    {
        Stream stream = new MemoryStream();

        Menu menu = new Menu();
        menu.Piatti.Add(new Piatto { Nome = "Penne", Tipo = "Primi", Prezzo = 1 });
        menu.Piatti.Add(new Piatto { Nome = "Spaghettu", Tipo = "Primi", Prezzo = 12});
        menu.Piatti.Add(new Piatto { Nome = "Penne1", Tipo = "Primi", Prezzo = 2});
        menu.Piatti.Add(new Piatto { Nome = "Penne2", Tipo = "Secondi", Prezzo = 1 });
        menu.Piatti.Add(new Piatto { Nome = "Penne3", Tipo = "Secondi", Prezzo = 1 });
        menu.Tipologia.Add("Primi");
        menu.Tipologia.Add("Secondi");
        menu.Tipologia.Add("Dolci");
        string json = JsonConvert.SerializeObject(menu);
        XmlSerializer ser = new XmlSerializer(typeof(Menu));
        File.WriteAllText(Path.Combine(Application.dataPath,"savefile.json"), json);
        ser.Serialize(stream, menu);

        stream.Position = 0;
        var menu1 = ser.Deserialize(stream) as Menu;

        foreach(var ordine in menu1.Tipologia)
        {
            var piatti = menu1.GetPiattiByTipo(ordine);
            foreach(var p in piatti)
            {
               
            }
        }
        var dolci = menu1.GetPiattiByTipo("Dolci");

    }
}




