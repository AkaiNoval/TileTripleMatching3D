using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public static class MouseOverUIUtil
{
    public static bool IsMouseOverUI() => EventSystem.current.IsPointerOverGameObject();
    public static bool IsMouseOverUIWithIgnores()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        for (int i = 0; i < raycastResults.Count; i++)
        {
            if (raycastResults[i].gameObject.TryGetComponent(out MouseUIIgnores mouseUIIgnores))
            {
                raycastResults.RemoveAt(i);
                i--;
            }
        }

        return raycastResults.Count > 0;
    }
}
