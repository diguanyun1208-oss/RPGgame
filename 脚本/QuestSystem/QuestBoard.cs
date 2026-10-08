using UnityEngine;

public class QuestBoard : MonoBehaviour
{
    [SerializeField] private QuestSO questToOffer;  //任务提供
    [SerializeField] private QuestSO questToTurnIn; //待提交任务

    private bool playerInRange;

    private void Update()
    {
        if(playerInRange && Input.GetButtonDown("互动键"))
        {
            //是否可提交                                这个事件的作用是允许我们传入一个任务并得到返回结果，
            bool canTurnIn = questToTurnIn != null && QuestEvents.IsQuestComplete?.Invoke(questToTurnIn) == true;

            if(canTurnIn)
            {
                QuestEvents.OnQuestTurnInRequested?.Invoke(questToTurnIn);
            }
            else
            {
                QuestEvents.OnQuestOfferRequested?.Invoke(questToOffer);
            }
        }
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

}
