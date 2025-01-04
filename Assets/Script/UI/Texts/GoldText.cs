using TMPro;
using UnityEngine;

public class GoldText : MonoBehaviour
{
    private TMP_Text text;

    private void Awake()
    {
        this.text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        GoldManager.Instance.OnGoldChanged += this.UpdateText;
        this.UpdateText();
    }

    private void UpdateText()
    {
        this.text.text = GoldManager.Instance.GoldCount.ToString();
    }
}
