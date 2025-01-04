using UnityEngine;
using UnityEngine.UI;

public class BackgroundList : MonoBehaviour
{
    [SerializeField] private BackGroundListSO backGroundSO;
    private RectTransform checkMark;
    private Transform contentHolder;

    private void Awake()
    {
        this.contentHolder = transform.Find("Content");
        this.checkMark = transform.Find("CheckMark").GetComponent<RectTransform>();
    }

    private void Start()
    {
        foreach (Sprite background in this.backGroundSO.backGroundList)
            this.SpawnBackgroundObj(background);

        this.SetInitCheckMark();
    }

    private void SpawnBackgroundObj(Sprite background)
    {
        GameObject backgroundObj = new GameObject();
        RectTransform rectTransform = backgroundObj.AddComponent<RectTransform>();

        backgroundObj.transform.SetParent(this.contentHolder, false);
        Image image = backgroundObj.AddComponent<Image>();
        image.sprite = background;

        Button button = backgroundObj.AddComponent<Button>();
        button.onClick.AddListener(() => this.OnClick(backgroundObj));
    }

    private void SetInitCheckMark()
    {
        BackGround data = (BackGround) SaveLoad.LoadData(DataTypes.BackGround);
        GameObject backgroundObj;
        if (data == null)
            backgroundObj = this.contentHolder.transform.GetChild(0).gameObject;
        else
            backgroundObj = this.contentHolder.transform.GetChild(data.index).gameObject;

        this.checkMark.gameObject.SetActive(true);
        this.SetCheckMark(backgroundObj);
    }

    public void OnClick(GameObject backgroundObj)
    {
        Effect.Vibrate();
        SoundManager.Instance.PlaySFX(SoundEffect.Pick);

        BackGround data = new(backgroundObj.transform.GetSiblingIndex());
        SaveLoad.SaveData(data, DataTypes.BackGround);

        this.SetCheckMark(backgroundObj);
        BackGroundManager.Instance.SetBackGround(backgroundObj.GetComponent<Image>().sprite);
    }

    private void SetCheckMark(GameObject backgroundObj)
    {
        this.checkMark.transform.SetParent(backgroundObj.transform, false);

        this.checkMark.pivot = new Vector2(1, 0);
        this.checkMark.anchorMin = new Vector2(1, 0);
        this.checkMark.anchorMax = new Vector2(1, 0);
        this.checkMark.anchoredPosition = Vector2.zero;
    }
}
