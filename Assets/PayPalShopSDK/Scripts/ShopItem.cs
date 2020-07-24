using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
	public Sprite itemImage;
	public string itemName, itemDesc, itemPrice;

	private Image img;
	private Text nameText, priceText, descTesxt;
	
	private void Start()
	{
		img = transform.Find("ItemImage").GetComponent<Image>();
		nameText = transform.Find("ItemName").GetComponent<Text>();
		priceText = transform.Find("ItemCost").GetComponent<Text>();
		descTesxt = transform.Find("ItemDesc").GetComponent<Text>();
		if (itemImage == null)
		{
			itemImage = Resources.Load<Sprite>("ItemSprites/DefaultImage");
		}
		img.sprite = itemImage;
		if (itemName.Length > 100)
		{
			itemName = itemName.Substring(0, 99);
		}
		nameText.text = itemName;
		priceText.text = CurrencyHelper.GetCurrencySymbol(ShopSettings.inst.currencyCode) + itemPrice;
		descTesxt.text = itemDesc;
	}

	public void Buy()
	{
		ShopActions.inst.StartOrder();
	}
}
