using UnityEngine;

[CreateAssetMenu(fileName = "ActorSO", menuName = "对话/NPC")]
public class ActorSO : ScriptableObject
{
    public string actorName;  //存储角色名字
    public Sprite portrait; //存储头像
}
