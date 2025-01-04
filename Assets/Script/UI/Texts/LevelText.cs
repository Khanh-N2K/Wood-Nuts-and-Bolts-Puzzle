using TMPro;
using UnityEngine;

public class LevelText : MonoBehaviour
{
    private TMP_Text text;

    private void Start()
    {
        this.text = GetComponent<TMP_Text>();
        LevelSpawner.Instance.onLevelSpawned += this.UpdateText;
        this.UpdateText();
    }

    private void UpdateText()
    {
        string newText = "Level: " + LevelSpawner.Instance.CurrentLevel.ToString();
        this.text.text = newText;
    }
}
