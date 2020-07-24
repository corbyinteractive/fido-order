using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class Comanda : MonoBehaviour
{
    public static Comanda C5;
    public Text Totale;
    public Text text_Totale;
    public Text LimitText;
    public Image textContainer;
    private float totale;
    public Button Invia_Button;
    public Text Invia_Button_Text;
    public GameObject PanelOrdineConclusoOK;
    public GameObject PanelOrdineConclusoKO;
    public GameObject Paga_Buttons;
    public GameObject CanvasComanda;
    public GameObject CanvasConferma;
    public GameObject CanvasDati;
    public GameObject CanvasConsegna;
    public GameObject ComandaQuestion;
    public Button Ok_Button;
    public GameObject Shop;
    public GameObject Shopsetting;
    public Text ComandaText;
    public Text Confirmation_Text;
    public static bool Pay = false;
    public ToggleGroup ToggleRoot;
    public Image Bollino;
    public Text BollinoText;
    private Toggle tog;
    private Button[] buttons;
    //public Toggle[] toggle = new Toggle[50];
    public List<Ordine> ordini { get; set; } = new List<Ordine>();

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        C5 = this;
    }
    public void Apricomanda()
    {
        CanvasComanda.SetActive(!CanvasComanda.active);
    }
    public void Buy()
    {
        Debug.Log("Dopo buy");
        Shopsetting.SetActive(true);
        Shop.SetActive(true);
        CanvasConferma.SetActive(false);

    }
    public void PaypalCancel()
    {
        Shopsetting.SetActive(false);
        Shop.SetActive(false);
    }
    
    public void Conferma()
    {
        if (ContentCreator.Db == 1)
        {
            if (!Pay)
            {
                if ((ContentCreator.seat == "" && ContentCreator.sala == "") || (ContentCreator.seat == null && ContentCreator.sala == null) && !ContentCreator.C8.comunicazioneCompilata)
                {
                    ContentCreator.C8.PanelPosizione.SetActive(true);
                    ContentCreator.C8.GruppoPosizione.SetActive(true);
                }
                else if (!ContentCreator.C8.comunicazioneCompilata)
                {
                    ContentCreator.C8.PanelPosizione.SetActive(true);
                    ContentCreator.C8.GruppoPosizione.SetActive(false);
                }
                else
                {
                    string response = CallWebService.MMS.SendTableOrder(ordini);
                    if (response == "OK")
                    {
                        Confirmation_Text.text = "Notifica inviata!" + '\n' + '\n' + "Un cameriere arriverà a breve per confermare le vostre scelte.";
                        CanvasConferma.SetActive(true);
                        EraseComanda();
                        DataService ds = new DataService("DatiCliente.db");
                        //ds.SaveOrdine(ordini,Guid.NewGuid().ToString());

                    }
                    else
                    {
                        Confirmation_Text.text = "Ordine Fallito!" + '\n' + '\n' + "Assicurati di avere connessione ad internet e ritenta";
                        CanvasConferma.SetActive(true);
                    }
                }
            }
            else
            {
                if ((ContentCreator.minimoValore > 0f && totale >= ContentCreator.minimoValore) || (ContentCreator.minimoValore == 0f))
                {

                    if (!ContentCreator.ClienteTKy)
                    {
                        ContentCreator.Ordering = true;
                        CanvasDati.SetActive(true);
                    }
                    else
                    {

                        if (ContentCreator.seat == null)
                            ContentCreator.seat = "";
                        if (ContentCreator.sala == null)
                            ContentCreator.sala = "";
                        if (ContentCreator.currentMode == "Dlv")
                        {
                            ContentCreator.seat = "";
                            ContentCreator.sala = "";
                        }
                        ContentCreator.Ordering = true;
                        CanvasConsegna.SetActive(true);
                    }
                }
                else
                {
                    StartCoroutine(ShowMessage("Limite minimo di " + ContentCreator.minimoValore.ToString() + "€ per ordine non raggiunto", 2));
                }
            }
            
        }
        else if (ContentCreator.Db == 0)
        {
            Confirmation_Text.text = "Notifica non inviata!" + '\n' + '\n' + "Il sistema di notifica non è al momento raggiungibile. Comunicate il vostro ordine direttamente al cameriere. Grazie.";
            CanvasConferma.SetActive(true);
        }

    }
    public void PagaConPaypal()
    {
        ShopActions.Orderstring = Payment.PrepareJson(ordini, "consegna a casa", "https://fidoapp.page.link/payback", "https://fidoapp.page.link/payback");
        Buy();
    }
    public void PagaConPin(string tipOrder, string tipPag)
    {
        string response = CallWebService.MMS.SendTkyOrDlv(ordini, tipOrder, tipPag);
        if (response == "OK")
        {
            PanelOrdineConclusoOK.gameObject.SetActive(true);
            EraseComanda();
            DataService ds = new DataService("DatiCliente.db");
            //ds.SaveOrdine(ordini, Guid.NewGuid().ToString());
        }
        else
        {
            PanelOrdineConclusoKO.gameObject.SetActive(true);
        }
    }
    public void InviaOrdinePagato()
    {
        string response = CallWebService.MMS.SendTkyOrDlv(ordini, ContentCreator.currentMode, "Pay");
        if (response == "OK")
        {
            PanelOrdineConclusoOK.gameObject.SetActive(true);
            EraseComanda();
        }
        else
        {
            PanelOrdineConclusoKO.gameObject.SetActive(true);
        }
    }
    public void ChiudiEsiti()
    {
        Data_Tipo.DatTip.Resetpanel();
        PanelOrdineConclusoOK.gameObject.SetActive(false);
        PanelOrdineConclusoKO.gameObject.SetActive(false);
        CanvasConsegna.gameObject.SetActive(false);
        CanvasDati.gameObject.SetActive(false);
        CanvasComanda.gameObject.SetActive(false);
    }
    public void Esci()
    {
        CanvasConferma.SetActive(false);
    }
    public void KeepComanda()
    {
        ComandaQuestion.SetActive(false);
    }
    public void EraseComanda()
    {
        int i = 0;
        foreach (Ordine o in ordini)
        {
            
                var toggle = ToggleRoot.GetComponentsInChildren<Toggle>();
                var toggleText = toggle[i].GetComponentsInChildren<Text>();
                int quantità = o.Quantità;
                quantità = quantità - quantità;
                int[] splitted = new int[2];
                splitted = GetIntArray(quantità);
                toggleText[2].text = splitted[1].ToString();
                toggleText[3].text = splitted[0].ToString();
                if (quantità == 0)
                {
                    //toggle[i].GetP.SetActive(false);
                    buttons = toggle[i].GetComponentsInChildren<Button>();
                    buttons[0].onClick.RemoveAllListeners();
                    buttons[1].onClick.RemoveAllListeners();
                    buttons[2].onClick.RemoveAllListeners();
                    ToggleRoot.transform.GetChild(i).gameObject.SetActive(false);
                    ordini.RemoveAt(i);
                    break;
                }
                //toggleText[3].text = quantità.ToString();
                toggleText[1].text = (quantità * o.PrezzoBase).ToString() + "€";
                o.Prezzo = quantità * o.PrezzoBase;
                o.Quantità = quantità;
                break;
            
            i++;
        }
        totale = 0f;
        ordini.Clear();
        if (totale <= 0f)
        {
            Invia_Button.interactable = false;
        }
        else if (totale > 0f)
        {
            Invia_Button.interactable = true;
        }
        if (totale > 0f && !Pay)
        {
            Paga_Buttons.gameObject.SetActive(false);
            Ok_Button.gameObject.SetActive(true);
        }
        if (totale > 0f && Pay)
        {
            Paga_Buttons.gameObject.SetActive(true);
            Ok_Button.gameObject.SetActive(false);
        }
        ComandaQuestion.SetActive(false);
        int q = 0;
        foreach (Ordine o in ordini)
        {
            q += o.Quantità;
        }
        BollinoText.text = q.ToString();
        if (q == 0)
        {
            Bollino.gameObject.SetActive(false);
        }
        else
        {
            Bollino.gameObject.SetActive(true);
        }
    }
    public void Question()
    {
        ComandaQuestion.SetActive(true);
    }
    public void DecreasePiatto(string Nome)
    {
        int i = 0;
        foreach (Ordine o in ordini)
        {
            if (o.Nome == Nome)
            {
                var toggle = ToggleRoot.GetComponentsInChildren<Toggle>();
                var toggleText = toggle[i].GetComponentsInChildren<Text>();
                int quantità = o.Quantità;
                quantità = quantità - 1;
                int[] splitted = new int[2];
                splitted = GetIntArray(quantità);
                toggleText[2].text = splitted[1].ToString();
                toggleText[3].text = splitted[0].ToString();
                if (quantità == 0)
                {
                    //toggle[i].GetP.SetActive(false);
                    buttons = toggle[i].GetComponentsInChildren<Button>();
                    buttons[0].onClick.RemoveAllListeners();
                    buttons[1].onClick.RemoveAllListeners();
                    buttons[2].onClick.RemoveAllListeners();
                    ToggleRoot.transform.GetChild(i).gameObject.SetActive(false);
                    ordini.RemoveAt(i);
                    break;
                }
                //toggleText[3].text = quantità.ToString();
                toggleText[1].text = (quantità * o.PrezzoBase).ToString() + "€";
                o.Prezzo = quantità * o.PrezzoBase;
                o.Quantità = quantità;
                break;
            }
            i++;
        }
        totale = 0f;
        foreach (Ordine o in ordini)
        {
            if (o.Prezzo > 0)
            {
                totale = totale + o.Prezzo;

            }
        }
        Totale.text = totale.ToString() + " €";
        int q = 0;
        foreach (Ordine o in ordini)
        {
            q += o.Quantità;
        }
        BollinoText.text = q.ToString();
        if (q == 0)
        {
            Bollino.gameObject.SetActive(false);
        }
        else
        {
            Bollino.gameObject.SetActive(true);
        }
        if (totale <= 0f)
        {
            Invia_Button.interactable = false;
        }
        else if (totale > 0f)
        {
            Invia_Button.interactable = true;
        }
        if (totale > 0f && !Pay)
        {
            Paga_Buttons.gameObject.SetActive(false);
            Ok_Button.gameObject.SetActive(true);
        }
        if (totale > 0f && Pay)
        {
            Paga_Buttons.gameObject.SetActive(true);
            Ok_Button.gameObject.SetActive(false);
        }
    }
    public void ErasePiatto(string Nome, int indice)
    {
        int i = 0;
        foreach (Ordine o in ordini)
        {
            if (o.Nome == Nome)
            {
                var toggle = ToggleRoot.GetComponentsInChildren<Toggle>();
                buttons = toggle[i].GetComponentsInChildren<Button>();
                buttons[0].onClick.RemoveAllListeners();
                buttons[1].onClick.RemoveAllListeners();
                buttons[2].onClick.RemoveAllListeners();
                //toggle[i].gameObject.SetActive(false);
                ToggleRoot.transform.GetChild(indice).gameObject.SetActive(false);
                ordini.RemoveAt(i);
                    break;
            }
            i++;
        }
        totale = 0f;
        foreach (Ordine o in ordini)
        {
            if (o.Prezzo > 0)
            {
                totale = totale + o.Prezzo;

            }
        }
        Totale.text = totale.ToString() + " €";
        int q = 0;
        foreach (Ordine o in ordini)
        {
            q += o.Quantità;
        }
        BollinoText.text = q.ToString();
        if (q == 0)
        {
            Bollino.gameObject.SetActive(false);
        }
        else
        {
            Bollino.gameObject.SetActive(true);
        }
        if (totale <= 0f)
        {
            Invia_Button.interactable = false;
        }
        else if (totale > 0f)
        {
            Invia_Button.interactable = true;
        }
        if (totale > 0f && !Pay)
        {
            Paga_Buttons.gameObject.SetActive(false);
            Ok_Button.gameObject.SetActive(true);
        }
        if (totale > 0f && Pay)
        {
            Paga_Buttons.gameObject.SetActive(true);
            Ok_Button.gameObject.SetActive(false);
        }
    }
    public void AddPiatto(string Nome, string[] EsclIngredienti, float Prezzo, int Quantità, int idart,string Guid)
    {
        if (ContentCreator.limiteValore > 0f && (totale + Prezzo < ContentCreator.limiteValore) || (ContentCreator.limiteValore == 0f) || (ContentCreator.currentMode == "Com"))
        {


            CanvasComanda.SetActive(true);
            //var toggle = ToggleRoot.GetComponentsInChildren<Toggle>();
            bool added = false;
            int i = 0;
            foreach (Ordine o in ordini)
            {
                if (o.Nome == Nome)
                {
                    var toggle = ToggleRoot.GetComponentsInChildren<Toggle>();
                    var toggleText = toggle[i].GetComponentsInChildren<Text>();
                    int quantità = o.Quantità;
                    //int.TryParse(toggleText[3].text, out quantità);
                    quantità = quantità + Quantità;
                    int[] splitted = new int[2];
                    splitted = GetIntArray(quantità);
                    toggleText[2].text = splitted[1].ToString();
                    toggleText[3].text = splitted[0].ToString();
                    toggleText[1].text = (quantità * Prezzo).ToString() + "€";
                    o.Prezzo = quantità * Prezzo;
                    o.Quantità = quantità;
                    added = true;
                    int q = 0;
                    q += o.Quantità;
                    BollinoText.text = q.ToString();
                    Bollino.gameObject.SetActive(true);
                    break;
                }
                i++;

            }

            if (!added)
            {
                var toggle = ToggleRoot.GetComponentsInChildren<Toggle>();
                for (int ii = 0; ii < 49; ii++)
                {
                    if (!ToggleRoot.transform.GetChild(ii).gameObject.active && !added)
                    {
                        ToggleRoot.transform.GetChild(ii).gameObject.SetActive(true);
                        tog = ToggleRoot.transform.GetChild(ii).GetComponentInChildren<Toggle>();
                        tog.name = ii.ToString();
                        //toggle[ii].name = ii.ToString();
                        int[] splitted = new int[2];
                        int quantità = Quantità;
                        var toggleText = tog.GetComponentsInChildren<Text>();
                        splitted = GetIntArray(quantità);
                        toggleText[2].text = splitted[1].ToString();
                        toggleText[3].text = splitted[0].ToString();
                        toggleText[0].text = Nome;
                        buttons = tog.GetComponentsInChildren<Button>();
                        buttons[0].onClick.AddListener(() => OnButtonClick("min", Nome, EsclIngredienti, Prezzo, Quantità, ii, idart, Guid));
                        buttons[1].onClick.AddListener(() => OnButtonClick("add", Nome, EsclIngredienti, Prezzo, Quantità, ii, idart, Guid));
                        buttons[2].onClick.AddListener(() => OnButtonClick("era", Nome, EsclIngredienti, Prezzo, Quantità, ii, idart, Guid));

                        toggleText[1].text = (Quantità * Prezzo).ToString() + "€";
                        Ordine add = new Ordine();
                        add.Nome = Nome;
                        add.Indice = ii;
                        add.Prezzo = Prezzo;
                        add.Quantità = Quantità;
                        add.PrezzoBase = Prezzo;
                        add.IDart = idart;
                        add.Guid = Guid;
                        ordini.Insert(ii, add);

                        added = true;
                        break;
                    }
                }
                int q = 0;
                foreach (Ordine o in ordini)
                {
                    q += o.Quantità;
                }
                BollinoText.text = q.ToString();
                if (q == 0)
                {
                    Bollino.gameObject.SetActive(false);
                }
                else
                {
                    Bollino.gameObject.SetActive(true);
                }
            }
            totale = 0f;
            foreach (Ordine o in ordini)
            {
                if (o.Prezzo > 0)
                {
                    totale = totale + o.Prezzo;

                }
            }
        }
        else
        {
            flashMessage();
        }
        Totale.text = totale.ToString() + " €";
        if (totale <= 0f)
        {
            Invia_Button.interactable = false;
        }
        else if (totale > 0f)
        {
            Invia_Button.interactable = true;
        }
        if (totale > 0f && !Pay)
        {
            Paga_Buttons.gameObject.SetActive(false);
            Ok_Button.gameObject.SetActive(true);
        }
        if (totale > 0f && Pay)
        {
            Paga_Buttons.gameObject.SetActive(true);
            Ok_Button.gameObject.SetActive(false);
        }
    }
    public void flashMessage()
    {
        StartCoroutine(ShowMessage( "In questo locale non puoi spendere più di "+ ContentCreator.limiteValore.ToString() + "€ per ordine.",2));
        
    }
    IEnumerator ShowMessage(string message, float delay)
    {
        LimitText.text = message;
        textContainer.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        textContainer.gameObject.SetActive(false);
    }
    public void OnButtonClick(string key, string name, string[] EsclIngredienti, float Prezzo, int Quantità, int indice, int idart, string GUid)
    {
        if (key == "min")
        {
            DecreasePiatto(name);
        }
        else if (key == "era")
        {
            ErasePiatto(name, indice);
        }
        else if (key == "add")
        {
            AddPiatto(name, EsclIngredienti, Prezzo, Quantità, idart, GUid);
        }
        Debug.Log(key + " " + name);
    }
    public class Ordine
    {
        public float PrezzoBase;
        public string Nome;
        public float Prezzo;
        public int Quantità;
        public int Indice;
        public string[] EsclusioneIngredienti;
        public int IDart;
        public string Guid;
        public float Val;

    }
    int[] GetIntArray(int num)
    {
        int nume = num;
        if (num == 0) { return new int[] { 0,0 }; }
        List<int> listOfInts = new List<int>();
        
        while (num > 0)
        {
            listOfInts.Add(num % 10);
            num = num / 10;
        }
        if (nume < 10)
            listOfInts.Add(0);
        //listOfInts.Reverse();
        return listOfInts.ToArray();
    }
   
   
    // Update is called once per frame
    void Update()
    {
       
       
       
    }
}
