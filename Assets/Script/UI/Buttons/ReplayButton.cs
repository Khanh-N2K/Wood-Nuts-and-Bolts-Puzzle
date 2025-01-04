using UnityEngine;

public class ReplayButton : MonoBehaviour
{
    public void OnClick()
    {
        PopupManager.Instance.ShowConfirmBox("Replay", "Are you sure?", this.OnYesAction);
    }

    private void OnYesAction()
    {
        PopupManager.Instance.GenerateLastRound();
        PopupManager.Instance.CloseAllPopups();
    }
}
