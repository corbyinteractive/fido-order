using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

public class ClassConverter : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public static Locale localifromSS(CallWebService.MenuSoftwareSolutions menuSS)
    {
        Locale locale = new Locale();
        bool.TryParse(menuSS.EnableTAV, out locale.s_Base);
        bool.TryParse(menuSS.EnableTKY, out locale.s_TakeAway);
        bool.TryParse(menuSS.EnableDLV, out locale.s_Delivery);
        bool.TryParse(menuSS.EnableConsegnaInterna, out locale.s_consegnaInterna);
        bool.TryParse(menuSS.RichiediDatiAccesso, out locale.s_RichiediDatiAccesso);
        bool.TryParse(menuSS.EnableMD, out locale.s_EnableMD);
        float.TryParse(menuSS.LimitPZ, out locale.s_LimitPZ);
        float.TryParse(menuSS.LimitVAL, out locale.s_LimitVAL);
        float.TryParse(menuSS.LimitMinVAL, out locale.s_LimitMinVAL);
        locale.s_nomeLocale = menuSS.Denominazione;
        locale.s_utenza = menuSS.UtenzaCode;
        locale.s_pinPag = menuSS.PinPag;
        locale.s_enablePP = menuSS.EnablePP;
        locale.s_CID = menuSS.PPchiave;
        locale.s_SKE = menuSS.PPcodice;
        locale.s_enableNEXI = menuSS.EnableNEXI;
        locale.s_NEXIalias = menuSS.NEXIalias;
        locale.s_NEXImac = menuSS.NEXImac;
        locale.s_Landing = menuSS.MessaggioLanding;
        locale.s_LandingGen = menuSS.MessaggioLandingDelivery;
        bool.TryParse(menuSS.EnablePagaCons, out locale.s_EnablePagaCons); 

        return locale;
    }

    public static string menufromSS(CallWebService.MenuSoftwareSolutions menuSS)
    {
        NumberStyles style;
        CultureInfo culture;
        Menu Menu1 = new Menu();
        Menu1.Piatti = new List<Piatto>();
        foreach (CallWebService.ProductList v in menuSS.ProductList)
        {
            Piatto p = new Piatto();
            p.Nome = v.Descr;
            p.Ingredienti = v.Descr2;
            p.base64Img = v.base64Img;
            p.GUID = v.GUID;
            int.TryParse(v.idart, out p.idart);
            p.Prezzo = v.Prezzo;
            p.ProdType = v.ProdType;
            p.Note = v.Appunti;
            bool alreadyExist = Menu1.Tipologia.Contains(v.Categoria);
            if (!alreadyExist)
                Menu1.Tipologia.Add(v.Categoria);
            p.Tipo = v.Categoria;
            if (v.ListaAllergeni != null)
            {
                foreach (var al in v.ListaAllergeni)
                {

                    p.Allergeni.Add(al.id);
                    Menu1.Allergene[al.id] = al.NomeAllergene;
                }
            }
            Menu1.Piatti.Add(p);
        }
        return (JsonConvert.SerializeObject(Menu1));
            
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
