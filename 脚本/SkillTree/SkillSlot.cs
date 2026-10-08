using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class SkillSlot : MonoBehaviour
{
    public List<SkillSlot> prerequisiteSkillSlots;  //创建一个列表   前置技能槽位

    public SkillSO skillSO;

    public int currentLevel; //技能当前等级
    public bool isUnlocked; //判断是否已解锁

    public Button skillButton;  //公共的技能按钮组件
    public Image skillIcon; //更加方便添加技能图片
    public TMP_Text skillLevelText; //技能等级文字


    //观察者模式以在事件发生时收到通知。 我们将创建事件，其他脚本可以(或不可以)订阅
    public static event Action<SkillSlot> OnAbilityPointSpent;  //关于技能点数支配
    public static event Action<SkillSlot> OnSkillMaxed;  //技能已满级


    private void OnValidate()     //当你对脚本的变量进行更改时，此方法会运行。
    {
        if(skillSO != null && skillLevelText != null && skillIcon != null)
        {
            UpdateUI();
        }
    }

    public void TryUpgradeSkill()  //特定技能升级
    {
        if(isUnlocked && currentLevel < skillSO.maxLevel)
        {
            currentLevel++;
            OnAbilityPointSpent?.Invoke(this); //空值检查  这里的问号只是为了确保事件确实存在(从而避免不必要的错误消息)

            if(currentLevel >= skillSO.maxLevel)
            {
                OnSkillMaxed?.Invoke(this);
            }

            UpdateUI();
        }
    }

    public bool CanUnlockSkill()  //可解锁技能
    {
        foreach(SkillSlot slot in  prerequisiteSkillSlots)
        {
            if(!slot.isUnlocked || slot.currentLevel < slot.skillSO.maxLevel)
            {
                return false;
            }
        }
        return true;
    }


    public void Unlock()
    {
        isUnlocked = true;
        UpdateUI();
    }


    private void UpdateUI()
    {
        if (skillSO != null && skillIcon != null)
            skillIcon.sprite = skillSO.skillIcon;

        if(isUnlocked)     //如果解锁 则是 当前等级/最大等级  并且颜色为白色
        {
            if (skillButton != null) 
                skillButton.interactable = true;

            if (skillLevelText != null && skillSO != null) 
                skillLevelText.text = currentLevel.ToString() + "/" + skillSO.maxLevel.ToString();

            if (skillIcon != null) 
                skillIcon.color = Color.white;
        }
        else  //否则则是未解锁    技能颜色为灰色
        {
            if (skillButton != null) 
                skillButton.interactable = false;

            if (skillLevelText != null) 
                skillLevelText.text = "未解锁";

            if (skillIcon != null) 
                skillIcon.color = Color.grey;
        }
    }
}
