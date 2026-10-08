using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "物品栏/物品")]
public class ItemSO : ScriptableObject
{
    public string itemName; //物品名称
    [TextArea]public string itemDescription; //物品描述
    public Sprite icon;  //物品精灵照片

    public bool isGold;  //判断是否为稀有道具   我们的库存会以不同于其他物品的方式处理黄金，因此这 将通知<库存> 该物品是黄金还是普通物品。
    public bool isEXP;

    public int stackSize = 3; //使我们能够独立地为每个项目定制堆叠大小

    [Header("属性状态")]
    public int currentHealth; //当前生命值
    public int maxHealth; //最大生命值
    public int speed; //速度
    public int damage; //伤害


    [Header("临时物品")]
    public float duration; //持续时间

  
}
