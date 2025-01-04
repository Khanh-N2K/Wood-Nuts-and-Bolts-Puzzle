using UnityEngine;

public class NextRoundButton : MonoBehaviour
{
    public void OnClick()
    {
        PopupManager.Instance.GenerateNextRound();
        PopupManager.Instance.CloseAllPopups();
    }
}
