using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesManager: MonoBehaviour
{
    private List<List<Plate>> platesList = new();

    private void Awake()
    {
        for(int i=0; i<transform.childCount; i++)
        {
            platesList.Add(new());
            foreach(Plate plate in transform.GetChild(i).GetComponentsInChildren<Plate>())
                platesList[i].Add(plate);
        }
    }

    public List<Plate> GetPlatesList()
    {
        List<Plate> platesList1D = new();
        foreach(var platesLayer in this.platesList)
            foreach(Plate plate in platesLayer)
                platesList1D.Add(plate);
        return platesList1D;
    }

    public int GetPlatesCount()
    {
        return this.GetPlatesList().Count;
    }

    public void DisablePlates()
    {
        foreach(Plate plate in this.GetPlatesList()) 
            plate.gameObject.SetActive(false);
    }

    public IEnumerator AnimateSpawning(float moveTime, Canvas canvas)
    {
        Vector3 targetPos;
        Tween tween;
        foreach(Plate plate in this.GetPlatesList())
        {
            plate.gameObject.SetActive(true);
            targetPos = plate.transform.position;

            Vector3 topPosition = new Vector3(0f, Screen.height, 0f);
            RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.GetComponent<RectTransform>(), topPosition, canvas.worldCamera, out Vector3 worldTopPosition);
            plate.transform.position = new Vector3(plate.transform.position.x, worldTopPosition.y, plate.transform.position.z);

            tween = plate.transform.DOMove(targetPos, moveTime);

            yield return tween.WaitForCompletion();
            moveTime *= 0.9f;
            SoundManager.Instance.PlaySFX(SoundEffect.Place);
        }
    }
}