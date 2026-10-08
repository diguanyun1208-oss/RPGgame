using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public Player_Combat combat;

    private void OnEnable()
    {
        SkillSlot.OnAbilityPointSpent += HandleAbilityPointSpent;
    }
    private void OnDisable()
    {
        SkillSlot.OnAbilityPointSpent -= HandleAbilityPointSpent;
    }


    private void HandleAbilityPointSpent(SkillSlot slot)  //技能点处理方法
    {
        if (slot == null || slot.skillSO == null)
            return;

        string skillName = slot.skillSO.skillName;

        switch(skillName)
        {
            case "最大生命值提升":
                if (StatsManger.Instance != null)
                    StatsManger.Instance.UpdateMaxHealth(1);
                break;

            case "挥砍":
                if (combat != null)
                    combat.enabled = true;
                break;

            // 以下技能的专属效果尚未实现，先保留占位，避免误报“未知技能”
            case "毒气":
            case "火焰魔法":
            case "冰霜魔法":
            case "闪电魔法":
            case "衰弱":
                break;

            default:
                Debug.LogWarning("未知技能：" + skillName);
                break;
        }
    }
}