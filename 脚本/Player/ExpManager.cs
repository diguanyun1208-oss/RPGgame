using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ExpManager : MonoBehaviour
{
    public int level; //等级
    public int currentExp; //当前经验值
    public int expToLevel = 10; //升级所需经验值
    public float expGrowthMultiplier = 1.2f; //增长倍率   每次升级后的经验值都增长20%     
    public Slider expSlider; //经验值划块
    public TMP_Text currentLevelText;  //当前经验值文本

    //获取技能点数事件
    public static event Action<int> OnLevelUp;  //升级时获取点数

    private void Start()
    {
        UpdateUI();
    }

    
    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.Return))
    //    {
    //        GainExperience(2);
    //    }
      
    //}

    //订阅事件  监听
    private void OnEnable()
    {
        //正在监听敌人血量中的 “当怪物被击败时” 的事件      后面则是监听器 调用一个方法
        Enemy_Health.OnMonsterDefeated += GainExperience;
        InventoryManger.OnExperienceGained += GainExperience;
    }
    private void OnDisable()
    {
        //取消订阅事件
        Enemy_Health.OnMonsterDefeated -= GainExperience;
        InventoryManger.OnExperienceGained -= GainExperience;
    }

    public void GainExperience(int amount)    //获得经验叠加
    {
        currentExp += amount;
        if(currentExp >= expToLevel)
        {
            LevelUp();
        }

        UpdateUI();
    }

    private void LevelUp() //升级等级
    {
        level++;
        currentExp -= expToLevel;
        expToLevel = Mathf.RoundToInt(expToLevel * expGrowthMultiplier);    //所需经验值增加
        OnLevelUp?.Invoke(2);
    }

    public void UpdateUI()  //ui的更新
    {
        expSlider.maxValue = expToLevel;
        expSlider.value = currentExp;
        currentLevelText.text = "等级: " + level;
    }

}
