using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    [SerializeField] private int sceneBuildIndex;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    public void OnClick()
    {
        Effect.Vibrate();
        Effect.PlayButtonSfx();

        SceneManager.LoadScene(this.sceneBuildIndex);
    }
}