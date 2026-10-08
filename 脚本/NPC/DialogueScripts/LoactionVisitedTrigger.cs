using UnityEngine;

public class LoactionVisitedTrigger : MonoBehaviour
{
    [SerializeField] private LocationSO locationVisited;
    [SerializeField] private bool destroyOnTouch = true;   //触摸即销毁

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GameManager.Instance.LocationHistoryTracker.RecordLoaction(locationVisited);
            
            if(destroyOnTouch)
            {
                Destroy(gameObject);
            }
        }
    }
}
