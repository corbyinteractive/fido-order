using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SampleButton : MonoBehaviour
{

    public Button buttonComponent;
    public Text nameLabel;
    public string iconImage;
    public Text priceText;


    private Piatto item;
    private ShopScrollList scrollList;

    // Use this for initialization
    void Start()
    {
        buttonComponent.onClick.AddListener(HandleClick);
    }

    public void Setup(Piatto currentItem, ShopScrollList currentScrollList)
    {
        item = currentItem;
        nameLabel.text = item.Nome;
        iconImage = item.UrlFoto;
        priceText.text = ("€ "+item.Prezzo.ToString());
        scrollList = currentScrollList;

    }

    public void HandleClick()
    {
        scrollList.TryTransferItemToOtherShop(item);
    }
}