using TMPro;
using UnityEngine;

public class QuestLogSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text questNameText;
    [SerializeField] private TMP_Text questLevelText;

    public QuestSO currentQuest;

    public QuestLogUI questLogUI;

    private void OnValidate()   // OnValidate()  每当您在检查器中更改内容时都会运行。
    {
        if(currentQuest != null)
            SetQuest(currentQuest);
        else
            gameObject.SetActive(false);
    }


    public void SetQuest(QuestSO questSO)
    {
        currentQuest = questSO;

        questNameText.text = questSO.questName;
        questLevelText.text = " Lv. " + questSO.questLevel.ToString();

        gameObject.SetActive(true);
    }


    //清空已完成和被丢弃的任务
    public void ClearSlot()
    {
        currentQuest = null;
        gameObject.SetActive(false);
    }


    public void OnSlotClicked()   //在当前槽位上点击
    {
        questLogUI.HandleQuestClicked(currentQuest);
    }
}
