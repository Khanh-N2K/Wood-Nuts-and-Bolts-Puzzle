using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private BoardSkinListSO boardSkinSO;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetSkin()
    {
        BoardSkinData data = (BoardSkinData) SaveLoad.LoadData(DataTypes.BoardSkinData);
        int index;
        if(data == null)
            index = 0;
        else
            index = data.index;
        this.spriteRenderer.sprite = this.boardSkinSO.list[index];
    }
}
