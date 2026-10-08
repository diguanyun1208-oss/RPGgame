using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<QuestSO, Dictionary<QuestObjective, int>> questProgress = new();
    private List<QuestSO> completedQuests = new(); //已完成任务列表

    //订阅事件
    private void OnEnable()
    {
        QuestEvents.IsQuestComplete += IsQuestComplete;
    }
    private void OnDisable()
    {
        QuestEvents.IsQuestComplete -= IsQuestComplete;
    }

    #region 任务接受区域
    public bool IsQuestAccepted(QuestSO questSO)
    {
        return questProgress.ContainsKey(questSO);
    }


    public List<QuestSO> GetActiveQuests()
    {
        return new List<QuestSO>(questProgress.Keys); //键就是任务脚本化对象
    }


    public void AcceptQuest(QuestSO questSO)
    {
        questProgress[questSO] = new Dictionary<QuestObjective, int>(); //进度字典

        foreach(var objective in questSO.objectives)
        {
            UpdateObjectiveProgress(questSO, objective);
        }
    }
    #endregion

    #region 任务完成逻辑区域
    //判断任务是否已完成
    public bool IsQuestComplete(QuestSO questSO)
    {
        if(!questProgress.TryGetValue(questSO,out var progressDict))   //尝试获取这个任务，如果这个任务存在，则返回一个进度字典
            return false;

        foreach(var objective in questSO.objectives)
        {
            UpdateObjectiveProgress(questSO, objective);
        }

        foreach(var objective in questSO.objectives)
        {
            if (progressDict[objective] < objective.requiredAmount)
                return false;
        }

        return true;

    }

    //任务完成
    public void CompleteQuest(QuestSO questSO)
    {
        questProgress.Remove(questSO);
        completedQuests.Add(questSO);

        foreach(var objective in questSO.objectives)
        {
            if(objective.targetItem != null && objective.requiredAmount > 0)
            {
                InventoryManger.Instance.RemoveItem(objective.targetItem, objective.requiredAmount);
            }
        }

        //待定：放发奖励功能
        foreach(var reward in questSO.rewards)
        {
            InventoryManger.Instance.AddItem(reward.itemSO, reward.quantity);
        }
    }


   public bool GetCompleteQuest(QuestSO questSO)   //判断接受的任务是否已完成
    {
        return completedQuests.Contains(questSO);
    }

    #endregion

    public void UpdateObjectiveProgress(QuestSO questSO,QuestObjective objective)  //更新目标进度
    {
        if (!questProgress.ContainsKey(questSO))
            return;

        var progressDictionary = questProgress[questSO];
        int newAmount = 0;

        if(objective.targetItem != null)
            newAmount=InventoryManger.Instance.GetItemQuantity(objective.targetItem);

        else if(objective.targetLocation != null && GameManager.Instance.LocationHistoryTracker.HasVisited(objective.targetLocation))
            newAmount=objective.requiredAmount;

        else if(objective.targetNPC != null && GameManager.Instance.DIalogueHistoryTracker.HasSpokenWith(objective.targetNPC))
            newAmount=objective.requiredAmount;

        progressDictionary[objective] = newAmount;
    }


    public string  GetProgressText(QuestSO questSO,QuestObjective objective)
    {
        int currentAmount = GetCurrentAmount(questSO, objective);  //待办： 弄清楚我们目前有多少资源

        if (currentAmount >= objective.requiredAmount)
            return "完成";

        else if (objective.targetItem != null)
            return $"{currentAmount}/{objective.requiredAmount}";

        else
            return "进行中";
    }

    public int GetCurrentAmount(QuestSO questSO, QuestObjective objective)
    {
        //查看这个超级字典里有没有关于 特定任务的值 ，如果有的话 它会吐出那个迷你字典
        if (questProgress.TryGetValue(questSO, out var objectiveDictionary))
            if (objectiveDictionary.TryGetValue(objective, out int amount))    //检查那个迷你字典里有没有我们要找的目标，如果有的话 它就会传出进度值
                return amount;

        return 0;
    }



}
