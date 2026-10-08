using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public float speed; //速度
    public float attackRange = 2; //敌人攻击范围
    public float attackCooldown = 2; //攻击冷却时间
    public float playerDetectRange = 5; //玩家检测范围
    public Transform detectionPoint; //检测点
    public LayerMask playerLayer; //玩家层级

    private float attackCooldownTimer; //攻击冷却计时器 负责倒计时
    private int facingDirection = -1; //初始化敌人面朝方向 左为-1 右为1
    private  EnemyState enemyState; //敌人的状态

    private Rigidbody2D rb;
    private Transform player; //玩家位置
    private Animator anim; //私有的动画控制器引用

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //让rb获取刚体组件
        anim = GetComponent<Animator>();

        facingDirection = transform.localScale.x < 0 ? -1 : 1;
        ChangeState(EnemyState.Idle); //改变状态
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyState != EnemyState.Knockback)
        {
            CheckForPlayer();
            if (attackCooldownTimer > 0)
            {
                attackCooldownTimer -= Time.deltaTime; //冷却时间大于0  则计时器减去帧间隔时间来实现
            }

            if (enemyState == EnemyState.Chasing) //检查是否是追逐状态
            {
                Chase();
            }
            else if (enemyState == EnemyState.Attacking) //检查是否为攻击状态
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    void Chase()
    {
   
        if (player.position.x > transform.position.x && facingDirection == -1 ||
              player.position.x < transform.position.x && facingDirection == 1)
        {
            Flip();
        }
        Vector2 direction = (player.position - transform.position).normalized; //玩家位置-敌人位置 最后再归一化  防止在追逐时收到速度影响
        rb.linearVelocity = direction * speed;
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale=new Vector3(transform.localScale.x * -1,transform.localScale.y,transform.localScale.z);
    }

 
    private void CheckForPlayer()  //检查玩家
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, playerDetectRange, playerLayer);
        
        if(hits.Length > 0) //在第一时间看到玩家
        {
            player = hits[0].transform;

            if (Vector2.Distance(transform.position, player.position) <= attackRange && attackCooldownTimer <= 0)  //如果敌人和玩家之间的距离小于它的攻击范围 则开始攻击
            {
                //如果玩家处于攻击范围内并且冷却时间已准备好
                attackCooldownTimer = attackCooldown; //重置冷却时间
                ChangeState(EnemyState.Attacking);
            }

            else if(Vector2.Distance(transform.position, player.position) > attackRange && enemyState != EnemyState.Attacking) //敌人和玩家之间的距离大于它的攻击范围 则开始追逐
            {
                ChangeState(EnemyState.Chasing);
            }
        }
        else //假设敌人一开始看不到玩家
        {
            rb.linearVelocity = Vector2.zero;
            ChangeState(EnemyState.Idle);
        }
    }


    public void ChangeState(EnemyState newState)
    {
        //退出当前动画
        if (enemyState == EnemyState.Idle)
            anim.SetBool("isIdle", false);
        else if(enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing",false);
        else if (enemyState == EnemyState.Attacking)
            anim.SetBool("isAttacking", false);

        //更新当前的状态
        enemyState = newState;

        //更新新的动画
        if (enemyState == EnemyState.Idle)
            anim.SetBool("isIdle", true);
        else if (enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing", true);
        else if (enemyState == EnemyState.Attacking)
            anim.SetBool("isAttacking", true);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectionPoint.position,playerDetectRange);
    }
}

//创建一个公共的枚举
public enum EnemyState
{
    Idle,
    Chasing,
    Attacking,
    Knockback,
}
