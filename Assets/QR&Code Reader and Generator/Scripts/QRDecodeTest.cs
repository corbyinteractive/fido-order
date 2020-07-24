using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System.Web;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

public class QRDecodeTest : MonoBehaviour
{
    public static bool CheckURLValid(string source) => Uri.TryCreate(source, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttps || uriResult.Scheme == Uri.UriSchemeHttp);

    private int ok;

    public static string m_errore;

    public QRCodeDecodeController e_qrController;

    public Text UiText;

    public GameObject resetBtn;
    public GameObject LoadingPanel;
    public GameObject scanLineObj;

    private void Start()
    {

        if (this.e_qrController != null)
        {
            this.e_qrController.onQRScanFinished += new QRCodeDecodeController.QRScanFinished(this.qrScanFinished);
        }
    }

    private void Update()
    {
    }

    private void qrScanFinished(string dataText)
    {

        ok = 1;
        string codLocale;
        if (CheckURLValid(dataText))
        {
            if (dataText.Contains("streetshot"))
            {
                Uri myUri = new Uri(dataText);
                codLocale = ParseQueryString(myUri.Query).Get("Identify");
                string seat = ParseQueryString(myUri.Query).Get("Tav");
                 Locali lc = Locali.Load("https://safemenu.altervista.org/wp-content/" + codLocale + "/Locali.xml");
                    ContentCreator.urlLocali = "https://safemenu.altervista.org/wp-content/" + codLocale + "/Locali.xml";
                    foreach (Locale locale in lc.CollezioneLocali)
                    {
                        ok = string.Compare(locale.s_ShortName, codLocale);
                        if (ok == 0)
                        {
                            ContentCreator.Restaurant = codLocale;
                            ContentCreator.seat = seat;
                         
                            ContentCreator.Db = 0;
                           
                            LoadingPanel.gameObject.SetActive(true);
                            if (NaturalOrientation.tablet == false)
                            {
                                SceneManager.LoadSceneAsync("Menu");

                                break;
                            }
                            else
                            {
                                SceneManager.LoadSceneAsync("Menu_Tablet");

                                break;
                            }
                        }

                    }
            }
            else
            {
                Uri myUri = new Uri(dataText);
                string param1 = ParseQueryString(myUri.Query).Get("Identify");
                string param2 = ParseQueryString(myUri.Query).Get("pos");
                string param3 = ParseQueryString(myUri.Query).Get("store");
                string[] result = param1.Split(' ');
                try
                {

                    ContentCreator.Utenza = result[1];
                    ContentCreator.Restaurant = result[0];
                }
                catch
                {
                    ContentCreator.Utenza = "";
                    ContentCreator.Restaurant = param1; //Punto vendita
                }
                try
                {
                    result = param2.Split(' ');
                    ContentCreator.seat = result[1];
                    ContentCreator.sala = result[0];

                }
                catch
                {
                    ContentCreator.sala = "";
                    ContentCreator.seat = param2;
                }
                if (param3 != null)
                {
                    int.TryParse(param3, out ContentCreator.daysToStore);
                }
                ContentCreator.urlLocali = dataText;
                ContentCreator.Db = 1;
                ok = 0;
                LoadingPanel.gameObject.SetActive(true);
                if (NaturalOrientation.tablet == false)
                {
                    SceneManager.LoadSceneAsync("Menu");

                    return;
                }
                else
                {
                    SceneManager.LoadSceneAsync("Menu_Tablet");

                    return;
                }
            }
        }
        else
        {
            try
            {
                CoordinateLocaliQR coordQR = JsonConvert.DeserializeObject<CoordinateLocaliQR>(dataText);
                try
                {
                    ContentCreator.Utenza = coordQR.Utenza;
                }
                catch
                {

                }
                try
                {
                    ContentCreator.Restaurant = coordQR.Ristorante;
                }
                catch
                {

                }
                try
                {

                    ContentCreator.sala = coordQR.Sala;

                }
                catch
                {

                }
                try
                {
                    ContentCreator.seat = coordQR.Tavolo;
                }
                catch
                {

                }
                try
                {
                    ContentCreator.Db = coordQR.Db;
                }
                catch
                {

                }
                try
                {
                    ContentCreator.daysToStore = coordQR.Scadenza;
                }
                catch
                {

                }
                Locali lc = Locali.Load("https://safemenu.altervista.org/wp-content/" + ContentCreator.Restaurant + "/Locali.xml");
                ContentCreator.urlLocali = "https://safemenu.altervista.org/wp-content/" + ContentCreator.Restaurant + "/Locali.xml";
                foreach (Locale locale in lc.CollezioneLocali)
                {
                    ok = string.Compare(locale.s_ShortName, ContentCreator.Restaurant);
                    if (ok == 0)
                    {
                        LoadingPanel.gameObject.SetActive(true);
                        if (NaturalOrientation.tablet == false)
                        {
                            SceneManager.LoadSceneAsync("Menu");

                            break;
                        }
                        else
                        {
                            SceneManager.LoadSceneAsync("Menu_Tablet");

                            break;
                        }
                    }

                }

            }
            catch
            {


                try
                {
                    string[] result = dataText.Split(' ');

                    codLocale = result[0];

                    Locali lc = Locali.Load("https://safemenu.altervista.org/wp-content/" + codLocale + "/Locali.xml");
                    ContentCreator.urlLocali = "https://safemenu.altervista.org/wp-content/" + codLocale + "/Locali.xml";
                    foreach (Locale locale in lc.CollezioneLocali)
                    {
                        ok = string.Compare(locale.s_ShortName, codLocale);
                        if (ok == 0)
                        {
                            ContentCreator.Restaurant = result[0];
                            ContentCreator.seat = result[1];
                            if (result.Length > 2)
                            {
                                int Dbn;
                                int.TryParse(result[2], out Dbn);
                                ContentCreator.Db = Dbn;
                            }
                            else
                            {
                                ContentCreator.Db = 0;
                            }
                            LoadingPanel.gameObject.SetActive(true);
                            if (NaturalOrientation.tablet == false)
                            {
                                SceneManager.LoadSceneAsync("Menu");

                                break;
                            }
                            else
                            {
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
        }
        if (ok != 0)
            this.UiText.text = m_errore;
        if (this.resetBtn != null)
        {
            this.resetBtn.SetActive(true);
        }
        if (this.scanLineObj != null)
        {
            this.scanLineObj.SetActive(false);
        }
    }
    public static NameValueCollection ParseQueryString(string s)
    {
        NameValueCollection nvc = new NameValueCollection();

        // remove anything other than query string from url
        if (s.Contains("?"))
        {
            s = s.Substring(s.IndexOf('?') + 1);
        }

        foreach (string vp in Regex.Split(s, "&"))
        {
            string[] singlePair = Regex.Split(vp, "=");
            if (singlePair.Length == 2)
            {
                nvc.Add(singlePair[0], singlePair[1]);
            }
            else
            {
                // only one key with no value specified in query string
                nvc.Add(singlePair[0], string.Empty);
            }
        }

        return nvc;
    }
    public void GoBack()
    {
        if (Application.productName.ToUpper() == "PINEAT")
        {
            SceneManager.LoadScene("PinEat Main");
        }
        else
        {
            SceneManager.LoadScene("Main");
        }
    }
    public void Reset()
    {
        if (this.e_qrController != null)
        {
            this.e_qrController.Reset();
        }
        if (this.UiText != null)
        {
            this.UiText.text = string.Empty;
        }
        if (this.resetBtn != null)
        {
            this.resetBtn.SetActive(false);
        }
        if (this.scanLineObj != null)
        {
            this.scanLineObj.SetActive(true);
        }
    }

    public void GotoNextScene(string scenename)
    {
        if (this.e_qrController != null)
        {
            this.e_qrController.StopWork();
        }
        Application.LoadLevel(scenename);
    }
}
