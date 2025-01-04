using UnityEngine;

public class SupportFunctions_UI : MonoBehaviour
{
    public static SupportFunctions_UI Instance {  get; private set; }

    [SerializeField] private GameObject pauseButton;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void ShowSupportFunctions(bool show, bool alsoSetInteract)
    {
        transform.GetChild(0).gameObject.SetActive(show);
        pauseButton.SetActive(show);
        if(alsoSetInteract)
            InputManager.Instance.AllowPickBolt(show);
    }
}
