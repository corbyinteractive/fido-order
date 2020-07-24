using System.Collections;
using System;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Data_Tipo : MonoBehaviour
{
    public static Data_Tipo DatTip; 
    public Text ResultText;
    public Text Data;
    public Text TextPinMatch;
    public GameObject PanelOrari;
    public Button ButtonPinpag;
    public Button ButtonPaypal;
    public Button ButtonConsegna;
    public Dropdown DropdownOrari;
    public GameObject ControllaIndirizzo;
    public GameObject Pagamenti;
    public GameObject PagaConsegna;
    public GameObject PanelPin;
    public InputField Pin;
    public InputField Indirizzo;
    public InputField Citta;
    public InputField Comune;
    public InputField Comunicazione;
    public InputField Provincia;
    public InputField PosizInterna;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        DatTip = this;
    }
    public void ControllaOrari()
    {
        List<string> Orari = CallWebService.MMS.CheckDisponibilita("AAA", ContentCreator.Restaurant, Data.text, Comanda.C5.ordini.Capacity.ToString());
        if (Orari.Capacity >= 1 && Orari != null)
        {
            DropdownOrari.ClearOptions();
            foreach (string o in Orari)
            {
                ResultText.gameObject.SetActive(false);
                ResultText.text = "";
                DropdownOrari.options.Add(new Dropdown.OptionData() { text = o });
            }
            PanelOrari.SetActive(true);
            Comunicazione.gameObject.SetActive(true);
            if (ContentCreator.currentMode == "Dlv" || ContentCreator.currentMode == "Cin")
            {
                DataService ds = new DataService("DatiCliente.db");
                IEnumerable<DatiCliente> Dci;
                Dci = ds.GetDatiCliente();
                if (ContentCreator.currentMode == "Dlv")
                {
                    Indirizzo.gameObject.SetActive(true);
                    Citta.gameObject.SetActive(true);
                    Comune.gameObject.SetActive(true);
                    Provincia.gameObject.SetActive(true);
                    PosizInterna.gameObject.SetActive(false);
                    ControllaIndirizzo.SetActive(true);
                }
                else
                {
                    Indirizzo.gameObject.SetActive(false);
                    Citta.gameObject.SetActive(false);
                    Comune.gameObject.SetActive(false);
                    Provincia.gameObject.SetActive(false);
                    PosizInterna.gameObject.SetActive(true);
                    ControllaIndirizzo.SetActive(true);
                }
                
               foreach (DatiCliente D in Dci)
              {
                        if (D.Id == 1)
                        {
                            Indirizzo.text = D.Indirizzo;
                            Citta.text = D.Citta;
                        try
                        {
                            if (ContentCreator.sala.Length > 3)
                            {
                                PosizInterna.text += ContentCreator.sala + " ";
                                PosizInterna.readOnly = true;
                            }
                        }
                        catch { }
                        try
                        {
                            if (ContentCreator.seat.Length > 3)
                            {
                                PosizInterna.text += ContentCreator.seat + " ";
                                PosizInterna.readOnly = true;
                            }
                        }
                        catch
                        {

                        }
                        }
               }
                
            }
           
            if (ContentCreator.pagaConsegnaAbilitata)
            {
                PagaConsegna.gameObject.SetActive(true);
            }
            if (ContentCreator.PinPagOn || ContentCreator.Key.Length > 10)
            {
                Pagamenti.gameObject.SetActive(true);
            }
            
            if (ContentCreator.PinPagOn)
            {
                ButtonPinpag.gameObject.SetActive(true);
            }
            if (ContentCreator.Key.Length > 10)
            {
                ButtonPaypal.gameObject.SetActive(true);
            }
            ButtonConsegna.gameObject.SetActive(true);
        }
        else
        {
            ResultText.gameObject.SetActive(true);
            ResultText.text = "Nessuna disponibilità per la data indicata!";
        }
    }
    public void PaywithPaypal()
    {
        string one = Data.text;
        string two = DropdownOrari.options[DropdownOrari.value].text;

        //DateTime dt = Convert.ToDateTime(one + " " + two + ":00.00");
        
        ContentCreator.OrarioConsegna = (one + " " + two + ":00.00");
        ContentCreator.indirizzoSped = Indirizzo.text;
        ContentCreator.NoteSpediz = Comunicazione.text;
        ContentCreator.cittaSped = Citta.text;
        ContentCreator.PvSped = Provincia.text;
        ContentCreator.ComuneSped = Comune.text;
        ContentCreator.PosizioneInterna = PosizInterna.text;
        ContentCreator.comunicazioniInserite = Comunicazione.text;
        Comanda.C5.PagaConPaypal();
    }
    public void PaywithPin()
    {
        PanelPin.gameObject.SetActive(true);
    }
    public void Resetpanel()
    {
        Pin.text = "";
        PanelPin.SetActive(false);
        PanelOrari.SetActive(false);
        PagaConsegna.gameObject.SetActive(false);
        Pagamenti.gameObject.SetActive(false);
        Comune.text = "";
        Comunicazione.text = "";
        Provincia.text = "";
        PosizInterna.text = "";
        Comunicazione.gameObject.SetActive(false);
        ControllaIndirizzo.SetActive(false);
    }
    public void DateChanged()
    {
        PanelOrari.SetActive(false);
        Comunicazione.gameObject.SetActive(false);
        if (ContentCreator.currentMode == "Dlv")
        {
            Indirizzo.gameObject.SetActive(false);
            Citta.gameObject.SetActive(false);
            Comune.gameObject.SetActive(false);
            Provincia.gameObject.SetActive(false);
            PosizInterna.gameObject.SetActive(false);
            ControllaIndirizzo.SetActive(false);
        }
        else
        {
            Indirizzo.gameObject.SetActive(false);
            Citta.gameObject.SetActive(false);
            Comune.gameObject.SetActive(false);
            Provincia.gameObject.SetActive(false);
            PosizInterna.gameObject.SetActive(false);
            ControllaIndirizzo.SetActive(false);
        }
        if (ContentCreator.pagaConsegnaAbilitata)
        {
            PagaConsegna.gameObject.SetActive(false);
        }
        if (ContentCreator.PinPagOn || ContentCreator.Key.Length > 10)
        {
            Pagamenti.gameObject.SetActive(false);
        }

        if (ContentCreator.PinPagOn)
        {
            ButtonPinpag.gameObject.SetActive(false);
        }
        if (ContentCreator.Key.Length > 10)
        {
            ButtonPaypal.gameObject.SetActive(false);
        }
        ButtonConsegna.gameObject.SetActive(false);
    }
    public void VerifyPin()
    {
        if (Pin.text == ContentCreator.PinPag)
        {
            string one = Data.text;
            string two = DropdownOrari.options[DropdownOrari.value].text;
            
            //DateTime dt = Convert.ToDateTime(one + " " + two+":00.00");
            
            ContentCreator.OrarioConsegna = (one + " " + two + ":00.00");
            ContentCreator.indirizzoSped = Indirizzo.text;
            ContentCreator.NoteSpediz = Comunicazione.text;
            ContentCreator.cittaSped = Citta.text;
            ContentCreator.PvSped = Provincia.text;
            ContentCreator.ComuneSped = Comune.text;
            ContentCreator.PosizioneInterna = PosizInterna.text;
            ContentCreator.comunicazioniInserite = Comunicazione.text;
            Comanda.C5.PagaConPin(ContentCreator.currentMode,"Pin");
        }
        else
        {
            TextPinMatch.text = "PinErrato";
        }
    }
    public void PayOnDelivery()
    {
        string one = Data.text;
        string two = DropdownOrari.options[DropdownOrari.value].text;

        //DateTime dt = Convert.ToDateTime(one + " " + two + ":00.00");
        
        ContentCreator.OrarioConsegna = (one + " " + two + ":00.00");
        ContentCreator.indirizzoSped = Indirizzo.text;
        ContentCreator.NoteSpediz = Comunicazione.text;
        ContentCreator.cittaSped = Citta.text;
        ContentCreator.PvSped = Provincia.text;
        ContentCreator.ComuneSped = Comune.text;
        ContentCreator.PosizioneInterna = PosizInterna.text;
        ContentCreator.comunicazioniInserite = Comunicazione.text;
        Comanda.C5.PagaConPin(ContentCreator.currentMode, "NO");
        //Comanda.C5.PagaConPaypal();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
