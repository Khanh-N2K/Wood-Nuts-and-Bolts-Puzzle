using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDrag : MonoBehaviour
{
    private BombExplosion bombExplosion;

    [Header("Radius illustration")]
    private LineRenderer lineRenderer;
    [SerializeField] private int segmentCount;

    [Header("Plate animate")]
    [SerializeField] private float plateBlendColorTime;
    
    private Vector3 offset;
    private Dictionary<Plate, UnityEngine.Color> originalColors = new();

    private void Awake()
    {
        this.LoadReferences();
    }

    private void Start()
    {
        this.DrawRadius();
    }

    private void LoadReferences()
    {
        this.bombExplosion = GetComponent<BombExplosion>();
        this.lineRenderer = GetComponent<LineRenderer>();
    }

    private void DrawRadius()
    {
        this.lineRenderer.positionCount = this.segmentCount + 1;

        float angle = 20f;
        for (int i = 0; i < (this.segmentCount + 1); i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * this.bombExplosion.Radius;
            float y = Mathf.Cos(Mathf.Deg2Rad * angle) * this.bombExplosion.Radius;

            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
            angle += (360f / this.segmentCount);
        }
    }

    void OnMouseDown()
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        this.lineRenderer.enabled = true;
        StartCoroutine(this.RotateBorder());
        StartCoroutine(this.AnimatePlatesInRange());
    }

    void OnMouseDrag()
    {
        Vector3 newPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        transform.position = Camera.main.ScreenToWorldPoint(newPosition) + offset;
    }

    void OnMouseUp()
    {
        StopAllCoroutines();
        this.RestoreColors();
        this.bombExplosion.Explode();
        this.lineRenderer.enabled = false;
    }

    private IEnumerator RotateBorder()
    {
        while (true)
        {
            Vector3 rootPos = lineRenderer.GetPosition(0);

            for (int i = 0; i < this.segmentCount; i++)
                lineRenderer.SetPosition(i, lineRenderer.GetPosition(i + 1));
            lineRenderer.SetPosition(this.segmentCount, rootPos);

            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator AnimatePlatesInRange()
    {
        HashSet<Plate> blendList = new();

        while (true)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, this.bombExplosion.Radius, Config.Instance.PlatesLayerMask);

            Plate plate;
            HashSet<Plate> newBlendList = new();
            foreach (Collider2D collider in colliders)
            {
                plate = collider.GetComponent<Plate>();

                if (plate.PinnedBoltCount <= 1)
                    newBlendList.Add(plate);
            }

            foreach (Plate newPlate in newBlendList)
                if (!blendList.Contains(newPlate))
                    AnimatePlate(newPlate, true);

            foreach (Plate oldPlate in blendList)
                if (!newBlendList.Contains(oldPlate))
                    AnimatePlate(oldPlate, false);

            blendList = newBlendList;

            yield return new WaitForSeconds(0.2f);
        }
    }

    private void AnimatePlate(Plate plate, bool animate)
    {
        SpriteRenderer renderer = plate.SpriteRenderer;
        if (animate)
        {
            if (!originalColors.ContainsKey(plate))
                originalColors[plate] = renderer.color;

            renderer.DOBlendableColor(Color.white, this.plateBlendColorTime)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.Linear);
        }
        else
        {
            renderer.DOKill();

            if (originalColors.TryGetValue(plate, out UnityEngine.Color originalColor))
            {
                renderer.color = originalColor;
                originalColors.Remove(plate);
            }
        }
    }

    private void RestoreColors()
    {
        SpriteRenderer renderer;
        List<Plate> platesToRemove = new();

        foreach (var kvp in originalColors)
        {
            Plate plate = kvp.Key;
            UnityEngine.Color originalColor = kvp.Value;
            renderer = plate.SpriteRenderer;

            renderer.DOKill();
            renderer.color = originalColor;

            platesToRemove.Add(plate);
        }

        foreach (Plate plate in platesToRemove)
            originalColors.Remove(plate);
    }
}
