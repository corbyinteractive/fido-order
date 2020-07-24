using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ProductDescription : MonoBehaviour
{
    public static ProductDescription C4;
    public GameObject Container;
    public GameObject Immagine;
    public GameObject containerIngredienti;
    public GameObject RicercaFoto;
    public GameObject ContainerAllergeni;
    public Comanda.Ordine piattoCorrente;
    [HideInInspector]
    public int LoadStatus = 0;
    [HideInInspector]
    public string ImageBytes;
    public Text nome;
    public Text prezzo;
    public Text allergeni;
    public Text ingredienti;
    
    public Image cellImage;
    Cubequad.c_Data LogoData;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        C4 = this;
    }
    
    public void HideMe()
    {
        containerIngredienti.gameObject.SetActive(false);
        Immagine.gameObject.SetActive(false);
        Container.gameObject.SetActive(false);
    }
    public void aggiungiAComanda()
    {
        Container.SetActive(false);
        
        
        Comanda.C5.AddPiatto(piattoCorrente.Nome, piattoCorrente.EsclusioneIngredienti, piattoCorrente.Prezzo, piattoCorrente.Quantità,piattoCorrente.IDart,piattoCorrente.Guid);
        
    }
    public void CreateCanvas(string Nome, string Ingredienti, string Allergeni, string UrlFoto, string[] EsclIngredienti, float Prezzo, bool ConFoto, int Idart, string note, string Guid )
    {
        ingredienti.text = null;
        Immagine.gameObject.SetActive(false);
        cellImage.sprite = null;
        piattoCorrente = new Comanda.Ordine();
        piattoCorrente.Nome = Nome;
        piattoCorrente.Prezzo = Prezzo;
        piattoCorrente.Quantità = 1;
        piattoCorrente.EsclusioneIngredienti = EsclIngredienti;
        piattoCorrente.IDart = Idart;
        piattoCorrente.Guid = Guid;
        Container.SetActive(true);
        nome.text = Nome;
        prezzo.text = Prezzo + " €";
        if (ContentCreator.Db == 1)
        {
            if (UrlFoto != null)
            {
                if (UrlFoto.Length >= 50)
                {
                    byte[] b64_bytes = System.Convert.FromBase64String(UrlFoto);

                    var tex = new Texture2D(1, 1);
                    tex.LoadImage(b64_bytes);
                    if (tex != null)
                    {
                        var rec = new Rect(0, 0, tex.width, tex.height);
                        cellImage.sprite = Sprite.Create(tex, rec, new Vector2(0, 1), 100);
                        //cellImage.sprite = Sprite.Create(texture, new Rect(0, 0, data.imageDimensions.x, data.imageDimensions.y), new Vector2(0, 0));
                        cellImage.preserveAspect = true;

                    }
                }
                else
                {
                    RicercaFoto.gameObject.SetActive(true);
                  CallWebService.MMS.getImage("AAA", ContentCreator.Restaurant, Guid); 
                    
                }
            }
            else
            {
                Immagine.gameObject.SetActive(false);
            }
        }
        if (ConFoto)
        {
            Immagine.gameObject.SetActive(true);
            if (ContentCreator.Db == 0)
            {
                LogoData = new Cubequad.c_Data()
                {
                    imageUrl = UrlFoto,
                    imageDimensions = new Vector2(643f, 227f)
                };
                try
                {
                    SetData(LogoData);
                }
                catch
                {
                    Immagine.gameObject.SetActive(false);
                }
            }
           

        }
        else
        {
            Immagine.gameObject.SetActive(false);
        }
        if (Ingredienti != null && Ingredienti.Length > 4)
        {
            containerIngredienti.gameObject.SetActive(true);
            ingredienti.text = Ingredienti;
        }
        if (Allergeni != null && Allergeni.Length > 3)
        {
            ContainerAllergeni.SetActive(true);
            allergeni.text = Allergeni;
        }
        else
        {
            ContainerAllergeni.SetActive(false);
            allergeni.text = "No";
        }
        if (note != null && note.Length > 3)
        {
            ingredienti.text = note + '\n' + Ingredienti;
        }
    }
    #region IMAGE_PROCESSING
    private Coroutine _loadImageCoroutine;

    public void SetData(Cubequad.c_Data data)
    {
        _loadImageCoroutine = StartCoroutine(LoadRemoteImage(data));
    }

    public IEnumerator LoadRemoteImage(Cubequad.c_Data data)
    {
        string path = data.imageUrl;

        Texture2D texture = null;

        // Get the remote texture

#if UNITY_2017_4_OR_NEWER
        var webRequest = UnityWebRequestTexture.GetTexture(path);
        yield return webRequest.SendWebRequest();
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
            cellImage.sprite = Sprite.Create(texture, rec, new Vector2(0, 1), 100);
            //cellImage.sprite = Sprite.Create(texture, new Rect(0, 0, data.imageDimensions.x, data.imageDimensions.y), new Vector2(0, 0));
            cellImage.preserveAspect = true;
        }
        else
        {
            ClearImage();
        }
    }

    public void ClearImage()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (LoadStatus == 1)
        {
            
                byte[] b64_bytes = System.Convert.FromBase64String(ImageBytes);
                var tex = new Texture2D(1, 1);
                tex.LoadImage(b64_bytes);
                if (tex != null)
                {
                    var rec = new Rect(0, 0, tex.width, tex.height);
                    cellImage.sprite = Sprite.Create(tex, rec, new Vector2(0, 1), 100);
                    //cellImage.sprite = Sprite.Create(texture, new Rect(0, 0, data.imageDimensions.x, data.imageDimensions.y), new Vector2(0, 0));
                    cellImage.preserveAspect = true;
                    Immagine.gameObject.SetActive(true);
                RicercaFoto.gameObject.SetActive(false);
                CallWebService.MMS.stopGetImage();
                LoadStatus = 0;
            }
                else
                {
                    Immagine.gameObject.SetActive(false);
                LoadStatus = 0;
                RicercaFoto.gameObject.SetActive(false);
                CallWebService.MMS.stopGetImage();
            }
            
        }
        else if (LoadStatus == 2)
        {
            Immagine.gameObject.SetActive(false);
            LoadStatus = 0;
            RicercaFoto.gameObject.SetActive(false);
            CallWebService.MMS.stopGetImage();
        }
    }
    #endregion
}


