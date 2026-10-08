using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    public int expReward = 3; //经验奖励
    //设置一个委托    怪物被击败  传递一个整数 经验值
    public delegate void MonsterDefeated(int exp);
    //设置一个 静态事件  意味着游戏中只有一个这样的实例
    public static event MonsterDefeated OnMonsterDefeated; //当怪物被击败时


    public int currentHealth;   //当前生命值
    public int maxHealth;   //最大生命值

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if(currentHealth <= 0)
        {
            OnMonsterDefeated(expReward);
            Destroy(gameObject);
        }
    }
}
