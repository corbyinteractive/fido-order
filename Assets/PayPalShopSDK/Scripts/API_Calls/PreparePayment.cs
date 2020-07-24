using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparePayment : MonoBehaviour {

	public string accessToken, transactionDescription, itemName, itemDescription, itemPrice, itemCurrency;
	public string JSONrequest;
	[HideInInspector]
	public PreparePaymentResponse API_Success;
	[HideInInspector]
	public ErrorResponse API_Error;

	private void Start ()
	{
		StartCoroutine(APICall());
	}

	private void Success(string response)
	{
		API_Success = JsonUtility.FromJson<PreparePaymentResponse>(response);
	}

	private void Error(string response, string error)
	{
		if (error != "")
			API_Error = JsonUtility.FromJson<ErrorResponse>(response);
		if (API_Error == null)
		{
			API_Error = new ErrorResponse();
			API_Error.message = error;
		}
	}

	private IEnumerator APICall()
	{
		Dictionary<string, string> APIheaders = new Dictionary<string, string>();
		APIheaders.Add("Content-Type", "application/json");
		APIheaders.Add("Authorization", "Bearer " + accessToken);
		byte[] formData = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(CreateRequest()));
		string url = ShopSettings.inst.IsLive() ? "https://api.paypal.com/v1/payments/payment" : "https://api.sandbox.paypal.com/v1/payments/payment";
		WWW www = new WWW(url, formData, APIheaders);
		yield return www;
		if (www.error == null || www.error == "")
		{
			Success(www.text);
		}
		else
		{
			if (www.error.Contains("Unable to complete SSL connection"))
			{
				StartCoroutine(WorkAround());
			}
			else
			{
				InfoWindow.inst.OrderError();
				Debug.Log("Error connecting to " + url + " Error: " + www.error + " Details: " + www.text);
				Error(www.text, www.error);
			}
		}    
	}

	private PreparePaymentRequest CreateRequest()
	{
		PreparePaymentRequest request = JsonUtility.FromJson<PreparePaymentRequest>(JSONrequest);
        request.transactions[0].invoice_number = System.Guid.NewGuid().ToString();
        /*request.transactions[0].amount.total = itemPrice;
		request.transactions[0].amount.currency = itemCurrency;
		request.transactions[0].description = transactionDescription;
		request.transactions[0].invoice_number = System.Guid.NewGuid().ToString();
		request.transactions[0].item_list.items[0].name = itemName;
		request.transactions[0].item_list.items[0].description = itemDescription;
		request.transactions[0].item_list.items[0].price = itemPrice;
		request.transactions[0].item_list.items[0].currency = itemCurrency;

        // request.transactions[0].amount.currency = itemCurrency;
        // request.transactions[0].description = transactionDescription;
        // request.transactions[0].invoice_number = System.Guid.NewGuid().ToString();
        */

        return request;
	}
	//A workaround for a problem in some Unity versions, where Unity gives an error saying the api.paypal.com SSL certificate is invalid for some reason. 
	//What happens in this function is basically the same as the APICall function, but the data is send to my website instead and from there to api.paypal.com and back.
	//If you have your own website and you want to implement this workaround there, you can find the PHP code in Scripts > PHP.
	private IEnumerator WorkAround()
	{
		WWWForm form = new WWWForm();
		form.AddField("authorization", accessToken);
		form.AddField("Json", JsonUtility.ToJson(CreateRequest()));
		WWW www = new WWW("https://api.toolwareassets.com/v1/paypal/preparepayment" + (ShopSettings.inst.IsLive() ? "?mode=live" : "?mode=sandbox"), form);
		yield return www;
		if (!www.text.Contains("error"))
		{
			Success(www.text);
		}
		else
		{
			InfoWindow.inst.OrderError();
			Debug.Log("Connection Error: " + www.text);
			Error(www.text, www.text);
		}
	}
}
