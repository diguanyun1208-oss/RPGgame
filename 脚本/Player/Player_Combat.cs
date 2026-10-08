using UnityEngine;

public class Player_Combat : MonoBehaviour
{
    public Transform attackPoint; //攻击位置
    public LayerMask enemyLayer; //敌人遮罩


    public Animator anim; //创建公共的动画

    public float cooldown = 2; //冷却时间
    private float timer; //冷却计时器

    private void Update()
    {
        if(timer > 0)        //这个检查使我们的代码更高效。如果计时器已经耗尽，就不需要再运行它了
        {
            timer -= Time.deltaTime;    //冷却时间大于0  则计时器减去帧间隔时间来实现
        }
    }

    public void Attack()
    {
        if (timer <= 0)
        {
            anim.SetBool("isAttacking", true);
            timer = cooldown;
        }
    }

    public void DealDamage()
    {
       
        //创建了一个列表  包含了范围内的所有敌人
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, StatsManger.Instance.weaponRange, enemyLayer);
        if (enemies.Length > 0)
        {
            enemies[0].GetComponent<Enemy_Health>().ChangeHealth(-StatsManger.Instance.damage);
            enemies[0].GetComponent<Enemy_Knockback>().Knockback(transform, StatsManger.Instance.knockbackForce, StatsManger.Instance.knockbackTime, StatsManger.Instance.stunTime);
        }
    }

    public void FinishAttacking()  //完成攻击后
    {
        anim.SetBool("isAttacking", false);
    }

    private void OnDrawGizmosSelected()
    {
        
        Gizmos.color = Color.red;

        float circleRange = 1f;  //先给一个默认的半径值
        if (StatsManger.Instance != null)  //只有当单例管理器存在时，才去获取真实的武器范围
        {
            circleRange = StatsManger.Instance.weaponRange;
        }
        //绘制圆圈
        Gizmos.DrawWireSphere(attackPoint.position, circleRange);
    }

}
