using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;   //静态:在一个场景中只能有一个  实例:一个提醒我们这个对象只能有1个“实例”的名称。 静态:使其他脚本更容易访问此内容

    public DialogueManager DialogueManager;
    public DIalogueHistoryTracker DIalogueHistoryTracker;
    public LocationHistoryTracker LocationHistoryTracker;
    public QuestManager QuestManager;

    [Header("持久化对象")]
    public GameObject[] persistenObjects;

    private void Awake()
    {
        if (Instance != null)
        {
            CleanUpAndDestroy();  //销毁并清理
            return; //返回阻止方法的其余部分执行任何更多逻辑。 我们现在其实不需要这个(因为我们正在销毁对象)，但以后会派上用场。
        }

        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); //现在当我们移动时，此对象将保持存在。在场景之间(而不是自毁)
            MarkPersistentObjects();
        }
    }


    //标记持久化对象
    private void MarkPersistentObjects()
    {
        foreach(GameObject obj in persistenObjects)
        {
            if (obj != null)
            {
                DontDestroyOnLoad(obj);
            }
        }
    }


    private void CleanUpAndDestroy()
    {
        foreach(GameObject obj in persistenObjects)
        {
            Destroy(obj);
        }

        Destroy(gameObject);
    }
}
