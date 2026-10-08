using System.Collections;
using UnityEngine;

public class NPC_Wander : MonoBehaviour
{
    [Header("漫游区域")]
    public float wanderWidth = 5;
    public float wanderHeight = 5;
    public Vector2 startingPosition; //起始位置  我们漫游区域的起始中心点。

    public float pauseDuration = 1;  //暂停时长

    public float speed = 2;
    public Vector2 target; //目标位置

    private Rigidbody2D rb; //在设置视频中添加的刚体，用于控制物理动作如移动
    private bool isPaused;  //判断是否暂停

    private Animator anim;

    //唤醒该方法在系统进入“唤醒”状态时立即运行(因此甚至早于启动方法)。
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    private void OnEnable()   //此方法每次脚本开启(或启用)时都会运行
    {
        StartCoroutine(PauseAndPickNewDestination());
    }

    private void Update()
    {
        if(isPaused)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        //计算范围是否小于0.1 是的话就重新获取随机位置
        if (Vector2.Distance(transform.position, target) < .1f)
            StartCoroutine(PauseAndPickNewDestination());

        Move();
       
    }


    private void Move()
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;

        //direction.x<0表示他的目标在左边 在向左走  transform.localScale.x > 0 表示NPC在面向右侧  反之向右走但是面向左边
        if (direction.x < 0 && transform.localScale.x > 0 || direction.x > 0 && transform.localScale.x < 0)
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

        rb.linearVelocity = direction * speed;
    }


    IEnumerator PauseAndPickNewDestination()   //暂停并选择新的目的地
    {
        isPaused = true;
        anim.Play("Idle");
        yield return new WaitForSeconds(pauseDuration);

        target = GetRandomTarget();
        isPaused = false;
        anim.Play("Walk");
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!enabled) return;
        StartCoroutine(PauseAndPickNewDestination());
    }


    private Vector2 GetRandomTarget()   //获取随机目标
    {
        float halfWidth = wanderWidth / 2;
        float halfHeight=wanderHeight / 2;
        int edge = Random.Range(0, 4); //edge=Random.Range  每当我们选择一个点时，它将随机选择矩形的一个

        return edge switch
        {
            0 => new Vector2(startingPosition.x - halfWidth, Random.Range(startingPosition.y - halfHeight, startingPosition.y + halfHeight)),  //矩形左边
            1 => new Vector2(startingPosition.x + halfWidth, Random.Range(startingPosition.y - halfHeight, startingPosition.y + halfHeight)),  //右边
            2 => new Vector2(Random.Range(startingPosition.x - halfWidth, startingPosition.x + halfWidth), startingPosition.y - halfHeight),   //底部
            _ => new Vector2(Random.Range(startingPosition.x - halfWidth, startingPosition.x + halfWidth), startingPosition.y + halfHeight),   //顶端
        };
    }


    //绘制漫游区域
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(startingPosition,new Vector3(wanderWidth,wanderHeight,0));
    }
}
