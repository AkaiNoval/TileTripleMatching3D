using UnityEngine;
using UnityEngine.EventSystems;

public class RetransformOnMouseOver : MonoBehaviour
{
    private Vector3 originalScale;
    private bool isMouseOver = false;

    private void Awake()
    {
        originalScale = transform.localScale;
    }
    private void OnEnable()
    {
        transform.localScale = originalScale;
    }
    private void Update()
    {
        if (isMouseOver) return;
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }
    private void OnMouseOver()
    {
        if (MouseOverUIUtil.IsMouseOverUIWithIgnores()) return;
        transform.localScale = originalScale * 1.1f;
        isMouseOver = true;
    }
    private void OnMouseExit()
    {
        transform.localScale = originalScale;
        isMouseOver = false;
    }
}
