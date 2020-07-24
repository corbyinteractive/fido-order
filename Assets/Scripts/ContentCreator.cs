using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_2017_4_OR_NEWER
using UnityEngine.Networking;
using Cubequad;



    public class ContentCreator : MonoBehaviour
    {
    public static ContentCreator C8;
    public GameObject CanvasComanda;
    public GameObject CanvasProdotto;
    public GameObject PanelFidelity;
    public bool comunicazioneCompilata;
    public static bool loadFromCache;
    public static string fidelity;
    public static string comunicazioniInserite;
    private string nomePiatto;
    private string prezzo;
    private string myString;
    private string md5;
    private string token;
    private string NomeLocaleLong;
    public static bool registrazioneObbligatoria;
    public Text ConfirmExitText;
    public GameObject ConfirmExit;
    public GameObject DatatipoPanel;
    public GameObject PinPanel;
    public GameObject ButtonComanda;
    public GameObject PannelloRegistrazione;
    public GameObject SelectionBtnRoot;
    public GameObject PanelPosizione;
    public GameObject GruppoPosizione;
    public InputField ComunicazioneCliente;
    public InputField PosizioneManuale;
    public static bool proceed;
    public static bool hideTotal;
    //public Transform SingVoceMenu;
    [HideInInspector]
    public Menu_Ristorante menu;
    public CallWebService.MenuSoftwareSolutions menuSS;
    //public List<Piatto> Piatti { get; set; } = new List<Piatto>();
    public static string indirizzoSped;
    public static string cittaSped;
    public static string PvSped;
    public static string ComuneSped;
    public static string NoteSpediz;
    public static string PosizioneInterna;
    private string urlMenu;
    public static string Key, secret;
    public Text loadtext;
    public Text ControlloPosizione;
    private bool CodeUniversal;
    public static string currentMode; 
    public static bool ClienteRegMin;
    public static bool ClienteRegistrato;
    public static bool ClienteTKy;
    public static string OrarioConsegna;
    public static DatiCliente datiCliente = new DatiCliente();
    public static string Restaurant;
    public static string Utenza;// =;
    public static string seat;
    public static string sala;
    public static int Db = 1;
    public static int daysToStore;
    public static bool Ordering;
    public static bool Tky;
    public static bool Dlv;
    public static float limitePezzi;
    public static float limiteValore;
    public static float minimoValore;
    public static string PinPag;// = "1234";
    public static bool PinPagOn;
    public static bool consInt;
    public static bool pagaConsegnaAbilitata;
    public static bool Base = true;
    public static string urlLocali;  //="https://safemenu.altervista.org/wp-content/MolRa/Locale.xml";
    //public string url = "https://safemenu.altervista.org/wp-content/"+Restaurant+"/Menu.xml";
    //public string url = "https://safemenu.altervista.org/wp-content/MolRa/Menu.xml";
    private string preUrl1 = "https://safemenu.altervista.org/wp-content/";
    private string preUrl = "https://aruba.streetshot.it/pinapp/get.php?item="; //MolRa&token=k6dduaqb1&md5=039e8a640a0d15b99641b7e3a2e08340
    public Image cellImage;
    public Text welcome;
    public GameObject WelcomePanel;
    public GameObject SelectionPanel;
    public GameObject Loader;
       
        
        c_Data LogoData;
    private void Awake()
    {
        C8 = this;
    }
    void Start()
        {
        
        DataService C = new DataService("DatiCliente.db");
        if (false)
        {
            
            ContentCreator.ClienteRegistrato = C.CheckDatiCliente();
            if (ContentCreator.ClienteRegistrato)
            {
                ContentCreator.ClienteTKy = C.GetTipoDati();
                ContentCreator.ClienteRegMin = C.GetRegMin();
                IEnumerable<DatiCliente> Dci = C.GetDatiCliente();
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

            }
        }
        


        //Update();
        if ((seat == "" && sala == "")||(seat == "X" || sala == "X")||(seat == null && sala == null))
        {
            SelectionPanel.gameObject.SetActive(true);
        }
        if (Db == 0)
        {
            //loadtext.text = ("Db= " + Db + "Utenza =" + Utenza + "Restaurant =" + Restaurant);
            if (urlLocali == null)
            {
                urlLocali ="https://safemenu.altervista.org/wp-content/MolRa/Locale.xml";
                Restaurant = "MolRa";
                seat = "T0";
                Db = 0;
            }
            Locali lc = Locali.Load(urlLocali);
            foreach (Locale locale in lc.CollezioneLocali)
            {
                NomeLocaleLong = locale.s_nomeLocale;
                hideTotal = locale.s_HideTotal;
                if (hideTotal)
                {
                    Color col = new Color(0, 0, 0, 0);
                    Comanda.C5.Totale.color = col;
                    Comanda.C5.text_Totale.color = col;
                }
                else
                {
                    Color col = new Color(0, 0, 0, 1);
                    Comanda.C5.Totale.color = col;
                    Comanda.C5.text_Totale.color = col;
                }
                ColorController.ColoreSfondi = hexToColor(locale.s_ColorTheme);
                ColorController.ColoreRiquadri = hexToColor(locale.s_ColorFrame);
                print(locale.s_Landing);
                print(locale.s_ShortName);
                print(locale.s_Base);
                if (locale.s_ShortName == Restaurant)
                {
                    welcome.text = locale.s_Landing;
                    //Key = "AaK1Dsz3z2xqhYs2JaHOXQpxUxxdO3ZYihg0IuebKVxUwU2xh4vS0BThdSz4rG4uSi-QU6gMXFy_QsNE";// locale.s_CID;
                    //secret = "EBjXni131N_Ci2795OXE2qc11ndEncytbujnbHTK2Ez4o9HszD0ra2Rw8F_O03pluGxYWiZ46jwwsYiY"; // locale.s_SKE;
                    Key = locale.s_CID;
                    secret = locale.s_SKE;
                    consInt = locale.s_consegnaInterna;
                    Tky = locale.s_TakeAway;
                    Dlv = locale.s_Delivery;
                    Base = locale.s_Base;
                    bool MD = locale.s_EnableMD;
                    SelectionBtnRoot.transform.GetChild(0).gameObject.SetActive(MD);
                    SelectionBtnRoot.transform.GetChild(1).gameObject.SetActive(Base);
                    SelectionBtnRoot.transform.GetChild(2).gameObject.SetActive(Tky);
                    SelectionBtnRoot.transform.GetChild(3).gameObject.SetActive(Dlv);
                    SelectionBtnRoot.transform.GetChild(4).gameObject.SetActive(consInt);
                    if (seat == "Tky" || seat == "Dlv")
                    {
                        welcome.text = locale.s_LandingGen;
                        CodeUniversal = true;
                        Comanda.C5.Invia_Button_Text.text = "Vai al pagamento";
                        Comanda.C5.ComandaText.text = "procedere con il pagamento?";
                        Comanda.Pay = true;
                        currentMode = seat;
                    }
                }
            }

           

            const string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789"; //add the characters you want
            int charAmount = UnityEngine.Random.Range(8, 12); //set those to the minimum and maximum length of your string
            for (int i = 0; i < charAmount; i++)
            {
                myString += glyphs[UnityEngine.Random.Range(0, glyphs.Length)];
            }
            token = myString;
            myString += "GKtsh!hdisa's^nlaoek";
            md5 = GetHash(myString);
            if (Restaurant != null)
            {
                urlMenu = preUrl1 + Restaurant + "/Menu4.json";
                //urlMenu = preUrl + Restaurant + "&token="+token+"&md5="+md5;
                LogoData = new c_Data()
                {
                    imageUrl = preUrl1 + Restaurant + "/Logo.png",
                    imageDimensions = new Vector2(643f, 227f)
                };
            }
            else
            {
                urlMenu = "https://aruba.streetshot.it/pinapp/get.php?item=MolRa.menu.json&token=k6dduaqb1&md5=039e8a640a0d15b99641b7e3a2e08340";
                LogoData = new c_Data()
                {
                    imageUrl = "https://aruba.streetshot.it/pinapp/get.php?item=MolRa.Logo.png&token=k6dduaqb1&md5=039e8a640a0d15b99641b7e3a2e08340",
                    imageDimensions = new Vector2(643f, 227f)
                };
            }
            SetData(LogoData);
            //ColorController.CController.CambiaColori();
            menu = Menu_Ristorante.LoadMenu(urlMenu, Db);
            Loader.gameObject.SetActive(false);
            if (!loadFromCache)
            AggiungiLocale();
        } else if (Db == 1)
        {
            //menuSS = CallWebService.LoadSSMenu(Utenza, Restaurant);
            CallWebService.LoadSSMenu(Restaurant, Utenza);

        }
    }
    public void AggiungiLocale()
    {
        DataService C = new DataService("DatiCliente.db");
        CoordinateLocali CL = new CoordinateLocali();
        CL.Db = Db;
        if (daysToStore != null && daysToStore >= 1)
        {
            CL.Exipiry = DataService.DateTimeToUnixTimestamp(DateTime.Today.AddDays(daysToStore));
        }
        else
        {
            CL.Exipiry = DataService.DateTimeToUnixTimestamp(DateTime.Now.AddHours(8));
        }
        CL.LongName = NomeLocaleLong;
        CL.Restaurant = Restaurant;
        CL.Utenza = Utenza;
        CL.Sala = sala;
        CL.Seat = seat;
        C.AddLocale(CL);
    }
    public void PopulateMenuAsync()
    {
        ColorController.ColoreSfondi = hexToColor(menuSS.ColorTheme);
        ColorController.ColoreRiquadri = hexToColor(menuSS.ColorFrame);
        NomeLocaleLong = menuSS.Denominazione;
        if (menuSS.LogoBase64Img.Length > 10)
        {
            string b64_string = menuSS.LogoBase64Img;
            b64_string = Regex.Replace(b64_string, @"^data:image\/[a-zA-Z]+;base64,", string.Empty);
            byte[] b64_bytes = System.Convert.FromBase64String(b64_string);

            var tex = new Texture2D(1, 1);
            tex.LoadImage(b64_bytes);
            if (tex != null)
            {
                var rec = new Rect(0, 0, tex.width, tex.height);
                cellImage.sprite = Sprite.Create(tex, rec, new Vector2(0, 1), 100);
                cellImage.preserveAspect = true;
            }
        }
        DataService ds = new DataService("DatiCliente.db");
        fidelity = ds.GetFidelity(Restaurant);
        if (fidelity == null)
            fidelity = "";
        if (fidelity.Length > 12)
        {
            fidelity.Remove(fidelity.Length - 1);
            QREncodeTest.QR.Encode(fidelity);
        }
        Locale locale = ClassConverter.localifromSS(menuSS);
        welcome.text = locale.s_Landing;
        if (locale.s_Landing.Length < 3 || locale.s_Landing == null)
            WelcomePanel.gameObject.SetActive(false);
        Key = locale.s_CID;
        if (locale.s_pinPag.Length > 3)
        {
            PinPagOn = true;
        }
        secret = locale.s_SKE;
        //Key = "AaK1Dsz3z2xqhYs2JaHOXQpxUxxdO3ZYihg0IuebKVxUwU2xh4vS0BThdSz4rG4uSi-QU6gMXFy_QsNE";// locale.s_CID;
        //secret = "EBjXni131N_Ci2795OXE2qc11ndEncytbujnbHTK2Ez4o9HszD0ra2Rw8F_O03pluGxYWiZ46jwwsYiY"; // locale.s_SKE;
        consInt = locale.s_consegnaInterna;
        pagaConsegnaAbilitata = locale.s_EnablePagaCons;
        Tky = locale.s_TakeAway;
        Dlv = locale.s_Delivery;
        Base = locale.s_Base;
        limitePezzi = locale.s_LimitPZ;
        limiteValore = locale.s_LimitVAL;
        minimoValore = locale.s_LimitMinVAL;
        PinPag = locale.s_pinPag;
        registrazioneObbligatoria = locale.s_RichiediDatiAccesso;
        bool MD = locale.s_EnableMD;
        SelectionBtnRoot.transform.GetChild(0).gameObject.SetActive(MD);
        SelectionBtnRoot.transform.GetChild(1).gameObject.SetActive(Base); //Base
        SelectionBtnRoot.transform.GetChild(2).gameObject.SetActive(Tky);
        SelectionBtnRoot.transform.GetChild(3).gameObject.SetActive(Dlv);
        SelectionBtnRoot.transform.GetChild(4).gameObject.SetActive(consInt);
        SelectionBtnRoot.transform.GetChild(5).gameObject.SetActive(fidelity.Length >= 12);
              
       
        if (locale.s_RichiediDatiAccesso)
        {
            if (!ClienteRegMin || !ClienteRegistrato || fidelity.Length <= 11)
            {
                registrazioneObbligatoria = (true);
                PannelloRegistrazione.SetActive(true);
            }
        }
        SelectionPanel.SetActive(true);
        string menuSs = ClassConverter.menufromSS(menuSS);
        menu = Menu_Ristorante.LoadMenu(menuSs, Db);

        ColorController.CController.CambiaColori();
        Loader.gameObject.SetActive(false);
        if (!loadFromCache)
        AggiungiLocale();
    }
    public void SetMode (int Mode)
    {
        switch (Mode)
        {
            case 0:
                currentMode = "Con";
                Comanda.C5.Invia_Button.gameObject.SetActive(false);
                Comanda.Pay = false;
                SelectionPanel.SetActive(false);
                break;
            case 1:
                currentMode = "Com";
                Comanda.C5.Invia_Button.gameObject.SetActive(true);
                Comanda.C5.Invia_Button_Text.text = "Invia al Cameriere";
                Comanda.C5.ComandaText.text = "Notifica inviata! " + '\n' +"" + '\n' + "Un cameriere arriverà breve per confermare le vostre scelte.";
                Comanda.Pay = false;
                SelectionPanel.SetActive(false);
                break;

            case 2:
                currentMode  = "Tky";
                Comanda.C5.Invia_Button.gameObject.SetActive(true);
                Comanda.C5.Invia_Button_Text.text = "Vai al pagamento";
                Comanda.C5.ComandaText.text = "procedere con il pagamento?";
                Comanda.Pay = true;
                SelectionPanel.SetActive(false);
                break;
            case 3:
                currentMode  = "Dlv";
                Comanda.C5.Invia_Button.gameObject.SetActive(true);
                Comanda.C5.Invia_Button_Text.text = "Vai al pagamento";
                Comanda.C5.ComandaText.text = "procedere con il pagamento?";
                Comanda.Pay = true;
                SelectionPanel.SetActive(false);
                break;
            case 4:
                currentMode = "Cin";
                Comanda.C5.Invia_Button.gameObject.SetActive(true);
                Comanda.C5.Invia_Button_Text.text = "Vai al pagamento";
                Comanda.C5.ComandaText.text = "procedere con il pagamento?";
                Comanda.Pay = true;
                SelectionPanel.SetActive(false);
                break;
            case 5:
                PanelFidelity.SetActive(true);
                break;

        }
    }
    public string GetHash(string usedString)
    { //Create a Hash to send to server
        MD5 md5 = MD5.Create();
        //byte[] bytes = Encoding.ASCII.GetBytes(usedString+secretKey);     // this wrong because can't receive korean character
        byte[] bytes = Encoding.UTF8.GetBytes(usedString + "GKtsh!hdisa's^nlaoek");
        byte[] hash = md5.ComputeHash(bytes);

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("x2"));
        }
        return sb.ToString();
    }
    public string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }
    public void ExitApplication()
    {
        if (PinPanel.active)
        {
            PinPanel.SetActive(false);
            return;
        }
        if (PanelFidelity.active)
        {
            PanelFidelity.SetActive(false);
        }
        if (DatatipoPanel.active)
        {
            Data_Tipo.DatTip.Resetpanel();
            DatatipoPanel.SetActive(false);
            return;
        }
        if (PanelPosizione.active)
        {
            PanelPosizione.SetActive(false);
        }
        if (CanvasComanda.active || CanvasProdotto.active || PannelloRegistrazione.active)
        {
            ButtonComanda.SetActive(true);
            CanvasComanda.SetActive(false);
            CanvasProdotto.SetActive(false);
            PannelloRegistrazione.SetActive(false);
        }
        else
        {
            if ((CodeUniversal == true && SelectionPanel.active) || (Db == 1 && SelectionPanel.active))
            {
                ConfirmExitText.text = "Uscire da " + Application.productName;
                ConfirmExit.gameObject.SetActive(true);
            }
            else if ((CodeUniversal == true && !SelectionPanel.active)||(Db == 1 && !SelectionPanel.active))
            {
                int pezzi;
                int.TryParse(Comanda.C5.BollinoText.text, out pezzi);
                if (pezzi >= 1)
                {
                    Comanda.C5.Question();
                    SelectionPanel.gameObject.SetActive(true);
                }
                else
                SelectionPanel.gameObject.SetActive(true);
                
            }
            
            if (CodeUniversal == false && Db != 1)
            {
                ConfirmExitText.text = "Uscire da " + Application.productName;
                ConfirmExit.gameObject.SetActive(true);
            }
        }
    }
    public void DoNotQuit()
    {
        ConfirmExit.gameObject.SetActive(false);
    }
    public void QuitApplication()
    {
        Application.Quit();
    }
    public void CheckPosizione()
    {
        if (PosizioneManuale.text.Length > 3 && GruppoPosizione.active)
        {
            seat = PosizioneManuale.text;
            sala = "";
            comunicazioniInserite = ComunicazioneCliente.text;
            comunicazioneCompilata = true;
            PanelPosizione.SetActive(false);
            
        }
        else if (!GruppoPosizione.active)
        {
            comunicazioniInserite = ComunicazioneCliente.text;
            comunicazioneCompilata = true;
            PanelPosizione.SetActive(false);
        }
        else
        {
            ControlloPosizione.gameObject.SetActive(true);
        }
    }
    public void HideWelcome()
    {
        WelcomePanel.SetActive(false);
    }
    #region IMAGE_PROCESSING
    private Coroutine _loadImageCoroutine;

        public void SetData(c_Data data)
        {
            _loadImageCoroutine = StartCoroutine(LoadRemoteImage(data));
        }

        public IEnumerator LoadRemoteImage(c_Data data)
        {
            string path = data.imageUrl;

            Texture2D texture = null;

            // Get the remote texture

#if UNITY_2017_4_OR_NEWER
            var webRequest = UnityWebRequestTexture.GetTexture(path);
            WWW w = new WWW(path);
       
            yield return webRequest.SendWebRequest();
            string str = "";
        var merda = webRequest.GetType();
            var dd = webRequest.GetResponseHeader(str);
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.LogError("Failed to download image [" + path + "]: " + webRequest.error);
            }
            else
            {
                texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
            }
#else
            WWW www = new WWW(path);
            yield return www;
            texture = www.texture;
#endif

            if (texture != null)
            {
                var rec = new Rect(0, 0, texture.width, texture.height);
                cellImage.sprite = Sprite.Create(texture, rec, new Vector2(0, 1),100);
                //cellImage.sprite = Sprite.Create(texture, new Rect(0, 0, data.imageDimensions.x, data.imageDimensions.y), new Vector2(0, 0));
                cellImage.preserveAspect = true;
            }
           /* else
            {
                ClearImage();
            }*/
        }
    /*
            public void ClearImage()
            {
                cellImage.sprite = defaultSprite;
            }
            // Update is called once per frame
            */
    public static Color hexToColor(string hex)
    {
        if (hex != null)
        {
            hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
            hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
            byte a = 255;//assume fully visible unless specified in hex
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            //Only use alpha if the string has enough characters
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return new Color32(r, g, b, a);
        }
        else
        {
            return new Color(0, 0, 0, 0);
        }
    }
    void Update()
        {
        if (proceed)
        {
            proceed = false;
            PopulateMenuAsync();
        }
           
        }
    #endregion
}


#endif