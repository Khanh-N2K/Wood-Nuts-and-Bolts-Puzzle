using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltsManager : MonoBehaviour
{
    [SerializeField] private BoltSkinListSO boltSkinSO;

    public List<Bolt> boltsList { get; private set; } = new();

    private void Awake()
    {
        foreach (Bolt bolt in transform.GetComponentsInChildren<Bolt>())
            this.boltsList.Add(bolt);
    }

    public void HighLightBolts(bool highLight)
    {
        foreach(Bolt bolt in this.boltsList)
            bolt.Model.Highlight(highLight);
    }

    public void DisableBolts()
    {
        foreach(Bolt bolt in this.boltsList)
            bolt.gameObject.SetActive(false);
    }

    public IEnumerator AnimateSpawning(float moveTime, Canvas canvas)
    {
        Vector3 targetPos;
        Tween tween;
        int hitSound;
        foreach (Bolt bolt in this.boltsList)
        {
            bolt.gameObject.SetActive(true);
            targetPos = bolt.transform.position;

            Vector3 topPosition = new Vector3(0f, Screen.height, 0f);
            RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.GetComponent<RectTransform>(), topPosition, canvas.worldCamera, out Vector3 worldTopPosition);
            bolt.transform.position = new Vector3(bolt.transform.position.x, worldTopPosition.y, bolt.transform.position.z);

            tween = bolt.transform.DOMove(targetPos, moveTime);

            yield return tween.WaitForCompletion();
            moveTime *= 0.9f;
            hitSound = Random.Range(1, 4);
            if (hitSound == 1)
                SoundManager.Instance.PlaySFX(SoundEffect.Hit1);
            else if (hitSound == 2)
                SoundManager.Instance.PlaySFX(SoundEffect.Hit2);
            else
                SoundManager.Instance.PlaySFX(SoundEffect.Hit3);
        }
    }

    public void SetSkins()
    {
        BoltSkinData data = (BoltSkinData)SaveLoad.LoadData(DataTypes.BoltSkinData);
        BoltSkin skin;
        if (data == null)
            skin = this.boltSkinSO.list[0];
        else
            skin = this.boltSkinSO.list[data.index];

        BoltModel boltModel;
        foreach(Bolt bolt in this.boltsList)
        {
            boltModel = bolt.Model;
            boltModel.ChangeSkin(skin);
        }
    }
}
