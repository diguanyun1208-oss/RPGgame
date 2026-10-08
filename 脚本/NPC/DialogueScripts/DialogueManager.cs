using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    

    [Header("UI参考")]
    public CanvasGroup canvasGroup;
    public Image portrait; //将用于存放我们肖像的精灵图的UI元素
    public TMP_Text actorName; //演员名字
    public TMP_Text dialogueText; //对话文本
    public Button[] choiceButtons; //公共按钮

    public bool isDialogueActive;

    private DialogueSO currentDialogue;
    private int dialogueIndex; //记录当前是文本的哪一行

    private float lastDialogueEndTime; //最后的对话结束时间
    private float dialogueCooldown; //对话冷却时间  //多久后开启另一段对话


    private void Awake()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        foreach(var button in choiceButtons)
            button.gameObject.SetActive(false);
    }


    public bool CanStartDialogue()
    {
        //(unscaledTime)未缩放时间  游戏运行了多长时间。 未缩放，意味着如果我们暂停游戏(我们可能在某些对话期间想这样做)，它将继续计时。
        return Time.unscaledTime - lastDialogueEndTime >= dialogueCooldown; 
           
    }

    public void StartDialogue(DialogueSO dialogueSO)
    {

        currentDialogue = dialogueSO;
        dialogueIndex = 0;
        isDialogueActive = true;
        ShowDialogue(); 
    }

    public void AdvanceDialogue()   //推进对话
    {
        //防守检查仅在有可推进的线路时才应推进。
        if (dialogueIndex < currentDialogue.lines.Length)  //能够识别出对话何时结束
            ShowDialogue();
        else
            ShowChoices();
    }

    private void ShowDialogue()
    {
        DialogueLine line = currentDialogue.lines[dialogueIndex];

        GameManager.Instance.DIalogueHistoryTracker.RecordNPC(line.speaker);

        portrait.sprite = line.speaker.portrait;
        actorName.text = line.speaker.actorName;

        dialogueText.text = line.text;

        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        dialogueIndex++;
    }


    private void ShowChoices()
    {
        ClearChoices();

        if(currentDialogue.options.Length > 0)
        {
            for(int i = 0; i < currentDialogue.options.Length; i++)
            {
                var option = currentDialogue.options[i];

                choiceButtons[i].GetComponentInChildren<TMP_Text>().text = option.optionText;
                choiceButtons[i].gameObject.SetActive(true);

                choiceButtons[i].onClick.AddListener(() => ChooseOption(option.nextDialogue));
            }
            EventSystem.current.SetSelectedGameObject(choiceButtons[0].gameObject);
        }
        else
        {
            if(currentDialogue.turnInQuestOnEnd != null &&
                GameManager.Instance.QuestManager.IsQuestComplete(currentDialogue.turnInQuestOnEnd))  //如果有一个任务要提交，并且那个任务已经完成
            {
                QuestEvents.OnQuestTurnInRequested?.Invoke(currentDialogue.turnInQuestOnEnd);
                EndDialogue();
            }
            else if(currentDialogue.offerQuestOnEnd != null)
            {
                EndDialogue();
                QuestEvents.OnQuestOfferRequested?.Invoke(currentDialogue.offerQuestOnEnd);
            }
            else
            {
                choiceButtons[0].GetComponentInChildren<TMP_Text>().text = "结束";
                choiceButtons[0].onClick.AddListener(EndDialogue);
                choiceButtons[0].gameObject.SetActive(true);

                EventSystem.current.SetSelectedGameObject(choiceButtons[0].gameObject);
            }
        }
    }


    private void ChooseOption(DialogueSO dialogueSO)
    {
        if (dialogueSO == null)
            EndDialogue();
        else
        {
            ClearChoices();
            StartDialogue(dialogueSO);
        }
    }


    private void EndDialogue()
    {
        dialogueIndex = 0;
        isDialogueActive = false;
        ClearChoices();

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        lastDialogueEndTime = Time.unscaledTime; //记录我们对话结束的时间
    }

    //清理选项
    private void ClearChoices()
    {
        foreach(var button in choiceButtons)
        {
            button.gameObject.SetActive(false);
            button.onClick.RemoveAllListeners();
        }
    }

}
