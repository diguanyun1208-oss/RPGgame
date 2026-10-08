using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup menuBar;
    private bool isMenuActive;

    [SerializeField] private CanvasGroup statsMenu;
    [SerializeField] private CanvasGroup skillsMenu;
    [SerializeField] private CanvasGroup questsMenu;


    [SerializeField] private Image menuToggleImage; //菜单切换图片
    [SerializeField] private Sprite openSprite; //打开图片
    [SerializeField] private Sprite closeSprite; //关闭图片


    public void ToggleMenu(CanvasGroup target)  //切换菜单
    {
        //关闭所有打开的菜单
        SetMenuState(statsMenu, false);
        SetMenuState(skillsMenu, false);
        SetMenuState(questsMenu, false);

        //打开目标菜单
        SetMenuState(target, true);
    }


    public void ToggleMainMenu()  //切换主菜单
    {
        isMenuActive = !isMenuActive; //IsMenuActive 一个用于跟踪迷你菜单状态的布尔值

        //如果isMenuActive是菜单激活状态，那么该菜单也将被开启
        SetMenuState(menuBar, isMenuActive);

        //·当菜单打开时，我们希望显示关闭菜单的选项。
        menuToggleImage.sprite = isMenuActive ? closeSprite : openSprite;


        //每次切换小菜单时关闭所有菜单
        SetMenuState(statsMenu, false);
        SetMenuState(skillsMenu, false);
        SetMenuState(questsMenu, false);

        EventSystem.current.SetSelectedGameObject(null);
    }




    private void SetMenuState(CanvasGroup group,bool isActive)   //设置菜单状态 判断它是否为激活还是关闭状态
    {
        group.alpha = isActive ? 1 : 0;
        group.interactable = isActive;
        group.blocksRaycasts = isActive;
    }
}
