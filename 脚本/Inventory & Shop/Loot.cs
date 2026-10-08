using System;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public ItemSO itemSO; //对物品可视化脚本的引用
    public SpriteRenderer sr; //精灵渲染器
    public Animator anim;

    public bool canBenPickedUp = true; //判断物品是否能捡起

    public int quantity; //数量

    //静态 该变量属于类(其他脚本无需了解此战利品即可查看)    
    //事件 一个其他脚本可以订阅的通知系统。
    //Action 让我们选择要传递的参数(此处为itemSo和<int>)
    public static event Action<ItemSO, int> OnItemLooted; 

    private void OnValidate()
    {
        if (itemSO == null)
            return;

       UpdateAppearance();

    }

    //初始化背包丢弃物品方法
    public void Initialize(ItemSO itemSO,int quantity)
    {
        this.itemSO = itemSO;
        this.quantity = quantity;
        canBenPickedUp=false;
        UpdateAppearance();
    }

    //更新外观
    private void UpdateAppearance()
    {
        sr.sprite = itemSO.icon;
        this.name = itemSO.itemName;  //this.name 更改层级中游戏对象的名称
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && canBenPickedUp == true)
        {
            anim.Play("LootPickup");
            OnItemLooted?.Invoke(itemSO, quantity);  //？问号只是确保有监听者存在  有的话则会调用这个特定事件
            Destroy(gameObject, .5f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            canBenPickedUp = true;
        }
    }

}
