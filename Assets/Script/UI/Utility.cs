using UnityEngine;

public static class Utility
{
    public static RectTransform GetRootCanvas (Transform canvasToGetRoot)
    {
        while (canvasToGetRoot != null)
        {
            Canvas canvas = canvasToGetRoot.GetComponent<Canvas>();
            if (canvas != null && canvas.isRootCanvas)
            {
                return canvas.GetComponent<RectTransform>();
            }
            canvasToGetRoot = canvasToGetRoot.parent;
        }
        return null;
    }

    public static float GetHeightInPixels(RectTransform rectTransform)
    {
        if (rectTransform == null)
            return 0f;

        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        float barHeight = Mathf.Abs(corners[1].y - corners[0].y);
        return barHeight;
    }

    public static Bounds GetBounds(Transform transform)
    {
        Renderer renderer = transform.GetComponent<Renderer>();
        if (renderer != null)
        {
            return renderer.bounds;
        }

        Bounds bounds = new Bounds(transform.position, Vector3.zero);
        foreach (Renderer childRenderer in transform.GetComponentsInChildren<Renderer>())
        {
            bounds.Encapsulate(childRenderer.bounds);
        }

        return bounds;
    }
}