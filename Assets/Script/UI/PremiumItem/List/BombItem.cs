using UnityEngine;

public class BombItem : PremiumItem
{
    [SerializeField] private BombManager bombManager;

    protected override void OnYesPayment()
    {
        base.OnYesPayment();

        bombManager.ActiveBomb();
    }
}
