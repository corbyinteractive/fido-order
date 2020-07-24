using System.Collections;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class InitDb : MonoBehaviour
{
    public GameObject ButtonLocaliVisitati;
    public GameObject CanvasLocaliVisitati;
    public GameObject ButtonParent;
    public GameObject LoadingWindow;
    public Text VersionText;
    // Start is called before the first frame update
    void Start()
    {
        VersionText.text = Application.version;
        DataService ds = new DataService("DatiCliente.db");
        ContentCreator.ClienteRegistrato = ds.CheckDatiCliente();
        if (ContentCreator.ClienteRegistrato)
        {
            ContentCreator.ClienteTKy = ds.GetTipoDati();
            ContentCreator.ClienteRegMin = ds.GetRegMin();
            IEnumerable<DatiCliente> Dci = ds.GetDatiCliente();
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
        /*CoordinateLocaliQR CL = new CoordinateLocaliQR();
        CL.Ristorante = "CorRa";
        CL.PuntoVendita = "Ravenna";
        CL.Sala = "Sala 1";
        CL.Tavolo = "Tavolo 1";
        CL.Scadenza = 5;
        CL.Db = 0;

        var QrSample = JsonConvert.SerializeObject(CL);*/

        ds.DeleteExpired();
        int i = 0;
        int count = 0;
        IEnumerable<CoordinateLocali> LocVis = ds.GetVisitedLocali();
        foreach (CoordinateLocali cl in LocVis)
        {
            ButtonParent.transform.GetChild(i).gameObject.SetActive(true);
            //var Gos = ButtonParent.GetComponentsInChildren<GameObject>();
            var Buttons = ButtonParent.GetComponentsInChildren<Button>();
            Buttons[i].transform.gameObject.SetActive(true);
            Buttons[i].name = i.ToString();
            Buttons[i].onClick.AddListener(() => OnLocaleClick(i, cl.Utenza, cl.Restaurant, cl.Sala, cl.Seat, cl.Db));
            var btnText = Buttons[i].GetComponentInChildren<Text>();
            btnText.text = cl.LongName;
            if (cl.Restaurant.Length > 3)
            {
                count++;
            }
            i++;

        }
        if (count >= 1)
        {
            ButtonLocaliVisitati.SetActive(true);
        }
    }
    public void Eraseall()
    {
        DataService ds = new DataService("DatiCliente.db");
        ds.eraseAll();
    }
    public void Back()
    {
        CanvasLocaliVisitati.SetActive(false);
    }
    public void ShowLocali()
    {
        CanvasLocaliVisitati.SetActive(true);
    }
    public void OnLocaleClick(int id, string Utenza, string Restaurant, string Sala, string Seat, int Db)
    {
        ContentCreator.loadFromCache = true;
        try
        {
            ContentCreator.Utenza = Utenza;
        }
        catch
        {

        }
        try
        {
            ContentCreator.Restaurant = Restaurant;
        }
        catch
        {

        }
        try
        {

            ContentCreator.sala = Sala;

        }
        catch
        {

        }
        try
        {
            ContentCreator.seat = Seat;
        }
        catch
        {

        }
        try
        {
            ContentCreator.Db = Db;
        }
        catch
        {

        }
        if (Db == 0)
        {
            try {
                int ok = 1;
                Locali lc = Locali.Load("https://safemenu.altervista.org/wp-content/" + ContentCreator.Restaurant + "/Locali.xml");
                ContentCreator.urlLocali = "https://safemenu.altervista.org/wp-content/" + ContentCreator.Restaurant + "/Locali.xml";
                foreach (Locale locale in lc.CollezioneLocali)
                {
                    ok = string.Compare(locale.s_ShortName, ContentCreator.Restaurant);
                    if (ok == 0)
                    {
                        if (NaturalOrientation.tablet == false)
                        {
                            LoadingWindow.SetActive(true);
                            SceneManager.LoadSceneAsync("Menu");

                            break;
                        }
                        else
                        {
                            LoadingWindow.SetActive(true);
                            SceneManager.LoadSceneAsync("Menu_Tablet");

                            break;
                        }
                    }

                }
            }
            catch
            {

            }
        }
        if (Db == 1)
        {
            if (Utenza != null)
                ContentCreator.urlLocali = "https://gccloud.software-solution.it/BookingPlatform/Identify.aspx?Identify=" + Restaurant + " " + Utenza;
            else
                ContentCreator.urlLocali = "https://gccloud.software-solution.it/BookingPlatform/Identify.aspx?Identify=" + Restaurant;
            if (NaturalOrientation.tablet == false)
            {
                LoadingWindow.SetActive(true);
                SceneManager.LoadSceneAsync("Menu");

                return;
            }
            else
            {
                LoadingWindow.SetActive(true);
                SceneManager.LoadSceneAsync("Menu_Tablet");

                return;
            }

        }
    }


}
