using UnityEngine;
using TMPro;

public class StatsManger : MonoBehaviour
{

    //单例模式一种提供对其他脚本轻松访问的编码模式，且仅有一个自身的“实例"

    public static StatsManger Instance; //静态的数值管理器实例
    public TMP_Text healthText; //生命值改变

    public StatsUI statsUI;

    [Header("战斗属性")]
    public int damage;   //伤害
    public float weaponRange; // 武器攻击范围
    public float knockbackForce; //击退力度
    public float knockbackTime; //击退时间
    public float stunTime;  //击晕时间

    [Header("移动属性")]
    public int speed;

    [Header("生命值")]
    public int maxHealth;   //最大生命值
    public int currentHealth; //当前生命值

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdateMaxHealth(int amount)  //每次升级后的最大生命值
    {
        maxHealth += amount;
        if (healthText != null)
            healthText.text = "HP: " + currentHealth + "/" + maxHealth;
    }

    public void UpdateHealth(int amount)  //每次升级后的生命值
    {
        currentHealth += amount;
        if(currentHealth >= maxHealth)  //如果我们的治疗后的生命值大于最大生命值   则当前生命值为最大生命值  防止出现生命值溢出现象
        {
            currentHealth = maxHealth;
        }
        if (healthText != null)
            healthText.text = "HP: " + currentHealth + "/" + maxHealth;
    }

    public void UpdateSpeed(int amount)  //每次升级后的速度
    {
        speed += amount;
        statsUI.UpdateAllStats();
    }
}
