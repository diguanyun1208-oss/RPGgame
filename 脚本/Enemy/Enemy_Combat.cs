using UnityEngine;

public class Enemy_Combat : MonoBehaviour
{
    public int damage = 1; //创建一个公共变量

    public Transform attckPoint; //攻击点
    public float weaponRange;  //武器攻击范围
    public float knockbackForce;  //击退力
    public float stunTime;  //击晕时间
    public LayerMask playerLayer; //遮罩检查玩家是否在攻击范围内 


    public void Attack() //攻击方法
    {
        Collider2D[] hits=Physics2D.OverlapCircleAll(attckPoint.position,weaponRange,playerLayer);

        if(hits.Length > 0)
        {
            hits[0].GetComponent<PlayerHealth>().ChangeHealth(-damage);
            hits[0].GetComponent<PlayerMovement>().Knockback(transform,knockbackForce,stunTime);
        }
    }
}
