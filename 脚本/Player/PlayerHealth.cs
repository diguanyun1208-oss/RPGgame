using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{

    public TMP_Text healthText;  //文本血量
    public Animator healthTextAnim; //公开的动画引用  文本血量动画

    private void Start()
    {
        healthText.text = "HP: " + StatsManger.Instance.currentHealth + " / " + StatsManger.Instance.maxHealth;  //改变我的文本血量
    }

    //添加一个数值  是伤害也是治疗
    public void ChangeHealth(int amount)
    {
        StatsManger.Instance.currentHealth += amount;
        healthTextAnim.Play("TextUpdate"); //只想让动画器播放这个动画

        healthText.text = "HP: " + StatsManger.Instance.currentHealth + " / " + StatsManger.Instance.maxHealth;

        if (StatsManger.Instance.currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }

}
