using System;
using System.Collections;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance { get; private set; }

    [Header("References")]
    private HolesSpawner holesSpawner;

    [Header("UI")]
    [SerializeField] private GameObject winRoundMenu;
    [SerializeField] private GameObject LoseRoundMenu;

    public event Action OnNewLevel;

    private void Awake()
    {
        this.SetSingleton();
        this.holesSpawner = GetComponentInChildren<HolesSpawner>();
    }

    private void SetSingleton()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        CurrentLevel data = (CurrentLevel) SaveLoad.LoadData(DataTypes.CurrentLevel);
        int startLevel = 1;
        if (data != null)
        {
            if (data.currentLevel >= LevelSpawner.Instance.MaxLevel)
                startLevel = LevelSpawner.Instance.MaxLevel;
            else
                startLevel = data.currentLevel;
        }
        StartCoroutine(this.PrepareNewRound(startLevel));
    }

    public IEnumerator PrepareCurrentRound()
    {
        yield return StartCoroutine(PrepareNewRound(LevelSpawner.Instance.CurrentLevel));
    }
    
    public IEnumerator PrepareNextRound()
    {
        yield return StartCoroutine(PrepareNewRound(LevelSpawner.Instance.CurrentLevel + 1));
    }

    public IEnumerator PrepareNewRound(int index)
    {
        this.OnNewLevel?.Invoke();

        SupportFunctions_UI.Instance.ShowSupportFunctions(false, true);

        if (index > LevelSpawner.Instance.MaxLevel)
            yield return StartCoroutine(LevelSpawner.Instance.SpawnLevel(index - 1));
        else 
            yield return StartCoroutine(LevelSpawner.Instance.SpawnLevel(index));

        RoundTimer.Instance.ResetTimer();
        PlateCounter.Instance.CountNewPlates();
        this.holesSpawner.SpawnPlateHoles();
        StateManager.OnNewRound();
        BoltCounter.CountBolts();
        SupportFunctions_UI.Instance.ShowSupportFunctions(true, true);
        AutoResizeCamera.Instance.AdjustCameraToFitBoard(Config.Instance.boardManager.transform);
    }

    public void WinRound()
    {
        LevelSpawner.Instance.CheckUnlockNewLevel();
        RoundTimer.Instance.PauseCounting();

        PopupManager.Instance.OpenNewPopup(this.winRoundMenu);
        RewardManager.Instance.CheckReward(LevelSpawner.Instance.CurrentLevel, RoundTimer.Instance.TimeLeft);

        SoundManager.Instance.PlaySFX(SoundEffect.OnWin);
    }

    public void LoseRound()
    {
        RoundTimer.Instance.PauseCounting();

        PopupManager.Instance.OpenNewPopup(this.LoseRoundMenu);
        Effect.PlayPopupSfx();
        SoundManager.Instance.PlaySFX(SoundEffect.OnLose);
    }

    private void OnApplicationPause(bool pause)
    {
        if (!pause)
            return;

        CurrentLevel data = new(LevelSpawner.Instance.AvailableLevel);
        SaveLoad.SaveData(data, DataTypes.CurrentLevel);
    }
}
