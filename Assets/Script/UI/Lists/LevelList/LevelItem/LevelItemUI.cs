using UnityEngine;
using UnityEngine.UI;

public class LevelItemUI : ExternInitialized_GameObject
{
    protected LevelStatus levelStatus;

    private GameObject starObjects;

    public override void Initialize()
    {
        this.starObjects = transform.Find("Stars").gameObject;
    }

    public void SetLevelStatus(LevelStatus newStatus, bool isCompletedOnce)
    {
        this.levelStatus = newStatus;

        if (isCompletedOnce)
            this.starObjects.SetActive(true);
        else
            this.starObjects.SetActive(false);
    }

    public void OnClick()
    {
        Effect.Vibrate();

        if (this.levelStatus != LevelStatus.unlocking)
        {
            PopupManager.Instance.ShowInformBox("Nah", "You're not ready for this level yet!");
            return;
        }

        PopupManager.Instance.ShowConfirmBox("Level " + ((int) transform.GetSiblingIndex() + 1).ToString(), "Play this level?", this.GenerateLevel);
    }

    protected virtual void GenerateLevel()
    {
        PopupManager.Instance.GenerateChoosedLevel(transform.GetSiblingIndex() + 1);

        PopupManager.Instance.CloseAllPopups();
    }

    public void SetImage(Sprite sprite)
    {
        GetComponent<Image>().sprite = sprite;
    }
}
