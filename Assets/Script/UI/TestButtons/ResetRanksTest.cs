using UnityEngine;

public class ResetRanksTest : MonoBehaviour
{
    public void OnClick()
    {
        LevelRank data = (LevelRank)SaveLoad.LoadData(DataTypes.LevelRank);
        data.rankList.Clear();
        SaveLoad.SaveData(data, DataTypes.LevelRank);
    }
}
