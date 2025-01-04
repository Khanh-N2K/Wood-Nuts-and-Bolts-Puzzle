using UnityEngine;

public class CloseButton : MonoBehaviour
{
    public void OnClose()
    {
        if (PopupManager.Instance != null) 
            PopupManager.Instance.OpenLastPopup();
    }
}
