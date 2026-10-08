using UnityEngine;

public class ToogleSkillTree : MonoBehaviour
{
    public CanvasGroup statsCanvas;  //统计画布
    private bool skillTreeOpen = false; //判断技能树是否打开

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("切换技能树"))
        {
            if (skillTreeOpen)
            {
                Time.timeScale = 1;
                statsCanvas.alpha = 0;
                statsCanvas.blocksRaycasts = false;
                skillTreeOpen = false;
            }
            else
            {
                Time.timeScale = 0;
                statsCanvas.alpha = 1;
                statsCanvas.blocksRaycasts = true;
                skillTreeOpen = true;
            }
        }
    }
}
