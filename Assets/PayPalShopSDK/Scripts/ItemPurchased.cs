using UnityEngine;

public class ItemPurchased : MonoBehaviour
{
	public static void AddItem(string itemName)
	{
        Comanda.C5.InviaOrdinePagato();
		/* 
		 * Add your own code here that will run when an item is purchased. For example:
		 * if(itemName == "A few Gold bars")
		 * {
		 *		PlayerPrefs.SetInt("GoldBars", PlayerPrefs.GetInt("GoldBars") + 100);
		 *		//Note: Ofcourse this isn't safe, when a player knows how to edit PlayerPrefs he can add as many GoldBars as he wants. 
		 *		//If you want to safe it locally, you can check out SecuredPrefs & Passwords :)
		 *		https://assetstore.unity.com/packages/tools/utilities/securedprefs-passwords-145404
		 * }	
		*/
		Debug.Log("The following item is succesfully purchased: \"" + itemName + "\"");
	}
}
