using System.Collections.Generic;
using UnityEngine;

public class LocationHistoryTracker : MonoBehaviour
{
    public readonly HashSet<LocationSO> locationsVisited = new HashSet<LocationSO>();     //访问过的地点
    //只读使该引用“不可变”一一这能防止你无意中重新赋值该变量(例如用一个新的空列表替换原有的列表)。

    //列表                               哈希集  (哈希集还提供更快的查找速度)
    //最适合在以下情况下使用               最适合在以下情况下使用
    //1)顺序重要                          1)顺序不重要
    //2)您希望允许重复项                   2)你不想允许重复项    





    //记录NPC
    public void RecordLoaction(LocationSO locationSO)
    {
        if (locationsVisited.Add(locationSO))
        {
            Debug.Log("Just visited to " + locationSO.displayName);
        }
    }


    //用于检查 NPC是否已经交流过
    public bool HasVisited(LocationSO locationSO)
    {
        return locationsVisited.Contains(locationSO);  
    }
}
