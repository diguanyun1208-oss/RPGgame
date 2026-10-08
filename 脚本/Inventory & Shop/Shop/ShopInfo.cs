using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopInfo : MonoBehaviour
{
    public CanvasGroup infoPanel; //信息面板

    public TMP_Text itemNameText;
    public TMP_Text itemDescriptionText;

    [Header("属性描述")]
    public TMP_Text[] statTexts;

    //让面板能够移动并跟随鼠标
    private RectTransform infoPanelRect; //RectTransform 控制UI元素的位置、大小及其他属性


    private void Awake()
    {
        infoPanelRect= GetComponent<RectTransform>();//获取组件。搜索脚本所在的游戏中对象，查找位于<> 之间的任何组件
    }


    public void ShowItemInfo(ItemSO itemSO)  //显示信息面板
    {
        infoPanel.alpha = 1;

        itemNameText.text = itemSO.itemName;
        itemDescriptionText.text = itemSO.itemDescription;

        List<string> stats = new List<string>();
        if (itemSO.currentHealth > 0) 
            stats.Add("生命值: " + itemSO.currentHealth.ToString());

        if (itemSO.damage > 0) 
            stats.Add("攻击: " + itemSO.damage.ToString());

        if (itemSO.speed > 0) 
            stats.Add("速度: " + itemSO.speed.ToString());

        if (itemSO.duration > 0) 
            stats.Add("持续时间: " + itemSO.duration.ToString());

        if (stats.Count <= 0)
            return;

        for (int i = 0; i < statTexts.Length; i++)
        {
            if (i < stats.Count)
            {
                statTexts[i].text = stats[i];
                statTexts[i].gameObject.SetActive(true);
            }
            else
            {
                statTexts[i].gameObject.SetActive(false);
            }
        }
    }


    public void HideItemInfo()   //关闭信息面板
    {
        infoPanel.alpha = 0;

        itemNameText.text = "";
        itemDescriptionText.text = "";
    }


    public void FollowMouse()  //跟随鼠标移动
    {
        Vector3 mousePosition=Input.mousePosition;
        Vector3 offset = new Vector3(10, -10, 0);  //出现在哪个位置   偏移量

        infoPanelRect.position = mousePosition + offset;
    }

}
