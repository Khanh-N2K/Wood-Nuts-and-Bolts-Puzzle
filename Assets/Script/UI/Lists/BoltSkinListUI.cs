using UnityEngine;
using UnityEngine.UI;

public class BoltSkinListUI : MonoBehaviour
{
    [SerializeField] private BoltSkinListSO boltSkinsSO;
    private RectTransform checkMark;
    private Transform contentHolder;

    private void Awake()
    {
        this.contentHolder = transform.Find("Content");
        this.checkMark = transform.Find("CheckMark").GetComponent<RectTransform>();
    }

    private void Start()
    {
        foreach(BoltSkin boltSkin in this.boltSkinsSO.list)
            this.SpawnBoltSkinObj(boltSkin);

        this.SetInitCheckMark();
    }

    private void SpawnBoltSkinObj(BoltSkin boltSkin)
    {
        GameObject skinObj = new GameObject();
        RectTransform rectTransform = skinObj.AddComponent<RectTransform>();

        skinObj.transform.SetParent(this.contentHolder, false);
        Image image = skinObj.AddComponent<Image>();
        image.sprite = boltSkin.defaultSkin;

        // Re-scale
        Vector2 slotSize = GetComponentInChildren<GridLayoutGroup>().cellSize;

        float spriteWidth = boltSkin.defaultSkin.rect.width;
        float spriteHeight = boltSkin.defaultSkin.rect.height;

        float scaleX = spriteWidth / slotSize.x;
        float scaleY = spriteHeight / slotSize.y;

        if (scaleX > scaleY)
            rectTransform.localScale = new Vector3(1, spriteHeight / spriteWidth, 1);
        else if (scaleY > scaleX)
            rectTransform.localScale = new Vector3(spriteWidth / spriteHeight, 1, 1);

        // Add button
        Button button = skinObj.AddComponent<Button>();
        button.onClick.AddListener(() => this.OnClick(skinObj));
    }

    private void SetInitCheckMark()
    {
        BoltSkinData data = (BoltSkinData) SaveLoad.LoadData(DataTypes.BoltSkinData);
        GameObject boltSkinObj;
        if (data == null)
            boltSkinObj = this.contentHolder.transform.GetChild(0).gameObject;
        else
            boltSkinObj = this.contentHolder.transform.GetChild(data.index).gameObject;

        this.checkMark.gameObject.SetActive(true);
        this.SetCheckMark(boltSkinObj);
    }

    public void OnClick(GameObject boltSkinObj)
    {
        Effect.Vibrate();
        SoundManager.Instance.PlaySFX(SoundEffect.Pick);

        this.SetCheckMark(boltSkinObj);

        BoltSkinData data = new(boltSkinObj.transform.GetSiblingIndex());
        SaveLoad.SaveData(data, DataTypes.BoltSkinData);
    }

    private void SetCheckMark(GameObject boltSkinObj)
    {
        this.checkMark.transform.SetParent(boltSkinObj.transform, false);

        this.checkMark.pivot = new Vector2(1, 0);
        this.checkMark.anchorMin = new Vector2(1, 0);
        this.checkMark.anchorMax = new Vector2(1, 0);
        this.checkMark.anchoredPosition = Vector2.zero;
    }
}
