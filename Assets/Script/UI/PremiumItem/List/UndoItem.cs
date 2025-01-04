using UnityEngine;
using UnityEngine.UI;

public class UndoItem : PremiumItem
{
    private CanvasGroup canvasGroup;
    private Button button;

    private void Awake()
    {
        this.canvasGroup = GetComponent<CanvasGroup>();
        this.button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        StateManager.onStateCountChanged.ClearAllListeners();
        StateManager.onStateCountChanged.AddListener(this.CheckShowItem);
    }

    private void CheckShowItem()
    {
        if (StateManager.StateCount > 0)
        {
            this.canvasGroup.alpha = 1;
            this.button.interactable = true;
        }
        else
        {
            this.canvasGroup.alpha = 0.7f;
            this.button.interactable = false;
        }
    }

    protected override void OnYesPayment()
    {
        base.OnYesPayment();

        Undo.Instance.OnUndo();
    }
}
