using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler       //接口 是一种行为的蓝图。它定义了方法，但让我们可以在其中编写实际逻辑。
{
    public ItemSO itemSO;
    public int quantity;

    public Image itemImage;   //这里存放的是当插槽被使用时将显示的图标
    public TMP_Text quantityText;

    private InventoryManger inventoryManager;

    //活跃商店
    private static ShopManager activeShop; //该变量将在所有库存槽位之间共享。一旦其中一个被更新，所有槽位都将获得信息。


    private void Start()
    {
        inventoryManager = GetComponentInParent<InventoryManger>();  //GetComponentInParent 会向上遍历层级结构，查找具有此脚本的对象。
    }

    private void OnEnable()
    {
        ShopKeeper.OnShopStateChanged += HandleShopStateChange;
    }
    private void OnDisable()
    {
        ShopKeeper.OnShopStateChanged -= HandleShopStateChange;
    }


    //处理商店状态变化
    private void HandleShopStateChange(ShopManager shopManager,bool isOpen)
    {
        activeShop = isOpen ? shopManager : null;
    }


    public void OnPointerClick(PointerEventData eventData)  //当我们在该对象上点击时，将被呼叫
    {
       if(quantity > 0)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                if (activeShop != null)
                {
                    activeShop.SellItem(itemSO);
                    quantity--;
                    UpdateUI();
                }
                else
                {
                    if (itemSO.currentHealth > 0 && StatsManger.Instance.currentHealth >= StatsManger.Instance.maxHealth)
                        return;

                    //判断如果物品数量大于0   就进去判断是否是鼠标左键点击   是的话就使用这个物品
                    inventoryManager.UseItem(this);
                }
            }

            else if(eventData.button == PointerEventData.InputButton.Right)
            {
                inventoryManager.DropItem(this);
            }
        }
    }

    public void UpdateUI()
    {
        if (quantity <= 0)
            itemSO = null;

        if(itemSO != null) //判断物品是否为空
        {
            itemImage.sprite = itemSO.icon;  //这将为槽位提供一个物品精灵(它从itemso中获取)
            itemImage.gameObject.SetActive(true);  //如果存在一个物品...请打开槽位的图像游戏对象。
            quantityText.text=quantity.ToString();
        }
        else
        {
            itemImage.gameObject.SetActive(false);
            quantityText.text = ""; //这将使当槽位为空时数量文本消失
        }
    }
}
