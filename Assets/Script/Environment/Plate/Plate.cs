using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    public Rigidbody2D RigidBody { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }
    private List<PlateHole> plateHoles = new();
    private Bolt hangingBolt;

    [Header("Parameters")]
    [SerializeField] private float despawnDistance;
    private bool counted = true;

    public int PinnedBoltCount { get; private set; }

    private void Awake()
    {
        this.LoadReferences();
        this.Reset();
    }

    private void LoadReferences()
    {
        this.RigidBody = GetComponent<Rigidbody2D>();
        this.SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Reset()
    {
        gameObject.layer = transform.parent.gameObject.layer;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        string layerName = LayerMask.LayerToName(gameObject.layer);
        spriteRenderer.sortingLayerID = SortingLayer.NameToID(layerName);
        spriteRenderer.color = transform.parent.GetComponent<LayerColor>().Color;
    }

    public void UpdatePlateHoles()
    {
        this.PinnedBoltCount = transform.childCount;
        this.EndUpdateBoltCount(0);
    }

    public void UnpinBolt()
    {
        this.UpdateBoltCount(false);
    }

    public void PinNewBolt()
    {
        this.UpdateBoltCount(true);
    }

    private void UpdateBoltCount(bool pinBolt)
    {
        int oldPinnedBoltCount = this.PinnedBoltCount;

        if(pinBolt)
            this.PinnedBoltCount++;
        else 
            this.PinnedBoltCount--;

        this.EndUpdateBoltCount(oldPinnedBoltCount);
    }

    private void EndUpdateBoltCount(int oldPinnedBoltCount)
    {
        this.UpdatePhysic(oldPinnedBoltCount);
        this.UpdatePlateCount(oldPinnedBoltCount);
    }

    private void UpdatePhysic(int oldPinnedBoltCount)
    {
        if (this.PinnedBoltCount == 1)
            this.SetJoint();
        else if (oldPinnedBoltCount == 1)
            this.RemoveJoint();

        if (this.PinnedBoltCount <= 1)
            this.RigidBody.bodyType = RigidbodyType2D.Dynamic;
        else
            this.RigidBody.bodyType = RigidbodyType2D.Static;
    }

    private void UpdatePlateCount(int oldPinnedBoltCount)
    {
        if (this.PinnedBoltCount == 0)
            StartCoroutine(DespawnByDistance());
        else if (this.PinnedBoltCount == 1 && oldPinnedBoltCount == 0)
        {
            if (this.counted)
            {
                StopAllCoroutines();
                return;
            }

            PlateCounter.Instance.UpdatePlateCount(PlateCounter.Instance.PlateCount + 1);
            this.counted = true;
        }
    }

    private IEnumerator DespawnByDistance()
    {
        while (true)
        {
            float distance = Vector3.Distance(transform.position, Vector3.zero);
            if (distance > this.despawnDistance)
            {
                PlateCounter.Instance.UpdatePlateCount(PlateCounter.Instance.PlateCount - 1);
                this.counted = false;
                this.SpriteRenderer.DOFade(0, 0.5f)
                    .OnComplete(() =>
                    {
                        gameObject.SetActive(false);
                    });
                yield break;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    private void SetJoint() 
    {
        foreach(PlateHole plateHole in this.plateHoles)
        {
            if (plateHole.Bolt != null)
            {
                this.hangingBolt = plateHole.Bolt;
                break;
            }
        }

        this.hangingBolt?.SetJoint(this.RigidBody);
    }

    private void RemoveJoint() 
    {
        this.hangingBolt.RemoveJoint(this.RigidBody);
        this.hangingBolt = null;
    }
    
    public void AddPlateHole(PlateHole plateHole)
    {
        this.plateHoles.Add(plateHole);
    }

    public void RemoveSelf()
    {
        foreach (PlateHole hole in this.plateHoles)
        {
            if (hole.Bolt == null)
                continue;

            List<PlateHole> newPlateHoleList = new();
            foreach(PlateHole hole2 in hole.Bolt.PlateHoles)
            {
                newPlateHoleList.Add(hole2);
            }
            newPlateHoleList.Remove(hole);
            hole.Bolt.PinToNewHoles(hole.Bolt.WallHole, newPlateHoleList);
            return;
        }
    }
}
