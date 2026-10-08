using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestSO", menuName = "QuestSO")]
public class QuestSO : ScriptableObject
{
    public string questName;
    [TextArea] public string questDescription;   //任务描述   TextArea可以在检查器里面有书写空间
    public int questLevel;

    public List<QuestObjective> objectives;
    public List<QuestReward> rewards;
}


[System.Serializable]
public class QuestObjective     //任务目标
{
    public string description;

    [SerializeField] private Object target;

    public ItemSO targetItem => target as ItemSO;    //现在这是一个属性    如果目标是物品，那么其他脚本将通过与targetltem对话来看到它。
    public ActorSO targetNPC => target as ActorSO;
    public LocationSO targetLocation => target as LocationSO;

    public int requiredAmount; //所需目标物品数量
    
}


[System.Serializable]
public class QuestReward  //任务奖励
{
    public ItemSO itemSO;
    public int quantity;
}
