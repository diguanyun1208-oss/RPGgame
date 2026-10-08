using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestRewardSlot : MonoBehaviour
{
    public Image rewardImage;
    public TMP_Text rewardQuantity;

    //显示任务奖励
    public void DisplayReward(Sprite sprite,int quantity)
    {
        rewardImage.sprite = sprite;
        rewardQuantity.text = quantity.ToString();
    }
}
