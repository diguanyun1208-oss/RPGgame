using UnityEngine;

public class NPC : MonoBehaviour
{
    public enum NPCState { Default, Idle, Patrol, Wander, Talk}   //NPC的状态
    public NPCState currentState = NPCState.Patrol;  //当前状态
    private NPCState defaultState;  //默认状态

    public NPC_Patrol patrol;
    public NPC_Wander wander;
    public NPC_Talk talk;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        defaultState=currentState;
        SwitchState(currentState);
    }

    public void SwitchState(NPCState newState)
    {
        currentState = newState;

        //布尔运算符 根据语句的结果设置布尔值。
        //在这里，布尔值表示是否启用脚本 而语句是判断newState是否等于某个特定的NPC状态
        patrol.enabled = newState == NPCState.Patrol;
        wander.enabled = newState == NPCState.Wander;
        talk.enabled = newState == NPCState.Talk;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            SwitchState(NPCState.Talk);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            SwitchState(defaultState);
    }


}
