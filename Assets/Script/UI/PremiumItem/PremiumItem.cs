using UnityEngine;

public class PremiumItem : MonoBehaviour
{
    [SerializeField] private int cost;
    [SerializeField] private string itemName;

    public void OnUseItem()
    {
        Effect.PlayButton2Sfx();
        Effect.Vibrate();

        if (GoldManager.Instance.GoldCount >= this.cost)
            PopupManager.Instance.ShowPaymentConfirmBox(this.itemName, this.cost, this.OnYesPayment);
        else
            PopupManager.Instance.ShowInformBox("Nuh uh!", "Sorry but you're poor!\nThis function is not for you.");
    }

    protected virtual void OnYesPayment()
    {
        GoldManager.Instance.UseGold(this.cost);
        PopupManager.Instance.OpenLastPopup();
    }
}
