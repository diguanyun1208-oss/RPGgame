using UnityEngine;

public class Player_Bow : MonoBehaviour
{
    public Transform launchPoint; //发射点
    public GameObject arrowPrefab; //弓箭预设体

    public PlayerMovement playerMovement; //对玩家移动的公共引用

    private Vector2 aimDirection = Vector2.right;  //设置一个瞄准方向

    public float shootCooldown = .5f; //射击冷却时间
    private float shootTimer; //射击计时器

    public Animator anim; //动画控制器引用

    // Update is called once per frame
    void Update()
    {
        shootTimer -= Time.deltaTime;

        HandleAiming();

        if(Input.GetButtonDown("射击") && shootTimer <= 0)
        {
            playerMovement.isShooting = true;
            anim.SetBool("isShooting", true);
        }
       
    }

    private void OnEnable()
    {
        anim.SetLayerWeight(0, 0);
        anim.SetLayerWeight(1, 1);
    }
    private void OnDisable()
    {
        anim.SetLayerWeight(0, 1);
        anim.SetLayerWeight(1, 0);
    }


    //处理瞄准
    private void HandleAiming()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if(horizontal != 0 || vertical != 0)
        {
            aimDirection = new Vector2(horizontal, vertical).normalized;
            anim.SetFloat("aimX",aimDirection.x);
            anim.SetFloat("aimY",aimDirection.y);
        }
    }

    public void Shoot()
    {
        if (shootTimer <= 0)
        {
            //实例化预制体  出现在发射点，没有任何旋转
            Player_Arrow arrow = Instantiate(arrowPrefab, launchPoint.position, Quaternion.identity).GetComponent<Player_Arrow>();
            arrow.direction = aimDirection;
            shootTimer = shootCooldown;
        }
        anim.SetBool("isShooting", false);
        playerMovement.isShooting = false;
    }
}
