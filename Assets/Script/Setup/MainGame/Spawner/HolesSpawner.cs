using System.Collections.Generic;
using UnityEngine;

public class HolesSpawner : MonoBehaviour
{
    [SerializeField] private GameObject plateHolePrefab;

    private void SpawnHoles()
    {
        List<Bolt> boltsList = Config.Instance.boltsManager.boltsList;
        foreach(Bolt bolt in boltsList)
        {
            WallHole wallHole = this.SpawnWallHole(bolt);
            this.SpawnPlateHoles(bolt, wallHole);
        }
    }

    private WallHole SpawnWallHole(Bolt bolt)
    {
        WallHole wallHole = Config.Instance.wallHolesManager.SpawnWallHole(bolt.transform.position);

        wallHole.PinBolt(bolt);
        bolt.ChangeWallHole(wallHole);

        return wallHole;
    }

    private void SpawnPlateHoles(Bolt bolt, WallHole wallHole)
    {
        List<Plate> plates = ColliderCatcher.Instance.CatchPlates(wallHole);

        Vector3 holeLocalScale;
        Vector3 plateScale;
        PlateHole plateHole;
        SpriteMask plateHoleSpriteMask;
        SpriteRenderer plateHoleRenderer;
        int plateSortingLayerID;
        foreach (Plate plate in plates)
        {
            GameObject newPlateHoleObject = Instantiate(this.plateHolePrefab, bolt.transform.position, bolt.transform.rotation, plate.transform);
            plateHole = newPlateHoleObject.GetComponent<PlateHole>();

            plateHole.InitPinnedBolt(bolt);
            bolt.AddPlateHole(plateHole);
            plate.AddPlateHole(plateHole);

            plateScale = plate.transform.localScale;
            holeLocalScale = new
            (
                0.8f / plateScale.x,
                0.8f / plateScale.y,
                0.8f / plateScale.z
            );
            newPlateHoleObject.transform.localScale = holeLocalScale;
            newPlateHoleObject.transform.rotation = plate.transform.rotation;

            plateHoleSpriteMask = plateHole.GetComponentInChildren<SpriteMask>();
            plateSortingLayerID = plate.GetComponent<SpriteRenderer>().sortingLayerID;
            plateHoleSpriteMask.frontSortingLayerID = plateSortingLayerID;
            plateHoleSpriteMask.backSortingLayerID = plateSortingLayerID;

            plateHoleRenderer = plateHole.GetComponentInChildren<SpriteRenderer>();
            plateHoleRenderer.sortingLayerID = plateSortingLayerID;
        }
    }

    private void InitUpdatePlate()
    {
        List<Plate> platesList = Config.Instance.platesManager.GetPlatesList();
        foreach (Plate plate in platesList)
            plate.UpdatePlateHoles();
    }

    public void SpawnPlateHoles()
    {
        this.SpawnHoles();
        this.InitUpdatePlate();
    }
}
