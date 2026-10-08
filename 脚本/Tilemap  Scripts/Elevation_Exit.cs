using UnityEngine;

public class Elevation_Exit : MonoBehaviour
{
    public Collider2D[] mountainColliders; //山脉碰撞体
    public Collider2D[] boundaryColliders; //边界碰撞体

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") //判断是否是标签为玩家的 进入   //顺利从山上下来
        {
            foreach (Collider2D mountain in mountainColliders)
            {
                mountain.enabled = true; 
            }

            foreach (Collider2D boundary in boundaryColliders)
            {
                boundary.enabled = false;
            }

            collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 5; //将其精灵渲染层级回到原来的层级
        }
    }
}
