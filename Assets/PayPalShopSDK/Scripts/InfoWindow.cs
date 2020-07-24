using UnityEngine;
using UnityEngine.UI;

public class InfoWindow : MonoBehaviour
{
	public static InfoWindow inst;
	public GameObject window;
    public GameObject ShopMain;
	public Text title, msg;
	public Button confirmButton;
	public GameObject loader, info;
	public enum InfoType
	{
		UNSPECIFIED,
		SHOP_LOADING,
		SHOP_LOADING_ERROR,
		ORDER_ERROR,
		ORDER_SUCCESS,
	};
	public InfoType infoType;

	private void Awake()
	{
		inst = this;
	}

	public void ShowWindow()
	{
		window.SetActive(true);
	}

	public void HideWindow()
	{
		window.SetActive(false);
		title.text = "";
		msg.text = "";
		infoType = InfoType.UNSPECIFIED;
	}

	public void OnConfirm()
	{
		if (infoType == InfoType.SHOP_LOADING_ERROR || infoType == InfoType.ORDER_ERROR)
		{
			HideWindow();
			ExitShop.Exit();
            ShopMain.SetActive(false);
            Comanda.C5.PaypalCancel();
        }
		else if(infoType == InfoType.ORDER_SUCCESS)
		{
			HideWindow();
			ItemPurchased.AddItem(ShopActions.inst.itemName);
			ShopActions.inst.ResetCheckoutScreen();
			ShopActions.inst.EnableShopWindow();
            ShopMain.SetActive(false);
            Comanda.C5.PaypalCancel();
        }
	}

	public void LoadShop()
	{
		infoType = InfoType.SHOP_LOADING;
		title.text = "";
		msg.text = "";
		info.SetActive(false);
		loader.SetActive(true);
		confirmButton.gameObject.SetActive(false);
	}

	public void LoadingError()
	{
		infoType = InfoType.SHOP_LOADING_ERROR;
		title.text = "Oops!";
		msg.text = "An error occured when loading the shop. Please try again later.";
		confirmButton.gameObject.SetActive(true);
		loader.SetActive(false);
	}

	public void OrderError()
	{
		ShopActions.inst.LoadWindow.SetActive(false);
		ShowWindow();
		infoType = InfoType.ORDER_ERROR;
		title.text = "Error";
		msg.text = "Something went wrong. Please try again Later.";
		confirmButton.gameObject.SetActive(true);
		loader.SetActive(false);
	}

	public void CancelOrder()
	{
		HideWindow();
		infoType = InfoType.UNSPECIFIED;
		info.SetActive(false);
		loader.SetActive(false);
		ShopActions.inst.ResetCheckoutScreen();
		ShopActions.inst.EnableShopWindow();
	}

	public void OrderSuccess()
	{
		infoType = InfoType.ORDER_SUCCESS;
		title.text = "Thank you";
		msg.text = "Your purchase was successful. Click the button below to return to the shop.";
		confirmButton.gameObject.SetActive(true);
		loader.SetActive(false);
		info.SetActive(false);
	}
}
