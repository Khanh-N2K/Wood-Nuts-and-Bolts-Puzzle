using System.Collections.Generic;
using UnityEngine;

public class ColliderCatcher : MonoBehaviour
{
    public static ColliderCatcher Instance { get; private set; }

    private void Awake()
    {
        this.SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public WallHole CatchWallHole(Vector2 mousePos)
    {
        Collider2D holeCollider = Physics2D.OverlapPoint(mousePos, ((Config)Config.Instance).WallHoleLayerMask);

        if (holeCollider == null) return null;
        return holeCollider.transform.parent.GetComponent<WallHole>();
    }

    public List<PlateHole> CatchPlateHoles(WallHole wallHole)
    {
        Vector2 wallHolePos = wallHole.transform.position;
        float wallHoleRadius = wallHole.StandardRadius;

        Collider2D[] holeColliders = Physics2D.OverlapCircleAll(wallHolePos, wallHoleRadius, ((Config)Config.Instance).PlateHoleLayerMask);
        List<PlateHole> plateHoleList = new();
        foreach (Collider2D hole in holeColliders)
        {
            plateHoleList.Add(hole.transform.parent.GetComponent<PlateHole>());
        }

        return plateHoleList;
    }

    public List<Plate> CatchPlates(WallHole wallHole)
    {
        Vector2 wallHolePos = wallHole.transform.position;
        float wallHoleRadius = wallHole.StandardRadius;

        Collider2D[] plateColliders = Physics2D.OverlapCircleAll(wallHolePos, wallHoleRadius, ((Config)Config.Instance).PlatesLayerMask);
        List<Plate> plateList = new();
        foreach (Collider2D plateCollider in plateColliders)
            plateList.Add(plateCollider.GetComponent<Plate>());

        return plateList;
    }
}
