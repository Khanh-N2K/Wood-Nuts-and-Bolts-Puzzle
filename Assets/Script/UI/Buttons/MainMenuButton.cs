using UnityEngine;

public class MainMenuButton : MonoBehaviour
{
    public void OnClick()
    {
        PopupManager.Instance.ShowConfirmBox("Main Menu", "Back to main menu?", this.OnYesAction);
    }

    private void OnYesAction()
    {
        PopupManager.Instance.CloseAllPopups();
        PopupManager.Instance.ReturnMainMenu();
    }
}
