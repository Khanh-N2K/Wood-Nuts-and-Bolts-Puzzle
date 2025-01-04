using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHolesManager: MonoBehaviour
{
    [SerializeField] private GameObject wallHolePrefab;

    private List<WallHole> wallHolesList = new();

    private void Awake()
    {
        foreach (WallHole wallHole in transform.GetComponentsInChildren<WallHole>())
            this.wallHolesList.Add(wallHole);
    }

    public WallHole SpawnWallHole(Vector3 position)
    {
        GameObject wallHoleObj = Instantiate(this.wallHolePrefab, position, Quaternion.identity, transform);
        WallHole wallHole = wallHoleObj.GetComponent<WallHole>();
        wallHolesList.Add(wallHole);
        return wallHole;
    }

    public void DisableWallHoles()
    {
        foreach(WallHole wallHole in wallHolesList)
            wallHole.gameObject.SetActive(false);
    }

    public IEnumerator AnimateSpawning(float waitTime)
    {
        foreach(WallHole wallHole in this.wallHolesList)
        {
            wallHole.gameObject.SetActive(true);
            yield return new WaitForSeconds(waitTime);
        }
    }
}