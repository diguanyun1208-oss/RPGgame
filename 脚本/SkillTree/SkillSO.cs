using UnityEngine;


[CreateAssetMenu(fileName = "NewSkill",menuName = "技能树/Skill(技能)")]
public class SkillSO : ScriptableObject
{
    public string skillName;  //技能名称
    public int maxLevel; //最大技能等级
    public Sprite skillIcon; //技能图片

}
