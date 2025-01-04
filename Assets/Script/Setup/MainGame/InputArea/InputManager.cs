using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { private set; get; }

    private Bolt pickingBolt;
    public Bolt PickingBolt { get => pickingBolt; }

    public bool IsAllowPickBolt { get; private set; } = true;
    private bool isRemovingBolt = false;
    
    private void Awake()
    {
        this.SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        if(RoundManager.Instance != null)
            RoundManager.Instance.OnNewLevel += this.FreePickingBolt;
    }

    private void OnMouseDown()
    {
        if (!this.IsAllowPickBolt)
            return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (this.pickingBolt != null)
            this.PinBolt(mousePos);
        else
            this.PickBolt(mousePos);
    }

    private void PinBolt(Vector2 mousePos)
    {
        if(this.pickingBolt == null) 
            return;

        WallHole wallHole = ColliderCatcher.Instance.CatchWallHole(mousePos);
        if (wallHole == null)
            return;

        List<PlateHole> plateHoles = ColliderCatcher.Instance.CatchPlateHoles(wallHole);
        List<Plate> plates = ColliderCatcher.Instance.CatchPlates(wallHole);

        this.pickingBolt.TryPinBolt(wallHole, plateHoles, plates);
    }

    public void FreePickingBolt()
    {
        this.pickingBolt?.AnimatePinBolt();
        this.pickingBolt = null;
    }

    private void PickBolt(Vector2 mousePos)
    {
        Collider2D boltCollider = Physics2D.OverlapPoint(mousePos, Config.Instance.BoltLayerMask.value);
        Bolt bolt = boltCollider?.transform.parent.GetComponent<Bolt>();

        if(this.isRemovingBolt)
        {
            if (bolt == null)
                return;

            bolt.RemoveSelf();
            this.isRemovingBolt = false;
            BoltRemoval.EndRemoveBolt();
            
            return;
        }

        PickBolt(bolt);
    }

    public void PickBolt(Bolt bolt)
    {
        this.pickingBolt = bolt;
        bolt?.AnimatePickBolt();
        if (bolt != null)
            SoundManager.Instance.PlaySFX(SoundEffect.Unpin);
    }

    public void AllowPickBolt(bool input)
    {
        this.IsAllowPickBolt = input;
    }

    public void OnRemoveBolt(bool removable)
    {
        this.FreePickingBolt();
        this.isRemovingBolt = removable;
    }
}
