using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConfirmPaymentBox : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text cost;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    public void Show(string title, float cost, List<UnityAction> yesAction, List<UnityAction> noAction)
    {
        this.title.text = title;
        this.cost.text = cost.ToString();
        this.yesButton.onClick.RemoveAllListeners();
        foreach(UnityAction action in yesAction)
            this.yesButton.onClick.AddListener(action);
        this.noButton.onClick.RemoveAllListeners();
        foreach (UnityAction action in noAction)
            this.noButton.onClick.AddListener(action);
    }

}
