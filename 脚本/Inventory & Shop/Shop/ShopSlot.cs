using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerMoveHandler       //接口 是一种行为的蓝图。它定义了方法，但让我们可以在其中编写实际逻辑。
{
    public ItemSO itemSO;
    public TMP_Text itemNameText;
    public TMP_Text priceText;
    public Image itemImage;

    [SerializeField] private ShopManager shopManager;
    [SerializeField] private ShopInfo shopInfo;

    public int price; //价格   让我们让这个脚本保存价格以备将来参考


    //每个店主在游戏开始时初始化他们所有的槽位  传入一个商品 一个代表商品的价格
    public void Initialized(ItemSO newItemSO, int price)
    {
        //用信息填充槽位
        itemSO = newItemSO;
        itemImage.sprite = itemSO.icon;
        itemNameText.text = itemSO.itemName;
        this.price = price;
        priceText.text = price.ToString();

    }


    //购买按钮点击
    public void OnBuyButtonClicked()
    {
        shopManager.TryBuyItem(itemSO, price);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemSO != null)
            shopInfo.ShowItemInfo(itemSO);
       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        shopInfo.HideItemInfo();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (itemSO != null)
            shopInfo.FollowMouse();
    }

}
