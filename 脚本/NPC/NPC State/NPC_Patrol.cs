using System.Collections;
using UnityEngine;

public class NPC_Patrol : MonoBehaviour
{
    public Vector2[] patrolPoints;  //巡逻点数组
    public float speed = 2;  //速度

    public float pauseDuration = 1.5f;  //暂停时长
    private bool isPaused; //判断是否暂停

    private int currentPatorlIndex; //当前巡逻点索引

    private Vector2 target; //目标点

    private Rigidbody2D rb;
    private Animator anim;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        anim=GetComponentInChildren<Animator>();  //GetComponentlnChildren 会遍历层级结构(从第一个子对象开始，依次检查下一个，最终到达孙子组件)
        StartCoroutine(SetPatorlPoint());
    }

    // Update is called once per frame
    void Update()
    {
        if(isPaused)
        {
            rb.linearVelocity = Vector2.zero;
            return;
            //返回 在NPC暂停期间，阻止此方法执行该点以下的任何代码。
        }

        //局部变量当我们只需要在特定方法内使用变量时，可以在该方法中声明它，而不是在脚本开头声明。
        Vector2 direction = ((Vector3)target - transform.position).normalized;   //归一化 使模长为一

        //direction.x<0表示他的目标在左边 在向左走  transform.localScale.x > 0 表示NPC在面向右侧  反之向右走但是面向左边
        if (direction.x < 0 && transform.localScale.x > 0 || direction.x > 0 && transform.localScale.x < 0)
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

        rb.linearVelocity = direction * speed;

        if (Vector2.Distance(transform.position, target) < .1f)  // Distance 是一个Unity辅助方法，用于计算两个对象之间的距离。
            StartCoroutine(SetPatorlPoint());
    }

    IEnumerator SetPatorlPoint()  //设置巡逻点    //协程  这些可以暂停和重新启动，而不会阻塞脚本的主线程。
    {
        isPaused = true;
        anim.Play("Idle");
        yield return new WaitForSeconds(pauseDuration);

        //%模运算用于遍历数组。 将索引除以数组长度并返回余数。 如果余数为1，则会回溯到数组中的第1个元素，依此类推。
        currentPatorlIndex = (currentPatorlIndex + 1) % patrolPoints.Length;  //当到达列表末尾是，会重新开始
        target = patrolPoints[currentPatorlIndex];
        isPaused = false;
        anim.Play("Walk");

    }

}
