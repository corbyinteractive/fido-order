using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MessageFade : MonoBehaviour
{
    public int size = 0;
    public bool Triggered = false;
    public float minimum = 0.0f;
    public float maximum = 1f;
    public float pause = 1f;
    private bool fadeout = false;
    private bool started = false;
    public float duration = 2.0f;
    public bool fadestart = false;
    //public Text message;
    public Canvas LogoCanvas;
    private float startTime;
    private int touches;
    public Image[] Images = new Image[0];
    public GameObject Canvases;
    
    void Start()
    {
        fadestart = true;
        started = false;
    }
    public void changescene (string scenename)
    {
        SceneManager.LoadScene(scenename);
    }
    

    
    public void TriggerFade()
    {
        if (!started)
        {
            startTime = Time.time;
            float t2 = Time.time;
            started = true;
        }
    }
    void Update()
    {
        
        if ((fadestart == true) && (started == false) && !Triggered)
        {
            startTime = Time.time;
            float t2 = Time.time;
            started = true;
            
        }
        if (fadestart)
        {
            float t = (Time.time - startTime) / duration;
            if (fadeout == false)
            {
                foreach (Image img in Images)
                {
                    img.gameObject.SetActive(true);
                    img.color = new Color(1f, 1f, 1f, 1f);
                }
                
            }
            if (((Time.time - t) > (pause + duration)) & (fadeout == false))
            {
                fadeout = true;
                startTime = Time.time;
                t = (Time.time - startTime) / duration;
            }

            if (fadeout == true)
            {
                float Cintensity = Mathf.SmoothStep(maximum, minimum, t);
                //message.color = new Color(1f, 1f, 1f, Cintensity);
                foreach (Image img in Images)
                {
                    img.color = new Color(1f, 1f, 1f, Cintensity);
                }
                if (Cintensity == 0f)
                {
                    foreach (Image img in Images)
                    {
                        img.gameObject.SetActive(false);
                    }
                    Canvases.gameObject.SetActive(false);
                    /*foreach (Canvas cvs in Canvases)
                    {
                        cvs.gameObject.SetActive(false);
                    }*/
                   
                }
                
                
            }
            
        }
    }
}
