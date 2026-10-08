using UnityEngine;

[CreateAssetMenu(menuName = "LocationSO")]
public class LocationSO : ScriptableObject
{
    public string locationID;  //位置ID  这是游戏在识别区域时将要查找的内容这将在整个游戏中保持不变。    例如:海滩1-1，森林2-3
    public string displayName; //显示名称  在地图上显示的内容， 这可以在运行时更改。例如，伟大的城市可能后来会变成被毁的城市。

}
