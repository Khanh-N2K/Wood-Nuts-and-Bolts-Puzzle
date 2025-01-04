using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InformBox : MonoBehaviour
{
    [SerializeField] private TMP_Text messageTitle;
    [SerializeField] private TMP_Text messageBody;
    [SerializeField] private Button _OK_Button;

    public void Show(string title, string body, UnityAction _OK_Action)
    {
        this.messageTitle.text = title;
        this.messageBody.text = body;
        this._OK_Button.onClick.RemoveAllListeners();
        this._OK_Button.onClick.AddListener(_OK_Action);
    }
}
