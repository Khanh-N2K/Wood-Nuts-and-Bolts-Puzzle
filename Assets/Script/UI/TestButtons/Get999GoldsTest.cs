using UnityEngine;

public class Get999GoldsTest : MonoBehaviour
{
    public void OnClick()
    {
        Gold data = new(999);
        SaveLoad.SaveData(data, DataTypes.Gold);
    }
}
