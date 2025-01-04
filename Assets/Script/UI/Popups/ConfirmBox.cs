using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConfirmBox : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text message;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    public void Show(string title, string message, List<UnityAction> yesAction, List<UnityAction> noAction)
    {
        this.title.text = title;
        this.message.text = message;
        this.yesButton.onClick.RemoveAllListeners();
        foreach (UnityAction action in yesAction)
            this.yesButton.onClick.AddListener(action);
        this.noButton.onClick.RemoveAllListeners();
        foreach (UnityAction action in noAction)
            this.noButton.onClick.AddListener(action);
    }

}
