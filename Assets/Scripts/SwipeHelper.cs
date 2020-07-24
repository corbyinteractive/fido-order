using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class SwipeHelper : MonoBehaviour
{
    private InputField myInput;
    // Use this for initialization
    void Start()
    {
        myInput = GetComponent<InputField>();
        myInput.enabled = false;
    }
    public void Activate()
    {
        myInput.gameObject.SetActive(false);
    }
    public void Deactivate()
    {
        myInput.DeactivateInputField();
    }
    // Update is called once per frame
    void Update()
    {

    }
}