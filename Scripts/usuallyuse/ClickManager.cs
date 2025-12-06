using UnityEngine;
using UnityEngine.Rendering; // SortingGroup用
using UnityEngine.EventSystems; // UIクリック判定用
using System.Linq;

public class ClickManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // UIをクリックしている場合は無視
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(worldPoint, Vector2.zero);

            if (hits.Length > 0)
            {
                var bestHit = hits
                    .OrderByDescending(h =>
                    {
                        var sg = h.collider.GetComponentInParent<SortingGroup>();
                        if (sg != null) return sg.sortingOrder;

                        var sr = h.collider.GetComponent<SpriteRenderer>();
                        return sr != null ? sr.sortingOrder : 0;
                    })
                    .First();

                bestHit.collider.SendMessage("OnClicked", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
