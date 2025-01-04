using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LevelListUI : MonoBehaviour
{
    [SerializeField] private LevelListSO levelListSO;
    private int levelCount { get => levelListSO.levelList.Count; }

    [Header("Sprites")]
    [SerializeField] private Sprite levelCompletedSprite;
    [SerializeField] private Sprite levelUnlockSprite;
    [SerializeField] private Sprite levelLockSprite;

    [Header("Holder")]
    [SerializeField] private GameObject levelItemPrefab;
    [SerializeField] private GameObject levelListHolder;

    [Header("Parameters")]
    private List<GameObject> levelItemList = new();
    private int oldAvailableLevel = 0;

    private void SpawnLevelItems()
    {
        TMP_Text indexText;
        for (int i = 1; i <= this.levelCount; i++)
        {
            GameObject levelItem = Instantiate(this.levelItemPrefab, this.levelListHolder.transform);
            this.levelItemList.Add(levelItem);

            ExternInitialized_GameObject[] components = levelItem.GetComponents<ExternInitialized_GameObject>();
            foreach(var component in components)
                component.Initialize();

            indexText = levelItem.GetComponentInChildren<TMP_Text>();
            indexText.text = i.ToString();
        }
    }

    private void OnEnable()
    {
        this.CheckSetIcon();
        this.SetStars();
    }

    private void CheckSetIcon()
    {
        if (this.levelItemList.Count == 0)
            this.SpawnLevelItems();

        AvailableLevel data = (AvailableLevel) SaveLoad.LoadData(DataTypes.AvailableLevel);
        int availableLevel = 1;
        if (data != null)
        {
            if (data.availableLevel > this.levelCount)
                availableLevel = this.levelCount + 1;
            else
                availableLevel = data.availableLevel;
        }

        if (this.oldAvailableLevel == availableLevel)
            return;
        
        this.oldAvailableLevel = availableLevel;
        this.SetIcon();
    }

    private void SetIcon()
    {
        LevelItemUI levelItemUI;
        for (int i=1; i< this.oldAvailableLevel; i++)
        {
            levelItemUI = this.levelItemList[i - 1].GetComponent<LevelItemUI>();
            levelItemUI.SetImage(this.levelCompletedSprite);
            levelItemUI.SetLevelStatus(LevelStatus.unlocking, true);
        }

        if (this.oldAvailableLevel > this.levelItemList.Count) 
            return;

        levelItemUI = this.levelItemList[this.oldAvailableLevel - 1].GetComponent<LevelItemUI>();
        levelItemUI.SetImage(this.levelUnlockSprite);
        levelItemUI.SetLevelStatus(LevelStatus.unlocking, false);

        for (int i= this.oldAvailableLevel + 1; i<= this.levelCount; i++)
        {
            levelItemUI = this.levelItemList[i - 1].GetComponent<LevelItemUI>();
            levelItemUI.SetImage(this.levelLockSprite);
            levelItemUI.SetLevelStatus(LevelStatus.locking, false);
        }
    }

    private void SetStars()
    {
        LevelRank data = (LevelRank) SaveLoad.LoadData(DataTypes.LevelRank);
        if (data == null)
            for (int i = 0; i < this.levelCount; i++)
            {
                StarRank_UI starUI = this.levelItemList[i].GetComponent<StarRank_UI>();
                starUI.ShowStars(RankStar.zero);
            }
        else
        {
            int minCount = Mathf.Min(this.levelCount, data.rankList.Count);

            for(int i=0; i<minCount; i++)
            {
                StarRank_UI starUI = this.levelItemList[i].GetComponent<StarRank_UI>();
                starUI.ShowStars(data.rankList[i]);
            }

            for(int i=minCount-1; i<data.rankList.Count - 1; i++)
            {
                StarRank_UI starUI = this.levelItemList[i].GetComponent<StarRank_UI>();
                starUI.ShowStars(RankStar.zero);
            }
        }
    }
}
