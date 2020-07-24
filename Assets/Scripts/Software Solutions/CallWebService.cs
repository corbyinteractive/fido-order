using System.IO;
using System.Xml;
using System.Text;
using System;
using UnityEngine.Networking;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class CallWebService : MonoBehaviour
{
    public static CallWebService MMS;
    private bool done;
    private void Awake()
    {
        MMS = this;
    }
    public string SendTableOrder(List<Comanda.Ordine> ordine)
    {
        DataService ds = new DataService("DatiCliente.db");
        IEnumerable<DatiCliente> Dci = ds.GetDatiCliente();
        
        Example example = new Example();

        OrderInfo orderinfo = new OrderInfo();
        orderinfo.RifSala = ContentCreator.sala;
        orderinfo.RifPosizione = ContentCreator.seat;
        orderinfo.CittaConsegna = "";
        orderinfo.Comune = "";
        orderinfo.Orario = DateTime.Now.ToString("dd/MM/yyyy HH:MM:ss.")+"000";
        orderinfo.Comunicazione = ContentCreator.comunicazioniInserite;
        orderinfo.Denominazione = ContentCreator.datiCliente.Nome;
        orderinfo.Email = ContentCreator.datiCliente.Email;
        orderinfo.Indirizzo = "" ;
        orderinfo.IndirizzoConsegna = "";
        orderinfo.PV = ContentCreator.C8.menuSS.PV;
        orderinfo.TipoPagamento = "NO";
        orderinfo.Tel = ContentCreator.datiCliente.Tel;
        orderinfo.TipoOrdine = "Tav";
        orderinfo.IdTransazionePag="";
        example.orderInfo = orderinfo;
        List<ShoppingCart> shoppingcartlist = new List<ShoppingCart>();

        
        foreach (Comanda.Ordine Co in ordine )
        {
            ShoppingCart shoppingCart = new ShoppingCart();
            shoppingCart.qta = Co.Quantità;
            shoppingCart.Descr = Co.Nome;
            shoppingCart.idart = Co.IDart;
            shoppingCart.GUID = Co.Guid;
            shoppingCart.Prezzo = (decimal)Co.PrezzoBase;
            shoppingCart.Val = (decimal)Co.Prezzo;
            shoppingcartlist.Add(shoppingCart);
        }
        string a = JsonConvert.SerializeObject(orderinfo);
        string b = JsonConvert.SerializeObject(shoppingcartlist);
        return sendCart("AAA", ContentCreator.Restaurant, ContentCreator.Utenza, a, b);
    }


    public string SendTkyOrDlv(List<Comanda.Ordine> ordine, string orderType, string TipPag)
    {
        string position = "";
        DataService ds = new DataService("DatiCliente.db");
        IEnumerable<DatiCliente> Dci = ds.GetDatiCliente();

        Example example = new Example();

        OrderInfo orderinfo = new OrderInfo();
        orderinfo.RifSala = ContentCreator.sala; 
        orderinfo.RifPosizione = ContentCreator.seat;
        if (orderType == "Dlv")
        {
            orderinfo.CittaConsegna = ContentCreator.cittaSped;
            orderinfo.Comune = ContentCreator.ComuneSped;
            orderinfo.Comunicazione = ContentCreator.NoteSpediz;
            orderinfo.Pv = ContentCreator.PvSped;
            orderinfo.IndirizzoConsegna = ContentCreator.indirizzoSped;
        }
        if (orderType == "Cin")
        {
            if (Data_Tipo.DatTip.PosizInterna.readOnly)
            {
                orderinfo.RifSala = ContentCreator.sala;
                orderinfo.RifPosizione = ContentCreator.seat;
            }
            else
            {
                orderinfo.RifPosizione = ContentCreator.PosizioneInterna;
            }
            
        }
        //orderinfo.IndirizzoConsegna = position;
        foreach (DatiCliente D in Dci)
        {
            if (D.Id == 1)
            {
                ContentCreator.datiCliente.Nome = D.Nome;
                ContentCreator.datiCliente.Cognome = D.Cognome;
                ContentCreator.datiCliente.Tel = D.Tel;
                ContentCreator.datiCliente.Email = D.Email;
                ContentCreator.datiCliente.Indirizzo = D.Indirizzo;
                ContentCreator.datiCliente.Citta = D.Citta;
                ContentCreator.datiCliente.CAP = D.CAP;
            }
        }
        orderinfo.Orario = ContentCreator.OrarioConsegna;
        orderinfo.Denominazione = ContentCreator.datiCliente.Nome + " " + ContentCreator.datiCliente.Cognome;
        orderinfo.Email = ContentCreator.datiCliente.Email;
        orderinfo.PV = ContentCreator.C8.menuSS.PV;
        orderinfo.TipoPagamento = TipPag;
        orderinfo.Tel = ContentCreator.datiCliente.Tel;
        orderinfo.TipoOrdine = orderType;
        orderinfo.Comunicazione = ContentCreator.NoteSpediz;
        example.orderInfo = orderinfo;
        List<ShoppingCart> shoppingcartlist = new List<ShoppingCart>();


        foreach (Comanda.Ordine Co in ordine)
        {
            ShoppingCart shoppingCart = new ShoppingCart();
            shoppingCart.qta = Co.Quantità;
            shoppingCart.Descr = Co.Nome;
            shoppingCart.idart = Co.IDart;
            shoppingCart.GUID = Co.Guid;
            shoppingCart.Prezzo = (decimal)Co.PrezzoBase;
            shoppingCart.Val = (decimal)Co.Prezzo;
            shoppingcartlist.Add(shoppingCart);
        }
        string a = JsonConvert.SerializeObject(orderinfo);
        string b = JsonConvert.SerializeObject(shoppingcartlist);
        return sendCart("AAA", ContentCreator.Restaurant, ContentCreator.Utenza, a, b);
    }
    public string setCli(string key, string utenza,string TokenFB, string InfoCli, string device)
    {
        string setCliAction = "http://tempuri.org/setCli";
        string pKey = key; // "AAA";
        string pUtenza = utenza; // "ABCD";
        string ptfb = "0000-0000";
        string pInfoCli = InfoCli;
        string pdevice = device;
        string xmlsendCart = $"<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><setCli xmlns=\"http://tempuri.org/\"><key>{pKey}</key><utenza>{pUtenza}</utenza><tokenFB>{ptfb}</tokenFB><InfoCli>{pInfoCli}</InfoCli><device>{pdevice}</device></setCli></soap:Body></soap:Envelope>";
        string response = PostsetCli("https://gccloud.software-solution.it/WS/EXT_SVC.asmx", xmlsendCart, setCliAction);     
        return response;
    }
    public string sendCart(string key, string utenza, string posizione, string a, string b)
    {
        string sendCartAction = "http://tempuri.org/sendCart";
        string pKey = key; // "AAA";
        string pUtenza = utenza; // "ABCD";
        string pPosizione = posizione; // "TTTT";
        string xmlsendCart = $"<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><sendCart xmlns=\"http://tempuri.org/\"><key>{pKey}</key><utenza>{pUtenza}</utenza><OrderInfo>{a}</OrderInfo><OrderCart>{b}</OrderCart></sendCart></soap:Body></soap:Envelope>";
        string response = PostCart("https://gccloud.software-solution.it/WS/EXT_SVC.asmx", xmlsendCart, sendCartAction);        //StartCoroutine(PostRequest("https://gccloud.software-solution.it:1501/WS/EXT_SVC.asmx", xmlGetUtenza, getUtenzaAction));
        return response;
    }
    public List<string> CheckDisponibilita(string key, string utenza, string data, string pz)
    {
        string CheckDisponibilitaAction = "http://tempuri.org/CheckDisponibilita";
        string pKey = key = "AAA";
        string pUtenza = ContentCreator.Restaurant + " " +ContentCreator.Utenza;// = "ABCD";
        string pData = data; // "29/05/2020";
        string pPz = pz;// "8";
        string xmlsendCart = $"<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><CheckDisponibilita xmlns=\"http://tempuri.org/\"><key>{pKey}</key><utenza>{pUtenza}</utenza><data>{pData}</data><pz>{pPz}</pz></CheckDisponibilita></soap:Body></soap:Envelope>";
        List<string> response = GetOrari("https://gccloud.software-solution.it/WS/EXT_SVC.asmx", xmlsendCart, CheckDisponibilitaAction);        //StartCoroutine(PostRequest("https://gccloud.software-solution.it:1501/WS/EXT_SVC.asmx", xmlGetUtenza, getUtenzaAction));
        return response;
    }
     List<string> GetOrari(string url, string xml, string soapAction)
    {
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(xml);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "text/xml; charset=utf-8");
        uwr.SetRequestHeader("SOAPAction", soapAction);
        uwr.SendWebRequest();
        while (!uwr.isDone)
        {

        }
        if (uwr.isNetworkError)
        {

            return null;

        }
        else
        {


            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new StringReader(uwr.downloadHandler.text));
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("GU", "http://tempuri.org/");
            string xmlPathPattern = "//GU:CheckDisponibilitaResult";
            XmlNode respNode = xmlDoc.SelectSingleNode(xmlPathPattern, nsmgr);
            object Rs;
            if (respNode != null)
            {
                string b64 = respNode.InnerText;
                List<string> orari = JsonConvert.DeserializeObject<List<string>>(b64);
                
                return orari;
            }
            return null;
        }

    }
    string PostsetCli(string url, string xml, string soapAction)
    {
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(xml);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "text/xml; charset=utf-8");
        uwr.SetRequestHeader("SOAPAction", soapAction);
        uwr.SendWebRequest();
        while (!uwr.isDone)
        {

        }
        if (uwr.isNetworkError)
        {
            return "Ko";
        }
        else
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new StringReader(uwr.downloadHandler.text));
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("GU", "http://tempuri.org/");
            string xmlPathPattern = "//GU:setCliResult";
            XmlNode respNode = xmlDoc.SelectSingleNode(xmlPathPattern, nsmgr);
            object Rs;
            if (respNode != null)
            {
                /*string b64 = respNode.InnerText;
                 byte[] data = Convert.FromBase64String(b64);
                 string decodedString = Encoding.UTF8.GetString(data);

                 return decodedString;
                */
                return respNode.InnerText;
            }
            return "Ko";
        }

    }

    string PostCart(string url, string xml, string soapAction)
    {
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(xml);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "text/xml; charset=utf-8");
        uwr.SetRequestHeader("SOAPAction", soapAction);
        uwr.SendWebRequest();
        while (!uwr.isDone)
        {

        }
        if (uwr.isNetworkError)
        {           
            return "Ko";
        }
        else
        {           
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new StringReader(uwr.downloadHandler.text));
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("GU", "http://tempuri.org/");
            string xmlPathPattern = "//GU:sendCartResult";
            XmlNode respNode = xmlDoc.SelectSingleNode(xmlPathPattern, nsmgr);
            object Rs;
            if (respNode != null)
            {
                return respNode.InnerText;               
            }
            return "Ko";
        }
        
    }
    //public static MenuSoftwareSolutions LoadSSMenu(string utenza, string posizione)
    public static void LoadSSMenu(string utenza, string posizione)
    {

        CallWebService.MMS.getShortUtenzaInfo("AAA", utenza, posizione);
        //MenuSoftwareSolutions menu = CallWebService.MMS.getUtenzaInfo("AAA", utenza, posizione);
        

    }
    public static byte[] Compress(object data)

    {

        byte[] result = null;

        using (MemoryStream memory = new MemoryStream())

        {

            using (GZipStream zip = new GZipStream(memory, CompressionMode.Compress, true))

            {



                BinaryFormatter formatter = new BinaryFormatter();



                formatter.Serialize(zip, data);

            }



            result = memory.ToArray();

        }



        return result;

    }
    public static object Decompress(byte[] compressedData)

    {

        object result = null;

        using (MemoryStream memory = new MemoryStream())

        {

            memory.Write(compressedData, 0, compressedData.Length);

            memory.Position = 0L;



            using (GZipStream zip = new GZipStream(memory, CompressionMode.Decompress, true))

            {

                zip.Flush();



                BinaryFormatter formatter = new BinaryFormatter();



                result = formatter.Deserialize(zip);



            }

        }



        return result;

    }
    // Start is called before the first frame update
    /*public MenuSoftwareSolutions getUtenzaInfo(string key, string utenza, string posizione)
    {
        string getUtenzaAction = "http://tempuri.org/getUtenzaInfo";
        string pKey = key; // "AAA";
        string pUtenza = utenza; // "ABCD";
        string pPosizione = posizione; // "TTTT";
        string xmlGetUtenza = $"<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><getUtenzaInfo xmlns=\"http://tempuri.org/\"><key>{pKey}</key><utenza>{pUtenza}</utenza><posizione>{pPosizione}</posizione></getUtenzaInfo></soap:Body></soap:Envelope>";
        MenuSoftwareSolutions menu = PostRequest("http://gccloud.software-solution.it:1500/WS/EXT_SVC.asmx", xmlGetUtenza, getUtenzaAction);        //StartCoroutine(PostRequest("https://gccloud.software-solution.it:1501/WS/EXT_SVC.asmx", xmlGetUtenza, getUtenzaAction));
        return menu;
    }*/
    public void getUtenzaInfo(string key, string utenza, string posizione)
    {
        string getUtenzaAction = "http://tempuri.org/getUtenzaInfo";
        string pKey = key; // "AAA";
        string pUtenza = utenza+ " " + posizione; // "ABCD";
        string pPosizione = ""; // "TTTT";
        string xmlGetUtenza = $"<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><getUtenzaInfo xmlns=\"http://tempuri.org/\"><key>{pKey}</key><utenza>{pUtenza}</utenza><posizione>{pPosizione}</posizione></getUtenzaInfo></soap:Body></soap:Envelope>";
        StartCoroutine(PostRequest("https://gccloud.software-solution.it/WS/EXT_SVC.asmx", xmlGetUtenza, getUtenzaAction));        //StartCoroutine(PostRequest("https://gccloud.software-solution.it:1501/WS/EXT_SVC.asmx", xmlGetUtenza, getUtenzaAction));
        
    }
    public void getImage(string key, string utenza, string id)
    {
        string getImageAction = "http://tempuri.org/getImage";
        string pKey = key; // "AAA";
        string pUtenza = utenza;
        string pid = id;
        string xmlGetImage = $"<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><getImage xmlns=\"http://tempuri.org/\"><key>{pKey}</key><utenza>{pUtenza}</utenza><id>{pid}</id></getImage></soap:Body></soap:Envelope>";
        StartCoroutine(GetImageRequest("https://gccloud.software-solution.it/WS/EXT_SVC.asmx", xmlGetImage, getImageAction));
    }
    public void stopGetImage()
    {
        StopAllCoroutines();
    }
    IEnumerator GetImageRequest(string url, string xml, string soapAction)
    {
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(xml);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "text/xml; charset=utf-8");
        uwr.SetRequestHeader("SOAPAction", soapAction);
        yield return uwr.SendWebRequest();
        while (!uwr.isDone)
        {

        }
        if (uwr.isNetworkError)
        {

            ProductDescription.C4.LoadStatus = 2;

        }
        else
        {


            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new StringReader(uwr.downloadHandler.text));
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("GU", "http://tempuri.org/");
            string xmlPathPattern = "//GU:getImageResult";
            XmlNode respNode = xmlDoc.SelectSingleNode(xmlPathPattern, nsmgr);
            object Rs;
            if (respNode != null)
            {
                if (respNode.InnerText.ToString().Length > 50)
                {
                    ProductDescription.C4.ImageBytes = respNode.InnerText;
                    ProductDescription.C4.LoadStatus = 1;
                    
                }
                else
                {
                    ProductDescription.C4.LoadStatus = 2;
                }
            }
            else
            {
                ProductDescription.C4.LoadStatus = 2;
            }
        }

    }
    public void getShortUtenzaInfo(string key, string utenza, string posizione)
    {
        string getUtenzaAction = "http://tempuri.org/getShortUtenzaInfo";
        string pKey = key; // "AAA";
        string pUtenza = utenza + " " + posizione; // "ABCD";
        string pPosizione = ""; // "TTTT";
        string xmlGetUtenza = $"<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body><getShortUtenzaInfo xmlns=\"http://tempuri.org/\"><key>{pKey}</key><utenza>{pUtenza}</utenza><posizione>{pPosizione}</posizione></getShortUtenzaInfo></soap:Body></soap:Envelope>";
        StartCoroutine(PostSimpleRequest("https://gccloud.software-solution.it/WS/EXT_SVC.asmx", xmlGetUtenza, getUtenzaAction));        //StartCoroutine(PostRequest("https://gccloud.software-solution.it:1501/WS/EXT_SVC.asmx", xmlGetUtenza, getUtenzaAction));

    }
    void Start()
    {
      
        
    }
    IEnumerator PostSimpleRequest(string url, string xml, string soapAction)
    {
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(xml);
        //uwr.certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "text/xml; charset=utf-8");
        uwr.SetRequestHeader("SOAPAction", soapAction);
        MenuSoftwareSolutions menu = new MenuSoftwareSolutions();
        yield return uwr.SendWebRequest();
        /*while (!uwr.isDone)
        {
            
        }*/
        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            menu = JsonConvert.DeserializeObject<MenuSoftwareSolutions>("error", new JsonSerializerSettings
            {
                // it culture separator is ","..
                Culture = new System.Globalization.CultureInfo("it-IT")
            });
            foreach (ProductList v in menu.ProductList)
            {
                v.base64Img = Regex.Replace(v.base64Img, @"^data:image\/[a-zA-Z]+;base64,", string.Empty);

            }


        }
        else
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new StringReader(uwr.downloadHandler.text));
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("GU", "http://tempuri.org/");
            string xmlPathPattern = "//GU:getShortUtenzaInfoResult";
            XmlNode respNode = xmlDoc.SelectSingleNode(xmlPathPattern, nsmgr);
            if (respNode != null)
            {
                string b64 = respNode.InnerText;
                byte[] decodedBytes = Convert.FromBase64String(b64);
                var receivedString = Decompress(decodedBytes);

                menu = JsonConvert.DeserializeObject<MenuSoftwareSolutions>(receivedString.ToString(), new JsonSerializerSettings
                {
                    // it culture separator is ","..
                    Culture = new System.Globalization.CultureInfo("it-IT"),
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore

                });
                foreach (ProductList v in menu.ProductList)
                {
                    v.base64Img = Regex.Replace(v.base64Img, @"^data:image\/[a-zA-Z]+;base64,", string.Empty);

                }
                ContentCreator.C8.menuSS = menu;
                ContentCreator.proceed = true;

            }
        }
    }
    IEnumerator PostRequest(string url, string xml, string soapAction)
    {
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(xml);
        //uwr.certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "text/xml; charset=utf-8");
        uwr.SetRequestHeader("SOAPAction", soapAction);
        MenuSoftwareSolutions menu = new MenuSoftwareSolutions();
        yield return uwr.SendWebRequest();
        /*while (!uwr.isDone)
        {
            
        }*/
        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            menu = JsonConvert.DeserializeObject<MenuSoftwareSolutions>("error", new JsonSerializerSettings
            {
                // it culture separator is ","..
                Culture = new System.Globalization.CultureInfo("it-IT")
            });
            foreach (ProductList v in menu.ProductList)
            {
                v.base64Img = Regex.Replace(v.base64Img, @"^data:image\/[a-zA-Z]+;base64,", string.Empty);

            }
            
            
        }
        else
        {
            // Debug.Log("Received: " + uwr.downloadHandler.text);
            string totVal = "";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new StringReader(uwr.downloadHandler.text));
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("GU", "http://tempuri.org/");
            string xmlPathPattern = "//GU:getUtenzaInfoResult";
            XmlNode respNode = xmlDoc.SelectSingleNode(xmlPathPattern, nsmgr);
            if (respNode != null)
            {
                string b64 = respNode.InnerText;
                byte[] decodedBytes = Convert.FromBase64String(b64);
                var receivedString = Decompress(decodedBytes);
               
                menu = JsonConvert.DeserializeObject<MenuSoftwareSolutions>(receivedString.ToString(), new JsonSerializerSettings
                {
                    // it culture separator is ","..
                    Culture = new System.Globalization.CultureInfo("it-IT"),
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore

                });
                foreach (ProductList v in menu.ProductList)
                {
                    v.base64Img = Regex.Replace(v.base64Img, @"^data:image\/[a-zA-Z]+;base64,", string.Empty);

                }
                ContentCreator.C8.menuSS = menu;
                ContentCreator.proceed = true;
                
            }
        } 
    }
   
    public class OrderInfo
    {
        public string Denominazione { get; set; }//2 (Nome Cliente anche null) - 3 -
        public string IndirizzoConsegna { get; set; }//
        public string CittaConsegna { get; set; }//        
        public string Indirizzo { get; set; }//
        public string Comune { get; set; }//        
        public string Pv { get; set; }// provincia perchè V minuscola
        public string Email { get; set; }//
        public string Tel { get; set; }//
        public string Comunicazione { get; set; }//
        public string TipoOrdine { get; set; }//
        public string Orario { get; set; }//
        public int PzFood { get; set; }//2,3     
        public int PzBeverage { get; set; }//2,3
        public string RifSala { get; set; }//
        public string RifPosizione { get; set; }//2 ( se non popolato chieder num tav) - 3 -
        public string TipoPagamento { get; set; }
        public string IdTransazionePag { get; set; }
        public string PV { get; set; }// Punto Vendita
    }
    public class ShoppingCartList
    {
        public List<ShoppingCart> shoppingCart { get; set; }
    }

    public class ShoppingCart
    {
        public int idart { get; set; }
        public string GUID { get; set; }
        public string Descr { get; set; }
        public string base64Img { get; set; }
        public int qta { get; set; }
        public decimal Prezzo { get; set; }
        public decimal Val { get; set; }
    }

    public class Example
    {
        public OrderInfo orderInfo { get; set; }

    }
    public class Example2
    {

        public IList<ShoppingCart> shoppingCart { get; set; }
    }
    
    public class ListaAllergeni
    {
        public string id { get; set; }
        public string NomeAllergene { get; set; }
    }

    public class Lingue
    {
        public string Language { get; set; }
        public string Translation { get; set; }
    }

    public class ProductList
    {
        public string idart { get; set; }
        public string GUID { get; set; }
        public string Categoria { get; set; }
        public string CatColor { get; set; }
        public string Descr { get; set; }
        public string Descr2 { get; set; }
        public string Appunti { get; set; }
        public string base64Img { get; set; }
        public float Prezzo { get; set; }
        public string ProdType { get; set; }
        public IList<ListaAllergeni> ListaAllergeni { get; set; }
        public IList<Lingue> Lingue { get; set; }
    }

    public class MenuSoftwareSolutions
    {
        public string UtenzaCode { get; set; }
        public string PVCode { get; set; }
        public string PV { get; set; }
        public string RifPosizione { get; set; }
        public string Denominazione { get; set; }
        public string LogoBase64Img { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string MessaggioLanding { get; set; }
        public string MessaggioLandingDelivery { get; set; }
        public string RichiediDatiAccesso { get; set; }
        public string EnableMD { get; set; }                //Abilita Menu Digitale.
        public string EnableTKY { get; set; }
        public string EnableDLV { get; set; }
        public string EnableTAV { get; set; }               //Abilta Invio Cameriere.
        public string EnableConsegnaInterna { get; set; }
        public string EnablePagaCons { get; set; }
        public string PinPag { get; set; }
        public string EnablePP { get; set; }
        public string PPcodice { get; set; }
        public string PPchiave { get; set; }
        public string EnableNEXI { get; set; }
        public string NEXIalias { get; set; }
        public string NEXImac { get; set; }
        public string ColorTheme { get; set; }
        public string ColorFrame { get; set; }
        public string LimitPZ { get; set; }
        public string LimitVAL { get; set; }
        public string LimitMinPZ { get; set; }
        public string LimitMinVAL { get; set; }
        public IList<ProductList> ProductList { get; set; }
        public string EX1 { get; set; }
        public string EX2 { get; set; }
        public string EX3 { get; set; }
        public string EX4 { get; set; }
        public string EX5 { get; set; }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
