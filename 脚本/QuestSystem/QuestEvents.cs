using UnityEngine;
using System;

public static class QuestEvents
{
    //Action  与只能由你放入其中的脚本调用的“事件动作”不同 动作可以被任何其他脚本调用
    //我们将在需要从任务板、NPC、物品地点等提供任务时调用此功能。
    public static Action<QuestSO> OnQuestOfferRequested;  //任务提供请求

    public static Action<QuestSO> OnQuestTurnInRequested; //任务提交请求

    public static Action<QuestSO> OnQuestAccepted; //任务已接受

    public static Func<QuestSO, bool> IsQuestComplete;   //任务是否完成
}
