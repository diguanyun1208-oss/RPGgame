using UnityEngine;

public class Player_Arrow : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 direction = Vector2.right;  //箭发射的方向
    public float lifeSpan = 2; //箭的寿命   2秒
    public float speed;

    public LayerMask enemyLayer; //敌人层级遮罩
    public LayerMask obstacleLayer; //障碍层允许我们标记任何我们希望箭头插入的图层。

    public SpriteRenderer sr; //精灵渲染器:允许我们更改箭头外观的组件(以便其能埋入物体中)
    public Sprite buriedSprite; //精灵图 当箭矢扎入物体时将出现的新精灵
   

    public int damage; //弓箭伤害

    public float knockbackForce; //击退力度
    public float knockbackTime; //击退时间
    public float stunTime; //击晕时间


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.linearVelocity = direction * speed;
        RotateArrow();
        Destroy(gameObject, lifeSpan);
    }

    //箭头旋转
    private void RotateArrow()
    {
        // Atan2返回一个值(以弧度为单位)，可用于计算两点之间的角度      Rad2Deg将弧度计算转换为角度(我们实际上可以使用)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //位运算符将碰撞对象的图层转换为二进制数，然后与敌方LayerMask进行比较，以判断是否存在重叠。  1<<是左移一位生成对应的二进制值
        if ((enemyLayer.value & (1<<collision.gameObject.layer)) > 0)
        {
            collision.gameObject.GetComponent<Enemy_Health>().ChangeHealth(-damage);
            collision.gameObject.GetComponent<Enemy_Knockback>().Knockback(transform, knockbackForce, knockbackTime, stunTime);
            AttachToTarget(collision.gameObject.transform);
        }
        else if ((obstacleLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            AttachToTarget(collision.gameObject.transform);
        }
    }

    //附着到目标
    private void AttachToTarget(Transform target)
    {
        //当箭击中某物时 第一反应时更改精灵图
        sr.sprite = buriedSprite;  //新精灵图=扎入的精灵图

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic; //锁定运动，不受物理力影响

        transform.SetParent(target);
    }

}
