using UnityEngine;
using UnityEngine.UI;

public class BoardSkinsList_UI : MonoBehaviour
{
    [SerializeField] private BoardSkinListSO boardSkinSO;
    private RectTransform checkMark;
    private Transform contentHolder;

    private void Awake()
    {
        this.contentHolder = transform.Find("Content");
        this.checkMark = transform.Find("CheckMark").GetComponent<RectTransform>();
    }

    private void Start()
    {
        foreach (Sprite board in this.boardSkinSO.list)
            this.SpawnBoardObj(board);

        this.SetInitCheckMark();
    }

    private void SpawnBoardObj(Sprite board)
    {
        GameObject boardObj = new();
        RectTransform rectTransform = boardObj.AddComponent<RectTransform>();

        boardObj.transform.SetParent(this.contentHolder, false);
        Image image = boardObj.AddComponent<Image>();
        image.sprite = board;

        Button button = boardObj.AddComponent<Button>();
        button.onClick.AddListener(() => this.OnClick(boardObj));
    }

    private void SetInitCheckMark()
    {
        BoardSkinData data = (BoardSkinData)SaveLoad.LoadData(DataTypes.BoardSkinData);
        GameObject boardObj;
        if (data == null)
            boardObj = this.contentHolder.transform.GetChild(0).gameObject;
        else
            boardObj = this.contentHolder.transform.GetChild(data.index).gameObject;

        this.checkMark.gameObject.SetActive(true);
        this.SetCheckMark(boardObj);
    }
    private void OnClick(GameObject boardObj)
    {
        Effect.Vibrate();
        SoundManager.Instance.PlaySFX(SoundEffect.Pick);

        BoardSkinData data = new(boardObj.transform.GetSiblingIndex());
        SaveLoad.SaveData(data, DataTypes.BoardSkinData);

        this.SetCheckMark(boardObj);
    }

    private void SetCheckMark(GameObject boardObj)
    {
        this.checkMark.transform.SetParent(boardObj.transform, false);

        this.checkMark.pivot = new Vector2(1, 0);
        this.checkMark.anchorMin = new Vector2(1, 0);
        this.checkMark.anchorMax = new Vector2(1, 0);
        this.checkMark.anchoredPosition = Vector2.zero;
    }
}
