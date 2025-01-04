using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    public static LevelSpawner Instance {  get; private set; }

    [Header("References")]
    [SerializeField] private GameObject levelHolder;
    [SerializeField] private LevelListSO levelListSO;
    [SerializeField] private Canvas canvas;

    [Header("Parameters")]
    [SerializeField] private float plateMoveTime;
    [SerializeField] private float wallHoleSpawnWaitTime;
    [SerializeField] private float boltMoveTime;

    [Header("Data")]
    private int currentLevel;
    public int CurrentLevel { get => this.currentLevel; }
    private GameObject currentLevelObject;
    private List<GameObject> levelObjectList = new();
    public int AvailableLevel { get; private set; }
    public int MaxLevel { get; private set; }

    // Events
    public event Action onLevelSpawned;

    private void Awake()
    {
        this.SetSingleton();
        this.LoadParameters();
    }

    private void SetSingleton()
    {
        if(LevelSpawner.Instance != null)
            Destroy(this);
        else 
            Instance = this;
    }

    private void LoadParameters()
    {
        this.MaxLevel = this.levelListSO.levelList.Count;
        this.levelObjectList = this.levelListSO.levelList;

        AvailableLevel data = (AvailableLevel) SaveLoad.LoadData(DataTypes.AvailableLevel);
        if (data == null)
        {
            this.AvailableLevel = 1;
            return;
        }

        this.AvailableLevel = data.availableLevel;
    }

    public IEnumerator SpawnLevel(int levelIndex)
    {

        Destroy(this.currentLevelObject);
        yield return null;

        this.currentLevelObject = Instantiate(this.levelObjectList[levelIndex - 1], this.levelHolder.transform);
        yield return null;

        Config.Instance.boltsManager.SetSkins();
        Config.Instance.boardManager.SetSkin();

        yield return StartCoroutine(this.AnimateLevelSpawning());

        this.currentLevel = levelIndex;
        CurrentLevel data = new(this.currentLevel);
        SaveLoad.SaveData(data, DataTypes.CurrentLevel);
        this.onLevelSpawned?.Invoke();
    }

    private IEnumerator AnimateLevelSpawning()
    {
        BoltsManager boltsManager = Config.Instance.boltsManager;
        WallHolesManager wallHolesManager = Config.Instance.wallHolesManager;
        PlatesManager platesManager = Config.Instance.platesManager;
        boltsManager.DisableBolts();
        yield return null;
        wallHolesManager.DisableWallHoles();
        yield return null;
        platesManager.DisablePlates();
        yield return null;

        yield return StartCoroutine(platesManager.AnimateSpawning(this.plateMoveTime, this.canvas));
        yield return StartCoroutine(wallHolesManager.AnimateSpawning(this.wallHoleSpawnWaitTime));
        yield return StartCoroutine(boltsManager.AnimateSpawning(this.boltMoveTime, this.canvas));
    }

    public void CheckUnlockNewLevel()
    {
        if (this.currentLevel + 1 > this.AvailableLevel)
        {
            this.AvailableLevel = this.currentLevel + 1;
            AvailableLevel data = new(this.AvailableLevel);
            SaveLoad.SaveData(data, DataTypes.AvailableLevel);
        }
    }
}
