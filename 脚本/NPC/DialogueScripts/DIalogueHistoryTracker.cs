using System.Collections.Generic;
using UnityEngine;

public class DIalogueHistoryTracker : MonoBehaviour
{
   
    public readonly HashSet<ActorSO> spokenNPCs = new HashSet<ActorSO>();
    //只读使该引用“不可变”一一这能防止你无意中重新赋值该变量(例如用一个新的空列表替换原有的列表)。第一个将是一个公共的void方法，名为record npc.ems fromthelist

   


    //记录NPC
    public void RecordNPC(ActorSO actorSO)
    {
        spokenNPCs.Add(actorSO);

        Debug.Log("Just spoke to " + actorSO.actorName);
    }


    //用于检查 NPC是否已经交流过
    public bool HasSpokenWith(ActorSO actorSO)
    {
        return spokenNPCs.Contains(actorSO);  //看spokenNPCs里面是否包含演员(actorSO)
    }

}
