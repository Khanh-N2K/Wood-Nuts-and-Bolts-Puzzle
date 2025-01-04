using System.Collections.Generic;
using UnityEngine;

public class LastMovedState
{
    public Bolt Bolt { get; private set; }
    public WallHole WallHole { get; private set; }
    public List<PlateHole> PlateHoleList { get; private set; }
    public List<Plate> PlateList { get; private set; }
    public List<Vector3> PlatePositionList { get; private set; }
    public List<Quaternion> PlateRotationList { get; private set; }
    public List<Vector2> PlateVelocityList { get; private set; }

    public LastMovedState(Bolt bolt, WallHole wallHole, List<PlateHole> plateHoleList, List<Plate> plateList, List<Vector3> platePositionList, List<Quaternion> plateRotationList, List<Vector2> plateVelocityList)
    {
        this.Bolt = bolt;
        this.WallHole = wallHole;
        this.PlateHoleList = plateHoleList;
        this.PlateList = plateList;
        this.PlatePositionList = platePositionList;
        this.PlateRotationList = plateRotationList;
        this.PlateVelocityList = plateVelocityList;
    }
}
