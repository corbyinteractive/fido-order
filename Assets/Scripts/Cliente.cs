using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class Cliente : MonoBehaviour
{
    public GameObject ButtonBack;
    public GameObject ButtonComanda;
    public GameObject ThisPanel;
    public Button ProcediButton;
    public InputField Nome;
    public InputField Cognome;
    public InputField Indirizzo;
    public InputField Citta;
    public InputField CAP;
    public InputField Phone;
    public InputField Email;
    public Toggle ADS;
    public Toggle Privacy;
    public Text Requirements;
    public bool ok;
    private bool hasAt;
    // Start is called before the first frame update
    void Start()
    {
        DataService ds = new DataService("DatiCliente.db");
        IEnumerable<DatiCliente> Dci;
        Dci = ds.GetDatiCliente();
        ButtonComanda.SetActive(false);
        if (ContentCreator.registrazioneObbligatoria)
            ButtonBack.SetActive(false);
        if (!Comanda.Pay)
        {
            Indirizzo.gameObject.SetActive(false);
            Citta.gameObject.SetActive(false);
            CAP.gameObject.SetActive(false);
            Email.gameObject.GetComponent<InputField>().placeholder.GetComponent<Text>().text = "Email (facoltativa)";
        }
        else
        {
            Indirizzo.gameObject.SetActive(true);
            Citta.gameObject.SetActive(true);
            CAP.gameObject.SetActive(true);
        }
        if (Comanda.Pay && !ContentCreator.ClienteTKy)
        {
            foreach (DatiCliente D in Dci)
            {
                if (D.Id == 1)
                {
                    Nome.text = D.Nome;
                    Cognome.text = D.Cognome;
                    Phone.text = D.Tel;
                    Email.text = D.Email;
                }
            }
        }
    }
    public void checkRequirements()
    {
        ok = true;
        hasAt = Email.text.IndexOf('@') > 0;
        if (Privacy.isOn && !hasAt && Comanda.Pay)
        {
            Requirements.text = "indirizzo mail non valido";
            ok = false;
        }
        if (!Privacy.isOn)
        {
            Requirements.text = "Devi accettare il trattamento dati";
            ok = false;
        }
        if (Phone.text.Length < 7) 
        {
            Requirements.text = "inserire il telefono";
            ok = false;
        }
        if (CAP.text.Length < 3 && Comanda.Pay) 
        {
            Requirements.text = "inserire il CAP";
            ok = false;
        }
        if (Citta.text.Length < 3 && Comanda.Pay) 
        {
            Requirements.text = "inserire la Città";
            ok = false;
        }
        if (Indirizzo.text.Length < 5 && Comanda.Pay)
        {
            Requirements.text = "inserire l'indirizzo";
            ok = false;
        }
        if (Cognome.text.Length < 3)
        {
            Requirements.text = "inserire il cognome";
            ok = false;
        }
        if (Nome.text.Length < 3)
        {
            Requirements.text = "inserire il nome";
            ok = false;
        }
        if (Privacy.isOn && ok)
        {
            DataService ds = new DataService("DatiCliente.db");
            Requirements.text = "";
            DatiCliente Dc = new DatiCliente();
            Dc.Nome = Nome.text;
            Dc.Cognome = Cognome.text;
            Dc.Tel = Phone.text;
            Dc.Email = Email.text;
            if (ADS.isOn)
            {
                Dc.Pubblicità = 1;
            }
            Dc.Privacy = 1;
            if (!Comanda.Pay)
            {
                ds.UpdateDatiClienteMin(Dc.Privacy,Dc.Pubblicità, DateTime.Now.ToString(),Dc.Nome,Dc.Cognome,Dc.Tel, Dc.Email, 1);
                IEnumerable<DatiCliente> Dci = ds.GetDatiCliente();
                
                        ContentCreator.datiCliente.Nome = Dc.Nome;
                        ContentCreator.datiCliente.Cognome = Dc.Cognome;
                        ContentCreator.datiCliente.Tel = Dc.Tel;
                        ContentCreator.datiCliente.Email = Dc.Email;
                        ContentCreator.ClienteRegMin = true;
            }        
            if (Comanda.Pay)
            {
                Dc.Indirizzo = Indirizzo.text;
                Dc.Citta = Citta.text;
                Dc.CAP = CAP.text;
                ds.UpdateDatiClienteTot(Dc.Privacy, Dc.Pubblicità, DateTime.Now.ToString(), Dc.Nome,Dc.Cognome,Dc.Indirizzo,Dc.Citta,Dc.CAP, Dc.Tel, Dc.Email);
                ContentCreator.ClienteTKy = true;
                ContentCreator.datiCliente.Nome = Dc.Nome;
                ContentCreator.datiCliente.Cognome = Dc.Cognome;
                ContentCreator.datiCliente.Tel = Dc.Tel;
                ContentCreator.datiCliente.Email = Dc.Email;
                ContentCreator.datiCliente.Indirizzo = Dc.Indirizzo;
                ContentCreator.datiCliente.Citta = Dc.Citta;
                ContentCreator.datiCliente.CAP = Dc.CAP;
            }
            ButtonComanda.SetActive(true);
            if (ContentCreator.registrazioneObbligatoria)
                ButtonBack.SetActive(true);
            ThisPanel.SetActive(false); 
            if (ContentCreator.registrazioneObbligatoria)
            {
                /* bool hasColumn = ds.CheckIfTableHasColumn("CoordinateLocali", "Fidelity");
                 if (!hasColumn)
                 {
                     ds.AddColumn("CoordinateLocali", "Fidelity");
                 }
                 string trial = ds.GetFidelity(ContentCreator.Restaurant);
                 */
                string trial = ds.GetFidelity(ContentCreator.Restaurant);
                if (trial == null || trial.Length < 12)
                {
                    string platform;
                    if (Application.platform == RuntimePlatform.Android)
                        platform = "Android";
                    else
                        platform = "Apple";
                    string json = JsonConvert.SerializeObject(Dc);
                    string fidelity = CallWebService.MMS.setCli("AAA", ContentCreator.Restaurant,"0000-0000", json, platform);
                    ds.SetFidelity(fidelity, ContentCreator.Restaurant);
                    QREncodeTest.QR.Encode(fidelity);
                    ContentCreator.C8.SelectionBtnRoot.transform.GetChild(5).gameObject.SetActive(true);
                }

            }
            
        }


        // Update is called once per frame
        
    }
    private void Update()
    {
        
    }
    public void OpenSite()
    {
        if (ContentCreator.Db == 0)
        Application.OpenURL("http://aruba.streetshot.it/pinapp/privacy.php");
        if (ContentCreator.Db == 1)
            Application.OpenURL("https://gccloud.software-solution.it/PrivacyPolicy.aspx");
    }
}
