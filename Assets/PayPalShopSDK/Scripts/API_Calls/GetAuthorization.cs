using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAuthorization : MonoBehaviour {


    public string clientID;// = ContentCreator.Key;
    public string secret;// = ContentCreator.secret;
	[HideInInspector]
	public GetTokenResponse API_Success;
	[HideInInspector]
	public ErrorResponse API_Error;

	private void Start ()
	{
        //StartCoroutine(APICall());
        clientID = ContentCreator.Key;
        secret = ContentCreator.secret;
        StartCoroutine(APICall());
    }
    public void FalseStart()
    {
        if (clientID == "" || secret == "")
            Debug.LogError("Please enter your clientID and secret. Read the documentation for more info");
        StartCoroutine(APICall());
    }

	private void Success(string response)
	{
		API_Success = JsonUtility.FromJson<GetTokenResponse>(response);
	}

	private void Error(string response, string error)
	{
		if(error != "")
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
		APIheaders.Add("Accept","application/json");
		APIheaders.Add("Accept-Language","it_IT");
		APIheaders.Add("Authorization","Basic " + System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(clientID + ":" + secret)));
		WWWForm form = new WWWForm();
		form.AddField("grant_type", "client_credentials");
		string url = ShopSettings.inst.IsLive () ? "https://api.paypal.com/v1/oauth2/token" : "https://api.sandbox.paypal.com/v1/oauth2/token";
		WWW www = new WWW(url, form.data, APIheaders);
		yield return www;
		if (www.error == null || www.error == "")
		{
			Success(www.text);
            ShopActions.inst.StartOrder();
		}
		else
		{
			if (www.error.Contains("Unable to complete SSL connection"))
			{
				StartCoroutine(WorkAround());
			}
			else
			{
				Debug.Log("Error connecting to " + url + " Error: " + www.error + " Details: " + www.text);
				Error(www.text, www.error);
			}
		}    
	}
	//A workaround for a problem in some Unity versions, where Unity gives an error saying the api.paypal.com SSL certificate is invalid for some reason. 
	//What happens in this function is basically the same as the APICall function, but the data is send to my website instead and from there to api.paypal.com and back.
	//If you have your own website and you want to implement this workaround there, you can find the PHP code in Scripts > PHP.
	private IEnumerator WorkAround()
	{
		WWWForm form = new WWWForm();
		form.AddField("authorization", System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(clientID + ":" + secret)));
		WWW www = new WWW("https://api.toolwareassets.com/v1/paypal/getauthorization" + (ShopSettings.inst.IsLive() ? "?mode=live" : "?mode=sandbox"), form);
		yield return www;
		if (!www.text.Contains("error"))
		{
			Success(www.text);
            ShopActions.inst.StartOrder();
        }
		else
		{
			Debug.Log("Connection Error: " + www.text);
			Error(www.text, www.text);
		}
	}
}
