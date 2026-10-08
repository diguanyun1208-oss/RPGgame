using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeper : MonoBehaviour
{
    public static ShopKeeper currentShopKeeper;

    public Animator anim;
    public CanvasGroup shopCanvansGroup;
    public ShopManager shopManager;

    //序列化字段序列化意味着我们可以在Inspector(Unity中)看到它 但......它仍然是私有属性，让私有和保护的也可以被显示编辑  所谓序列化就是把一个对象保存到一个文件或数据库字段中去
    [SerializeField] private List<ShopItems> shopItems; //物品商店列表       //列表 是动态的，你可以在游戏运行时添加项目
    [SerializeField] private List<ShopItems> shopWeapons; //武器商店列表 
    [SerializeField] private List<ShopItems> shopArmour; //防具商店列表 

    [SerializeField] private Camera shopkeeperCam;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 0, -1); //设置偏移量

    //商店状态更新事件
    public static event Action<ShopManager, bool> OnShopStateChanged;

    private bool playerInRange;  //当玩家位于商店店主的圆形碰撞体范围内时，playerlnRange 将进行追踪。
    private bool isShopOpen;


    // Update is called once per frame
    void Update()
    {
        if(playerInRange)
        {
            if (Input.GetButtonDown("互动键"))
            {
                if (!isShopOpen)
                {
                    Time.timeScale = 0;
                    isShopOpen = true;
                    currentShopKeeper = this;
                    OnShopStateChanged?.Invoke(shopManager, true); //在发送消息前检查是否有听众
                    shopCanvansGroup.alpha = 1;
                    shopCanvansGroup.blocksRaycasts = true;
                    shopCanvansGroup.interactable = true;

                    shopkeeperCam.transform.position = transform.position + cameraOffset;
                    shopkeeperCam.gameObject.SetActive(true);

                    OpenItemShop();
                }

            }

            else if (Input.GetButtonDown("Cancel"))
            {
                Time.timeScale = 1;
                isShopOpen = false;
                currentShopKeeper = null;
                OnShopStateChanged?.Invoke(shopManager, false); //在发送消息前检查是否有听众
                shopCanvansGroup.alpha = 0;
                shopCanvansGroup.blocksRaycasts = false;
                shopCanvansGroup.interactable = false;

                shopkeeperCam.gameObject.SetActive(false);
            }
        }
    }


    public void OpenItemShop()   //打开物品商店
    {
        shopManager.PopulateShopItems(shopItems);
    }

    public void OpenWeaponShop()  //打开武器商店
    {
        shopManager.PopulateShopItems(shopWeapons);
    }

    public void OpenArmourShop()  //打开防具商店
    {
        shopManager.PopulateShopItems(shopArmour);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            anim.SetBool("playerInRange", true);
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("playerInRange", false);
            playerInRange = false;
        }
    }
}
