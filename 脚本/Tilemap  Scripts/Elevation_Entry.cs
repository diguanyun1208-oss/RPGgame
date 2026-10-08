using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    //目的是关闭山脉碰撞器  让其角色站在上面移动

    public Collider2D[] mountainColliders; //山脉碰撞体
    public Collider2D[] boundaryColliders; //边界碰撞体

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") //判断是否是标签为玩家的 进入
        {
            foreach(Collider2D mountain in mountainColliders)
            {
                mountain.enabled = false;  //进入触发器时 会关闭山地碰撞体
            }

            foreach(Collider2D boundary in boundaryColliders)
            {
                boundary.enabled = true;
            }

            collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 15; //将其精灵渲染层级提升为15
        }
    }

}
