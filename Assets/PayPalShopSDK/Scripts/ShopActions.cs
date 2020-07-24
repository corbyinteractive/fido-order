using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;


public class ShopActions : MonoBehaviour
{
#if UNITY_WEBGL
	[DllImport("__Internal")]

    private static extern void openWindow(string url);
#endif
    public static ShopActions inst;
    public GameObject ShopWindow, LoadWindow;
    public Text StoreTitleField;
    public static string Orderstring;
	[HideInInspector]
	public string itemName;
	public enum CurrentStatus
	{
		NO_ITEM_SELECTED,
		STARTING_ORDER,
		READY,
		WAITING,
		COMPLETE
	};
	public CurrentStatus currentStatus = CurrentStatus.NO_ITEM_SELECTED;

	private void Awake()
	{
		inst = this;
	}

	public void OpenShop()
	{
        ShopWindow.SetActive(true);
        //SetStatus(CurrentStatus.NO_ITEM_SELECTED);
	}

	public void CloseShop()
	{
		gameObject.SetActive(false);
		ExitShop.Exit();
	}

	public void OpenPayPal()
	{
		string url = ShopSettings.inst.GetComponent<PreparePayment>().API_Success.links[1].href;
		if (Application.platform == RuntimePlatform.WebGLPlayer)
		{
#if UNITY_WEBGL
            openWindow(url);
#endif
		}
		else
		{
			Application.OpenURL(url);
		}
		OrderChecker newPaymentListener = ShopSettings.inst.gameObject.AddComponent<OrderChecker>();
		PreparePayment createPaymentAPI_Call = ShopSettings.inst.GetComponent<PreparePayment>();
		PreparePaymentResponse createPaymentResponse = createPaymentAPI_Call.API_Success;
		newPaymentListener.accessToken = createPaymentAPI_Call.accessToken;
		newPaymentListener.checkInterval = 10f;
		newPaymentListener.payID = createPaymentResponse.id;
		InvokeRepeating("CheckOrderStatus", 1f, 1f);
		SetStatus(CurrentStatus.WAITING);
	}

	public void OnOrderCancel()
	{
		if (currentStatus == CurrentStatus.WAITING)
		{
			LoadWindow.SetActive(false);
			InfoWindow.inst.CancelOrder();
            Comanda.C5.PaypalCancel();
		}
	}

	public void ResetCheckoutScreen()
	{
		Destroy(ShopSettings.inst.gameObject.GetComponent<PreparePayment>());
		Destroy(ShopSettings.inst.gameObject.GetComponent<GetPayment>());
		Destroy(ShopSettings.inst.gameObject.GetComponent<ExecutePayment>());
		Destroy(ShopSettings.inst.gameObject.GetComponent<OrderChecker>());
		SetStatus(CurrentStatus.NO_ITEM_SELECTED);
	}

	public void StartOrder()
    {
        //this.itemName = itemName;
        ShopWindow.SetActive(false);
        LoadWindow.SetActive(true);
		SetStatus(CurrentStatus.STARTING_ORDER);
		PreparePayment existingCreatePaymentAPIcall = ShopSettings.inst.gameObject.GetComponent<PreparePayment>();
		if (existingCreatePaymentAPIcall != null)
		{
			Destroy(existingCreatePaymentAPIcall);
		}
        /*
		PreparePayment createPaymentAPICall = ShopSettings.inst.gameObject.AddComponent<PreparePayment>();
		createPaymentAPICall.accessToken = ShopSettings.inst.GetComponent<GetAuthorization>().API_Success.access_token;
		createPaymentAPICall.transactionDescription = "Food Purchase";
		createPaymentAPICall.itemCurrency = ShopSettings.inst.currencyCode;
		createPaymentAPICall.itemDescription = itemToPurchase.itemDesc;
		createPaymentAPICall.itemName = itemToPurchase.itemName;
		createPaymentAPICall.itemPrice = itemToPurchase.itemPrice.ToString();
		createPaymentAPICall.JSONrequest = Resources.Load("JSON/PaymentRequest") as TextAsset;
        */
        PreparePayment createPaymentAPICall = ShopSettings.inst.gameObject.AddComponent<PreparePayment>();
        createPaymentAPICall.accessToken = ShopSettings.inst.GetComponent<GetAuthorization>().API_Success.access_token;
        /*createPaymentAPICall.transactionDescription = "Consegna a Domicilio";
        createPaymentAPICall.itemCurrency = ShopSettings.inst.currencyCode;
        createPaymentAPICall.itemDescription = "orecchiette alla capuana";
        createPaymentAPICall.itemName = "pasta";
        createPaymentAPICall.itemPrice = itemToPurchase.itemPrice.ToString();*/
        createPaymentAPICall.JSONrequest = Orderstring;
        //createPaymentAPICall.JSONrequest = Application.persistentDataPath("PaymentRequest") as TextAsset;

        //var sr = new StreamReader(Application.dataPath + "/" + "PaymentRequest") as TextAsset;
        InvokeRepeating("CheckOrderStatus", 1f, 1f);
	}
    
    public void EnableShopWindow()
	{
		if (currentStatus == CurrentStatus.WAITING)
		{
			InfoWindow.inst.CancelOrder();
			InfoWindow.inst.ShowWindow();
			return;
		}
        ShopWindow.SetActive(true);
    }

	private void CheckOrderStatus()
	{
		switch (currentStatus)
		{
			case CurrentStatus.NO_ITEM_SELECTED:
				break;
			case CurrentStatus.STARTING_ORDER:
				if (ShopSettings.inst.gameObject.GetComponent<PreparePayment>() != null && ShopSettings.inst.gameObject.GetComponent<PreparePayment>().API_Success != null)
				{
					SetStatus(CurrentStatus.READY);
					OpenPayPal();
				}
				break;
			case CurrentStatus.READY:
				break;
			case CurrentStatus.WAITING:
				if (ShopSettings.inst.gameObject.GetComponent<OrderChecker>().checkStatus == OrderChecker.CheckState.SUCCESS)
				{
					SetStatus(CurrentStatus.COMPLETE);
				}
				break;
			case CurrentStatus.COMPLETE:
				break;
		}
	}

	public void SetStatus(CurrentStatus newStatus)
	{
		switch (newStatus)
		{
			case CurrentStatus.NO_ITEM_SELECTED:
				//CancelInvoke("CheckOrderStatus");
				LoadWindow.SetActive(false);
				break;
			case CurrentStatus.STARTING_ORDER:
				LoadWindow.SetActive(true);
				break;
			case CurrentStatus.READY:
				LoadWindow.SetActive(false);
				CancelInvoke("CheckOrderStatus");
				break;
			case CurrentStatus.WAITING:
				InfoWindow.inst.info.SetActive(true);
				LoadWindow.SetActive(true);
				break;
			case CurrentStatus.COMPLETE:
				LoadWindow.SetActive(false);
				CancelInvoke("CheckOrderStatus");
				InfoWindow.inst.OrderSuccess();
				InfoWindow.inst.ShowWindow();
				break;
		}
		currentStatus = newStatus;
	}
}