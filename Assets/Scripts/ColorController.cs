using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ColorController : MonoBehaviour
{
    public bool AbilitaCambioColori;
    public Color emptyColor = new Color(0, 0, 0, 0);
    public static Color32 ColoreSfondi;// = new Color(1f,1f,1f,1f);
    public static Color32 ColoreTasti;
    public static Color32 ColoreRiquadri;// = new Color(0.8f,0.8f,0.8f,1f);
    public Image[] Sfondi = new Image[0];
    public Image[] Tasti = new Image[0];
    public Image[] riquadri = new Image[0];
    public static ColorController CController;
    // Start is called before the first frame update
    void Start()
    {
        CController = this;
    }
    public void CambiaColori()
    {
        
            foreach (Image img in Sfondi)
            {
                if (ColoreSfondi != emptyColor)
                img.color = ColoreSfondi;
            }
            foreach (Image img in Tasti)
            {
                if (ColoreTasti != emptyColor)
                img.color = ColoreTasti;
            }
            foreach (Image img in riquadri)
            {
                if (ColoreRiquadri != emptyColor)
                img.color = ColoreRiquadri;
            }
        

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
