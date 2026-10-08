using UnityEngine;
using TMPro;

public class SkillTreeManager : MonoBehaviour
{
    public SkillSlot[] skillSlots; //技能槽数组
    public TMP_Text pointsText; //技能点数
    public int availablePoints;  //可用技能点数


    //订阅事件
    private void OnEnable()
    {
        SkillSlot.OnAbilityPointSpent += HandleAbilityPointsSpent;
        SkillSlot.OnSkillMaxed += HandleSkillMaxed;
        ExpManager.OnLevelUp += UpdateAbilityPoints;
    }
    //取消订阅
    private void OnDisable()
    {
        SkillSlot.OnAbilityPointSpent -= HandleAbilityPointsSpent;
        SkillSlot.OnSkillMaxed -= HandleSkillMaxed;
        ExpManager.OnLevelUp -= UpdateAbilityPoints;
    }



    private void Start()
    {
        foreach(SkillSlot slot in skillSlots)
        {
            //实际上给按钮添加一个监听器    //事先检查一下是否真的有技能点
            slot.skillButton.onClick.AddListener(() => CheckAvailablePoints(slot));
        }
        UpdateAbilityPoints(0);
    }

    private void CheckAvailablePoints(SkillSlot slot)
    {
        if(availablePoints >0)
        {
            slot.TryUpgradeSkill();
        }
    }

    private void HandleAbilityPointsSpent(SkillSlot skillSlot)   //已使用能力点数
    {
        if(availablePoints > 0)
        {
            UpdateAbilityPoints(-1);
        }
    }

    private void HandleSkillMaxed(SkillSlot skillSlot)
    {
        foreach(SkillSlot slot in skillSlots)
        {
            if (!slot.isUnlocked && slot.CanUnlockSkill())
            {
                slot.Unlock();
            }
        }
    }

    public void UpdateAbilityPoints(int amount)  //升级后可用的点数
    {
        availablePoints += amount;
        pointsText.text = "技能点: " + availablePoints;
    }
}
