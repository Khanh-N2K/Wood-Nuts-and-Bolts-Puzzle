using UnityEngine;
using UnityEngine.UI;

public class RemoveBoltItem : PremiumItem
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
        BoltCounter.onBoltCountChanged.ClearAllListeners();
        BoltCounter.onBoltCountChanged.AddListener(this.CheckshowItem);
    }

    private void CheckshowItem()
    {
        if (BoltCounter.BoltCount > 0)
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

        BoltRemoval.OnRemoveBolt();
    }
}
