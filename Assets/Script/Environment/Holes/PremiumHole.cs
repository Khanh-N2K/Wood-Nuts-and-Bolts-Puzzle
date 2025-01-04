using UnityEngine;

public class PremiumHole:MonoBehaviour
{
    [SerializeField] private GameObject inputArea;
    [SerializeField] private GameObject premiumIcon;
    [SerializeField] private int cost;

    public void PayForUsing()
    {
        GoldManager.Instance.UseGold(this.cost);
        this.inputArea.SetActive(true);
        this.premiumIcon.SetActive(false);
        gameObject.SetActive(false);

        PopupManager.Instance.CloseAllPopups();
    }

    private void OnMouseDown()
    {
        if (!InputManager.Instance.IsAllowPickBolt)
            return;

        if (GoldManager.Instance.GoldCount >= this.cost)
            PopupManager.Instance.ShowPaymentConfirmBox("New hole", 1, this.PayForUsing);
        else
            PopupManager.Instance.ShowInformBox("Nuh uh!", "Sorry but you're poor!\nThis premium hole is not for you.");
    }
}