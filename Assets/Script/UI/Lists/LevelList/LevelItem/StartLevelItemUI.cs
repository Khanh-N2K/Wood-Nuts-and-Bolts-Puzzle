using UnityEngine.SceneManagement;

public class StartLevelItemUI : LevelItemUI
{
    protected override void GenerateLevel()
    {
        CurrentLevel data = new(transform.GetSiblingIndex() + 1);
        SaveLoad.SaveData(data, DataTypes.CurrentLevel);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
