using UnityEngine;
using System.Collections.Generic;
using System;

public class ShopManager : MonoBehaviour
{
    

    //序列化字段序列化意味着我们可以在Inspector(Unity中)看到它 但......它仍然是私有属性，让私有和保护的也可以被显示编辑  所谓序列化就是把一个对象保存到一个文件或数据库字段中去
    //[SerializeField] private List<ShopItems> shopItems; //列表 是动态的，你可以在游戏运行时添加项目

    [SerializeField] private ShopSlot[] shopSlots; //数组 你无法在运行时更改其中的项目数量，但数组来说使用起来非常方便。

    [SerializeField] private InventoryManger inventoryManager;


    //填充商店物品
    public void PopulateShopItems(List<ShopItems> shopItems)
    {
        for(int i=0; i < shopItems.Count && i < shopSlots.Length;i++)
        {
            ShopItems shopItem=shopItems[i];
            shopSlots[i].Initialized(shopItem.itemSO, shopItem.price);
            shopSlots[i].gameObject.SetActive(true);
        }

        for(int i=shopItems.Count; i < shopSlots.Length;i++)
        {
            shopSlots[i].gameObject.SetActive(false);
        }
    }


    //尝试购买物品 
    public void TryBuyItem(ItemSO itemSO,int price)
    {
        if(itemSO != null && inventoryManager.gold >= price)   //防止在传递空值时出现边缘情况。
        {
            if(HasSpaceForItem(itemSO))
            {
                inventoryManager.gold -= price;
                inventoryManager.goldText.text=inventoryManager.gold.ToString();
                inventoryManager.AddItem(itemSO, 1);
            }
        }
    }

    //判断是否还有空间能容纳物品
    private bool HasSpaceForItem(ItemSO itemSO)
    {
        foreach(var slot in inventoryManager.itemSlots)
        {
            if (slot.itemSO == itemSO && slot.quantity < itemSO.stackSize)
                return true;
            else if (slot.itemSO == null)
                return true;
        }
        return false;
    }


    //出售商品
    public void SellItem(ItemSO itemSO)
    {
        if (itemSO == null)
            return;

        foreach(var slot in shopSlots)
        {
            if(slot.itemSO == itemSO)
            {
                inventoryManager.gold += slot.price;  //原价卖回给商店  此处修改卖回商店时的价钱
                inventoryManager.goldText.text = inventoryManager.gold.ToString();
                return;
            }
        }
    }

}

//System 使我们能够访问System命名空间 (与脚本顶部添加"using System"的作用相同)
[System.Serializable]   //可序列化  这意味着我们可以在检查器中看 到这个
public class ShopItems
{
    public ItemSO itemSO;
    public int price;
}