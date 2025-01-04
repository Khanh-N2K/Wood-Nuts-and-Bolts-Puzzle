using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    [Header("References")]
    private Collider2D modelCollider;
    public BoltModel Model { get; private set; }
    public WallHole WallHole { get; private set; }
    public List<PlateHole> PlateHoles { get; private set; } = new();
    private List<HingeJoint2D> joints = new();

    [Header("Parameters")]
    [SerializeField] private float moveTime;
    public float GetMoveTime { get => this.moveTime; }

    private void Awake()
    {
        this.LoadReferences();
    }

    private void LoadReferences()
    {
        this.Model = transform.GetComponentInChildren<BoltModel>();
        this.modelCollider = transform.Find("CollisionCollider")?.GetComponent<Collider2D>();
    }

    public void AnimatePickBolt()
    {
        Effect.Vibrate();
        this.Model.AnimatePicking(true);
    }

    public void AnimatePinBolt()
    {
        this.Model.AnimatePicking(false);
    }

    public void TryPinBolt(WallHole wallHole, List<PlateHole> plateHoles, List<Plate> plates)
    {
        if (wallHole.Bolt != null)
        {
            if (wallHole.Bolt != this)
            {
                this.AnimatePinBolt();
                InputManager.Instance.PickBolt(wallHole.Bolt);
            }
            else
                InputManager.Instance.FreePickingBolt();
            return;
        }

        if (plates.Count != plateHoles.Count)
            return;

        if (plateHoles.Count == 0 || this.CanPin(wallHole, plateHoles))
        {
            this.PinToNewHoles(wallHole, plateHoles);
            InputManager.Instance.FreePickingBolt();
        }
    }

    private bool CanPin(WallHole wallHole, List<PlateHole> plateHoles)
    {
        foreach (PlateHole hole in plateHoles)
        {
            if (hole.Bolt != null)
                return false;
            if (Vector2.Distance(wallHole.transform.position, hole.transform.position) >= ((Config)Config.Instance).HolesMinThreshold)
                return false;
        }

        return true;
    }

    public void PinToNewHoles(WallHole wallHole, List<PlateHole> plateHoles)
    {
        this.SaveState();
        Effect.Vibrate();
        SoundManager.Instance.PlaySFX(SoundEffect.Pin);

        this.MoveToNewHole(wallHole, plateHoles);
    }

    private void MoveToNewHole(WallHole wallHole, List<PlateHole> plateHoles)
    {
        this.ChangeHoles(wallHole, plateHoles);

        if (wallHole == null)
            return;

        this.SetColliderTrigger(true);
        transform.DOMove(wallHole.transform.position, this.moveTime)
            .OnComplete(() =>
            {
                this.SetColliderTrigger(false);
            });
        this.Model.transform.DOKill();
    }

    public void ChangeHoles(WallHole wallHole, List<PlateHole> plateHoles)
    {
        this.ChangeWallHole(wallHole);
        this.ReplacePlateHoles(plateHoles);
    }

    public void ChangeWallHole(WallHole wallHole)
    {
        this.WallHole?.PinBolt(null);
        this.WallHole = wallHole;
        this.WallHole?.PinBolt(this);
    }

    private void ReplacePlateHoles(List<PlateHole> plateHoles)
    {
        foreach (PlateHole oldHole in this.PlateHoles)
            oldHole.PinBolt(null);
        this.PlateHoles.Clear();

        if (plateHoles == null)
            return;

        foreach (PlateHole newHole in plateHoles)
        {
            this.PlateHoles.Add(newHole);
            newHole.PinBolt(this);
        }
    }


    private void SaveState()
    {
        List<PlateHole> plateHoleList = new();
        List<Plate> plateList_2 = new();
        List<Plate> plateList_1 = new();
        List<Vector3> platePositionList = new();
        List<Quaternion> plateRotationList = new();
        List<Vector2> plateVelocityList = new();

        Plate loopingPlate;
        foreach(PlateHole plateHole in this.PlateHoles)
        {
            plateHoleList.Add(plateHole);
            loopingPlate = plateHole.Plate;
            if(loopingPlate.PinnedBoltCount == 2)
                plateList_1.Add(loopingPlate);
        }

        List<Plate> platesList = Config.Instance.platesManager.GetPlatesList();
        foreach(Plate plate in platesList)
            if(plate.PinnedBoltCount == 1)
                plateList_2.Add(plate);

        plateList_2 = plateList_1.Union(plateList_2).ToList();    

        Vector3 loopingVelocity;
        foreach(Plate plate in plateList_2)
        {
            platePositionList.Add(plate.transform.position);
            plateRotationList.Add(plate.transform.rotation);
            loopingVelocity = new Vector2(plate.RigidBody.velocity.x, plate.RigidBody.velocity.y);
            plateVelocityList.Add(loopingVelocity);
        }

        LastMovedState savingState = new(
            this,
            this.WallHole,
            plateHoleList,
            plateList_2,
            platePositionList,
            plateRotationList,
            plateVelocityList
        );
        StateManager.SaveState(savingState);
    }

    public void AddPlateHole(PlateHole plateHole)
    {
        this.PlateHoles.Add(plateHole);
    }

    public void SetColliderTrigger(bool trigger)
    {
        if(this.modelCollider != null) 
            this.modelCollider.isTrigger = trigger;
    }

    public void SetJoint(Rigidbody2D plateBody)
    {
        HingeJoint2D newJoint = gameObject.AddComponent<HingeJoint2D>();
        newJoint.connectedBody = plateBody;
        newJoint.enableCollision = false;
        this.joints.Add(newJoint);
    }

    public void RemoveJoint(Rigidbody2D plateBody)
    {
        foreach(HingeJoint2D joint in this.joints)
        {
            if (joint.connectedBody == plateBody)
            {
                this.joints.Remove(joint);
                Destroy(joint);
                break;
            }
        }
    }

    public void RemoveSelf()
    {
        this.PinToNewHoles(null, null);
        BoltCounter.UpdateCount(-1);
        gameObject.SetActive(false);
    }
}