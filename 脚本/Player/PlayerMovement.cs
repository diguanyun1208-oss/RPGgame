using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //创建公共变量
    public int facingDirection = 1; //初始化面朝方向  1为右   -1为左

    public Rigidbody2D rb; //创建刚体
    public Animator anim; //动画控制器的引用

    private bool isKnockedBack;  //是否击退
    public bool isShooting; //看是否在射击  如果在射击就停下不移动

    public Player_Combat player_Combat; //引用公共的玩家攻击

    private void Update()
    {
        if(Input.GetButtonDown("攻击") && player_Combat.enabled==true)  //看是否按下的是攻击键
        {
            player_Combat.Attack();
        }
    }

    // Fixed  Update is called once per frame
    void FixedUpdate()
    {
        if(isShooting == true)
        {
            rb.linearVelocity = Vector2.zero;
        }

        else if (isKnockedBack == false)
        {
            //存储水平和垂直方向的值
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            if (horizontal > 0 && transform.localScale.x < 0 ||
                horizontal < 0 && transform.localScale.x > 0)
            {
                Flip(); //如果按右键，但是玩家面向左边 就翻转  反之则相同
            }

            anim.SetFloat("horizontal", Mathf.Abs(horizontal));
            anim.SetFloat("vertical", Mathf.Abs(vertical));

            //设置刚体的速度
            rb.linearVelocity = new Vector2(horizontal, vertical) * StatsManger.Instance.speed;
        }
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);  
    }

    public void Knockback(Transform enemy,float force,float stunTime) //击退效果
    {
        isKnockedBack = true; //禁用玩家移动
        Vector2 direction = (transform.position- enemy.position).normalized; //归一化，防止产生奇怪的击退效果
        rb.linearVelocity = direction * force;
        StartCoroutine(KnockbackCounter(stunTime));
    }

    //创建协程
    IEnumerator KnockbackCounter(float stunTime)    //击晕时间   
    {
        yield return new WaitForSeconds(stunTime);
        rb.linearVelocity=Vector2.zero;
        isKnockedBack = false;
    }
}
