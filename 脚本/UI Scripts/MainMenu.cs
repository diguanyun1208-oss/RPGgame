using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("背景(可在此更换背景图片)")]
    public Image backgroundImage;  //背景图片

    [Header("主按钮(可在此更换按钮图片)")]
    public Button loginButton;     //登录按钮
    public Button helpButton;      //说明按钮
    public Button settingsButton;  //设置按钮
    public Button quitButton;      //退出按钮

    [Header("说明面板")]
    public CanvasGroup helpPanel;     //说明面板(控制显示/隐藏)
    public TMP_Text helpContentText;  //说明内容文本
    public Button helpCloseButton;    //说明面板的关闭按钮

    [Header("设置面板")]
    public CanvasGroup settingsPanel;    //设置面板(控制显示/隐藏)
    public Button[] resolutionButtons;   //分辨率按钮(与 resolutions 数组一一对应)
    public Button settingsCloseButton;   //设置面板的关闭按钮

    [Header("游戏场景")]
    public string gameSceneName = "SampleScene";  //登录后进入的游戏场景名

    [Header("过场白屏")]
    public FadeTransition fadeTransition;  //场景切换的渐入渐退
    public float fadeTime = 2f;  //白屏停留时间(秒)

    private bool isLoggingIn;  //防止重复点击登录

    //预设分辨率(与 resolutionButtons 一一对应)
    private readonly Vector2Int[] resolutions = new Vector2Int[]
    {
        new Vector2Int(1920, 1080),
        new Vector2Int(1600, 900),
        new Vector2Int(1366, 768),
        new Vector2Int(1280, 720),
    };

    private void Start()
    {
        //绑定主按钮事件
        loginButton.onClick.AddListener(OnLoginClicked);
        helpButton.onClick.AddListener(OnHelpClicked);
        settingsButton.onClick.AddListener(OnSettingsClicked);
        quitButton.onClick.AddListener(OnQuitClicked);

        //绑定面板关闭按钮
        helpCloseButton.onClick.AddListener(() => SetPanelActive(helpPanel, false));
        settingsCloseButton.onClick.AddListener(() => SetPanelActive(settingsPanel, false));

        //绑定分辨率按钮
        for (int i = 0; i < resolutionButtons.Length && i < resolutions.Length; i++)
        {
            int index = i;  //局部变量捕获,避免闭包问题
            resolutionButtons[i].onClick.AddListener(() => ApplyResolution(index));
        }

        //初始状态:两个面板都隐藏
        SetPanelActive(helpPanel, false);
        SetPanelActive(settingsPanel, false);
    }

    //登录:进入游戏(带渐入渐退过场)
    private void OnLoginClicked()
    {
        if (isLoggingIn) return;  //防止重复点击
        isLoggingIn = true;

        if (fadeTransition != null)
            fadeTransition.TransitionToScene(gameSceneName, fadeTime);
        else
            SceneManager.LoadScene(gameSceneName);  //没有过场组件时直接进入
    }

    //说明:显示说明面板
    private void OnHelpClicked()
    {
        SetPanelActive(settingsPanel, false);
        SetPanelActive(helpPanel, true);
    }

    //设置:显示设置面板
    private void OnSettingsClicked()
    {
        SetPanelActive(helpPanel, false);
        SetPanelActive(settingsPanel, true);
    }

    //退出:退出游戏
    private void OnQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    //应用指定索引的分辨率
    private void ApplyResolution(int index)
    {
        if (index < 0 || index >= resolutions.Length) return;
        Vector2Int res = resolutions[index];
        Screen.SetResolution(res.x, res.y, Screen.fullScreen);
    }

    //设置面板显示/隐藏
    private void SetPanelActive(CanvasGroup group, bool active)
    {
        if (group == null) return;
        group.alpha = active ? 1 : 0;
        group.interactable = active;
        group.blocksRaycasts = active;
    }
}
