using UnityEngine;

public class ScaleOnMouseOver : MonoBehaviour
{
    private Vector3 originalScale;
    private bool isMouseOver = false;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void Update()
    {
        if (isMouseOver)
        {
            // Continuously set the rotation to (0, current rotation around Y, 0)
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }
    }

    private void OnMouseEnter()
    {
        // Scale up by 10%
        transform.localScale = originalScale * 1.1f;
        isMouseOver = true;
    }

    private void OnMouseExit()
    {
        // Restore the original scale
        transform.localScale = originalScale;
        isMouseOver = false;
    }
}
