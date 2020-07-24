using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class ShopSettings : MonoBehaviour
{

	public static ShopSettings inst;
	public string currencyCode;
	public enum PayPalMode
	{
		SANDBOX,
		LIVE
	}
	public PayPalMode payPalMode;
	
	private void Awake()
	{
		inst = this;
	}

	private void Start()
	{
		InvokeRepeating("GetAuthentication", 1f, 1f);
		InfoWindow.inst.LoadShop();
		InfoWindow.inst.ShowWindow();
	}

	private void GetAuthentication()
	{
		bool valid = GetComponent<GetAuthorization>().API_Success.access_token != "";
		bool invalid = GetComponent<GetAuthorization>().API_Error.message != "";
		if (valid)
		{
			ShopActions.inst.OpenShop();
			InfoWindow.inst.HideWindow();
			CancelInvoke("GetAuthentication");
		}
		else if (invalid)
		{
			CancelInvoke("GetAuthentication");
			InfoWindow.inst.LoadingError();
		}
	}

	public bool IsLive()
	{
		return payPalMode == PayPalMode.LIVE;
	}
}
