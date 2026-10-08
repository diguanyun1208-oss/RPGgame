using System;
using TMPro;
using UnityEngine;

public class InventoryManger : MonoBehaviour
{
    public static InventoryManger Instance;
    public InventorySlot[] itemSlots; //使这个成为可容纳我们所有槽位的下拉菜单

    public UseItem useItem;

    public int gold; //金币数量
    public TMP_Text goldText; //金币数量文本

    public GameObject lootPrefab; //创建一个空的游戏对象 用于存放 战利品预制体
    public Transform player;  //玩家位置

    public static event Action<int> OnExperienceGained;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        foreach(var slot in itemSlots)
        {
            slot.UpdateUI();
        }

        goldText.text = gold.ToString();
    }


    //订阅中 现在，每当一个战利品对象调用此事件时，该脚本将开始“监听
    private void OnEnable()
    {
        Loot.OnItemLooted += AddItem;
    }

    private void OnDisable()
    {
        Loot.OnItemLooted -= AddItem;
    }


    public void AddItem(ItemSO itemSO,int quantity)
    {
        if(itemSO.isGold)
        {
            gold += quantity;
            goldText.text=gold.ToString();  //ToString() 将“gold”整数转换为字符串，以便TMP可以读取它数量
            return;  //终止该方法，因此我们不会浪费资源运行不必要的代码。
        }

        if(itemSO.isEXP)
        {
            //获得经验值
            OnExperienceGained?.Invoke(quantity);
            return;
        }
       
        //处理物品堆叠
        foreach(var slot in itemSlots) //它是同一物品，而且还有剩余空间
        {
            if(slot.itemSO == itemSO && slot.quantity < itemSO.stackSize)  //如果这是相同的物品并且还有空间
            {
                int availableSpace = itemSO.stackSize - slot.quantity;  //(插槽能容纳多少)-(槽中已有的数量)
                int amountToAdd = Mathf.Min(availableSpace, quantity);

                slot.quantity += amountToAdd;
                quantity -= amountToAdd;

                slot.UpdateUI();

                if (quantity <= 0)
                    return;
            }
        }

        //如果还有物品，我们现在将查看空槽位。
        foreach (var slot in itemSlots)
        {
            if (slot.itemSO == null)
            {
                int amountToAdd = Mathf.Min(itemSO.stackSize, quantity);
                slot.itemSO = itemSO;  //用新物品的数据填充槽位的项目和数量变量
                slot.quantity = quantity;
                slot.UpdateUI();
                return;
            }
        }
  
        if(quantity > 0)
            DropLoot(itemSO, quantity);
        
    }

    //移除物品
    public void RemoveItem(ItemSO itemSO,int quantity)
    {
        for(int i = 0; i < itemSlots.Length; i++)
        {
            var slot = itemSlots[i];

            //跳过与物品不匹配的槽位
            if (slot.itemSO != itemSO)
                continue;

            if(slot.quantity > quantity)
            {
                //仅移除我们需要的内容
                slot.quantity -= quantity;
                slot.UpdateUI();
                quantity = 0;
            }

            else
            {
                //从这个槽位中取走全部内容
                quantity -= slot.quantity;
                slot.itemSO = null;
                slot.quantity = 0;
                slot.UpdateUI();
            }

        }
    }



    //丢弃物品  用于鼠标右键
    public void DropItem(InventorySlot slot)
    {
        DropLoot(slot.itemSO, 1);
        slot.quantity--;
        if(slot.quantity <= 0)
        {
            slot.itemSO = null;
        }
        slot.UpdateUI();
    }



    //丢弃战利品
    private void DropLoot(ItemSO itemSO,int quantity)
    {
        //现在我们可以使用这个参考来与我们正在掉落的物品上的战利品脚本进行交互。
        Loot loot=Instantiate(lootPrefab,player.position,Quaternion.identity).GetComponent<Loot>(); //identity  设置无旋转
        loot.Initialize(itemSO, quantity);
    }



    public void UseItem(InventorySlot slot)
    {
        if(slot.itemSO != null && slot.quantity >= 0)
        {
            useItem.ApplyItemEffects(slot.itemSO);

            slot.quantity--;
            if(slot.quantity <= 0)
            {
                slot.itemSO = null;
            }
            slot.UpdateUI();

        }
    }


    //拥有物品
    public bool HasItem(ItemSO itemSO)
    {
        foreach(var slot in itemSlots)
        {
            if(slot.itemSO == itemSO && slot.quantity > 0)
                return true;
        }
        return false;
    }

    internal int GetItemQuantity(ItemSO itemSO)
    {
        int total = 0;

        foreach(var slot in itemSlots)
        {
            if (slot.itemSO == itemSO)
                total += slot.quantity;
        }
        return total;
    }
}
