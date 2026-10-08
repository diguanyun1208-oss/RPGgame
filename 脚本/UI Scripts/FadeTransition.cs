using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

//场景切换的过场脚本:进入场景时白屏渐退,登录切换时渐入白屏 → 停留 → 切场景 → 渐退
public class FadeTransition : MonoBehaviour
{
    public float fadeInDuration = 2f;        //登录切换时:渐入白屏时长(秒)
    public float fadeOutDuration = 2f;       //登录切换时:渐退变透明时长(秒)
    public float initialFadeDuration = 1f;   //进入登录界面时:白屏渐退时长(秒)

    private Image fadeImage;

    private void Awake()
    {
        fadeImage = GetComponent<Image>();

        //禁用 Animator,避免它的默认动画和代码渐变冲突
        Animator anim = GetComponent<Animator>();
        if (anim != null) anim.enabled = false;

        //跨场景保留,保证渐退在新场景加载后仍能播放
        DontDestroyOnLoad(gameObject);

        //进入登录界面时,先白屏,再渐退显示登录界面
        if (fadeImage != null)
            fadeImage.color = new Color(1f, 1f, 1f, 1f);
        StartCoroutine(FadeAlphaTo(0f, initialFadeDuration));
    }

    //执行过场:渐入白屏 → 停留 stayTime 秒 → 加载场景 → 渐退
    public void TransitionToScene(string sceneName, float stayTime)
    {
        StopAllCoroutines();  //停止进入场景时的渐退协程,避免冲突
        StartCoroutine(Transition(sceneName, stayTime));
    }

    private IEnumerator Transition(string sceneName, float stayTime)
    {
        //渐入白屏
        yield return StartCoroutine(FadeAlphaTo(1f, fadeInDuration));

        //白屏停留
        yield return new WaitForSeconds(stayTime);

        //加载新场景
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        StartCoroutine(FadeOutAndDestroy());
    }

    //渐退变透明,完成后销毁自己
    private IEnumerator FadeOutAndDestroy()
    {
        yield return StartCoroutine(FadeAlphaTo(0f, fadeOutDuration));
        Destroy(gameObject);
    }

    //把白屏透明度平滑渐变到 targetAlpha,持续 duration 秒
    private IEnumerator FadeAlphaTo(float targetAlpha, float duration)
    {
        if (fadeImage == null) yield break;

        float elapsed = 0f;
        float startAlpha = fadeImage.color.a;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = new Color(1f, 1f, 1f, Mathf.Lerp(startAlpha, targetAlpha, t));
            yield return null;
        }
        fadeImage.color = new Color(1f, 1f, 1f, targetAlpha);
    }
}
