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
using Cubequad;
public class Menu_Ristorante : MonoBehaviour
{
    Cubequad.c_Data LogoData;
    public static Menu menujson = new Menu();
    #region TOGGLEVARIABLES
    private List<Toggle[]> toggles = new List<Toggle[]>();
    #endregion
    Menu_Ristorante(Menu menu)
    {
        foreach (var tipologia in menu.Tipologia)
        {
            int i = 0;
            var toggleParent = temp.C2.InstantiateMenu(tipologia);
            var piatti = menu.GetPiattiByTipo(tipologia);
            var ScrollNested = toggleParent.GetComponentInChildren<ScrollRectNested>();
            var myswipymenu = toggleParent.GetComponentInParent<SwipyMenu>();
            ScrollNested.swipyMenu = myswipymenu;
            foreach (var p in piatti)
            {
                if (!p.Esaurito)
                {
                    var toggleRoot = toggleParent.GetComponentInChildren<ToggleGroup>();
                    if (NaturalOrientation.tablet == false)
                    {
                        Instantiate(temp.C2.sampleButtonPhone, toggleRoot.gameObject.transform);
                    }
                    else
                    {
                        Instantiate(temp.C2.sampleButtonTablet, toggleRoot.gameObject.transform);
                    }
                    toggleRoot.transform.GetChild(i).gameObject.SetActive(true);
                    var  toggle = toggleRoot.GetComponentsInChildren<Toggle>();

                    toggle[i].name = i.ToString();
                    //var toggleImg = toggleRoot.GetComponentsInChildren<Image>();
                    var toggleImage = toggleRoot.GetComponentsInChildren<Image>();
                    if (ColorController.ColoreRiquadri != new Color(0,0,0,0))
                    {
                        foreach (Image im in toggleImage)
                        {
                            toggleImage[i].color = ColorController.ColoreRiquadri;
                        }
                    }
                        /*Image ToggleBackground = toggleAct[i - hidden].image;
                    Image[] ToggleCheckmark = ToggleBackground.GetComponentsInChildren<Image>();
                    if (SessionMode[i] == 1)
                        ToggleCheckmark[1].color = new Color(0.29F, 0.94F, 0.58F, 0.31F);
                    else
                        ToggleCheckmark[1].color = new Color(0.94F, 0F, 0F, 0.31F);
                    //toggleAct[i-hidden].tag = RecordId[i].ToString();
    */
                    int id = LangEngine.Lan_ID;
                    switch (id)
                    {
                        case 1:
                            if (piatti[i].Nome_EN.Length > 0)
                                piatti[i].Nome = piatti[i].Nome_EN;
                            break;
                        case 2:
                            break;
                        case 3:
                            if (piatti[i].Nome_RU.Length > 0)
                                piatti[i].Nome = piatti[i].Nome_RU;
                            break;
                        case 4:
                            if (piatti[i].Nome_ES.Length > 0)
                                piatti[i].Nome = piatti[i].Nome_ES;
                            break;
                        case 5:
                            if (piatti[i].Nome_GE.Length > 0)
                                piatti[i].Nome = piatti[i].Nome_GE;
                            break;
                        case 6:
                            if (piatti[i].Nome_FR.Length > 0)
                                piatti[i].Nome = piatti[i].Nome_FR;
                            break;
                        default:
                           
                            break;
                    }
                    var  toggleText = toggle[i].GetComponentsInChildren<Text>();
                    toggleText[0].text = (piatti[i].Nome);
                    toggleText[1].text = ("€ " + piatti[i].Prezzo.ToString());
         
                    Toggle togg = toggle[i];
                    togg.onValueChanged.AddListener(delegate {
                        ToggleValueChanged(togg);
                    });
                    toggles.Add(toggle);
                }
                i++;
            }
        }
       
    }
   
    #region VISUALIZZA_SINGOLO_PIATTO
    void ToggleValueChanged(Toggle toggle)
    {
        var toggleText = toggle.GetComponentsInChildren<Text>();
        string piattoselezionato = toggleText[0].text; 
        foreach (var tipologia in menujson.Tipologia)
        {
            foreach(var p in menujson.Piatti)
            {
                if (p.Nome == piattoselezionato)
                {
                    string all = "";
                    foreach (var a in p.Allergeni)
                    {
                        try
                        {
                            all += menujson.Allergene[a] + ", ";
                        }
                        catch { }
                    }
                    if(ContentCreator.Db == 0)
                    ProductDescription.C4.CreateCanvas(p.Nome, p.Ingredienti, all, p.UrlFoto, p.EsclIngredienti, p.Prezzo, p.ConFoto,p.idart, p.Note, p.GUID);
                    else if (ContentCreator.Db == 1)
                        ProductDescription.C4.CreateCanvas(p.Nome, p.Ingredienti + p.Note, all, p.base64Img, p.EsclIngredienti, p.Prezzo, true, p.idart, "", p.GUID);
                    return;
                }
            }
        }
    }
   
    #endregion

    static List<string> ToStr(int[] src)
    {
        return (from v in src select v.ToString()).ToList();
    }
     
public static Menu_Ristorante LoadMenu(string url, int db)
    {
        #region LOADER_DATI
        
        WebClient client = new WebClient();

        /*
        string data = Encoding.Default.GetString(client.DownloadData(url));
        
        //Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(data));

        //StreamReader reader = new StreamReader(stream);

        //Menu menu = JsonConvert.DeserializeObject(typeof(Menu));
        Stream stream = new MemoryStream();
        
        Menu menu = new Menu();

        menu.Piatti.Add(new Piatto { Nome = "Padellina pavarazze alla marinara", Nome_EN = "Clams with wine sauce", Nome_GE = "Venusmuscheln in Weissweinsauce", Allergeni = ToStr(new int[] { 1, 4, 12, 14, 15 }), Tipo = "ANTIPASTI DI MARE CALDI E FREDDI", Prezzo = 12f });
        menu.Piatti.Add(new Piatto { Nome = "Canolicchi gratinati ", Nome_EN = "Broiled razor clam", Nome_GE = "Gratinierte Messermuscheln", Allergeni = ToStr(new int[] { 1, 2, 4, 12, 14 }), Tipo = "ANTIPASTI DI MARE CALDI E FREDDI", Prezzo = 12f });
        menu.Piatti.Add(new Piatto { Nome = "Antipasto Molinetto (assaggi di antipasti)", Nome_EN = "Mixed starter \"Molinetto\"", Nome_GE = "Gemischte Fischvorspeisenhäppchen Molinetto", Allergeni = ToStr(new int[] { 1, 2, 3, 4, 7, 9, 12, 14 }), Tipo = "ANTIPASTI DI MARE CALDI E FREDDI", Prezzo = 17f });
        menu.Piatti.Add(new Piatto { Nome = "Antipasto Caldo di mare (Polipo arrostito, gamberoni con aceto balsamico e pancetta, capesante gratinate, tortino di salmone)", Nome_EN = "Roasted octopus, King prawn with balsamic vinegar and fried bacon, scallops au gratin, Salmon and fresh cheese  ", Nome_GE = "Gerösteter Polyp, Riesengarnele mit Balsamicoessig und Bauchspeck, gratinierte Jakobsmuschel, Lachstörtchen ", Allergeni = ToStr(new int[] { 1, 2, 3, 4, 7, 12, 14, 15 }), Tipo = "ANTIPASTI DI MARE CALDI E FREDDI", Prezzo = 18f });
        menu.Piatti.Add(new Piatto { Nome = "Grand Piatto di Antipasti Caldi e Freddi per 2 Persone", Nome_EN = "Big Plate of starter hot and cold for 2 persons ", Nome_GE = "Grosser Vorspeisenteller warm und kalt für 2 Personen", Allergeni = ToStr(new int[] { 1, 2, 3, 4, 7, 9, 12, 14, 15 }), Tipo = "ANTIPASTI DI MARE CALDI E FREDDI", Prezzo = 55f });
        menu.Piatti.Add(new Piatto { Nome = "Gamberi con pancetta e aceto balsamico (5 Pz.)", Nome_EN = "King prawn with bacon and balsamic vinegar", Nome_GE = "Riesengarnelen mit Balsamicoessig und Bauchspeck", Allergeni = ToStr(new int[] { 2, 4, 12 }), Tipo = "ANTIPASTI DI MARE CALDI E FREDDI", Prezzo = 14f });
        menu.Piatti.Add(new Piatto { Nome = "Alici marinate con cipolla di Tropea ", Nome_EN = "Marinated anchovies with toast ", Nome_GE = "marinierte Sardellen mit Tropea-Zwiebelringe und Röstbrot", Allergeni = ToStr(new int[] { 4, 12 }), Tipo = "ANTIPASTI DI MARE CALDI E FREDDI", Prezzo = 12f });
        menu.Piatti.Add(new Piatto { Nome = "Cocktail di gamberi in salsa rosa e agrumi", Nome_EN = "Prawn cocktail with pink sauce and citrus", Nome_GE = "Krabbencocktail in Rosasauce mit Zitrusfrüchten", Allergeni = ToStr(new int[] { 2, 3, 4, 12 }), Tipo = "ANTIPASTI DI MARE CALDI E FREDDI", Prezzo = 12f });
        menu.Piatti.Add(new Piatto { Nome = "Polipo con patate e olive nere", Nome_EN = "Octopus with potatoes and black olives ", Nome_GE = "Polyp mit schwarzen Oliven und Kartoffeln", Allergeni = ToStr(new int[] { 4, 12, 14 }), Tipo = "ANTIPASTI DI MARE CALDI E FREDDI", Prezzo = 14f });
        menu.Piatti.Add(new Piatto { Nome = "Polipo arrostito con verdurine croccanti", Nome_EN = "Roasted octopus with roasted vegetables", Nome_GE = "Gerösteter Polyp mit gedünsteten Gemüse ", Allergeni = ToStr(new int[] { 4, 15 }), Tipo = "ANTIPASTI DI MARE CALDI E FREDDI", Prezzo = 16f });
        menu.Piatti.Add(new Piatto { Nome = "Insalata di mare", Nome_EN = "Seafood salad ", Nome_GE = "Meeresfrüchtesalat", Allergeni = ToStr(new int[] { 2, 4, 9, 12, 14 }), Tipo = "ANTIPASTI DI MARE CALDI E FREDDI", Prezzo = 9f });
        menu.Piatti.Add(new Piatto { Nome = "Tortino di salmone e squacquerone", Nome_EN = "Salmon and fresh cheese", Nome_GE = "Lachstörtchen mit Frischkäse", Allergeni = ToStr(new int[] { 1, 3, 4, 7 }), Tipo = "ANTIPASTI DI MARE CALDI E FREDDI", Prezzo = 10f });
        menu.Piatti.Add(new Piatto { Nome = "Poker di capesante gratinate (4 Pz.)", Nome_EN = "Four scallops au gratin", Nome_GE = "gratinierte Jakobsmuscheln", Allergeni = ToStr(new int[] { 1, 4, 12, 14 }), Tipo = "ANTIPASTI DI MARE CALDI E FREDDI", Prezzo = 15f });
        menu.Piatti.Add(new Piatto { Nome = "Acciughe del Cantabrico \"Nardin\" con burrata e pan brioches", Nome_EN = "Anchovies from Cantabrico \"Nardin\" with burrata cheese and Pan brioches ", Nome_GE = "Sardellen vom Cantabricomeer \"Nardin\" mit Burrata Käse  und Pan Brioche", Allergeni = ToStr(new int[] { 1, 4, 7 }), Tipo = "ANTIPASTI DI MARE CALDI E FREDDI", Prezzo = 14f });
        menu.Piatti.Add(new Piatto { Nome = "Ostrica fine de Claire", Nome_EN = "Oyster \"fine de Claire\" ", Nome_GE = "Austern \"fine de claire\"", Allergeni = ToStr(new int[] { 4, 12, 14 }), Tipo = "LE CRUDITA’", Prezzo = 12f });


        menu.Piatti.Add(new Piatto { Nome = "Carpaccio di pesce spada agli agrumi", Nome_EN = "Paper-thin slices of raw swordfish with citrus fruits", Nome_GE = "Schwertfisch-Carpaccio mit Zitrusfrüchten", Allergeni = ToStr(new int[] { 4 }), Tipo = "LE CRUDITA’", Prezzo = 12f });
        menu.Piatti.Add(new Piatto { Nome = "Tartare di tonno con salsa Guacamole", Nome_EN = "Tunafish tartar", Nome_GE = "Thunfisch Tatar", Allergeni = ToStr(new int[] { 4, 7 }), Tipo = "LE CRUDITA’", Prezzo = 16f });
        menu.Piatti.Add(new Piatto { Nome = "Tartare di gamberi", Nome_EN = "Shimps tartar", Nome_GE = "Garnelen Tatar", Allergeni = ToStr(new int[] { 2, 4, 12 }), Tipo = "LE CRUDITA’", Prezzo = 18f });
        menu.Piatti.Add(new Piatto { Nome = "Gran piatto di crudità secondo mercato salmone, tonno, branzino, scampi, ombrina, ricciola, ostrica)", Nome_EN = "Large dish of raw fish (salmon, tuna, sea bass, scampi, drum fish, amberjack, oyster)", Nome_GE = "grosser Teller mit rohem Fisch und Krebstieren (Lachs, Thunfisch, Wolfsbarsch, Scampi, Umberfisch, Bernsteinmakrele, und Austern)", Allergeni = ToStr(new int[] { 2, 4, 11, 12, 14 }), Tipo = "LE CRUDITA’", Prezzo = 35f });
        menu.Piatti.Add(new Piatto { Nome = "Piccola crudità(Salmone, Pesce Spada, Tonno, Scampo e 1 ostrica)", Nome_EN = "small row fish (salmon,sword fish, tuna, crayfish and 1 oyster", Nome_GE = "kleine rohe Fischvorspeise (Lachs,Schwertfisch,Thunfisch, Scampo und 1 Auster)", Allergeni = ToStr(new int[] { 2, 4, 11, 12, 14 }), Tipo = "LE CRUDITA’", Prezzo = 22f });



        menu.Piatti.Add(new Piatto { Nome = "Piadina di nostra produzione", Nome_EN = "thin unleavened flatbread house maked", Nome_GE = "ungesäuertes Fladenbrot hausgemacht", Allergeni = ToStr(new int[] { 1 }), Tipo = "ANTIPASTI CLASSICI", Prezzo = 1.50f });
        menu.Piatti.Add(new Piatto { Nome = "Piadina squacquerone e crudo di Parma", Nome_EN = "flatbread house maked with fresh cheese and Parma ham", Nome_GE = "Fladenbrot mit Frischkäse und Parmaschinken", Allergeni = ToStr(new int[] { 1, 7 }), Tipo = "ANTIPASTI CLASSICI", Prezzo = 9f });
        menu.Piatti.Add(new Piatto { Nome = "Piadina squacquerone e fichi caramellati", Nome_EN = "flatbread house maked with fresh cheese and caramelized fig", Nome_GE = "Fladenbrot mit Frischkäse und karamellisierten Feigen", Allergeni = ToStr(new int[] { 1, 7 }), Tipo = "ANTIPASTI CLASSICI", Prezzo = 9f });
        menu.Piatti.Add(new Piatto { Nome = "Crostini misti (4PZ)", Nome_EN = "savoury mixed toast", Nome_GE = "gem. Röstbrot bunt belegt", Allergeni = ToStr(new int[] { 1, 7, 12, 15 }), Tipo = "ANTIPASTI CLASSICI", Prezzo = 8f });
        menu.Piatti.Add(new Piatto { Nome = "Crostone con funghi porcini, scamorza e Pata Negra (3PZ)", Nome_EN = "Savoury toast with mushrooms, scamorza cheese and Pata Negra ham", Nome_GE = "Röstbrot mit Steinpilzen, Scamorzakäse und Pata Negra-Rohschinken", Allergeni = ToStr(new int[] { 1, 7, 12 }), Tipo = "ANTIPASTI CLASSICI", Prezzo = 15f });
        menu.Piatti.Add(new Piatto { Nome = "Prosciutto crudo di Parma con melone", Nome_EN = "Parma ham with melon", Nome_GE = "Parmaschinken mit Melone", Allergeni = ToStr(new int[] { }), Tipo = "ANTIPASTI CLASSICI", Prezzo = 10f });
        menu.Piatti.Add(new Piatto { Nome = "Tortino di erbette e squacquerone con salsa al parmigiano ", Nome_EN = "Green pie with spinach and fresh cheese", Nome_GE = "kl.Törtchen mit Spinat und Frischkäse", Allergeni = ToStr(new int[] { 1, 3, 7 }), Tipo = "ANTIPASTI CLASSICI", Prezzo = 10f });
        menu.Piatti.Add(new Piatto { Nome = "Tartare di manzo con nocciole piemontesi", Nome_EN = "beef tartare with hazelnut ", Nome_GE = "Rindstatar mit Hasselnüssen", Allergeni = ToStr(new int[] { 3, 8 }), Tipo = "ANTIPASTI CLASSICI", Prezzo = 15f });
        menu.Piatti.Add(new Piatto { Nome = "Carpaccio di manzo rucola, grana e noci", Nome_EN = "Paper-thin slices of beef with racket, parmesan cheese and nuts", Nome_GE = "Rinds-Carpaccio mit Rauke, Parmesankäse und Nüssen", Allergeni = ToStr(new int[] { 7, 8 }), Tipo = "ANTIPASTI CLASSICI", Prezzo = 12f });
        menu.Piatti.Add(new Piatto { Nome = "Pata Negra con crostini caldi", Nome_EN = "Pata Negro-Rohschinkenart mit Röstbrot", Nome_GE = "Pata Negro - Rohschinkenart mit Röstbrot", Allergeni = ToStr(new int[] { 1 }), Tipo = "ANTIPASTI CLASSICI", Prezzo = 15f });
        menu.Piatti.Add(new Piatto { Nome = "Galà di Formaggi  con confettura", Nome_EN = "Mixed cheeses with jam", Nome_GE = "Käseteller gemischt mit Konfitüre", Allergeni = ToStr(new int[] { 1, 7, 8, 12 }), Tipo = "ANTIPASTI CLASSICI", Prezzo = 10f });



        menu.Piatti.Add(new Piatto { Nome = "con crudo, salame, piadina, crostini, squacquerone, coppa e pecorino", Nome_EN = "with parma ham, sausages, piadina, pork, fresh and sheep’s cheese", Nome_GE = "mit Rohschinken, Salami, Piadinabrot, Presskopf und Schafskäse", Allergeni = ToStr(new int[] { 1, 7 }), Tipo = "TAGLIERE ROMAGNOLO", Prezzo = 18f });


        menu.Piatti.Add(new Piatto { Nome = "Tagliolini alla marinara", Nome_EN = "Noodles with seafood", Nome_GE = "kl. Bandnudeln mit Meeresfrüchten", Allergeni = ToStr(new int[] { 1, 2, 3, 4, 12, 14 }), Tipo = "PRIMI PIATTI DI MARE", Prezzo = 9f });
        menu.Piatti.Add(new Piatto { Nome = "Strichetti gamberi e zucchine", Nome_EN = "Noodles with crayfish and zucchinis", Nome_GE = "Schmetterlingsnudeln mit Krabben und Zucchini", Allergeni = ToStr(new int[] { 1, 2, 3, 4, 12 }), Tipo = "PRIMI PIATTI DI MARE", Prezzo = 10f });
        menu.Piatti.Add(new Piatto { Nome = "Tagliolini al salmone", Nome_EN = "Noodles with salmon and cream", Nome_GE = "kl.Bandnudeln mit Lachs und Sahne", Allergeni = ToStr(new int[] { 1, 3, 4, 7 }), Tipo = "PRIMI PIATTI DI MARE", Prezzo = 9f });
        menu.Piatti.Add(new Piatto { Nome = "Caramelle di pesce ripiene di crostacei con branzino pomodoro fresco e timo", Nome_EN = "Crostaceo caramelle with fresh tomato, seabass and thyme", Nome_GE = "Gefüllte Teigware (Krustentierfüllung) mit frischen Tomaten, Wolfsbarsch und Thymian", Allergeni = ToStr(new int[] { 1, 2, 3, 4, 7, 12 }), Tipo = "PRIMI PIATTI DI MARE", Prezzo = 13f });
        menu.Piatti.Add(new Piatto { Nome = "Strozzapreti porcini, panna e vongole", Nome_EN = "Noodles with mushroom cream and clams", Nome_GE = "Spätzle-Art mit Steinpilzen, Sahne und Venusmuscheln", Allergeni = ToStr(new int[] { 1, 3, 4, 7, 12, 15 }), Tipo = "PRIMI PIATTI DI MARE", Prezzo = 15f });
        menu.Piatti.Add(new Piatto { Nome = "Garganelli pesto e gamberi", Nome_EN = "Garganelli (a kind of italian pasta) with pesto and prawns ", Nome_GE = "Makkaroni-art  mit Pesto und Garnelen", Allergeni = ToStr(new int[] { 1, 3, 4, 7, 12, 15 }), Tipo = "PRIMI PIATTI DI MARE", Prezzo = 10f });
        menu.Piatti.Add(new Piatto { Nome = "Gnocchi ai gamberi e curry", Nome_EN = "Gnocchi with clams and curry", Nome_GE = "Gnocchi mit Krabben und Curry", Allergeni = ToStr(new int[] { 1, 3, 4, 7, 12, 15 }), Tipo = "PRIMI PIATTI DI MARE", Prezzo = 12f });
        menu.Piatti.Add(new Piatto { Nome = "Gnocchi mazzancolle carciofi e zafferano", Nome_EN = "Gnocchi with shrimp, artichokes and saffron", Nome_GE = "Gnocchi mit Shrimps, Artischocken und Safran", Allergeni = ToStr(new int[] { 1, 3, 4, 7, 12, 15 }), Tipo = "PRIMI PIATTI DI MARE", Prezzo = 15f });
        menu.Piatti.Add(new Piatto { Nome = "Spaghetti alle vongole", Nome_EN = "Spaghetti with clams", Nome_GE = "Spaghetti mit Venusmuscheln", Allergeni = ToStr(new int[] { 1, 4, 12, 14, 15 }), Tipo = "PRIMI PIATTI DI MARE", Prezzo = 14f });
        menu.Piatti.Add(new Piatto { Nome = "Risotto alla marinara", Nome_EN = "Risotto with seafood", Nome_GE = "Risotto mit Meeresfrüchten", Allergeni = ToStr(new int[] { 1, 4, 12, 14, 15 }), Tipo = "PRIMI PIATTI DI MARE", Prezzo = 11f });
        menu.Piatti.Add(new Piatto { Nome = "Risotto gamberi e zucchine", Nome_EN = "Risotto with crayfisch and zucchinis", Nome_GE = "Risotto mit Krabben und Zucchini", Allergeni = ToStr(new int[] { 2, 4, 12, 15 }), Tipo = "PRIMI PIATTI DI MARE", Prezzo = 11f });
        menu.Piatti.Add(new Piatto { Nome = "Risotto al nero di seppia", Nome_EN = "Risotto with cuttlefish ink", Nome_GE = "Schwarzes Fischrisotto", Allergeni = ToStr(new int[] { 2, 4, 12, 14, 15 }), Tipo = "PRIMI PIATTI DI MARE", Prezzo = 12f });
        menu.Piatti.Add(new Piatto { Nome = "Spaghetti allo scoglio", Nome_EN = "Spaghetti with mixed entire seafood and tomatoes sauce", Nome_GE = "Spaghetti mit ganzen Meeresfrüchten und Tomatensauce", Allergeni = ToStr(new int[] { 1, 2, 4, 12, 14, 15 }), Tipo = "PRIMI PIATTI DI MARE", Prezzo = 17f });
        menu.Piatti.Add(new Piatto { Nome = "Risotto allo scoglio bianco", Nome_EN = "Risotto with mixed entire seafood", Nome_GE = "Risotto mit ganzen Meeresfrüchten", Allergeni = ToStr(new int[] { 2, 4, 12, 14, 15 }), Tipo = "PRIMI PIATTI DI MARE", Prezzo = 17f });
        menu.Piatti.Add(new Piatto { Nome = "Strozzapreti allo scoglio", Nome_EN = "noodle with mixed entire seafood and tomatoes sauce", Nome_GE = "Spätzle-art mit ganzen Meeresfrüchten und Tomatensauce", Allergeni = ToStr(new int[] { 1, 2, 3, 4, 12, 14, 15 }), Tipo = "PRIMI PIATTI DI MARE", Prezzo = 17f });
        menu.Piatti.Add(new Piatto { Nome = "Risotto all’astice fresco (dal nostro acquario)", Nome_EN = "Risotto  maked with fresh European lobster", Nome_GE = "Risotto mit frischen Hummer", Allergeni = ToStr(new int[] { 2, 4, 7, 12, 15 }), Tipo = "PRIMI PIATTI DI MARE", Prezzo = 25f });
        menu.Piatti.Add(new Piatto { Nome = "Paccheri all’astice fresco (dal nostro acquario)", Nome_EN = "Paccheri (a kind of italian pasta) with fresh European lobster", Nome_GE = "Paccheri (Nudelart) mit frischen Hummer ", Allergeni = ToStr(new int[] { 1, 2, 4, 7, 12, 15 }), Tipo = "PRIMI PIATTI DI MARE", Prezzo = 25f });
        menu.Piatti.Add(new Piatto { Nome = "Spaghetti alla chitarra all’astice fresco (dal nostro acquario)", Nome_EN = "Noodles house maked with fresh European lobster", Nome_GE = "drahtgeschnittene Spaghetti mit frischen Hummer", Allergeni = ToStr(new int[] { 1, 2, 3, 4, 7, 12, 15 }), Tipo = "PRIMI PIATTI DI MARE", Prezzo = 25f });
        menu.Piatti.Add(new Piatto { Nome = "Strozzapreti \"Buonissimi\" (vongole, cozze, gamberi, zucchine e asparagi)", Nome_EN = "Noodles \"Buonissimi\" (with clams, mussels, crayfish, zucchinis and asparagus)", Nome_GE = "Spätzle-art \"Buonissimi\" (mit Venus u. Miesmuscheln, Krabben, Zucchini und Spargelspitzen)", Allergeni = ToStr(new int[] { 1, 2, 3, 4, 12, 14, 15 }), Tipo = "PRIMI PIATTI DI MARE", Prezzo = 14f });
        menu.Piatti.Add(new Piatto { Nome = "Passatelli porcini, gamberi e veraci", Nome_EN = "Passatelli with porcini mushrooms, clams and rowns", Nome_GE = "Passatelli mit Steinpilzen, Garnelen und Venusmuscheln", Allergeni = ToStr(new int[] { 1, 2, 3, 4, 7, 12, 14, 15 }), Tipo = "PRIMI PIATTI DI MARE", Prezzo = 15f });
        menu.Piatti.Add(new Piatto { Nome = "Spaghetti alla chitarra pesce spada melanzane pinoli tostati e basilico", Nome_EN = "Handmade spaghetti with swordfish, aubergines, toasted pine nuts and basil", Nome_GE = "Hausgemachte, Spaghetti mit Schwertfisch,  Auberginen, gerösteten Pinienkernen und Basilikum", Allergeni = ToStr(new int[] { 1, 3, 4, 5, 8, 12, 15 }), Tipo = "PRIMI PIATTI DI MARE", Prezzo = 14f });
        menu.Piatti.Add(new Piatto { Nome = "Spaghetti alla chitarra, tonno fresco, bottarga pomodorini e olive", Nome_EN = "Handmade spaghetti with fresh tuna, bottarga, small rounded tomatoes and black olives", Nome_GE = "Hausgemachte Spaghetti mit frischem Thunfisch und Bottarga, Cocktailtomaten und Oliven", Allergeni = ToStr(new int[] { 1, 3, 4, 15 }), Tipo = "PRIMI PIATTI DI MARE", Prezzo = 15f });
        menu.Piatti.Add(new Piatto { Nome = "Bis di mare (Tagliolini alla marinara e Caramelle di pesce con branzino pomodoro fresco e timo  min. 2 pers.)", Nome_EN = "Two fish dishes (Noodles with seafood and Crustaceans caramelle with fresh tomato, seabass and thyme - min 2 pax.)", Nome_GE = "Zwei versch. Nudelgerichte auf einem Teller (kl. Bandnudeln mit Meeresfrüchten, gefüllte Teigware (Krustentierfüllung)mit frischen Tomaten, Wolfsbarsch und Thymian - min. 2 pers.)", Allergeni = ToStr(new int[] { 1, 2, 3, 4, 7, 12, 14, 15 }), Tipo = "PRIMI PIATTI DI MARE", Prezzo = 12f });



        menu.Piatti.Add(new Piatto { Nome = "Tagliatelle al ragù", Nome_EN = "Noodles with meat sauce", Nome_GE = "Bandnudeln mit Hackfleischsauce", Allergeni = ToStr(new int[] { 1, 3, 9, 12, 15 }), Tipo = "PRIMI PIATTI CLASSICI", Prezzo = 9f });
        menu.Piatti.Add(new Piatto { Nome = "Strozzapreti panna e speck", Nome_EN = "Noodles with cream and smoked raw ham", Nome_GE = "Spätzle-art mit Sahne und Speck", Allergeni = ToStr(new int[] { 1, 3, 7 }), Tipo = "PRIMI PIATTI CLASSICI", Prezzo = 8f });
        menu.Piatti.Add(new Piatto { Nome = "Strozzapreti salsiccia e sangiovese", Nome_EN = "Noodles with sousages and sagiovese wine", Nome_GE = "Spätzle-Art mit ital. Hackfleischwurst und Rotweinsauce", Allergeni = ToStr(new int[] { 1, 3, 7, 12 }), Tipo = "PRIMI PIATTI CLASSICI", Prezzo = 9f });
        menu.Piatti.Add(new Piatto { Nome = "Garganelli panna, salsiccia e piselli", Nome_EN = "Noodles with cream,sausage and peas", Nome_GE = "Makkaroni-art mit Sahne, ital. Hackfleischwurst und Speck", Allergeni = ToStr(new int[] { 1, 3, 7, 12 }), Tipo = "PRIMI PIATTI CLASSICI", Prezzo = 8f });
        menu.Piatti.Add(new Piatto { Nome = "Passatelli in brodo", Nome_EN = "passatelli in broth", Nome_GE = "Passatelli mit Fleischbrühe", Allergeni = ToStr(new int[] { 1, 3, 7, 9 }), Tipo = "PRIMI PIATTI CLASSICI", Prezzo = 8.5f });
        menu.Piatti.Add(new Piatto { Nome = "Cappelletti in brodo", Nome_EN = "Cheese-filled Cappelletti in broth", Nome_GE = "Gef. Teigware(Käsefüllung)mit Fleischbrühe", Allergeni = ToStr(new int[] { 1, 3, 7, 9 }), Tipo = "PRIMI PIATTI CLASSICI", Prezzo = 8.5f });
        menu.Piatti.Add(new Piatto { Nome = "Cappelletti al ragù", Nome_EN = "Cheese-filled Cappelletti with meat sauce", Nome_GE = "Gef. Teigware(Käsefüllung) mit Hackfleischsauce", Allergeni = ToStr(new int[] { 1, 3, 9, 12, 15 }), Tipo = "PRIMI PIATTI CLASSICI", Prezzo = 10f });
        menu.Piatti.Add(new Piatto { Nome = "Cappelletti panna e speck", Nome_EN = "Cheese-filled Cappelletti with cream and smoked raw ham", Nome_GE = "Gef. Teigware (Käsefüllung) mit Sahne und Speck", Allergeni = ToStr(new int[] { 1, 3, 7 }), Tipo = "PRIMI PIATTI CLASSICI", Prezzo = 10f });
        menu.Piatti.Add(new Piatto { Nome = "Cappelletti panna, asparagi e prosciutto crudo", Nome_EN = "Cheese-filled Cappelletti with cream, asparagus and parma ham", Nome_GE = "Gef. Teigware (Käsefüllung) Spargel, Sahne und Parma schinken", Allergeni = ToStr(new int[] { 1, 3, 7 }), Tipo = "PRIMI PIATTI CLASSICI", Prezzo = 9f });
        menu.Piatti.Add(new Piatto { Nome = "Cappelletti all’oro con zafferano, pinoli e erba cipollina", Nome_EN = "Cheese-filled Cappelletti with saffron, pine-seeds and chives", Nome_GE = "Gef. Teigware (Käsefüllung) mit Safran, Pinienkerne und Schnittlauch", Allergeni = ToStr(new int[] { 1, 3, 7, 8 }), Tipo = "PRIMI PIATTI CLASSICI", Prezzo = 10f });
        menu.Piatti.Add(new Piatto { Nome = "Tortelli burro e salvia", Nome_EN = "Tortelli(Ricotta and spinach-filled) with butter and sage", Nome_GE = "Gef. Teigware (Ricotta u. Spinat) mit Butter und Salbei", Allergeni = ToStr(new int[] { 1, 3, 7 }), Tipo = "PRIMI PIATTI CLASSICI", Prezzo = 8.5f });
        menu.Piatti.Add(new Piatto { Nome = "Tortelli porcini, noci e mascarpone", Nome_EN = "Tortelli (Ricotta and spinach-filled) with mushrooms,walnuts and mascarpone cheese", Nome_GE = "Gef. Teigware mit Spargel und Mascarpone", Allergeni = ToStr(new int[] { 1, 3, 7, 8, 12, 15 }), Tipo = "PRIMI PIATTI CLASSICI", Prezzo = 11f });
        menu.Piatti.Add(new Piatto { Nome = "Tagliolini allo Scalogno", Nome_EN = "tagliolini with shallots", Nome_GE = "Tagliolini mit Schalotten", Allergeni = ToStr(new int[] { 1, 3 }), Tipo = "PRIMI PIATTI CLASSICI", Prezzo = 8.5f });
        menu.Piatti.Add(new Piatto { Nome = "Gnocchi gorgonzola e noci", Nome_EN = "Gnocchi with gorgonzola cheese and walnuts", Nome_GE = "Gnocchi mit Gorgonzolakäse, Nüsse und Sahne", Allergeni = ToStr(new int[] { 1, 3, 7, 8 }), Tipo = "PRIMI PIATTI CLASSICI", Prezzo = 10f });
        menu.Piatti.Add(new Piatto { Nome = "Passatelli con funghi porcini e crema di parmigiano, tartufati", Nome_EN = "Passatelli  with porcini mushrooms, truffles and parmigiano cream", Nome_GE = "Passatelli mit Steinpilzen,Trüffel und Parmesancreme", Allergeni = ToStr(new int[] { 1, 3, 7, 15 }), Tipo = "PRIMI PIATTI CLASSICI", Prezzo = 13f });
        menu.Piatti.Add(new Piatto { Nome = "Spaghetti alla chitarra con basilico, pomodoro, melanzane e ricotta stagionata", Nome_EN = "Noodles house maked with fresh basil, tomato, aubergines and seasoned ricotta cheese", Nome_GE = "Hausgemachte, Spaghetti mit Basilikum,  Tomatensauce, Auberginen, Basilikum und gereifter Ricotta", Allergeni = ToStr(new int[] { 1, 3, 7 }), Tipo = "PRIMI PIATTI CLASSICI", Prezzo = 11f });
        menu.Piatti.Add(new Piatto { Nome = "TRIS \"MOLINETTO\" min. 2 pers. Cappelletti ragù; Tortelli burro e salvia; Strozzapreti panna e speck", Nome_EN = "Cappelletti ragù; Tortelli burro e salvia; Strozzapreti panna e speck3 noodles courses \"MOLINETTO\" off one plaidCheese-filled Cappelletti with meat sauce Tortelli (Ricotta and spinach-filled) with butter and sage Noodles with cream and smoked raw ham", Nome_GE = "3 Nudelgerichte \"MOLINETTO\" auf einem TellerGef. Teigware (Käsefüllung) mit HackfleischsauceGef. Teigware (Ricotta u. Spinat) mit Butter und SalbeiSpätzle-art mit Sahne und Speck", Allergeni = ToStr(new int[] { 1, 3, 7, 912, 15 }), Tipo = "PRIMI PIATTI CLASSICI", Prezzo = 12f });



        menu.Piatti.Add(new Piatto { Nome = "Fritto misto", Nome_EN = "mixed fried fish", Nome_GE = "fritierte Meeresfrüchte", Allergeni = ToStr(new int[] { 1, 2, 3, 4, 5, 12, 14 }), Tipo = "SECONDI PIATTI DI MARE", Prezzo = 15f });
        menu.Piatti.Add(new Piatto { Nome = "Fischioni (calamari alla carta)", Nome_EN = "Whole grilled calamari \"Bellavista\"", Nome_GE = "In ganzen gegrillte Calamari \"Bellavista\"", Allergeni = ToStr(new int[] { 1, 4, 12, 14 }), Tipo = "SECONDI PIATTI DI MARE", Prezzo = 15f });
        menu.Piatti.Add(new Piatto { Nome = "Anelli di calamari*e ciuffi", Nome_EN = "Fried calamari rings", Nome_GE = "fritierte Calamari-Ringe", Allergeni = ToStr(new int[] { 1, 3, 4, 5, 12, 14 }), Tipo = "SECONDI PIATTI DI MARE", Prezzo = 14f });
        menu.Piatti.Add(new Piatto { Nome = "Spiedini misti", Nome_EN = "Calamaries, crayfish and cuttlefish brocchettes*", Nome_GE = "Garnelen-Tintenfisch und Krabbenspieße", Allergeni = ToStr(new int[] { 1, 2, 4, 12, 14 }), Tipo = "SECONDI PIATTI DI MARE", Prezzo = 15f });
        menu.Piatti.Add(new Piatto { Nome = "Coda di rospo alla griglia", Nome_EN = "Grilled monkfish", Nome_GE = "Gegrillter Seeteufel", Allergeni = ToStr(new int[] { 1, 4 }), Tipo = "SECONDI PIATTI DI MARE", Prezzo = 16f });
        menu.Piatti.Add(new Piatto { Nome = "Orata alla griglia", Nome_EN = "Grilled sea bream", Nome_GE = "Gegrillter Dorade", Allergeni = ToStr(new int[] { 1, 4 }), Tipo = "SECONDI PIATTI DI MARE", Prezzo = 16f });
        menu.Piatti.Add(new Piatto { Nome = "Branzino alla griglia", Nome_EN = "Grilled sea bass", Nome_GE = "Gegrillter  Wolfsbarsch", Allergeni = ToStr(new int[] { 1, 4 }), Tipo = "SECONDI PIATTI DI MARE", Prezzo = 16f });
        menu.Piatti.Add(new Piatto { Nome = "Pesce Spada alla griglia", Nome_EN = "Grilled swordfish", Nome_GE = "Gegrillter Schwertfisch", Allergeni = ToStr(new int[] { 1, 4 }), Tipo = "SECONDI PIATTI DI MARE", Prezzo = 15f });
        menu.Piatti.Add(new Piatto { Nome = "Sogliola alla griglia", Nome_EN = "Grilled sole", Nome_GE = "Gegrillter Seezunge", Allergeni = ToStr(new int[] { 1, 4 }), Tipo = "SECONDI PIATTI DI MARE", Prezzo = 15f });
        menu.Piatti.Add(new Piatto { Nome = "Filetto di Salmone alla griglia", Nome_EN = "grilled fillet of Salmon", Nome_GE = "gegrilltes Lachsfilets", Allergeni = ToStr(new int[] { 4 }), Tipo = "SECONDI PIATTI DI MARE", Prezzo = 17f });
        menu.Piatti.Add(new Piatto { Nome = "Filetto di Salmone al forno con arancio e scaglie di mandorla", Nome_EN = "baked salmon fillet with orange and almond flakes", Nome_GE = "gebakenes Lachsfilets mit Orangen und Mandelflocken", Allergeni = ToStr(new int[] { 4, 8, 12, 15 }), Tipo = "SECONDI PIATTI DI MARE", Prezzo = 19f });
        menu.Piatti.Add(new Piatto { Nome = "Filetto di branzino alle mele, mandorle tostate, balsamico alla pera e timo", Nome_EN = "Sea bass fillet with apples, toasted almonds, balsamic vinegar flavored with pear and thyme", Nome_GE = "Wolfsbarschfilet mit Äpfel, gerösteten Mandeln, Birnenbalsamico und Thymian", Allergeni = ToStr(new int[] { 4, 8, 12 }), Tipo = "SECONDI PIATTI DI MARE", Prezzo = 20f });
        menu.Piatti.Add(new Piatto { Nome = "Tagliata di tonno in crosta di sesamo con misticanza di verdure", Nome_EN = "Thinly sliced tuna fish with salad", Nome_GE = "Thunfischfilet in Scheiben auf gem. Salat", Allergeni = ToStr(new int[] { 4, 11 }), Tipo = "SECONDI PIATTI DI MARE", Prezzo = 18f });
        menu.Piatti.Add(new Piatto { Nome = "Grigliata mista di pesce (1/2 orata, spiedini di gamberi e seppia, pesce spada e mazzancolla)", Nome_EN = "grilled fish (with gilthead, 2 crayfish brochettes, swordfish and prawns)", Nome_GE = "gegrillter Fischteller (mit 1/2 Goldbrasse, Krabben und Tintenfischspieße, Schwertfisch und Riesengarnele)", Allergeni = ToStr(new int[] { 1, 4, 12, 14 }), Tipo = "SECONDI PIATTI DI MARE", Prezzo = 19f });
        menu.Piatti.Add(new Piatto { Nome = "Rombo a mosaico con mazzancolle e verdure grigliate", Nome_EN = "Grilled tubot with prawns and grilled vegetables ", Nome_GE = "Steinbutt vom Grill mit Riesengarnelen und gegr. Gemüse", Allergeni = ToStr(new int[] { 1, 2, 4, 9, 12, 15 }), Tipo = "SECONDI PIATTI DI MARE", Prezzo = 5.5f });
        menu.Piatti.Add(new Piatto { Nome = "Anguilla alla griglia con Polenta", Nome_EN = "Grilled eel  with polenta", Nome_GE = "Gegrillter Aal mit Polenta", Allergeni = ToStr(new int[] { 1, 4, 7 }), Tipo = "SECONDI PIATTI DI MARE", Prezzo = 5.5f });
        menu.Piatti.Add(new Piatto { Nome = "Spiedini di gamberi* argentini", Nome_EN = "Crayfish brocchettes*", Nome_GE = "Krabbenspieße*", Allergeni = ToStr(new int[] { 1, 2, 12 }), Tipo = "SECONDI PIATTI DI MARE", Prezzo = 16f });
        menu.Piatti.Add(new Piatto { Nome = "Mazzancolle al sale grosso con pinzimonio", Nome_EN = "Prawns with sea salt with fresh vegetables", Nome_GE = "Riesengarnelen mit groben Meeressalz und rohem Gemüse", Allergeni = ToStr(new int[] { 2, 9, 12 }), Tipo = "SECONDI PIATTI DI MARE", Prezzo = 19f });
        menu.Piatti.Add(new Piatto { Nome = "Grigliata di crostacei (1/2 astice fresco • 1 spiedino di gamberi *• 1 capasanta • 1 mazzancolla • 1 scampo)", Nome_EN = "Grilled seafood (1/2 fresh lobster • 1 skewered shrimp • 1 scallop • 1 prawn • 1 Norway lobster)", Nome_GE = "Gegrillte Meeresfrüchte (1/2 frischen Hummer • 1 Krabbenspieße• 1 Jakobsmuscheln • 1 Riesengarnele • 1 Kaisergranat)", Allergeni = ToStr(new int[] { 1, 2, 4, 12, 14 }), Tipo = "SECONDI PIATTI DI MARE", Prezzo = 29f });
        menu.Piatti.Add(new Piatto { Nome = "Astice fresco alla griglia con pinzimonio", Nome_EN = "Fresh grilled lobster with vegetables", Nome_GE = "Frischer gegrillter Hummer mit frischen  Gemüse", Allergeni = ToStr(new int[] { 1, 2, 12 }), Tipo = "SECONDI PIATTI DI MARE", Prezzo = 39f });



        menu.Piatti.Add(new Piatto { Nome = "CATALANA DI CROSTACEI (per 2 persone)", Nome_EN = "1 Spiny lobster • 2 prawns • 2 scampi • 2 scallops • 1 lobster • 2 crayfish brochettes ", Nome_GE = "1 Languste • 2 Riesengarnelen • 2 Scampi • 2 Jacobsmuscheln • 1 Hummer • 2 Krabbenspieße", Allergeni = ToStr(new int[] { 1, 2, 9, 12 }), Tipo = "SECONDI PIATTI DI MARE", Prezzo = 90f });



        menu.Piatti.Add(new Piatto { Nome = "Tagliere spiedo fritto per 2 persone (2 spiedini seppia • 4 spiedini gamberi* • 2 capesante • fritto misto • zucchine fritte)", Nome_EN = "Platter with mixed fish for 2 person(2cuttlefish brocchettes*  4 Shrimps brocchettes • 2 scallops • mixed fried fish • fried zucchinis)", Nome_GE = "gemischter Fischteller fuer 2 Personen(2 Tintenfischspieße • 4 Krabbenspieße* • 2 gratinierte Jacobsmuscheln • fritierte Meeresfrüchte • fritierte Zucchini)", Allergeni = ToStr(new int[] { 1, 2, 3, 5, 12, 14 }), Tipo = "I TAGLIERI DI MARE", Prezzo = 42f });



        menu.Piatti.Add(new Piatto { Nome = "Tagliere spiedo fritto per 3 persone (3 spiedini seppia • 6 spiedini gamberi* • 3 capesante • fritto misto • zucchine fritte )", Nome_EN = "Platter with mixed fish for 2 person(3 cuttlefish brocchettes* • 6 Shrimps brocchettes 3 scallops • mixed fried fish • fried zucchinis)", Nome_GE = "gemischter Fischteller fuer 2 Personen(3 Tintenfischspieße • 6 Krabbenspieße* • 3 gratinierte Jacobsmuscheln • fritierte Meeresfrüchte • fritierte Zucchini)", Allergeni = ToStr(new int[] { 1, 2, 3, 5, 12, 14 }), Tipo = "I TAGLIERI DI MARE", Prezzo = 66f });



        menu.Piatti.Add(new Piatto { Nome = "Tagliere del marinaio per 2 persone (1/2 orata • 1 coda di rospo • 2 spiedini gamberi* • 2 mazzancolle • fritto misto • zucchine fritte)", Nome_EN = "Platter with mixed grilled fish for 2 person(1/2 gilthead • angler-fish • 2 crayfish brocchettes* • 2 prawns • mixed fried fish • fried zucchinis)", Nome_GE = "gemischter gegrillter Fischteller fuer 2 Personen( 1/2 Goldbrasse • Seeteufel • 2 Krabbenspieße* • 2 Riesengarnelen • fritierte Meeresfrüchte • frietierte Zucchini)", Allergeni = ToStr(new int[] { 1, 2, 3, 4, 5, 12, 14 }), Tipo = "I TAGLIERI DI MARE", Prezzo = 44f });



        menu.Piatti.Add(new Piatto { Nome = "Tagliere del marinaio per 3 persone (1/2 orata • 1 coda di rospo • 3 mazzancolle • 3 spiedini di gamberi • 1 sogliola • fritto misto e zucchine fritte)", Nome_EN = "Platter with mixed grilled fish for 2 person(1/2 gilthead • angler-fish • 2 crayfish brocchettes* • 2 prawns • mixed fried fish • fried zucchinis)", Nome_GE = "gemischter gegrillter Fischteller fuer 2 Personen(1/2 Goldbrasse • Seeteufel • 2 Krabbenspieße* • 2 Riesengarnelen • frietierte Meeresfrüchte • fritierte Zucchini)", Allergeni = ToStr(new int[] { 1, 2, 3, 4, 5, 12, 14 }), Tipo = "I TAGLIERI DI MARE", Prezzo = 66f });



        menu.Piatti.Add(new Piatto { Nome = "ROMBO / BRANZINO / ORATA alla griglia, al forno o al sale Prodotto congelato ", Nome_EN = "Tubot / Seabass / Seabream (more than 500 gr) Grilled,baked,  boiled, in salt crust frozen product ", Nome_GE = "Steinbutt / Schwertfisch / Dorade (mehr als 500 gr) Gegrillt, vom Ofen oder in Salzkruste Tiefgefrorene Produkte", Allergeni = ToStr(new int[] { 4 }), Tipo = "dal mercato di Cesenatico", Prezzo = 5.5f });



        menu.Piatti.Add(new Piatto { Nome = "Tagliata di manzo al sale grosso, rosmarino e patate al forno", Nome_EN = "thinly sliced beef with salt and rosemary", Nome_GE = "Rindersteak in Scheiben  mit Salz und Rosmarin", Allergeni = ToStr(new int[] { 15 }), Tipo = "LE NOSTRE TAGLIATE", Prezzo = 18f });
        menu.Piatti.Add(new Piatto { Nome = "Tagliata di manzo rucola e grana", Nome_EN = "thinly sliced beef with ricket and parmesan cheese", Nome_GE = "Rindersteak in Scheiben  mit Rauke und Parmesankäse", Allergeni = ToStr(new int[] { 7 }), Tipo = "LE NOSTRE TAGLIATE", Prezzo = 18f });
        menu.Piatti.Add(new Piatto { Nome = "Tagliata di manzo con gorgonzola, in crosta di noci", Nome_EN = "Thinly sliced beef with gorgonzola cheese and walnut crust", Nome_GE = "Rindersteak mit Gorgonzola - Nusskruste", Allergeni = ToStr(new int[] { 7, 8 }), Tipo = "LE NOSTRE TAGLIATE", Prezzo = 19f });
        menu.Piatti.Add(new Piatto { Nome = "Tagliata di manzo al sangiovese con scalogno", Nome_EN = "thinly sliced beef with red wine and shallots", Nome_GE = "Rindersteak in Scheiben mit Rotwein und Schalotten", Allergeni = ToStr(new int[] { 12 }), Tipo = "LE NOSTRE TAGLIATE", Prezzo = 19f });
        menu.Piatti.Add(new Piatto { Nome = "Tagliata di manzo agli agrumi", Nome_EN = "thinly sliced beef with citrus fruits", Nome_GE = "Rindersteak in Scheiben mit Zitrusfrüchten", Allergeni = ToStr(new int[] { }), Tipo = "LE NOSTRE TAGLIATE", Prezzo = 19f });
        menu.Piatti.Add(new Piatto { Nome = "Tagliata di manzo ai funghi porcini", Nome_EN = "thinly sliced beef with mushrooms", Nome_GE = "Rindersteak in Scheiben mit Steinpilzen", Allergeni = ToStr(new int[] { 7, 15 }), Tipo = "LE NOSTRE TAGLIATE", Prezzo = 20f });
        menu.Piatti.Add(new Piatto { Nome = "Tris di Tagliate con sale rosmarino, gorgonzola in crosta di noci e sangiovese con scalogno", Nome_EN = "Three thinly sliced beef courses with salt and rosmarin gorgonolacheese and walnut crust and red wine and shallots", Nome_GE = "3 Rindersteaks in Scheiben mit Salz und Rosmarin  Gorgonzola Nusskruste und Rotwein mit Schalotten", Allergeni = ToStr(new int[] { 7, 8, 12, 15 }), Tipo = "LE NOSTRE TAGLIATE", Prezzo = 25f });
        menu.Piatti.Add(new Piatto { Nome = "Tagliata di pollo con misticanza di insalata", Nome_EN = "thinly sliced chicken with mixed salad", Nome_GE = "Hühnerbrust in Scheiben mit gemischten Salat", Allergeni = ToStr(new int[] { 15 }), Tipo = "LE NOSTRE TAGLIATE", Prezzo = 12f });



        menu.Piatti.Add(new Piatto { Nome = "Filetto alla griglia  e patate al forno", Nome_EN = "grilled fillet", Nome_GE = "Steak vom Grill", Allergeni = ToStr(new int[] { 15 }), Tipo = "I NOSTRI FILETTI", Prezzo = 20f });
        menu.Piatti.Add(new Piatto { Nome = "Filetto al pepe verde", Nome_EN = "Fillet with green peppercorns", Nome_GE = "Pfeffersteak", Allergeni = ToStr(new int[] { 7, 12, 15 }), Tipo = "I NOSTRI FILETTI", Prezzo = 20f });
        menu.Piatti.Add(new Piatto { Nome = "Filetto all’aceto balsamico", Nome_EN = "Fillet with balsamic vinegar ", Nome_GE = "Steak mit Balsamessigsauce", Allergeni = ToStr(new int[] { 7, 12, 15 }), Tipo = "I NOSTRI FILETTI", Prezzo = 20f });
        menu.Piatti.Add(new Piatto { Nome = "Filetto ai porcini", Nome_EN = "Fillet with porcini mushrooms", Nome_GE = "Steak mit Steinpilzsauce", Allergeni = ToStr(new int[] { 7, 15 }), Tipo = "I NOSTRI FILETTI", Prezzo = 24f });
        menu.Piatti.Add(new Piatto { Nome = "Picaña Brasiliana", Nome_EN = "Brazilian Picaña", Nome_GE = "Brasilianische Picaña", Allergeni = ToStr(new int[] { }), Tipo = "SECONDI CLASSICI", Prezzo = 19f });
        menu.Piatti.Add(new Piatto { Nome = "Galletto al limone grigliato con patate al forno", Nome_EN = "Grilled chicken flavoured with lemon,  with potatoes", Nome_GE = "Gegrilltes Hähnchen mit Zitrone und Kartoffeln", Allergeni = ToStr(new int[] { 15 }), Tipo = "SECONDI CLASSICI", Prezzo = 17f });
        menu.Piatti.Add(new Piatto { Nome = "Fiorentina di Scottona (Angus Irland) 10 Notti min. 1 kg", Nome_EN = "Scottona T-bone steak min. 1 kg", Nome_GE = "Fiorentina Steak vom Scottona-Rind min. 1kg", Allergeni = ToStr(new int[] { 15 }), Tipo = "SECONDI CLASSICI", Prezzo = 5.50f });
        menu.Piatti.Add(new Piatto { Nome = "Costata di Scottona (Angus Irland) 10 Notti ", Nome_EN = "Entrecote steak", Nome_GE = "doppeltes Entrecote vom Scottona-Rind", Allergeni = ToStr(new int[] { 15 }), Tipo = "SECONDI CLASSICI", Prezzo = 4.50f });
        menu.Piatti.Add(new Piatto { Nome = "Cosciotto di Castrato ai ferri con pomodoro e polenta", Nome_EN = "Grilled Mutton chop", Nome_GE = "Hammelfleisch mit gegrillten Tomaten und Polenta", Allergeni = ToStr(new int[] { 7, 15 }), Tipo = "SECONDI CLASSICI", Prezzo = 16f });
        menu.Piatti.Add(new Piatto { Nome = "Costolette di agnello con Polenta", Nome_EN = "Lamb chop  with polenta", Nome_GE = "Lammkotoletts mit Polenta", Allergeni = ToStr(new int[] { 7, 15 }), Tipo = "SECONDI CLASSICI", Prezzo = 17f });
        menu.Piatti.Add(new Piatto { Nome = "Salsiccia ai ferri con patate al forno", Nome_EN = "grilled sausages", Nome_GE = "ital. Hackfleischwurst vom Grill", Allergeni = ToStr(new int[] { 15 }), Tipo = "SECONDI CLASSICI", Prezzo = 12f });
        menu.Piatti.Add(new Piatto { Nome = "Stinco di mora romagnola", Nome_EN = "Pork knuckle with roast potatoes", Nome_GE = "Eisbein vom Schwein mit Offenkartoffein", Allergeni = ToStr(new int[] { 1, 9, 12, 15 }), Tipo = "SECONDI CLASSICI", Prezzo = 18f });
        menu.Piatti.Add(new Piatto { Nome = "Maialata (Braciola, salsiccia, pancetta, costa, polenta, pomodoro e bruschettina all’aglio) ", Nome_EN = "Barbecued mixed meat of pork (mixed chops, grilled sausages, tomatoes, polenta, tosted bread with galic)", Nome_GE = "Schweinefleischteller (mit Kotelett, ital.Hackfleischwurst, Bauchspeck, Rippchen, gegrillte Tomaten, Polenta, Röstbrot mit Knoblauch)", Allergeni = ToStr(new int[] { 1, 7, 15 }), Tipo = "SECONDI CLASSICI", Prezzo = 17f });

        menu.Piatti.Add(new Piatto { Nome = "Cotoletta alla milanese con patatine fritte", Nome_EN = "Wiener schnitzel with french fried potatoes", Nome_GE = "Wiener Schnitzel mit Pommes frites", Allergeni = ToStr(new int[] { 1, 3, 5 }), Tipo = "SECONDI CLASSICI", Prezzo = 10f });

        menu.Piatti.Add(new Piatto { Nome = "Scaloppine a piacere (funghi, limone, vino bianco, marsala, aceto balsamico)", Nome_EN = "Escalope (with mushrooms ,lemon, white wine, marsala o balsamic vinegar sauce)", Nome_GE = "Kalbsschnitzel (mit Champignons, Zitrone, Weisswein, Marsala oder Balsamessigsauce)", Allergeni = ToStr(new int[] { 1, 7, 15 }), Tipo = "SECONDI CLASSICI", Prezzo = 12f });

        menu.Piatti.Add(new Piatto { Nome = "Grigliata di carne per 1 pers. (costoletta d’agnello, salsiccia, costina di maiale e tagliata di scottona, vitello, con pomodoro e polenta alla griglia)", Nome_EN = "Mixed grilled meat for 1 pax. (mutton chop,grilled sausages, pork chop and sliced beef, veal with grilled polenta and tomato)", Nome_GE = "Gegrillter Fleischteller für 1 pers. (Hammelfleisch, ital Hackfleischwurst, Schweinsrippchen, Rindersteak in Scheiben, Kalbfleisch gegrillte Tomaten und Polenta) ", Allergeni = ToStr(new int[] { 7, 15 }), Tipo = "SECONDI CLASSICI", Prezzo = 17f });




        menu.Piatti.Add(new Piatto { Nome = "Tagliere di carne per 2 pers. (costolette d’agnello, salsicce, braciola di vitello, costa di maiale, tagliata di manzo, pomodori e polenta alla griglia, patate al forno)", Nome_EN = "Platter with mixed grilled meat for 2 pax. (lamb chop, grilled sausages, veal chop, pork chop, thinly slice of beef, tomatoes, polenta, roast potatoes)", Nome_GE = "Gegrillter Fleischteller für 2 Personen (Lammkoteletts, ital. Hackfleischwurst, Kalbskotelett, Schweinsrippchen, Rindersteak in Scheiben, gegrillte Tomaten und Polenta)", Allergeni = ToStr(new int[] { 7, 15 }), Tipo = "I TAGLIERI CLASSICI", Prezzo = 40f });

        menu.Piatti.Add(new Piatto { Nome = "Tagliere di carne per 3 pers. (costolette d’agnello, salsicce, braciola di vitello, costa di maiale, tagliata di manzo, pomodori e polenta alla griglia, patate al forno)", Nome_EN = "Platter with mixed grilled meat for 3 pax. (lamb chop, grilled sausages, veal chop, pork chop, thinly slice of beef, tomatoes, polenta, roast potatoes)", Nome_GE = "Gegrillter Fleischteller für 3 Personen  (Lammkoteletts, ital. Hackfleischwurst, Kalbskotelett, Schweinsrippchen, Rindersteak in Scheiben,gegrillte Tomaten und Polenta)", Allergeni = ToStr(new int[] { 7, 15 }), Tipo = "I TAGLIERI CLASSICI", Prezzo = 60f });



        menu.Piatti.Add(new Piatto { Nome = "Insalata mista", Nome_EN = "mixed Salad", Nome_GE = "gemischter Salat", Allergeni = ToStr(new int[] { }), Tipo = "CONTORNI", Prezzo = 5f });

        menu.Piatti.Add(new Piatto { Nome = "Insalata verde", Nome_EN = "green Salad", Nome_GE = "grüner Salat", Allergeni = ToStr(new int[] { }), Tipo = "CONTORNI", Prezzo = 4f });

        menu.Piatti.Add(new Piatto { Nome = "Patate al forno", Nome_EN = "roast potatoes ", Nome_GE = "Ofenkartoffeln", Allergeni = ToStr(new int[] { 15 }), Tipo = "CONTORNI", Prezzo = 5f });

        menu.Piatti.Add(new Piatto { Nome = "Patate fritte", Nome_EN = "French fries", Nome_GE = "Pommes frites", Allergeni = ToStr(new int[] { 5 }), Tipo = "CONTORNI", Prezzo = 5f });

        menu.Piatti.Add(new Piatto { Nome = "Zucchine fritte", Nome_EN = "fried courgettes", Nome_GE = "frittierte Zucchini", Allergeni = ToStr(new int[] { 1, 3, 5 }), Tipo = "CONTORNI", Prezzo = 5f });

        menu.Piatti.Add(new Piatto { Nome = "Radicchio e bruciatini con aceto balsamico", Nome_EN = "Radicchio with roasted bacon and hot balsamicvinegar", Nome_GE = "Radicchio mit gerösteten Bauchspeckstückchen mit heißem Balsamicoessig", Allergeni = ToStr(new int[] { 12 }), Tipo = "CONTORNI", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Spinaci al burro", Nome_EN = "Buttered spinach", Nome_GE = "Spinat in Butter", Allergeni = ToStr(new int[] { 7 }), Tipo = "CONTORNI", Prezzo = 6f });

        menu.Piatti.Add(new Piatto { Nome = "Spinaci saltati con aglio e peperoncino", Nome_EN = "Spinaci fried with galic and chili", Nome_GE = "Spinat sautiert mit Knoblauch und Chili", Allergeni = ToStr(new int[] { 15 }), Tipo = "CONTORNI", Prezzo = 6f });

        menu.Piatti.Add(new Piatto { Nome = "Verdure al vapore", Nome_EN = "steamed vegetables", Nome_GE = "gedünstetes gem. Gemüse", Allergeni = ToStr(new int[] { }), Tipo = "CONTORNI", Prezzo = 6f });

        menu.Piatti.Add(new Piatto { Nome = "Verdure alla griglia", Nome_EN = "grilled vegetables", Nome_GE = "gegrilltes gem. Gemüse", Allergeni = ToStr(new int[] { }), Tipo = "CONTORNI", Prezzo = 6f });

        menu.Piatti.Add(new Piatto { Nome = "Pinzimonio", Nome_EN = "fresh raw vegetables", Nome_GE = "frisches rohes Gemüse", Allergeni = ToStr(new int[] { 9 }), Tipo = "CONTORNI", Prezzo = 6f });



        menu.Piatti.Add(new Piatto { Nome = "Maxi Hamburger", Nome_EN = "Maxi Hamburger", Nome_GE = "Maxi Hamburger", Allergeni = ToStr(new int[] { 1, 5, 7, 8, 11, 12 }), Tipo = "SECONDI CLASSICI", Prezzo = 16f });

        menu.Piatti.Add(new Piatto { Nome = "Tortino di quinoa con hummus di ceci e nocciole", Nome_EN = "Quinoa soufflé with hummus chickpeas and hazelnut ", Nome_GE = "Quinoa-Souffle mit Hummus aus Kichererbsen und Hasselnuessen", Allergeni = ToStr(new int[] { 8, 9 }), Tipo = "PIATTI UNICI", Prezzo = 14f });

        menu.Piatti.Add(new Piatto { Nome = "Tomino con verdure alla griglia", Nome_EN = "grilled Cheese and vegetables ", Nome_GE = "gegrillte Camembertkäse und gegrilltes Gemüse", Allergeni = ToStr(new int[] { 7 }), Tipo = "PIATTI UNICI", Prezzo = 12f });

        menu.Piatti.Add(new Piatto { Nome = "Vegetariano: Tortino di squacquerone e spinaci, crostino con verdure, e patate al forno melanzane alla griglia", Nome_EN = "Vegetarian: Squacquerone cheese and spinach pie, vegetable savoury toast, roast potatoes, grilled aubergines", Nome_GE = "Vegetarisches: kl.Törtchen mit Spinat u. Frischkäse, Röstbrot mit Gemüse, Ofenkartoffel, gegrillte Auberginen", Allergeni = ToStr(new int[] { 1, 3, 7, 15 }), Tipo = "PIATTI UNICI", Prezzo = 12f });

        menu.Piatti.Add(new Piatto { Nome = "Tirolese: Tomino in camicia di speck, crostino, radicchio trevigiano ai ferri, patate al forno", Nome_EN = "Tirolean: Tomino cheese in a smoked ham jacket, savoury toast, grilled Treviso red radicchio, roast potatoes", Nome_GE = "nach Tiroler-Art: gebackener Käse im Speckhemd, Röstbrot, Radicchio vom Grill, Ofenkartoffeln", Allergeni = ToStr(new int[] { 1, 7, 15 }), Tipo = "PIATTI UNICI", Prezzo = 13f });

        menu.Piatti.Add(new Piatto { Nome = "Maxi Insalata con tonno e mozzarella", Nome_EN = "Insalatona: Maxi salad with tuna and mozzarella", Nome_GE = "Salatschüssel: gem.großer Salat mit Thunfisch und Mozzarella", Allergeni = ToStr(new int[] { 4, 7 }), Tipo = "PIATTI UNICI", Prezzo = 10f });

        menu.Piatti.Add(new Piatto { Nome = "Insalata Elisa insalata di campo, noci, mela, pecorino stagionato e fichi caramellati", Nome_EN = "mixed field salad, wallnuts, apple, pecorino cheese and caramelized fig", Nome_GE = "Feldsalat mit Wallnüssen, Apfel, Schafskäse und karamelisierten Feigen", Allergeni = ToStr(new int[] { 7, 8 }), Tipo = "PIATTI UNICI", Prezzo = 12f });

        menu.Piatti.Add(new Piatto { Nome = "Piatto Vegano dello chef", Nome_EN = "vegan single course with raviolo (broccoli filled) tofu stew with crunchy vegetables, seitan medallion in sesam crust and bruschetta with cherry tomatoes ", Nome_GE = "Veganerteller mit Ravioli (Brokkolifüllung) Tofu-ragout mit Gemüse, Seitan-medaillon in Sesamkruste und Bruschettabrot mit Cocktailtomaten ", Allergeni = ToStr(new int[] { 1, 6, 810, 11 }), Tipo = "PIATTI UNICI", Prezzo = 14f });

        menu.Piatti.Add(new Piatto { Nome = "Seitan e tofu con verdure saltate", Nome_EN = "Seita and tofu with rosted vegetables ", Nome_GE = "gegrillter Seitan und Tofu mit geröstetem Gemüse", Allergeni = ToStr(new int[] { 1, 6, 15 }), Tipo = "PIATTI UNICI", Prezzo = 14f });

        menu.Piatti.Add(new Piatto { Nome = "Caesar Salad (pollo alla piastra pan brioche misticanza grana e salsa cesar)", Nome_EN = "Chicken slices with salad, parma cheese, yeast dough bread and Cesar sauce", Nome_GE = "Gegrillte Hühnerbruststreifen auf Salat mit Parmesankäse, Hefeteigbrot und Cesarsauce", Allergeni = ToStr(new int[] { 1, 3, 7, 15 }), Tipo = "PIATTI UNICI", Prezzo = 14f });

        menu.Piatti.Add(new Piatto { Nome = "Insalatona Molinetto Estate (misticanza, frutta di stagione, gamberi e salsa rosa)", Nome_EN = "mixed salad with seasonal fruit, shrimps and pink sauce", Nome_GE = "Gemischter Salat mit Früchtender Saison und Coktailsauc", Allergeni = ToStr(new int[] { 2, 3, 4, 8 }), Tipo = "PIATTI UNICI", Prezzo = 14f });


        menu.Piatti.Add(new Piatto { Nome = "Mozzarella, crema tartufata, funghi", Nome_EN = "Mozzarella, trufﬂe puree, fresh mushrooms", Nome_GE = "Mozzarella, Trüffelcreme, Champignons", Allergeni = ToStr(new int[] { 1, 5, 7, 8, 15 }), Tipo = "GLI ASSAGGI DI PIZZA (per 2 persone)", Prezzo = 20f });

        menu.Piatti.Add(new Piatto { Nome = "Pomodoro, mozzarella, patate lesse, noci, gorgonzola, salsiccia", Nome_EN = "Tomatoes, mozzarella, boiled patatoes, walnuts, gorgonzola(sweet),spicy sausage", Nome_GE = "Tomaten, Mozzarella, gekochte Kartoffeln, Nüsse, Gorgonzola(süß), ital. Hackfleischwurst", Allergeni = ToStr(new int[] { 1, 5, 7, 8, 15 }), Tipo = "GLI ASSAGGI DI PIZZA (per 2 persone)", Prezzo = 20f });

        menu.Piatti.Add(new Piatto { Nome = "Pomodoro, mozzarella, pomodorini, rucola, grana", Nome_EN = "Tomatoes, Mozzarella, small round tomatoes, rocket, parmesan", Nome_GE = "Tomaten, Mozzarella, Kirschtomaten, Rauke, Parmesankäse", Allergeni = ToStr(new int[] { 1, 5, 7, 8, 15 }), Tipo = "GLI ASSAGGI DI PIZZA (per 2 persone)", Prezzo = 20f });

        menu.Piatti.Add(new Piatto { Nome = "Mozzarella, panna, radicchio, speck", Nome_EN = "Mozzarella, cream, radicchio, smoked ham", Nome_GE = "Mozzarella, Sahne, Radicchio, Speck", Allergeni = ToStr(new int[] { 1, 5, 7, 8, 15 }), Tipo = "GLI ASSAGGI DI PIZZA (per 2 persone)", Prezzo = 20f });



        menu.Piatti.Add(new Piatto { Nome = "Mozzarella, crema tartufata, funghi", Nome_EN = "Mozzarella, trufﬂe puree, fresh mushrooms", Nome_GE = "Mozzarella, Trüffelcreme, Champignons", Allergeni = ToStr(new int[] { 1, 5, 7, 8, 15 }), Tipo = "GLI ASSAGGI DI PIZZA (per 3 persone)", Prezzo = 30f });

        menu.Piatti.Add(new Piatto { Nome = "Pomodoro, mozzarella, patate lesse, noci, gorgonzola, salsiccia", Nome_EN = "Tomatoes, mozzarella, boiled patatoes, walnuts, gorgonzola(sweet),spicy sausage", Nome_GE = "Tomaten, Mozzarella, gekochte Kartoffeln, Nüsse, Gorgonzola(süß), ital. Hackfleischwurst", Allergeni = ToStr(new int[] { 1, 5, 7, 8, 15 }), Tipo = "GLI ASSAGGI DI PIZZA (per 3 persone)", Prezzo = 30f });

        menu.Piatti.Add(new Piatto { Nome = "Pomodoro, mozzarella, pomodorini, rucola, grana", Nome_EN = "Tomatoes, Mozzarella, small round tomatoes, rocket, parmesan", Nome_GE = "Tomaten, Mozzarella, Kirschtomaten, Rauke, Parmesankäse", Allergeni = ToStr(new int[] { 1, 5, 7, 8, 15 }), Tipo = "GLI ASSAGGI DI PIZZA (per 3 persone)", Prezzo = 30f });

        menu.Piatti.Add(new Piatto { Nome = "Mozzarella, panna, radicchio, speck", Nome_EN = "Mozzarella, cream, radicchio, smoked ham", Nome_GE = "Mozzarella, Sahne, Radicchio, Speck", Allergeni = ToStr(new int[] { 1, 5, 7, 8, 15 }), Tipo = "GLI ASSAGGI DI PIZZA (per 3 persone)", Prezzo = 30f });



        menu.Piatti.Add(new Piatto { Nome = "Panna, mozzarella, salmone, rucola", Nome_EN = "Cream Mozzarella, salmon, rocket", Nome_GE = "Mozzarella, Sahne, Lachs, Rauke", Allergeni = ToStr(new int[] { 1, 2, 4, 5, 7, 12, 14, 15 }), Tipo = "ASSAGGI DI MARE per 2 persone", Prezzo = 24f });

        menu.Piatti.Add(new Piatto { Nome = "Frutti di mare", Nome_EN = "Seafood ", Nome_GE = "Meeresfrüchte", Allergeni = ToStr(new int[] { 1, 2, 4, 5, 7, 12, 14, 15 }), Tipo = "ASSAGGI DI MARE per 2 persone", Prezzo = 24f });

        menu.Piatti.Add(new Piatto { Nome = "Pomodoro, tonno, olive", Nome_EN = "Tomato, tuna, olives", Nome_GE = "Tomate, Thunfisch, Oliven", Allergeni = ToStr(new int[] { 1, 2, 4, 5, 7, 12, 14, 15 }), Tipo = "ASSAGGI DI MARE per 2 persone", Prezzo = 24f });

        menu.Piatti.Add(new Piatto { Nome = "Mozzarella, gamberi, zucchine", Nome_EN = "Mozzarella, crayﬁsh, courgetles", Nome_GE = "Mozzarella, Krabben, Zucchini", Allergeni = ToStr(new int[] { 1, 2, 4, 5, 7, 12, 14, 15 }), Tipo = "ASSAGGI DI MARE per 2 persone", Prezzo = 24f });



        menu.Piatti.Add(new Piatto { Nome = "Panna, mozzarella, salmone, rucola", Nome_EN = "Cream Mozzarella, salmon, rocket", Nome_GE = "Mozzarella, Sahne, Lachs, Rauke", Allergeni = ToStr(new int[] { 1, 2, 4, 5, 7, 12, 14, 15 }), Tipo = "ASSAGGI DI MARE per 3 persone", Prezzo = 36f });

        menu.Piatti.Add(new Piatto { Nome = "Frutti di mare", Nome_EN = "Seafood ", Nome_GE = "Meeresfrüchte", Allergeni = ToStr(new int[] { 1, 2, 4, 5, 7, 12, 14, 15 }), Tipo = "ASSAGGI DI MARE per 3 persone", Prezzo = 36f });

        menu.Piatti.Add(new Piatto { Nome = "Pomodoro, tonno, olive", Nome_EN = "Tomato, tuna, olives", Nome_GE = "Tomate, Thunfisch, Oliven", Allergeni = ToStr(new int[] { 1, 2, 4, 5, 7, 12, 14, 15 }), Tipo = "ASSAGGI DI MARE per 3 persone", Prezzo = 36f });

        menu.Piatti.Add(new Piatto { Nome = "Mozzarella, gamberi, zucchine", Nome_EN = "Mozzarella, crayﬁsh, courgetles", Nome_GE = "Mozzarella, Krabben, Zucchini", Allergeni = ToStr(new int[] { 1, 2, 4, 5, 7, 12, 14, 15 }), Tipo = "ASSAGGI DI MARE per 3 persone", Prezzo = 36f });



        menu.Piatti.Add(new Piatto { Nome = "Schiacciatina rossa o bianca", Nome_EN = "plain pizza with o without tomatoes", Nome_GE = "Schiacciatina (Pizzabrot)mit oder ohne Tomatensauce", Allergeni = ToStr(new int[] { 1, 5 }), Tipo = "PIZZE", Prezzo = 2f });

        menu.Piatti.Add(new Piatto { Nome = "Margherita", Nome_EN = "Tomatoes and mozzarella", Nome_GE = "Tomaten, Mozzarella", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE", Prezzo = 4f });

        menu.Piatti.Add(new Piatto { Nome = "Faina (Schiacciatina bianca e crudo)", Nome_EN = "Plain pizza with Parma ham ", Nome_GE = "Pizzabrot mit Parmaschinken", Allergeni = ToStr(new int[] { 1, 5 }), Tipo = "PIZZE", Prezzo = 5f });

        menu.Piatti.Add(new Piatto { Nome = "Schiacciatina con verdure miste (grigliate e fresche)", Nome_EN = "Schiacciatina (plain pizza with vegetables", Nome_GE = "Pìzzabrot mit gegrilltem Gemüse", Allergeni = ToStr(new int[] { 1, 5 }), Tipo = "PIZZE", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Marinara (Pomodoro, aglio, olive)", Nome_EN = "Tomatoes, garlic, olives", Nome_GE = "Tomaten, Knoblauch u. Oliven", Allergeni = ToStr(new int[] { 1, 5, 15 }), Tipo = "PIZZE", Prezzo = 5f });

        menu.Piatti.Add(new Piatto { Nome = "Napoli (Pomodoro, mozzarella, acciughe, origano)", Nome_EN = "Tomatoes, mozzarella, anchovies, oregano", Nome_GE = "Tomaten, Mozzarella, Sardellen, Oregano", Allergeni = ToStr(new int[] { 1, 4, 5, 7 }), Tipo = "PIZZE", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Romana (Pomodoro, mozzarella, acciughe, capperi, origano)", Nome_EN = "Tomatoes, mozzarella, anchovies, capers, oregano", Nome_GE = "Tomaten, Mozzarella, Sardellen, Kapern, Oregano", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Funghi freschi", Nome_EN = "Tomatoes, Mozzarella,fresh mushrooms", Nome_GE = "Tomaten, Mozzarella, Champignons", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Olive nere", Nome_EN = "Tomatoes, mozzarella, black olives", Nome_GE = "Tomaten, Mozzarella, schwarze Oliven", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE", Prezzo = 6f });

        menu.Piatti.Add(new Piatto { Nome = "Carcioﬁ", Nome_EN = "Tomatoes, mozzarella,Artichokes", Nome_GE = "Tomaten, Mozzarella,Artischocken", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Melanzane alla griglia", Nome_EN = "Tomatoes, mozzarella, grilled aubergines", Nome_GE = "Tomaten, Mozzarella, gegrillte Auberginen", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Melanzane fritte", Nome_EN = "Tomatoes, mozzarella, fried aubergines", Nome_GE = "Tomaten, Mozzarella, fritierte Auberginen", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Salsiccia", Nome_EN = "Tomatoes, mozzarella,Spicy sausage", Nome_GE = "Tomaten, Mozzarella, ital. Hackfleischwurst", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Wurstel", Nome_EN = "Tomatoes, Mozzarella,Frankfurter", Nome_GE = "Tomaten, Mozzarella, Frankfurterwürstchen", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Prosciutto cotto", Nome_EN = "Tomatoes, Mozzarella,Boiled ham", Nome_GE = "Tomaten, Mozzarella, Kochschinken", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Salame piccante", Nome_EN = "Tomatoes, Mozzarella, Spicy salami", Nome_GE = "Tomaten, Mozzarella, scharfe Salami", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Pugliese (Pomodoro, mozzarella, olive, cipolla)", Nome_EN = "Tomatoes, mozzarella, olives, onion", Nome_GE = "Tomaten Mozzarella, Oliven, Zwiebeln", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Prosciutto crudo", Nome_EN = "Tomatoes, Mozzarella,Parma ham", Nome_GE = "Tomaten, Mozzarella, Parmaschinken", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE", Prezzo = 8f });

        menu.Piatti.Add(new Piatto { Nome = "Gorgonzola", Nome_EN = "Tomatoes, Mozzarella, Gorgonzola (sweet)", Nome_GE = "Tomaten, Mozzarella, Gorgonzola (süß)", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Wurstel e patatine fritte", Nome_EN = "Tomatoes, Mozzarella, Frankfurter,French fries", Nome_GE = "Tomaten, Mozzarella, Frankfurterwürstchen, Pommes frites", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE", Prezzo = 8f });

        menu.Piatti.Add(new Piatto { Nome = "Prosciutto cotto e funghi freschi", Nome_EN = "Tomatoes, Mozzarella, Boiled ham, fresh mushrooms", Nome_GE = "Tomaten, Mozzarella, Kochschinken, Champignons", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE", Prezzo = 8.5f });

        menu.Piatti.Add(new Piatto { Nome = "Prosciutto cotto e salsiccia", Nome_EN = "Tomatoes, Mozzarella,Boiled ham ,spícy sausage", Nome_GE = "Tomaten, Mozzarella, Kochschinken, ital. Hackfleischwurst", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE", Prezzo = 8.5f });

        menu.Piatti.Add(new Piatto { Nome = "Funghi freschi e salsiccia", Nome_EN = "Tomatoes, mozzarella, fresh muhrooms, spícy sausage", Nome_GE = "Tomaten, Mozzarella, Champignons, ital. Hackfleischwurst", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE", Prezzo = 8.5f });

        menu.Piatti.Add(new Piatto { Nome = "Rucola e grana", Nome_EN = "Tomatoes, mozzarella,Rocket, parmesan", Nome_GE = "Tomaten, Mozzarella, Rauke, Parmesankäse", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE", Prezzo = 8f });

        menu.Piatti.Add(new Piatto { Nome = "Prosciutto cotto e zucchine", Nome_EN = "Tomatoes, Mozzarella, Boiled ham, zucchini", Nome_GE = "Tomaten, Mozzarella, Kochschinken, Zucchini", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE", Prezzo = 8.5f });

        menu.Piatti.Add(new Piatto { Nome = "Messicana (Pomodoro, mozzarella, pancetta, cipolla, fagioli)", Nome_EN = "Tomatoes, mozzarella, bacon, onion, beans", Nome_GE = "Tomaten, Mozzarella, Bauchspeck, Zwiebel, weiße Bohnen", Allergeni = ToStr(new int[] { 1, 5, 7, 13 }), Tipo = "PIZZE", Prezzo = 9f });

        menu.Piatti.Add(new Piatto { Nome = "Erbazzona (Pomodoro, mozzarella, ricotta, spinaci)", Nome_EN = "Tomatoes, mozzarella, ricotta, spinach", Nome_GE = "Tomaten, Mozzarella, Ricotta, Spinat", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE", Prezzo = 9f });

        menu.Piatti.Add(new Piatto { Nome = "Tonno e cipolla", Nome_EN = "Tomatoes, mozzarella,Tuna, onion", Nome_GE = "Tomaten, Mozzarella, Thunﬁsch, Zwiebeln", Allergeni = ToStr(new int[] { 1, 4, 5, 7 }), Tipo = "PIZZE", Prezzo = 8f });

        menu.Piatti.Add(new Piatto { Nome = "Paolo il biondo (pomodoro, mozzarella, funghi freschi, pancetta)", Nome_EN = "Tomatoes, mozzarella, fresh mushrooms,smoked bacon", Nome_GE = "Tomaten, Mozzarella, Champignons, Bauchspeck", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE", Prezzo = 8.5f });

        menu.Piatti.Add(new Piatto { Nome = "Verdure (Pomodoro, mozzarella e verdure fresche e grigliate)", Nome_EN = "Tomatoes, mozzarella, fresh and grilled vegetables", Nome_GE = "Tomaten, Mozzarella, frisches u. gegrilltes Gemüse", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE", Prezzo = 8.5f });

        menu.Piatti.Add(new Piatto { Nome = "Gamberi", Nome_EN = "Tomatoes, mozzarella, crayﬁsh", Nome_GE = "Tomaten, Mozzarella, Krabben", Allergeni = ToStr(new int[] { 1, 2, 5, 7, 12 }), Tipo = "PIZZE", Prezzo = 9f });

        menu.Piatti.Add(new Piatto { Nome = "Regina margherita (Pomodoro, mozzarella di bufala, pomodorini, basilico)", Nome_EN = "Tomatoes, buffalo milk mozzarella, small round tomatoes, basil", Nome_GE = "Tomaten, Büffel-Mozzarella, Kirschtomaten, Basilikum", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE", Prezzo = 9f });

        menu.Piatti.Add(new Piatto { Nome = "Funghi porcini", Nome_EN = "Tomatoes, mozzarella, mushrooms(ceps)", Nome_GE = "Tomaten, Mozzarella, Steínpilze", Allergeni = ToStr(new int[] { 1, 5, 7, 15 }), Tipo = "PIZZE", Prezzo = 9f });

        menu.Piatti.Add(new Piatto { Nome = "Prosciutto crudo e funghi freschi", Nome_EN = "Tomatoes, mozzarella, Parma ham, fresh mushrooms", Nome_GE = "Tomaten, Mozzarella, Parmaschinken, Champignons", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE", Prezzo = 9f });

        menu.Piatti.Add(new Piatto { Nome = "Diavoletta (Salame piccante, pomini, rucola e grana)", Nome_EN = "Tomatoes, Mozzarella, Spicy salami, rocket, parmesan", Nome_GE = "Tomaten, Mozzarella, scharfe Salami, Rauke, Parmesankäse", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE", Prezzo = 9f });

        menu.Piatti.Add(new Piatto { Nome = "Capricciosa (Pomodoro, mozzarella, carcioﬁ, prosciutto cotto, funghi freschi, olive)", Nome_EN = "Tomatoes, mozzarella, artichokes boiled ham, fresh mushrooms, olives", Nome_GE = "Tomaten, Mozzarella, Artischocken Kochschinken, Oliven", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE", Prezzo = 9f });

        menu.Piatti.Add(new Piatto { Nome = "Quattro stagioni (Pomodoro, mozzarella, funghi freschi, salsiccia, carcioﬁ,cotto)", Nome_EN = "Tomato, mozzarella., mushrooms, spícy sausage, artichokes, boiled ham", Nome_GE = "Tomaten, Mozzarella, Pilze, ital. Hackfleischwurst,  Artischocken, Kochschinken", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE", Prezzo = 9f });

        menu.Piatti.Add(new Piatto { Nome = "Maialina (Pomodoro, mozzarella, prosciutto crudo, salsiccia, pancetta ,cotto)	", Nome_EN = "Tomatoes, mozzarella, Parma ham, spícy sausage, bacon and boyled ham", Nome_GE = "Tomaten, Mozzarella, Parmaschínken, ital. Hackfleischwurst, Bauchspeck, Kochschinken", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE", Prezzo = 10f });

        menu.Piatti.Add(new Piatto { Nome = "Frutti di mare (Pomodoro, seppie, gamberetti, calamari, capasanta)", Nome_EN = "Tomatoes , seafood", Nome_GE = "Tomaten, Meeresfrüchte", Allergeni = ToStr(new int[] { 1, 2, 4, 5, 14, 15 }), Tipo = "PIZZE", Prezzo = 12f });





        menu.Piatti.Add(new Piatto { Nome = "Calzone semplice (Pomodoro, mozzarella, prosciutto cotto)", Nome_EN = "Tomatoes, mozzarella, boiled ham", Nome_GE = "Tomaten, Mozzarella, Kochschinken", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "I NOSTRI CALZONI", Prezzo = 8f });

        menu.Piatti.Add(new Piatto { Nome = "Calzone montanaro (Pomodoro, mozzarella, ricotta, prosciutto cotto)", Nome_EN = "Tomato, mozzarella, boiled ham, ricotta", Nome_GE = "Tomaten, Mozzarella, Ricotta, Kochschinken", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "I NOSTRI CALZONI", Prezzo = 9f });

        menu.Piatti.Add(new Piatto { Nome = "Calzone farcito (Pomodoro, mozzarella, funghi trifolati, olive, prosciutto cotto)	", Nome_EN = "Tomato, mozzarella, sliced mushrooms cooked in oìl, garlic and parsley olives, boiled ham", Nome_GE = "Tomaten, mozzarella, marinierte Pilze, Oliven, Kochschinken", Allergeni = ToStr(new int[] { 1, 5, 7, 15 }), Tipo = "I NOSTRI CALZONI", Prezzo = 9f });

        menu.Piatti.Add(new Piatto { Nome = "Calzone ai formaggi (mix di formaggi meravigliosi)", Nome_EN = "Compound wonderful cheeses", Nome_GE = "verschiedene gemischte Käsesorten", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "I NOSTRI CALZONI", Prezzo = 9f });




        menu.Piatti.Add(new Piatto { Nome = "Mediterranea (Pomodoro, mozzarella, melanzane fritte, basilico e pinoli)", Nome_EN = "Tomatoes, mozzarella,  fried aubergines, basil and pine seeds", Nome_GE = "Tomaten, Mozzarella, fritierte Auberginen, Basilikum und Pinienkerne", Allergeni = ToStr(new int[] { 1, 5, 7, 8 }), Tipo = "PIZZE SPECIALI", Prezzo = 9f });

        menu.Piatti.Add(new Piatto { Nome = "Lady (Pomodoro, mozzarella, Prosciutto cotto, salsiccia e patate lesse)", Nome_EN = "Tomatoes, mozzarella, boiled ham, sausage, boiled potatoes", Nome_GE = "Tomaten, Mozzarella, Kochschinken, ital. Hackfleischwurst, gedünstete Kartoffeln", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE SPECIALI", Prezzo = 9f });

        menu.Piatti.Add(new Piatto { Nome = "Emiliana (Pomodoro, mozzarella, rucola, scaglie di grana, crudo)", Nome_EN = "Tomatoes, mozzarella, Parma ham, rocket, parmesan", Nome_GE = "Tomaten, Mozzarella, Parmaschinken, Rauke, Parmesankäse", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE SPECIALI", Prezzo = 10f });

        menu.Piatti.Add(new Piatto { Nome = "Cavallina (Pomodoro, mozzarella, bresaola, scaglie di grana, rucola)", Nome_EN = "Tomatoes, mozzarella, salted beef ﬂakes ,parmesan, rocket", Nome_GE = "Tomaten, Mozzarella, Bündnerfleisch, Parmesankäse, Rauke", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE SPECIALI", Prezzo = 10f });

        menu.Piatti.Add(new Piatto { Nome = "Trevigiana (Pomodoro, mozzarella, radicchio, pancetta affumicata)", Nome_EN = "Tomatoes, mozzarella, radicchio, smoked bacon", Nome_GE = "Tomaten, Mozzarella, Radicchio, geräucherter Bauchspeck", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE SPECIALI", Prezzo = 9f });

        menu.Piatti.Add(new Piatto { Nome = "Sottobosco (Pomodoro, mozzarella, porcini, funghi trifolati, funghi freschi)", Nome_EN = "Tomatoes mozzarella, mushrooms, fresh and sliced mushrooms cooked in oil, garlic and parsley", Nome_GE = "Tomaten, Mozzarella, Steinpilze, marinierte Pilze, Champignons", Allergeni = ToStr(new int[] { 1, 5, 7, 15 }), Tipo = "PIZZE SPECIALI", Prezzo = 10f });

        menu.Piatti.Add(new Piatto { Nome = "Tirolese (Pomodoro, mozzarella, panna, speck, radicchio)", Nome_EN = "Tomatoes, mozzarella, cream, smoked ham, radicchio", Nome_GE = "Tomaten, Mozzarella, Sahne, Speck, Radicchio", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE SPECIALI", Prezzo = 9f });

        menu.Piatti.Add(new Piatto { Nome = "Tir (Pomodoro, mozzarella, Funghi, radicchio, gorgonzola e salame piccante)", Nome_EN = "Tomatoes, mozzarella, mushrooms, radicchio, gorgonzola cheese, spicy salami", Nome_GE = "Tomaten, Mozzarella, Champignons, radicchio, gorgonzola (süß),scharfe Salami ", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE SPECIALI", Prezzo = 10f });

        menu.Piatti.Add(new Piatto { Nome = "Parmigiana (Pomodoro, mozzarella, melanzane, prosciutto cotto, scaglie di grana)", Nome_EN = "Tomatoes, mozzarella, aubergine, boiled ham, parmesan", Nome_GE = "Tomaten, Mozzarella, Auberginen, Kochschinken, Parmesankäse", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE SPECIALI", Prezzo = 10f });

        menu.Piatti.Add(new Piatto { Nome = "Patatosa (Pomodoro, mozzarella, patate lesse, noci, gorgonzola, salsiccia)", Nome_EN = "Tomatoes, Mozzarella, boiled patatoes, walnut, gorgonzola(sweet), sausage", Nome_GE = "Tomaten, Mozzarella, gedünstete Kartoffeln, Nüsse, Gorgonzola (süß) ,ital. Hackfleischwurst", Allergeni = ToStr(new int[] { 1, 5, 7, 8 }), Tipo = "PIZZE SPECIALI", Prezzo = 10f });

        menu.Piatti.Add(new Piatto { Nome = "Lupo (Pomodoro, mozzarella, gorgonzola (dolce), cipolla, salame piccante)", Nome_EN = "Tomatoes, mozzarella, gorgonzola(sweet), onion, spicy salami", Nome_GE = "Tomaten, Mozzarella, Gorgonzola (süß), Zwiebel, scharfe Salami", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE SPECIALI", Prezzo = 10f });

        menu.Piatti.Add(new Piatto { Nome = "Lap dance (pomodoro, mozzarella,melanzane alla griglia, peperoni salame piccante)", Nome_EN = "Tomatoes, mozzarella, aubergine, sweet peppers, spicy salame", Nome_GE = "Tomaten, Mozzarella, Auberginen, süßer Paprika, scharfe Salami", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE SPECIALI", Prezzo = 9f });

        menu.Piatti.Add(new Piatto { Nome = "Tedesca (Pomodoro, mozzarella, Funghi, cipolla, Wurstel e speck in cottura)", Nome_EN = "Tomatoes, mozzarella, mushrooms, onion, Frankfurter, smoked ham", Nome_GE = "Tomaten, Mozzarella, Champignons, Zwiebel, Frankfurter, Speck in Ofen", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "PIZZE SPECIALI", Prezzo = 10f });

        menu.Piatti.Add(new Piatto { Nome = "Delizia (Pomodoro, mozzarella, Radicchio, porcini e scaglie di grana)", Nome_EN = "Tomatoes, mozzarella, mushrooms, radicchio, mushrooms, parmesan", Nome_GE = "Tomaten, Mozzarella, Champignons, Steinpilze und Parmesankäse", Allergeni = ToStr(new int[] { 1, 5, 7, 15 }), Tipo = "PIZZE SPECIALI", Prezzo = 10f });

        menu.Piatti.Add(new Piatto { Nome = "Cupido (Pomodoro, mozzarella, Tonno, peperoni e pancetta)", Nome_EN = "Tomatoes, mozzarella, tuna fish, peppers, bacon", Nome_GE = "Tomaten, Mozzarella,Thunﬁsch, Paprika, Bauchspeck", Allergeni = ToStr(new int[] { 1, 4, 5, 7 }), Tipo = "PIZZE SPECIALI", Prezzo = 10f });




        menu.Piatti.Add(new Piatto { Nome = "Squacquerone e rucola", Nome_EN = "Mozzarella, squacquerone cheese, rocket", Nome_GE = "Mozzarella, ital. Frischkäse, Rauke", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "LE PIZZE BIANCHE", Prezzo = 8f });

        menu.Piatti.Add(new Piatto { Nome = "Mozzarella Stracchino e salsiccia", Nome_EN = "Mozzarella, stracchìno cheese, sausage", Nome_GE = "Mozzarella, Frischkäse, ital. Hackfleischwurst", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "LE PIZZE BIANCHE", Prezzo = 8f });

        menu.Piatti.Add(new Piatto { Nome = "Quattro formaggi (Mozzarella, e mix di formaggi)", Nome_EN = "Mozzarella, ﬂakes of parmesan, Gorgonzola, Scamorza", Nome_GE = "Büffel-mozzarella", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "LE PIZZE BIANCHE", Prezzo = 8f });

        menu.Piatti.Add(new Piatto { Nome = "Sole & Luna (Mozzarella, funghi freschi, pomodorini, scaglie di grana)", Nome_EN = "Mozzarella, fresh mushrooms, small round tomatoes, ﬂakes of parmesan", Nome_GE = "Mozzarella, Champignons, Kirschtomaten, Parmesankäse", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "LE PIZZE BIANCHE", Prezzo = 9f });

        menu.Piatti.Add(new Piatto { Nome = "Caprese (Mozzarella di bufala, basilico, pomodorini, olive)", Nome_EN = "Buffalo milk mozzarella, basìl, small round tomatoes, olives", Nome_GE = "Büffel-Mozzarella, Basilikum, Kirschtomaten, Oliven", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "LE PIZZE BIANCHE", Prezzo = 9f });

        menu.Piatti.Add(new Piatto { Nome = "Montanara (Mozzarella, funghi trifolati, speck)", Nome_EN = "Mozzarella, sliced mushrooms cooked in oil, garlic and parsley smoked ham", Nome_GE = "Mozzarella, marinierte Pilze, Speck", Allergeni = ToStr(new int[] { 1, 5, 7, 15 }), Tipo = "LE PIZZE BIANCHE", Prezzo = 9f });

        menu.Piatti.Add(new Piatto { Nome = "Tartufata (Mozzarella, funghi freschi, crema di tartufo)", Nome_EN = "Mozzarella, mushrooms, trufﬂe puree", Nome_GE = "Mozzarella, Champignons, Trüffelcreme", Allergeni = ToStr(new int[] { 1, 5, 7, 15 }), Tipo = "LE PIZZE BIANCHE", Prezzo = 9f });

        menu.Piatti.Add(new Piatto { Nome = "La Dama (mozzarella, radicchio, pancetta, noci e ristretto al balsamico)", Nome_EN = "Mozzarella, red chicory, bacon, walnuts and balsamic vinegar sauce", Nome_GE = "Mozzarella, rote Zichorie, Speck, Walnüsse und Balsamico-Essigsauce", Allergeni = ToStr(new int[] { 1, 5, 7, 8 }), Tipo = "LE PIZZE BIANCHE", Prezzo = 10f });

        menu.Piatti.Add(new Piatto { Nome = "Carbonara (Mozzarella, pancetta affumicata, uova, pepe, scaglie di grana)	", Nome_EN = "Mozzarella, smoked bacon, egg, pepper, flakes of parmesan", Nome_GE = "Mozzarella, geräucherter Bauchspeck, Eier, Pfeffer Parmesan", Allergeni = ToStr(new int[] { 1, 3, 5, 7 }), Tipo = "LE PIZZE BIANCHE", Prezzo = 10f });

        menu.Piatti.Add(new Piatto { Nome = "Fumè (Mozzarella, scamorza, salsiccia, melanzane)", Nome_EN = "Mozzarella, Scamorza cheese, spicy sausage, aubergines", Nome_GE = "Mozzarella, Scamorzakäse, ital. Hackfleischwurst, Auberginen", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "LE PIZZE BIANCHE", Prezzo = 9f });

        menu.Piatti.Add(new Piatto { Nome = "Monia (Mozzarella di bufala, porcini, rucola)", Nome_EN = "Buffalo milk mozzarella, porcini, rocket", Nome_GE = "Büffel-Mozzarella, Steinpilze, Rauke", Allergeni = ToStr(new int[] { 1, 5, 7, 15 }), Tipo = "LE PIZZE BIANCHE", Prezzo = 10f });

        menu.Piatti.Add(new Piatto { Nome = "Norvegese (Mozzarella, pomodorini, salmone, rucola)", Nome_EN = "Mozzarella, small round tomatoes, salmon, rocket", Nome_GE = "Mozzarella, Kirschtomaten, Lachs, Rauke", Allergeni = ToStr(new int[] { 1, 4, 5, 7 }), Tipo = "LE PIZZE BIANCHE", Prezzo = 10f });

        menu.Piatti.Add(new Piatto { Nome = "La Lei (Mozzarella, gamberi, zucchine)", Nome_EN = "Mozzarella, crayﬁsh, oourgettes", Nome_GE = "Mozzarella, Krabben, Zucchini", Allergeni = ToStr(new int[] { 1, 2, 5, 7, 12 }), Tipo = "LE PIZZE BIANCHE", Prezzo = 9f });

        menu.Piatti.Add(new Piatto { Nome = "La Genovese (Mozzarella, pesto, patate lesse, pomodorini, pinoli)", Nome_EN = "Mozzarella, pesto, pine nuts, boyled potatoes small round tomatoes", Nome_GE = "Mozzarella, Pestocreme, Pinienkernne, gedünstete Kartoffeln, Kirschtomaten", Allergeni = ToStr(new int[] { 1, 5, 7, 8, 15 }), Tipo = "LE PIZZE BIANCHE", Prezzo = 9f });

        menu.Piatti.Add(new Piatto { Nome = "Romagna Mia (Mozzarella, radicchio, salsiccia, gorgonzola)", Nome_EN = "Mozzarella, radicchio, gorgonzola cheese, sausace", Nome_GE = "Mozzarella, radicchio,  Gorgonzolakäse (süss), ital. Hackfleischwurst", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "LE PIZZE BIANCHE", Prezzo = 9f });

        menu.Piatti.Add(new Piatto { Nome = "Briosa (Mozzarella, funghi freschi brie e rucola)", Nome_EN = "Mozzarella, fresh mushrooms, brie and rocket", Nome_GE = "Mozzarella, Champignons, Briekäse und Rauke", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "LE PIZZE BIANCHE", Prezzo = 9f });

        menu.Piatti.Add(new Piatto { Nome = "Friarella (Mozzarella, salsiccia, friarelli e pomini)", Nome_EN = "Mozzarella, sausage, turnip greens and cherry tomatoes", Nome_GE = "Mozzarella, ital. Hackfleischwurst, grüne Rübe und Kirschtomaten", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "LE PIZZE BIANCHE", Prezzo = 10f });

        menu.Piatti.Add(new Piatto { Nome = "Sfiziosa (Mozzarella, carciofi, pancetta e taleggio in cottura)", Nome_EN = "Mozzarella, artichokes, bacon, Taleggio cheese", Nome_GE = "Mozzarella, Artischoken, Bauchspeck, Taleggiokaese", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "LE PIZZE BIANCHE", Prezzo = 10f });




        menu.Piatti.Add(new Piatto { Nome = "La Meravigliosa schiacciatina bianca(tutto in uscita, pomodorini secchi mozzarella di bufala pecorino a scaglie noci e basilico)", Nome_EN = "After cooking: dried tomatoes, buffalo mozzarella, pecorino flakes, nuts and basil", Nome_GE = "nach dem Kochen: getrockneten Tomaten, Büffelmozzarella , Pecorino Flocken, Nüssen und Basilikum", Allergeni = ToStr(new int[] { 1, 5, 7, 8 }), Tipo = "LE PIZZE D’AUTORE", Prezzo = 10f });

        menu.Piatti.Add(new Piatto { Nome = "Papa Francesco (Pomodoro, mozzarella, all’uscita bufala, pomodorini, scaglie di grana, crudo, rucola)	", Nome_EN = "Tomatoes, mozzarella, at the end  fresh buffalo milk mozzarella, small round tomatoes, parmesan, parma ham", Nome_GE = "Tomaten, Mozzarella zum Schluß: frischer Büffel-Mozzarella, Kirschtomaten, Parmesankäse, Parmaschinken", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "LE PIZZE D’AUTORE", Prezzo = 10f });

        menu.Piatti.Add(new Piatto { Nome = "Gully (Pomodoro, mozzarella di bufala, pomodorini, basilico, grana, olio extravergine, pepe)", Nome_EN = "Tomatoes, Mozzarella, Buffalo milk mozzarella, small round tomatoes, basil, parmesan, oil, pepper", Nome_GE = "Tomaten, Mozzarella, Büffel-mozzarella, Kirschtomaten, Basilikum, Parmesan, Olivenöl, Pfeffer", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "LE PIZZE D’AUTORE", Prezzo = 10f });

        menu.Piatti.Add(new Piatto { Nome = "Mokambo (Mozzarella, porcini, gorgonzola, scaglie di grana,melanzane)", Nome_EN = "Mozzarella, mushrooms, gorgonzola (sweet), parmesan, aubergines", Nome_GE = "Mozzarella, Steinpilze, Gorgonzola(süß), Parmesan, Melanzane", Allergeni = ToStr(new int[] { 1, 5, 7, 15 }), Tipo = "LE PIZZE D’AUTORE", Prezzo = 10f });

        menu.Piatti.Add(new Piatto { Nome = "Argentina (Pomodoro, mozzarella, salame piccante, gorgonzola, salsiccia e basilico)", Nome_EN = "Tomatoes,  mozzarella, spicy salami, gorgonzola sweet, spicy sausace and basil", Nome_GE = "Tomaten, Mozzarella, scharfe Salami, Gorgonzolakäse (süss), ital. Hackfleischwurst und Basilikum", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "LE PIZZE D’AUTORE", Prezzo = 10f });

        menu.Piatti.Add(new Piatto { Nome = "Gingko Biloba (pomodoro, mozzarella, porcini e speck)", Nome_EN = "Tomatoes, mozzarella, mushrooms and bacon", Nome_GE = "Tomaten, Mozzarella, Steinpilze und Speck", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "LE PIZZE D’AUTORE", Prezzo = 11f });

        menu.Piatti.Add(new Piatto { Nome = "La Norma (Melanzane fritte, pomodorini, in uscita basilico e ricotta stagionata)", Nome_EN = "Fried aubergines, cherry tomatoes, at the end basil and ricotta seasoned", Nome_GE = "fritierte Auberginen, Cocktailtomaten, zum Schluss Basilikum und gereifter Ricotta", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "LE PIZZE D’AUTORE", Prezzo = 11f });

        menu.Piatti.Add(new Piatto { Nome = "Silhouette (rossa, in uscita radicchio, rucola, pomodorini, arance e mandorle tostate)	", Nome_EN = "Tomatoes, at the end radicchio, Rocket, cherry tomatoes, oranges and toasted almonds", Nome_GE = "Tomatensauce, zum Schluss Radicchio, Rauke, Orangen und geroestete Mandeln", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "LE PIZZE D’AUTORE", Prezzo = 10f });

        menu.Piatti.Add(new Piatto { Nome = "Siciliana (rossa, in cottura cipolla di Tropea, pecorino stagionato, origano, alici)", Nome_EN = "Tomatoes, Tropea onions, seasoned sheepcheese, oregano, anchovies ", Nome_GE = "Tomaten, Tropea Zwiebel, gereifter Schafskaese, Oregano, Sardellen", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "LE PIZZE D’AUTORE", Prezzo = 10f });

        menu.Piatti.Add(new Piatto { Nome = "La Belpaese  schiacciatina soffice (in uscita prosciutto crudo, burrata, basilico e pomodorini secchi)", Nome_EN = "After cooking: raw ham, burrata cheese, basil and dried tomatoes", Nome_GE = "Nach dem Kochen: Rohschinken, Burrata Käse, Basilikum und getrocknete Tomaten", Allergeni = ToStr(new int[] { 1, 5, 7 }), Tipo = "LE PIZZE D’AUTORE", Prezzo = 12f });

        menu.Piatti.Add(new Piatto { Nome = "La Spagnola schiacciatina soffice  in uscita Pata Negra ( prosciutto iberico)", Nome_EN = "After cooking: Pata Negra spanish raw ham", Nome_GE = "Nach dem Kochen: Pata Negra Spanischer Rohschinken", Allergeni = ToStr(new int[] { 1, 5 }), Tipo = "LE PIZZE D’AUTORE", Prezzo = 13f });



        menu.Piatti.Add(new Piatto { Nome = "Impasto con farina di Kamut® o integrale", Nome_EN = "Produced with Kamut© flour", Nome_GE = "Pizza mit Kamut© Mehl", Allergeni = ToStr(new int[] { }), Tipo = "", Prezzo = 2f });

        menu.Piatti.Add(new Piatto { Nome = "Aggiunta di Prosciutto Crudo", Nome_EN = "Addition Parma ham", Nome_GE = "Aufpreis für Rohschinken", Allergeni = ToStr(new int[] { }), Tipo = "", Prezzo = 2f });

        menu.Piatti.Add(new Piatto { Nome = "Aggiunta mozzarella di Bufala", Nome_EN = "Addition of buffalo milk mozzarella", Nome_GE = "Aufpreis für Büffel-Mozzarella", Allergeni = ToStr(new int[] { }), Tipo = "", Prezzo = 2f });

        menu.Piatti.Add(new Piatto { Nome = "Pizza stesa", Nome_EN = "Super-thin big pizza", Nome_GE = "Pizza gross und extra dünn", Allergeni = ToStr(new int[] { }), Tipo = "", Prezzo = 2f });

        menu.Piatti.Add(new Piatto { Nome = "Doppio impasto", Nome_EN = "Double thin pizza", Nome_GE = "doppelter Pizzateig", Allergeni = ToStr(new int[] { }), Tipo = "", Prezzo = 2f });

        menu.Piatti.Add(new Piatto { Nome = "Pizza baby", Nome_EN = "small pizza", Nome_GE = "kleine Pizza", Allergeni = ToStr(new int[] { }), Tipo = "", Prezzo = -1f });



        menu.Piatti.Add(new Piatto { Nome = "Sorbetto al caffè", Nome_EN = "Coffee sorbet", Nome_GE = "Kaffeesorbet", Allergeni = ToStr(new int[] { 7 }), Tipo = "I DOLCI FATTI DA NOI ", Prezzo = 3f });

        menu.Piatti.Add(new Piatto { Nome = "Sorbetto al limone", Nome_EN = "Lemon sorbet", Nome_GE = "Zitronensorbet", Allergeni = ToStr(new int[] { 7 }), Tipo = "I DOLCI FATTI DA NOI ", Prezzo = 3f });

        menu.Piatti.Add(new Piatto { Nome = "Zuppa Inglese", Nome_EN = "Dessert with chocolate and vanilla creama, sponge biscuits and alkermes", Nome_GE = "Dessert mit Schichten aus Schokolade-Vanillepudding, Löffelbiskuits und Alchermes", Allergeni = ToStr(new int[] { 1, 3, 7 }), Tipo = "I DOLCI FATTI DA NOI ", Prezzo = 5f });

        menu.Piatti.Add(new Piatto { Nome = "Crema catalana", Nome_EN = "Catalan Cream", Nome_GE = "Katalanische Creme", Allergeni = ToStr(new int[] { 3, 7 }), Tipo = "I DOLCI FATTI DA NOI ", Prezzo = 5f });

        menu.Piatti.Add(new Piatto { Nome = "Mascarpone", Nome_EN = "Mascarpone", Nome_GE = "Mascarpone", Allergeni = ToStr(new int[] { 1, 3, 6, 7, 8 }), Tipo = "I DOLCI FATTI DA NOI ", Prezzo = 5f });

        menu.Piatti.Add(new Piatto { Nome = "Babà Romagnolo", Nome_EN = "Romagnolo baba with cream and fruit", Nome_GE = "Rum-Baba mit Creme und Obst", Allergeni = ToStr(new int[] { 1, 3, 7 }), Tipo = "I DOLCI FATTI DA NOI ", Prezzo = 6f });

        menu.Piatti.Add(new Piatto { Nome = "Budino Margot (Tipo Crema catalana con Amaretti, Cioccolata e caramello)", Nome_EN = "Pudding \"Catalan\" with amaretti, chocolate and caramel", Nome_GE = "Pudding nach Katalanischerart mit Amaretti, Schokolade und Karamel", Allergeni = ToStr(new int[] { 1, 3, 6, 7 }), Tipo = "I DOLCI FATTI DA NOI ", Prezzo = 5f });

        menu.Piatti.Add(new Piatto { Nome = "Cheesecake ai frutti di bosco", Nome_EN = "Feel cheescake with wild berry", Nome_GE = "Käsekuchen im Glas mit Waldfrüchte", Allergeni = ToStr(new int[] { 1, 3, 7, 8 }), Tipo = "I DOLCI FATTI DA NOI ", Prezzo = 6f });

        menu.Piatti.Add(new Piatto { Nome = "Semifreddo al torrone con salsa al mou", Nome_EN = "Nougat-Parfait with sauce mou", Nome_GE = "Halbgefrorenes Nougatparfait mit Mousauce", Allergeni = ToStr(new int[] { 3, 7, 8 }), Tipo = "I DOLCI FATTI DA NOI ", Prezzo = 5f });

        menu.Piatti.Add(new Piatto { Nome = "Rocher con croccante  ai cereali e pralinato al cioccolato", Nome_EN = "Mascarpone ice-cream with crunchy cereals and chocolate praline", Nome_GE = "Mascarponeeis ueberzogen mit Schokolade auf einer knusprigen Unterlage", Allergeni = ToStr(new int[] { 1, 3, 6, 7, 8 }), Tipo = "I DOLCI FATTI DA NOI ", Prezzo = 6f });

        menu.Piatti.Add(new Piatto { Nome = "Pavlova(meringa soffice)con panna alle fragole", Nome_EN = "Soft meringue with whipped strawberry cream ", Nome_GE = "Meringe mit Erdbeercreme", Allergeni = ToStr(new int[] { 3, 6, 7 }), Tipo = "I DOLCI FATTI DA NOI ", Prezzo = 6f });

        menu.Piatti.Add(new Piatto { Nome = "Semifreddo al pistacchio", Nome_EN = "pistachio-Parfait", Nome_GE = "Pistazien Nussparfait", Allergeni = ToStr(new int[] { 3, 7, 8 }), Tipo = "I DOLCI FATTI DA NOI ", Prezzo = 6f });

        menu.Piatti.Add(new Piatto { Nome = "Bavarese alle mandorle", Nome_EN = "Almond bavarian cream", Nome_GE = "Bayrísche Mandelcreme", Allergeni = ToStr(new int[] { 3, 7, 8 }), Tipo = "I DOLCI FATTI DA NOI ", Prezzo = 6f });

        menu.Piatti.Add(new Piatto { Nome = "Dolce dello Chef con zabaione e pistacchio", Nome_EN = "Zabaione and pistachis cream", Nome_GE = "Sabayon und Pistaziencreme", Allergeni = ToStr(new int[] { 1, 3, 6, 7, 8, 12 }), Tipo = "I DOLCI FATTI DA NOI ", Prezzo = 6f });

        menu.Piatti.Add(new Piatto { Nome = "Panna cotta al caramello Frutti di bosco", Nome_EN = "Panna cotta with caramel ore wild fruit", Nome_GE = "Panna cotta mit Karamelsauce oder Waldfrüchten", Allergeni = ToStr(new int[] { 7 }), Tipo = "I DOLCI FATTI DA NOI ", Prezzo = 5f });

        menu.Piatti.Add(new Piatto { Nome = "Tortino al cioccolato cuore morbido", Nome_EN = "Soft centred chocolate cake", Nome_GE = "Schokoladetörtchen mit weichem Schokoladenherz", Allergeni = ToStr(new int[] { 1, 3, 6, 7, 8 }), Tipo = "I DOLCI FATTI DA NOI ", Prezzo = 6f });

        menu.Piatti.Add(new Piatto { Nome = "Tortino al cioccolato extra fondente vegan", Nome_EN = "vegan Soft centred extra dark chocolate cake", Nome_GE = "Veganschokoladetörtchen mit weichem extra-Zartbitterschokoladenherz", Allergeni = ToStr(new int[] { 1, 6, 8 }), Tipo = "I DOLCI FATTI DA NOI ", Prezzo = 6f });

        menu.Piatti.Add(new Piatto { Nome = "Tortino al cioccolato senza glutine", Nome_EN = "Glutenfree Soft centred chocolate cake", Nome_GE = "Glutenfreies Schokoladetörtchen mit weichem Schokoladenherz ", Allergeni = ToStr(new int[] { 3, 6, 7, 8 }), Tipo = "I DOLCI FATTI DA NOI ", Prezzo = 6f });

        menu.Piatti.Add(new Piatto { Nome = "Gelato artigianale a pallina", Nome_EN = "homemade ice cream", Nome_GE = "Hausgemachtes Eis ", Allergeni = ToStr(new int[] { 1, 3, 7, 8, 12 }), Tipo = "I DOLCI FATTI DA NOI ", Prezzo = 1.5f });

        menu.Piatti.Add(new Piatto { Nome = "Gelato di yogurt artigianale", Nome_EN = "Homemade yogurt ice cream", Nome_GE = "Hausgemachtes Joghurteis", Allergeni = ToStr(new int[] { 7 }), Tipo = "I DOLCI FATTI DA NOI ", Prezzo = 5f });

        menu.Piatti.Add(new Piatto { Nome = "Macedonia", Nome_EN = "Fruit salad", Nome_GE = "Obstsalat", Allergeni = ToStr(new int[] { }), Tipo = "I DOLCI FATTI DA NOI ", Prezzo = 6f });

        menu.Piatti.Add(new Piatto { Nome = "Ananas", Nome_EN = "Pineapple", Nome_GE = "Ananas", Allergeni = ToStr(new int[] { }), Tipo = "I DOLCI FATTI DA NOI ", Prezzo = 5f });

        menu.Piatti.Add(new Piatto { Nome = "Cocomero", Nome_EN = "Watermelon", Nome_GE = "Wassermelone", Allergeni = ToStr(new int[] { }), Tipo = "I DOLCI FATTI DA NOI ", Prezzo = 6f });




        menu.Piatti.Add(new Piatto { Nome = "Pignoletto	Uve Pignoletto - Umberto Cesari", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Bollicine 375 ml", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "\"Sergio Brut\" Prosecco di Valdobbiadene - Mionetto", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Bollicine 375 ml", Prezzo = 10f });

        menu.Piatti.Add(new Piatto { Nome = "\"Nino Franco\" 	Prosecco di Valdobbiadene - Nino Franco", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Bollicine 375 ml", Prezzo = 10f });

        menu.Piatti.Add(new Piatto { Nome = "Ca’ del bosco\"Brut \"	Franciacorta Cuvée Prestige	- Ca’ del Bosco", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Bollicine 375 ml", Prezzo = 18f });



        menu.Piatti.Add(new Piatto { Nome = "\"Rambéla\" Famoso - Randi", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Bianchi 375 ml", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "I Frati Lugana	- Ca’ dei Frati", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Bianchi 375 ml", Prezzo = 10f });

        menu.Piatti.Add(new Piatto { Nome = "Gewürtztraminer Gewürtztraminer - Tramin", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Bianchi 375 ml", Prezzo = 10f });

        menu.Piatti.Add(new Piatto { Nome = "Verdicchio	Verdicchio - Garofoli", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Bianchi 375 ml", Prezzo = 9f });

        menu.Piatti.Add(new Piatto { Nome = "Falanghina	Falanghina I.G.T - Terredora", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Bianchi 375 ml", Prezzo = 9f });

        menu.Piatti.Add(new Piatto { Nome = "Greco di Tufo Greco di Tufo D.O.C.G - Terredora", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Bianchi 375 ml", Prezzo = 10f });

        menu.Piatti.Add(new Piatto { Nome = "\"Cuccaione\" (Sardegna)	Vermentino - Mancini", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Bianchi 375 ml", Prezzo = 9f });




        menu.Piatti.Add(new Piatto { Nome = "Le More Sangiovese Superiore - Castelluccio", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Rossi 375 ml", Prezzo = 8.50f });

        menu.Piatti.Add(new Piatto { Nome = "Centurione Sangiovese Superiore - Ferrucci", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Rossi 375 ml", Prezzo = 8f });

        menu.Piatti.Add(new Piatto { Nome = "Il Prugneto Sangiovese Superiore - Poderi dal Nespoli", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Rossi 375 ml", Prezzo = 8f });

        menu.Piatti.Add(new Piatto { Nome = "Burson Uve Longanesi - Randi", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Rossi 375 ml", Prezzo = 8.50f });

        menu.Piatti.Add(new Piatto { Nome = "Nero di Lambrusco Lambrusco - Ceci", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Rossi 375 ml", Prezzo = 8.5f });

        menu.Piatti.Add(new Piatto { Nome = "Ronchedone	Marzemino, Sangiovese Cabernet - Ca’ dei Frati", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Rossi 375 ml", Prezzo = 10f });

        menu.Piatti.Add(new Piatto { Nome = "Pinot Nero (indicato per il pesce)	Pinot Nero - Tramin", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Rossi 375 ml", Prezzo = 10f });

        menu.Piatti.Add(new Piatto { Nome = "Sedàra	Merlot e Nero D’avola - Donna Fugata", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Rossi 375 ml", Prezzo = 9f });




        menu.Piatti.Add(new Piatto { Nome = "Moët&Chandon Cuvée Resérve Impériale Brut - Moët & Chandon", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Champagne 750 ml", Prezzo = 55f });

        menu.Piatti.Add(new Piatto { Nome = "Brut Nature Dosage zero Pinot nero - Drappier", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Champagne 750 ml", Prezzo = 65f });

        menu.Piatti.Add(new Piatto { Nome = "Drappier Blanc de Blanc Chardonnay - Drappier", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Champagne 750 ml", Prezzo = 70f });

        menu.Piatti.Add(new Piatto { Nome = "Ruinart Cuvée Brut - Ruinart", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Champagne 750 ml", Prezzo = 80f });




        menu.Piatti.Add(new Piatto { Nome = "Cà del Bosco Brut Franciacorta Cuvée Prestige - Cà del Bosco", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Franciacorta 750 ml", Prezzo = 38f });

        menu.Piatti.Add(new Piatto { Nome = "Bellavista Alma Cuvée 	Brut - Bellavista", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Franciacorta 750 ml", Prezzo = 35f });

        menu.Piatti.Add(new Piatto { Nome = "Bersi Serlini Saten Saten Bersi - Serlini", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Franciacorta 750 ml", Prezzo = 27f });

        menu.Piatti.Add(new Piatto { Nome = "Bersi Serlini Extra Brut Millesimato Brut - Bersi Serlini", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Franciacorta 750 ml", Prezzo = 30f });

        menu.Piatti.Add(new Piatto { Nome = "Bersi Serlini Cuvée 4 Millesimato Brut - Bersi Serlini", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Franciacorta 750 ml", Prezzo = 30f });

        menu.Piatti.Add(new Piatto { Nome = "Antica Fratta Brut	Brut - Antica Fratta", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Franciacorta 750 ml", Prezzo = 29f });

        menu.Piatti.Add(new Piatto { Nome = "Antica Fratta Saten Brut Saten - Antica Fratta", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Franciacorta 750 ml", Prezzo = 32f });

        menu.Piatti.Add(new Piatto { Nome = "Le Marchesine Brut Satèn millesimato - Le Marchesine", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Franciacorta 750 ml", Prezzo = 35f });

        menu.Piatti.Add(new Piatto { Nome = "Monterossa Brut Prima Cuvée Brut - Monterossa", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Franciacorta 750 ml", Prezzo = 30f });

        menu.Piatti.Add(new Piatto { Nome = "Monterossa Saten Saten - Monterossa", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Franciacorta 750 ml", Prezzo = 30f });





        menu.Piatti.Add(new Piatto { Nome = "Mea Rosa Liguria di Levante i.g.t. rosato - Lunae Bosoni", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Rosé 750 ml", Prezzo = 17f });

        menu.Piatti.Add(new Piatto { Nome = "Rymarosé Sangiovese - Ciù Ciù", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Rosé 750 ml", Prezzo = 18f });

        menu.Piatti.Add(new Piatto { Nome = "Le Marchesine Brut Saten Mill.	Chard. Pinot B., Pinot N. - Le Marchesine", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Rosé 750 ml", Prezzo = 35f });

        menu.Piatti.Add(new Piatto { Nome = "Monterossa Flamingo Rosé Chardonnay, Pinot Nero - Monterossa", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Rosé 750 ml", Prezzo = 35f });

        menu.Piatti.Add(new Piatto { Nome = "Brut Rosé Talento Trento D.O.C. - Letrari", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Rosé 750 ml", Prezzo = 29f });





        menu.Piatti.Add(new Piatto { Nome = "Trento D.O.C. Dosaggio Zero Trento D.O.C. - Letrari", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Trento D.O.C. 750 ml", Prezzo = 29f });

        menu.Piatti.Add(new Piatto { Nome = "Quore Brut Riserva	100% Chardonnay - Letrari", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Trento D.O.C. 750 ml", Prezzo = 38f });

        menu.Piatti.Add(new Piatto { Nome = "Brut Riserva Pinot nero/Chardonnay - Letrari", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Trento D.O.C. 750 ml", Prezzo = 38f });



        menu.Piatti.Add(new Piatto { Nome = "Cartizze Dry Valdobbiadene Superiore - Bortolin", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Valdobbiadene 750ml", Prezzo = 32f });

        menu.Piatti.Add(new Piatto { Nome = "Valdobbiadene Prosecco di Valdobbiadene D.O.C. - Bortolin", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Valdobbiadene 750ml", Prezzo = 18f });

        menu.Piatti.Add(new Piatto { Nome = "Zan Prosecco senza l’aggiunta di solfiti - Bortolin", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Valdobbiadene 750ml", Prezzo = 18f });

        menu.Piatti.Add(new Piatto { Nome = "San Floriano Prosecco di Valdobbiadene Superiore - Nino Franco", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Valdobbiadene 750ml", Prezzo = 19f });

        menu.Piatti.Add(new Piatto { Nome = "\"Nino Brut\" Prosecco di Valdobbiadene D.O.C. - Nino Franco", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Valdobbiadene 750ml", Prezzo = 17f });

        menu.Piatti.Add(new Piatto { Nome = "Cuvée Oris	Prosecco di Valdobbiadene  D.O.C. - Villa Sandi", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Valdobbiadene 750ml", Prezzo = 18f });

        menu.Piatti.Add(new Piatto { Nome = "Sergio Cuvée Mionetto Prosecco di Valdobbiadene D.O.C. - Mionetto", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Valdobbiadene 750ml", Prezzo = 17f });




        menu.Piatti.Add(new Piatto { Nome = "Pignoletto Uve Pignoletto - Umberto Cesari", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Bollicine 750ml", Prezzo = 13f });

        menu.Piatti.Add(new Piatto { Nome = "\"E Fighet\" Pignoletto Uve Pignoletto - Galassi", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Bollicine 750ml", Prezzo = 13f });

        menu.Piatti.Add(new Piatto { Nome = "Passerina brut Uve Passerina - Velenosi", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Bollicine 750ml", Prezzo = 18f });

        menu.Piatti.Add(new Piatto { Nome = "Pecorino brut Uve Pecorino - Tenuta Ulisse", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Bollicine 750ml", Prezzo = 18f });

        menu.Piatti.Add(new Piatto { Nome = "Ribolla brut Ribolla gialla - Villa Sandi", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Bollicine 750ml", Prezzo = 18f });





        menu.Piatti.Add(new Piatto { Nome = "Vintage Tunina	Sauvi., Chard., Ribolla Gialla, Malvasia Istriana, Picolit Dolegna del Collio (GO) - Jermann", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "I Grandi Bianchi 750ml", Prezzo = 45f });

        menu.Piatti.Add(new Piatto { Nome = "Capichera Isola dei Nuraghi Vermentino IGT Arzachena  (OT) - Capichera", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "I Grandi Bianchi 750ml", Prezzo = 45f });

        menu.Piatti.Add(new Piatto { Nome = "Sauvignon de la Tour Sauvignon barrique Capriva del Friuli (GO) - Villa Russiz", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "I Grandi Bianchi 750ml", Prezzo = 35f });

        menu.Piatti.Add(new Piatto { Nome = "Gräfin de la Tour Chardonnay barrique Capriva del Friuli (GO) - Villa Russiz", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "I Grandi Bianchi 750ml", Prezzo = 35f });

        menu.Piatti.Add(new Piatto { Nome = "Il Livio DOC Pinot bianco Chardonnay, picolit Brazzano (GO) - Livio Felluga", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "I Grandi Bianchi 750ml", Prezzo = 30f });

        menu.Piatti.Add(new Piatto { Nome = "Sauvignon Sauvignon - Livio Felluga", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "I Grandi Bianchi 750ml", Prezzo = 25f });

        menu.Piatti.Add(new Piatto { Nome = "Pinot Grigio Pinot grigio Brazzano (GO) - Livio Felluga", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "I Grandi Bianchi 750ml", Prezzo = 25f });

        menu.Piatti.Add(new Piatto { Nome = "Friulano Friulano Brazzano (GO) - Livio Felluga", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "I Grandi Bianchi 750ml", Prezzo = 25f });

        menu.Piatti.Add(new Piatto { Nome = "Blangé Bio	Arneis Alba (CN) - Ceretto", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "I Grandi Bianchi 750ml", Prezzo = 25f });





        menu.Piatti.Add(new Piatto { Nome = "Progetto Uno Bio Albana di Romagna	Leone Conti	Faenza (RA)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Bianchi 750ml", Prezzo = 17f });

        menu.Piatti.Add(new Piatto { Nome = "Rambéla Famoso	Randi Fusignano(RA)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Bianchi 750ml", Prezzo = 14f });

        menu.Piatti.Add(new Piatto { Nome = "Podium	Verdicchio Garofoli Loreto (AN)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Bianchi 750ml", Prezzo = 18f });

        menu.Piatti.Add(new Piatto { Nome = "I Frati Lugana	Ca’ dei Frati Sirmione (BR)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Bianchi 750ml", Prezzo = 20f });

        menu.Piatti.Add(new Piatto { Nome = "Riesling Riesling Colterenzio	Cornaiano(BZ)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Bianchi 750ml", Prezzo = 20f });

        menu.Piatti.Add(new Piatto { Nome = "Moriz	Pinot Bianco Tramin	Termeno (BZ)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Bianchi 750ml", Prezzo = 22f });

        menu.Piatti.Add(new Piatto { Nome = "Gewürztraminer	Gewürztraminer Tramin Termeno (BZ)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Bianchi 750ml", Prezzo = 20f });

        menu.Piatti.Add(new Piatto { Nome = "Müller Thürgau	Müller Thürgau Tramin Termeno (BZ)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Bianchi 750ml", Prezzo = 18f });

        menu.Piatti.Add(new Piatto { Nome = "Sauvignon Sauvignon Tramin	Termeno (BZ)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Bianchi 750ml", Prezzo = 18f });

        menu.Piatti.Add(new Piatto { Nome = "Pinot Grigio Pinot grigio Tramin Termeno (BZ)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Bianchi 750ml", Prezzo = 18f });

        menu.Piatti.Add(new Piatto { Nome = "Chardonnay	Chardonnay Tramin Termeno (BZ)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Bianchi 750ml", Prezzo = 18f });

        menu.Piatti.Add(new Piatto { Nome = "Friulano Friulano Petrucco	Butrio (UD)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Bianchi 750ml", Prezzo = 18f });

        menu.Piatti.Add(new Piatto { Nome = "Ribolla Gialla	Ribolla Gialla Petrucco	Butrio (UD)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Bianchi 750ml", Prezzo = 18f });

        menu.Piatti.Add(new Piatto { Nome = "Pinot bianco Pinot bianco Petrucco	Butrio (UD)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Bianchi 750ml", Prezzo = 20f });

        menu.Piatti.Add(new Piatto { Nome = "Ribolla Ribolla Bastianich	Cividale del Friuli (UD)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Bianchi 750ml", Prezzo = 18f });

        menu.Piatti.Add(new Piatto { Nome = "Il Friulano Friulano Bastianich Cividale del Friuli (UD)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Bianchi 750ml", Prezzo = 18f });

        menu.Piatti.Add(new Piatto { Nome = "Greco di tufo Greco di Tufo Mastroberardino Atripalda (AV)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Bianchi 750ml", Prezzo = 18f });

        menu.Piatti.Add(new Piatto { Nome = "Falanghina	Falanghina Mastroberardino	Atripalda (AV) ", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Bianchi 750ml", Prezzo = 18f });

        menu.Piatti.Add(new Piatto { Nome = "Passerina  Bio	Passerina Ciù Ciù Offida (AP)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Bianchi 750ml", Prezzo = 18f });

        menu.Piatti.Add(new Piatto { Nome = "Pecorino Bio Pecorino Ciù Ciù Offida (AP)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Bianchi 750ml", Prezzo = 18f });

        menu.Piatti.Add(new Piatto { Nome = "Lunae (Tosco/Ligure) Vermentino Lunae Bosoni Ortonovo (SP) ", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Bianchi 750ml", Prezzo = 16f });

        menu.Piatti.Add(new Piatto { Nome = "Cuccaione (Sardegna) Vermentino di Gallura	Piero Mancini	Olbia", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Bianchi 750ml", Prezzo = 18f });

        menu.Piatti.Add(new Piatto { Nome = "Sur Sur Grillo	Donna Fugata Marsala (TP)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Bianchi 750ml", Prezzo = 18f });

        menu.Piatti.Add(new Piatto { Nome = "Anthilia Cataratto	Donna Fugata Marsala (TP)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Bianchi 750ml", Prezzo = 18f });

        menu.Piatti.Add(new Piatto { Nome = "Lighea Zibibbo	Donna Fugata Marsala (TP)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Bianchi 750ml", Prezzo = 18f });






        menu.Piatti.Add(new Piatto { Nome = "Barolo 2012 Barolo	Fontanafredda Serralunga d’Alba (CN)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "I Grandi Rossi 750ml", Prezzo = 56f });

        menu.Piatti.Add(new Piatto { Nome = "Amarone della Valpolicella	Amarone	Zenato Peschiera del Garda (VR)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "I Grandi Rossi 750ml", Prezzo = 45f });

        menu.Piatti.Add(new Piatto { Nome = "Valpolicella Ripasso Corvina Veronese, Rondinella	Zenato	Peschiera del Garda (VR)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "I Grandi Rossi 750ml", Prezzo = 32f });

        menu.Piatti.Add(new Piatto { Nome = "Il Bruciato Bolgheri Cabernet, Sauvignon, Merlot e Syrah Tenuta Guada al Tasso	Bolgheri (LI)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "I Grandi Rossi 750ml", Prezzo = 26f });

        menu.Piatti.Add(new Piatto { Nome = "Chianti Rufina Ris. Nipozzano D.O.C.G. Sangiovese Frescobaldi Nipozzanoi (FI)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "I Grandi Rossi 750ml", Prezzo = 24f });

        menu.Piatti.Add(new Piatto { Nome = "Campo ai Sassi Rosso di Montalcino DOC Sangiovese Frescobaldi Castel Giocondo	Montalcino (SI)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "I Grandi Rossi 750ml", Prezzo = 22f });

        menu.Piatti.Add(new Piatto { Nome = "Brunello di Montalcino DOCG Sangiovese	Frescobaldi Castel Giocondo	Montalcino(SI)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "I Grandi Rossi 750ml", Prezzo = 52f });

        menu.Piatti.Add(new Piatto { Nome = "Altrovino (biodinamico) Bio Merlot, Cabernet Franc. Due Mani Ripabella (PI)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "I Grandi Rossi 750ml", Prezzo = 38f });

        menu.Piatti.Add(new Piatto { Nome = "Cifra (biodinamico) Bio Cabernet Franc. Due Mani Ripabella(PI)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "I Grandi Rossi 750ml", Prezzo = 30f });

        menu.Piatti.Add(new Piatto { Nome = "Il Sasso Sangiovese Sup. Ris. D.O.C. Pertinello Galeata(FC)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "I Grandi Rossi 750ml", Prezzo = 30f });

        menu.Piatti.Add(new Piatto { Nome = "Domus Caia Sangiovese Ferrucci	Castelbolognese(RA)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "I Grandi Rossi 750ml", Prezzo = 35f });



        menu.Piatti.Add(new Piatto { Nome = "Bursòn	Uve Longanesi Randi Fusignano(RA)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Rossi 750ml", Prezzo = 15f });

        menu.Piatti.Add(new Piatto { Nome = "Ippogrifo Sangiovese Superiore da uve stramature Cantina Buon Pastore Sant’Alberto(RA)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Rossi 750ml", Prezzo = 19f });

        menu.Piatti.Add(new Piatto { Nome = "Monticino Rosso Sangiovese Superiore D.O.C. Monticino Rosso Imola(BO)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Rossi 750ml", Prezzo = 16f });

        menu.Piatti.Add(new Piatto { Nome = "Pertinello	Sangiovese Superiore D.O.C.	Pertinello	Galeata(FC)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Rossi 750ml", Prezzo = 18f });

        menu.Piatti.Add(new Piatto { Nome = "Pinot Nero	Pinot Nero	Pertinello Galeata(FC)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Rossi 750ml", Prezzo = 22f });

        menu.Piatti.Add(new Piatto { Nome = "E Fighet Sangiovese Superiore D.O.C. Aromi Italia Faenza(RA)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Rossi 750ml", Prezzo = 14f });

        menu.Piatti.Add(new Piatto { Nome = "Le More Sangiovese Superiore Castelluccio	Modigliana(FC)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Rossi 750ml", Prezzo = 15f });

        menu.Piatti.Add(new Piatto { Nome = "Centurione Sangiovese Superiore Ferrucci Castelbolognese(RA)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Rossi 750ml", Prezzo = 17f });

        menu.Piatti.Add(new Piatto { Nome = "Il Prugneto Sangiovese Superiore Poderi dal Nespoli Civitella(FC)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Rossi 750ml", Prezzo = 17f });

        menu.Piatti.Add(new Piatto { Nome = "Il Prugneto serie Oro Sangiovese Superiore Riserva Poderi dal Nespoli	Civitella(FC)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Rossi 750ml", Prezzo = 22f });

        menu.Piatti.Add(new Piatto { Nome = "S.cètt Bio senza solfiti Sangiovese Superiore Tenuta Santa Lucia Mercato Saraceno(FC)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Rossi 750ml", Prezzo = 17f });

        menu.Piatti.Add(new Piatto { Nome = "Nero di Lambrusco 	Lambrusco Ceci	Torrile(PR)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Rossi 750ml", Prezzo = 17f });

        menu.Piatti.Add(new Piatto { Nome = "Ronchedone	Marzemino, Sangiovese Cabernet Ca’ dei Frati Sirmione(BS)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Rossi 750ml", Prezzo = 20f });

        menu.Piatti.Add(new Piatto { Nome = "Pinot nero (indicato per pesce) Pinot Nero	Tramin Termeno(BZ)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Rossi 750ml", Prezzo = 20f });

        menu.Piatti.Add(new Piatto { Nome = "Heba Morellino di Scansano	Fattoria di Magliano Sterpeti(GR)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Rossi 750ml", Prezzo = 17f });

        menu.Piatti.Add(new Piatto { Nome = "Chianti Colli Fiorentini DOCG Sangiovese e Colorino Terre a Cona S. Donato in Collina(FI)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Rossi 750ml", Prezzo = 17f });

        menu.Piatti.Add(new Piatto { Nome = "Cantalupi Primitivo Salento IGP Tenuta Conti Zecca	Salice Salentino(LE)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Rossi 750ml", Prezzo = 18f });

        menu.Piatti.Add(new Piatto { Nome = "Sedàra	Nero d’Avola, Merlot Donna Fugata Marsala(TP)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini Rossi 750ml", Prezzo = 18f });




        menu.Piatti.Add(new Piatto { Nome = "Domus Aurea Albana Passita	Ferrucci Castel Bolognese(RA)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini da Dessert 750ml", Prezzo = 22f });

        menu.Piatti.Add(new Piatto { Nome = "Moscato Fior d’Arancio	Moscato	Don Giovanni da ponte Corbanese di T.(TV)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini da Dessert 750ml", Prezzo = 15f });

        menu.Piatti.Add(new Piatto { Nome = "Visciole Vino liquoroso Velenosi Ascoli Piceno", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini da Dessert 750ml", Prezzo = 15f });

        menu.Piatti.Add(new Piatto { Nome = "Albana Passita	Albana Fattoria Monticino Rosso	Casola Valsenio(RA)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "Vini da Dessert 750ml", Prezzo = 22f });




        menu.Piatti.Add(new Piatto { Nome = "Bollicine Bersi Serlini Brut Chardonnay Bersi Serlini - Provaglio d’Iseo(BS)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "LE MAGNUM 1,5L", Prezzo = 55f });

        menu.Piatti.Add(new Piatto { Nome = "Vini Bianchi Lugana Lugana doc	Cà dei Frati - Sirmione(BS) ", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "LE MAGNUM 1,5L", Prezzo = 45f });

        menu.Piatti.Add(new Piatto { Nome = "Vini Rossi Ronchedone	Marzemino, Sangiovese Cabernet Ca’ dei Frati - Sirmione(BS)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "LE MAGNUM 1,5L", Prezzo = 45f });

        menu.Piatti.Add(new Piatto { Nome = "Vini Rossi Otello Lambrusco Ceci - Torrile (PR)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "LE MAGNUM 1,5L", Prezzo = 42f });






        menu.Piatti.Add(new Piatto { Nome = "Birra chiara alla spina König Ludwig Hell	25cl.", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BIRRE ALLA SPINA", Prezzo = 3f });

        menu.Piatti.Add(new Piatto { Nome = "Birra chiara alla spina König Ludwig Hell	40cl.", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BIRRE ALLA SPINA", Prezzo = 4.50f });

        menu.Piatti.Add(new Piatto { Nome = "Birra chiara alla spina König Ludwig Hell	100cl.", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BIRRE ALLA SPINA", Prezzo = 9f });

        menu.Piatti.Add(new Piatto { Nome = "Birra Rossa alla spina König Ludwig Dunkel	25cl.", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BIRRE ALLA SPINA", Prezzo = 3.50f });

        menu.Piatti.Add(new Piatto { Nome = "Birra Rossa alla spina König Ludwig Dunkel	40cl.", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BIRRE ALLA SPINA", Prezzo = 5.50f });

        menu.Piatti.Add(new Piatto { Nome = "Birra Rossa alla spina König Ludwig Dunkel	100cl.", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BIRRE ALLA SPINA", Prezzo = 11f });

        menu.Piatti.Add(new Piatto { Nome = "WeisseKönig Ludwig Weiss	25cl.", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BIRRE ALLA SPINA", Prezzo = 4f });

        menu.Piatti.Add(new Piatto { Nome = "WeisseKönig Ludwig Weiss	40cl.", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BIRRE ALLA SPINA", Prezzo = 6f });

        menu.Piatti.Add(new Piatto { Nome = "WeisseKönig Ludwig Weiss	100cl.", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BIRRE ALLA SPINA", Prezzo = 12f });



        menu.Piatti.Add(new Piatto { Nome = "Gradisca La Birra di Fellini 2lt.", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BIRRE IN BOTTIGLIA", Prezzo = 25f });

        menu.Piatti.Add(new Piatto { Nome = "Ichnusa non filtrata 33cl.", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BIRRE IN BOTTIGLIA", Prezzo = 4f });

        menu.Piatti.Add(new Piatto { Nome = "Peroni Gran Riserva Bianca 50cl.", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BIRRE IN BOTTIGLIA", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Analcolica 33cl.", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BIRRE IN BOTTIGLIA", Prezzo = 5f });

        menu.Piatti.Add(new Piatto { Nome = "Warsteiner 66cl.", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BIRRE IN BOTTIGLIA", Prezzo = 5f });



        menu.Piatti.Add(new Piatto { Nome = "Caveja (weiss)	- Birrificio Valsenio 50cl.", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BIRRE ARTIGIANALI", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "I.R.A. (rossa stile irlandese) - Birrificio Valsenio 50cl.", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BIRRE ARTIGIANALI", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Redneck (speziata) adatta alle carni - Birrificio Valsenio	50cl.", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BIRRE ARTIGIANALI", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Poggia Nero Stout - Birrificio Valsenio 50cl.", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BIRRE ARTIGIANALI", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Dora (aroma erbaceo e di frutta fresca) - Birrificio La Mata 50cl.", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BIRRE ARTIGIANALI", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Salinae - Birra Sale di Cervia	33cl.", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BIRRE ARTIGIANALI", Prezzo = 8.50f });

        menu.Piatti.Add(new Piatto { Nome = "Salinae - Birra Sale di Cervia	75cl.", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BIRRE ARTIGIANALI", Prezzo = 15f });

        menu.Piatti.Add(new Piatto { Nome = "Hippy (birra chiara non pastorizzata e non filtrata) - Birrificio Ceci	75cl.", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BIRRE ARTIGIANALI", Prezzo = 15f });



        menu.Piatti.Add(new Piatto { Nome = "Acqua Valverde 75cl.", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BEVANDE", Prezzo = 2.50f });

        menu.Piatti.Add(new Piatto { Nome = "Bibite in bottiglia/lattina 33cl.", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BEVANDE", Prezzo = 3.50f });

        menu.Piatti.Add(new Piatto { Nome = "Coca Cola, Fanta in bottiglia di vetro 100cl.", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BEVANDE", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Coca Cola alla spina 25cl", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BEVANDE", Prezzo = 3f });

        menu.Piatti.Add(new Piatto { Nome = "Coca Cola alla spina 40cl", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BEVANDE", Prezzo = 4f });

        menu.Piatti.Add(new Piatto { Nome = "Coca Cola alla spina 100cl", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BEVANDE", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Vino della casa alla spina 1/4lt.", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BEVANDE", Prezzo = 2.50f });

        menu.Piatti.Add(new Piatto { Nome = "Vino della casa alla spina 1/2lt.", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BEVANDE", Prezzo = 3.50f });

        menu.Piatti.Add(new Piatto { Nome = "Vino della casa alla spina 1lt.", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BEVANDE", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Succhi di frutta ", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BEVANDE", Prezzo = 3.50f });



        menu.Piatti.Add(new Piatto { Nome = "Caffé", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BAR", Prezzo = 1f });

        menu.Piatti.Add(new Piatto { Nome = "Caffé decaffeinato", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BAR", Prezzo = 1.50f });

        menu.Piatti.Add(new Piatto { Nome = "Caffé d’orzo", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BAR", Prezzo = 1.50f });

        menu.Piatti.Add(new Piatto { Nome = "Caffé corretto", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BAR", Prezzo = 1.50f });

        menu.Piatti.Add(new Piatto { Nome = "Amari e liquori", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "BAR", Prezzo = 3f });



        menu.Piatti.Add(new Piatto { Nome = "Grappa Prime Uve bianca", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "DISTILLATI", Prezzo = 5f });

        menu.Piatti.Add(new Piatto { Nome = "Grappa 903 bianca", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "DISTILLATI", Prezzo = 4f });

        menu.Piatti.Add(new Piatto { Nome = "Grappa Storica nera", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "DISTILLATI", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Grappa 903 barrique", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "DISTILLATI", Prezzo = 5f });

        menu.Piatti.Add(new Piatto { Nome = "Grappa Prime Uve nera", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "DISTILLATI", Prezzo = 6f });

        menu.Piatti.Add(new Piatto { Nome = "Grappa Le 18 Lune di Marzadro", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "DISTILLATI", Prezzo = 5f });

        menu.Piatti.Add(new Piatto { Nome = "Grappa Amarone Barricata", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "DISTILLATI", Prezzo = 7f });



        menu.Piatti.Add(new Piatto { Nome = "Rum Zacapa 23 anni", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "DISTILLATI", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Rum Matusalem Gran Reserva 15 anni", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "DISTILLATI", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Rum Millionario 15 anni", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "DISTILLATI", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Rum Agricoli (Rum Nation)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "DISTILLATI", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Rum Demerara (Solera n°14)", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "DISTILLATI", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Rum Panama (Solera) 18 anni", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "DISTILLATI", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Rum Diplomatico", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "DISTILLATI", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Rum Trinidad", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "DISTILLATI", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Rum Reunion", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "DISTILLATI", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Rum Barbados", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "DISTILLATI", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Rum Jamaica", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "DISTILLATI", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Rum Brugal", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "DISTILLATI", Prezzo = 5f });



        menu.Piatti.Add(new Piatto { Nome = "Whisky Talisker 10 anni", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "DISTILLATI", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Whisky Oban 14 anni", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "DISTILLATI", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Whisky Lagavulin 16 anni", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "DISTILLATI", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Whisky Macallan 12", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "DISTILLATI", Prezzo = 7f });

        menu.Piatti.Add(new Piatto { Nome = "Whisky Jack Daniel’s", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "DISTILLATI", Prezzo = 5f });



        menu.Piatti.Add(new Piatto { Nome = "Cognac Remy Martin", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "DISTILLATI", Prezzo = 8f });

        menu.Piatti.Add(new Piatto { Nome = "Cognac Martel", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "DISTILLATI", Prezzo = 8f });

        menu.Piatti.Add(new Piatto { Nome = "Punta Banano liquore rigenerante per la bagnante 1/2lt.", Nome_EN = "", Nome_GE = "", Allergeni = ToStr(new int[] { }), Tipo = "DISTILLATI", Prezzo = 10f });





        menu.Tipologia.Add("ANTIPASTI DI MARE CALDI E FREDDI");

        menu.Tipologia.Add("LE CRUDITA’");

        menu.Tipologia.Add("ANTIPASTI CLASSICI");

        menu.Tipologia.Add("TAGLIERE ROMAGNOLO");

        menu.Tipologia.Add("PRIMI PIATTI DI MARE");

        menu.Tipologia.Add("PRIMI PIATTI CLASSICI");

        menu.Tipologia.Add("SECONDI PIATTI DI MARE");

        menu.Tipologia.Add("I TAGLIERI DI MARE");

        menu.Tipologia.Add("dal mercato di Cesenatico");

        menu.Tipologia.Add("LE NOSTRE TAGLIATE");

        menu.Tipologia.Add("I NOSTRI FILETTI");

        menu.Tipologia.Add("SECONDI CLASSICI");

        menu.Tipologia.Add("I TAGLIERI CLASSICI");

        menu.Tipologia.Add("CONTORNI");

        menu.Tipologia.Add("PIATTI UNICI");

        menu.Tipologia.Add("GLI ASSAGGI DI PIZZA (per 2 persone)");

        menu.Tipologia.Add("GLI ASSAGGI DI PIZZA (per 3 persone)");

        menu.Tipologia.Add("ASSAGGI DI MARE per 2 persone");

        menu.Tipologia.Add("PIZZE");

        menu.Tipologia.Add("I NOSTRI CALZONI");

        menu.Tipologia.Add("PIZZE SPECIALI");

        menu.Tipologia.Add("LE PIZZE BIANCHE");

        menu.Tipologia.Add("LE PIZZE D’AUTORE");

        menu.Tipologia.Add("I DOLCI FATTI DA NOI ");

        menu.Tipologia.Add("Bollicine 375 ml");

        menu.Tipologia.Add("Bianchi 375 ml");

        menu.Tipologia.Add("Rossi 375 ml");

        menu.Tipologia.Add("Champagne 750 ml");

        menu.Tipologia.Add("Franciacorta 750 ml");

        menu.Tipologia.Add("Vini Rosé 750 ml");

        menu.Tipologia.Add("Trento D.O.C. 750 ml");

        menu.Tipologia.Add("Valdobbiadene 750ml");

        menu.Tipologia.Add("Bollicine 750ml");

        menu.Tipologia.Add("I Grandi Bianchi 750ml");

        menu.Tipologia.Add("Vini Bianchi 750ml");

        menu.Tipologia.Add("I Grandi Rossi 750ml");

        menu.Tipologia.Add("Vini Rossi 750ml");

        menu.Tipologia.Add("Vini da Dessert 750ml");

        menu.Tipologia.Add("LE MAGNUM 1,5L");

        menu.Tipologia.Add("BIRRE ALLA SPINA");

        menu.Tipologia.Add("BIRRE IN BOTTIGLIA");

        menu.Tipologia.Add("BIRRE ARTIGIANALI");

        menu.Tipologia.Add("BEVANDE");

        menu.Tipologia.Add("BAR");

        menu.Tipologia.Add("DISTILLATI");

        // Allergeni

        menu.Allergene["1"]="Glutine";
        menu.Allergene["2"]="Crostacei";
        menu.Allergene["3"]="Uova";
        menu.Allergene["4"]="Pesce";
        menu.Allergene["5"]="Arachidi";
        menu.Allergene["6"]="Soia";
        menu.Allergene["7"]="Latte";
        menu.Allergene["8"]="Frutta a Guscio";
        menu.Allergene["9"]="Sedano";
        menu.Allergene["10"]="Senape";
        menu.Allergene["11"]="Sesamo";
        menu.Allergene["12"]="Anidride Solforosa";
        menu.Allergene["13"]="Lupini";
        menu.Allergene["14"]="Molluschi";
        menu.Allergene["15"]="Aglio";
        
        string json = JsonConvert.SerializeObject(menu);
        File.WriteAllText(Path.Combine(Application.dataPath, "Menu4.json"), json);
        

        stream.Position = 0;
*/
#endregion
        
        if (db == 0)
        {
            var json1 = new WebClient().DownloadString(url);
            Menu menu1 = JsonConvert.DeserializeObject<Menu>(json1);
            Menu_Ristorante.menujson = menu1;
            return new Menu_Ristorante(menu1);
        }
        else
        {
            var json1 = url;
            Menu menu1 = JsonConvert.DeserializeObject<Menu>(json1);
            Menu_Ristorante.menujson = menu1;
            return new Menu_Ristorante(menu1);
        }
        
        
    }
}

public class Menu
    {

        public List<Piatto> Piatti { get; set; } = new List<Piatto>();
        public IEnumerable<Piatto> GetAllDishes()
        {
            return Piatti;
        }
    public List<Piatto> GetPiattiByTipo(string tipo)
        {
            return (from p in Piatti where p.Tipo == tipo select p).ToList();
        }

        public List<string> Tipologia { get; set; } = new List<string>();
        public Dictionary<string,string> Allergene { get; set; } = new Dictionary<string, string>();
}

    public class Piatto
    {
    public int idart;

    public string Nome;
    public string Nome_EN;
    public string Nome_RU;
    public string Nome_ES;
    public string Nome_FR;
    public string Nome_GE;
    public string GUID;
    public string Ingredienti;
    public List<string> Allergeni = new List<string>();
    public string UrlFoto;
    public string base64Img;
    public string[] EsclIngredienti;
    public string Note;
    public float Prezzo;
    public string Tipo;
    public bool Esaurito;
    public string ProdType;
    [JsonIgnore]
        public bool ConFoto
        {
            get
            {
                if (UrlFoto != null)
                    return UrlFoto.Length > 0;
                return false;
            }
        }
        [JsonIgnore]
    public string nomeTradotto
    {
        get
        {
            int id = LangEngine.Lan_ID;
            switch (id)
            {
                case 1:
                    if (Nome_EN.Length > 0)
                        Nome = Nome_EN;
                    return Nome;
                    break;
                case 2:
                    return Nome;
                    break;
                case 3:
                    if (Nome_RU.Length > 0)
                        Nome = Nome_RU;
                    return Nome;
                    break;
                case 4:
                    if (Nome_ES.Length > 0)
                        Nome = Nome_ES;
                    return Nome;
                    break;
                case 5:
                    if (Nome_GE.Length > 0)
                        Nome = Nome_GE;
                    return Nome;
                    break;
                case 6:
                    if (Nome_FR.Length > 0)
                        Nome = Nome_FR;
                    return Nome;
                    break;
                default:
                    return Nome;
                    break;
            }
        }

    }
    }

