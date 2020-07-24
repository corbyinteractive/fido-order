using Cubequad;
using UnityEngine;
using UnityEngine.UI;
public class temp : MonoBehaviour
{
    public static temp C2;
    [Header("Menu Creation")]
    [SerializeField] private SwipyMenu swipyMenu;
    [SerializeField] private int amountOfMenusToCreate;
    [Header("My Headers Stuff")]
    [SerializeField] private float headerWidth = 150f;
    [SerializeField] private Font myFont;
    [SerializeField] private int myFontSize = 20;
    [Header("My Menus Stuff")]
    [SerializeField] public GameObject sampleButtonPhone;
    [SerializeField] public GameObject sampleButtonTablet;
    [SerializeField] private RectTransform myUIPrefab;


    private void Awake()
    {
        C2 = this;
    }
    public RectTransform InstantiateMenu (string headertext)
    {
        
            RectTransform headerRoot, menuRoot;
            RectTransform currentMenu= swipyMenu.AddMenu(out headerRoot, out menuRoot);



            ///set headers width
            swipyMenu.HeaderWidth = headerWidth;



            ///now new headers text is empty
            ///sadly but it seems to be a unity bug
            ///you can check it by yourself
            ///just create an empty RectTransform and call for it AddComponent<Text>():
            //var newTextComponent = testEmptyRectTransform.gameObject.AddComponent<Text>();
            ///and you can see that newTextComponent.font == null;
            ///or at least in my case..
            ///but any way I believe that default unity font won't satisfy you
            ///so I expected you to do something like this:
            var newHeaderText = headerRoot.GetComponentInChildren<Text>();
            newHeaderText.font = myFont != null ? myFont : Resources.GetBuiltinResource<Font>("Arial.ttf"); //if myFont not set in inspector than load default unity font
            newHeaderText.fontSize = myFontSize;
            newHeaderText.color = new Color32(0, 0, 0, 255);
            newHeaderText.text = headertext;



            ///instantiate your menu prefab
            Instantiate(myUIPrefab, menuRoot);
            return currentMenu;
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < amountOfMenusToCreate; i++)
            {
                RectTransform headerRoot, menuRoot;
                swipyMenu.AddMenu(out headerRoot, out menuRoot);



                ///set headers width
                swipyMenu.HeaderWidth = headerWidth;
                swipyMenu.HeadersHeight = 120f;



                ///now new headers text is empty
                ///sadly but it seems to be a unity bug
                ///you can check it by yourself
                ///just create an empty RectTransform and call for it AddComponent<Text>():
                //var newTextComponent = testEmptyRectTransform.gameObject.AddComponent<Text>();
                ///and you can see that newTextComponent.font == null;
                ///or at least in my case..
                ///but any way I believe that default unity font won't satisfy you
                ///so I expected you to do something like this:
                var newHeaderText = headerRoot.GetComponentInChildren<Text>();
                newHeaderText.font = myFont != null ? myFont : Resources.GetBuiltinResource<Font>("Arial.ttf"); //if myFont not set in inspector than load default unity font
                newHeaderText.fontSize = myFontSize;
                newHeaderText.color = new Color32(0, 0, 0, 255);
                newHeaderText.text = "Primi";



                ///instantiate your menu prefab
                Instantiate(myUIPrefab, menuRoot);
            }
        }
    }
}