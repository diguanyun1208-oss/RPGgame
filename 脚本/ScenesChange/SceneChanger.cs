using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneToLoad; //要加载的场景
    public Animator fadeAnim; //淡出动画
    public float fadeTime = .5f;

    //玩家进入目标场景后应出现的位置(填"目标场景"里的出生点坐标)
    public Vector2 newPlayerPosition;

    //用静态变量暂存目标位置:场景切换时本脚本所在的对象会被销毁,静态变量不会丢失
    private static Vector2 pendingPlayerPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            fadeAnim.Play("FadeToWhite");
            StartCoroutine(DelayFade());
        }
    }

    //添加一个协程  有延迟效果
    IEnumerator DelayFade()
    {
        yield return new WaitForSeconds(fadeTime);
        pendingPlayerPosition = newPlayerPosition;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneToLoad);
    }

    //场景加载完成后,把玩家移动到预设位置
    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = pendingPlayerPosition;
        }
    }
}
