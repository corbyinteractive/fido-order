using System.Collections;
using System.Net;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class Payment : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    
    public static string PrepareJson(List<Comanda.Ordine> ordini, string tipo, string returnurl, string cancelurl)
    {
        CurrentRequest Request = new CurrentRequest();
        float Totale = 0f;
        ItemList item_list = new ItemList();
        item_list.items = new List<item>();
        
//currentitem = null;
        foreach(Comanda.Ordine o in ordini)
        {
            item currentitem = new item();
            currentitem.name = o.Nome;
            currentitem.quantity = o.Quantità.ToString();
            currentitem.description = "";
            currentitem.price = o.PrezzoBase.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
            currentitem.currency = "EUR";
            //item_list.items.Capacity = item_list.items.Capacity + 1;
            item_list.items.Add(currentitem);
            Totale += o.Prezzo;
        }
        Transaction transaction = new Transaction();
        transaction.amount = new Amount { total = Totale.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture), currency = "EUR" };
        transaction.item_list = item_list;
        transaction.description = tipo;
        transaction.invoice_number = "";
        Request.intent = "sale";
        Request.redirect_urls = new RedirectUrls { return_url = returnurl, cancel_url = cancelurl };
        Request.payer = new Payer { payment_method = "paypal" };
        Request.transactions = new List<Transaction>();
        Request.transactions.Add(transaction);
        InputFields inputfields = new InputFields {no_shipping = 1, address_override = 1 };
        Experience exp = new Experience ();
        exp.input_fields = inputfields;
        Request.experience = exp;
        string json = JsonConvert.SerializeObject(Request);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "PaymentRequest.json"), json);
       
        return json;
        // File.WriteAllText(Path.Combine(Application.persistentDataPath, "PaymentRequest.json"), json);
        // print(Application.persistentDataPath);
    }

        // Update is called once per frame
        void Update()
        {

        }
    
}
public class RedirectUrls
{
    public string return_url { get; set; }
    public string cancel_url { get; set; }
}

public class Payer
{
    public string payment_method { get; set; }
}

public class Amount
{
    public string total { get; set; }
    public string currency { get; set; }
}

public class item
{
    public string name { get; set; }
    public string description { get; set; }
    public string quantity { get; set; }
    public string price { get; set; }
    public string currency { get; set; }
}

public class ItemList
{
    public List<item> items { get; set; }
}

public class Transaction
{
    public Amount amount { get; set; }
    public string description { get; set; }
    public string invoice_number { get; set; }
    public ItemList item_list { get; set; }
}

public class InputFields
{
    public int no_shipping { get; set; }
    public int address_override { get; set; }
}

public class Experience
{
    public InputFields input_fields { get; set; }
}

public class CurrentRequest
{
    public string intent { get; set; }
    public RedirectUrls redirect_urls { get; set; }
    public Payer payer { get; set; }
    public List<Transaction> transactions { get; set; }
    public Experience experience { get; set; }
}
