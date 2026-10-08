using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestLogUI : MonoBehaviour
{
    [SerializeField] private QuestManager questManager;

    [SerializeField] private TMP_Text questNameText;
    [SerializeField] private TMP_Text questDescriptionText;
    [SerializeField] private QuestObjectiveSlot[] objectiveSlots;
    [SerializeField] private QuestRewardSlot[] rewardSlots;

    private QuestSO questSO;

    [SerializeField] private QuestSO noAvailableQuestSO;  //无可用任务
    [SerializeField] private QuestLogSlot[] questSlots; //任务日志槽位数组

    [SerializeField] private CanvasGroup questCanvas;

    [SerializeField] private CanvasGroup acceptCanvasGroup;
    [SerializeField] private CanvasGroup declineCanvasGroup;
    [SerializeField] private CanvasGroup completeCanvasGroup;


    //订阅事件
    private void OnEnable()
    {
        QuestEvents.OnQuestOfferRequested += ShowQuestOffer;
        QuestEvents.OnQuestTurnInRequested += ShowQuestTurnIn;
    }

    private void OnDisable()
    {
        QuestEvents.OnQuestOfferRequested -= ShowQuestOffer;
        QuestEvents.OnQuestTurnInRequested -= ShowQuestTurnIn;
    }


    #region 显示任务方法
    //显示任务提供
    public void ShowQuestOffer(QuestSO incomingQuestSO)     //当我们的任务板提供任务时，它会与谁交谈
    {
        if(questManager.IsQuestAccepted(incomingQuestSO) || questManager.GetCompleteQuest(incomingQuestSO))
        {
            questSO = noAvailableQuestSO;
            SetCanvasState(acceptCanvasGroup, false);
            SetCanvasState(declineCanvasGroup, true);
            SetCanvasState(completeCanvasGroup, false);
        }
        else
        {
            questSO = incomingQuestSO;
            SetCanvasState(acceptCanvasGroup, true);
            SetCanvasState(declineCanvasGroup, true);
            SetCanvasState(completeCanvasGroup, false);
        }

        HandleQuestClicked(questSO);
        SetCanvasState(questCanvas, true);

    }

    public void ShowQuestTurnIn(QuestSO incomingQuestSO)  //显示任务提交
    {
        questSO = incomingQuestSO;

        HandleQuestClicked(questSO);

        SetCanvasState(completeCanvasGroup, true);
        SetCanvasState(acceptCanvasGroup, false);
        SetCanvasState(declineCanvasGroup, false);
        SetCanvasState(questCanvas, true);
        
    }

    #endregion

    #region 按钮点击方法
    //接受任务按钮
    public void OnAcceptQuestClicked()
    {
        QuestEvents.OnQuestAccepted?.Invoke(questSO);

        questManager.AcceptQuest(questSO);
        SetCanvasState(completeCanvasGroup, false);
        SetCanvasState(acceptCanvasGroup, false);
        SetCanvasState(declineCanvasGroup, false);

        RefreshQuestList();  //刷新任务列表
        HandleQuestClicked(noAvailableQuestSO);
    }

    //拒绝任务按钮
    public void OnDeclineQuestClicked()
    {
        SetCanvasState(questCanvas, false);
    }


    //任务完成点击事件
    public void OnCompleteQuestClicked()
    {
        questManager.CompleteQuest(questSO);
        RefreshQuestList();
        HandleQuestClicked(noAvailableQuestSO);
        SetCanvasState(completeCanvasGroup, false);
    }
    #endregion

    //设置画布状态
    private void SetCanvasState(CanvasGroup group, bool activate)
    {
        group.alpha = activate ? 1 : 0;
        group.blocksRaycasts = activate;
        group.interactable=activate;
    }


    //刷新任务列表
    public void RefreshQuestList()
    {
        List<QuestSO> activeQuests = questManager.GetActiveQuests();

        for(int i = 0; i < questSlots.Length; i++)
        {
            if(i < activeQuests.Count)
            {
                questSlots[i].SetQuest(activeQuests[i]);
            }
            else
            {
                questSlots[i].ClearSlot();
            }
        }
    }



    //处理任务点击
    public void HandleQuestClicked(QuestSO questSO)
    {
        this.questSO = questSO;

        questNameText.text=questSO.questName;
        questDescriptionText.text=questSO.questDescription;

        DisplayObjectives();
        DisplayRewards();
    }


    private void DisplayObjectives()   //显示目标
    {
        for(int i = 0; i < objectiveSlots.Length; i++)
        {
            if(i < questSO.objectives.Count)
            {
                var objective = questSO.objectives[i];

                questManager.UpdateObjectiveProgress(questSO, objective);   //更新实时进度值

                int currentAmount = questManager.GetCurrentAmount(questSO, objective);  //当前所获取的数量 
                string progress=questManager.GetProgressText(questSO, objective);
                bool isComplete=currentAmount >= objective.requiredAmount;

                objectiveSlots[i].gameObject.SetActive(true);
                objectiveSlots[i].RefreshObjectives(objective.description, progress, isComplete);

            }
            else
            {
                objectiveSlots[i].gameObject.SetActive(false);
            }
        }

    }


    private void DisplayRewards()    //显示奖励
    {
        for(int i = 0; i < rewardSlots.Length; i++)
        {
            if(i < questSO.rewards.Count)
            {
                var reward=questSO.rewards[i];
                rewardSlots[i].DisplayReward(reward.itemSO.icon, reward.quantity);
                rewardSlots[i].gameObject.SetActive(true);
            }
            else
            {
                rewardSlots[i].gameObject.SetActive(false);
            }
        }
    }



}
