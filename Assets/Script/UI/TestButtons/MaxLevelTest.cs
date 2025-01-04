using UnityEngine;

public class MaxLevelTest : MonoBehaviour
{
    [SerializeField] private LevelListSO levelListSO;

    public void OnClick()
    {

        AvailableLevel data = new(this.levelListSO.levelList.Count);
        SaveLoad.SaveData(data, DataTypes.AvailableLevel);
    }
}
