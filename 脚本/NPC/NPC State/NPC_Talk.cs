using System.Collections.Generic;
using UnityEngine;

public class NPC_Talk : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public Animator interactAnim; //交互动画

    public List<DialogueSO> conversations;
    public DialogueSO currentConversation;  //当前对话


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }


    private void Start()
    {
        QuestEvents.OnQuestAccepted += OnQuestAccepted_RemoveOfferings;
    }

    private void OnDestroy()
    {
        QuestEvents.OnQuestAccepted -= OnQuestAccepted_RemoveOfferings;
    }


    private void OnEnable()
    {
        rb.linearVelocity = Vector2.zero;
        anim.Play("Idle");
        interactAnim.Play("Open");
    }

    private void OnDisable()
    {
        interactAnim.Play("Close");
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void Update()
    {
        if(Input.GetButtonDown("互动键"))
        {
            if (GameManager.Instance.DialogueManager.isDialogueActive)
                //待办事项:推进对话
                GameManager.Instance.DialogueManager.AdvanceDialogue();
            else
            {
                if (GameManager.Instance.DialogueManager.CanStartDialogue())
                {
                    CheckForNewConversation();
                    GameManager.Instance.DialogueManager.StartDialogue(currentConversation);
                }
            }
        }
    }


    private void CheckForNewConversation()    //检查新的对话  //找到有效对话
    {
        for(int i = 0; i < conversations.Count; i++)
        {
            //变量 这是一个通用变量类型。我也可以在这里使用DialoguesO，但这些代码行变得相当长，所以我用var来保持它们更简洁。
            var convo = conversations[i];
            if(convo != null && convo.IsConditionMet())
            {
                currentConversation = convo;

                //如果仅限一次性使用，请删除此行       //示例:"今天是丰收节! 一旦那天过去，就不再需要它了.
                if (convo.removeAfterPlay)
                    conversations.RemoveAt(i);  //移除第i行


                //移除当此对话框播放时应被清除的其他对话(例如任务完成)
                if(convo.removeTheseOnPlay != null && convo.removeTheseOnPlay.Count > 0)
                {
                    foreach(var toRemove in convo.removeTheseOnPlay)
                    {
                        conversations.Remove(toRemove);
                    }
                }

      
                break;
            }
        }
    }


    private void OnQuestAccepted_RemoveOfferings(QuestSO acceptedQuest)
    {
        for(int i = conversations.Count - 1; i >= 0; i--)
        {
            var convo = conversations[i];
            if (convo == null)
                continue;

            if (convo.offerQuestOnEnd == acceptedQuest)
                conversations.RemoveAt(i);
        }
    }



}
