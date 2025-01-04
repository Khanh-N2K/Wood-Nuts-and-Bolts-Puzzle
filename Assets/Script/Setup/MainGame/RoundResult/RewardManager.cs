using UnityEngine;
using UnityEngine.Events;

public class RewardManager: MonoBehaviour
{
    private static RewardManager instance;
    public static RewardManager Instance { get => instance; }

    private LevelRank data;
    public UnityEvent<RankStar, bool> onGetReward = new();

    [SerializeField] private ExternInitialized_GameObject rewardUI;

    private void Awake()
    {
        this.SetSingleton();
        this.SetParameters();
    }

    private void SetSingleton()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void SetParameters()
    {
        LevelRank savedData = (LevelRank) SaveLoad.LoadData(DataTypes.LevelRank);
        if(savedData == null)
        {
            savedData = new LevelRank();
        }
        this.data = savedData;

        rewardUI.Initialize();
    }

    public void CheckReward(int level, int timeLeft)
    {
        int totalTime = RoundTimer.Instance.LevelTime;
        int timeTaken = totalTime - timeLeft;
        float percentageOfTimeUsed = (float) timeTaken / totalTime;

        RankStar newRank;
        if (percentageOfTimeUsed <= 0.5f)
            newRank = RankStar.three;
        else if (percentageOfTimeUsed <= 0.75f)
            newRank = RankStar.two;
        else
            newRank = RankStar.one;

        RankStar oldRank = data.GetRank(level);
        if (oldRank < newRank)
        {
            this.data.UpdateRank(level, newRank);
            SaveLoad.SaveData(data, DataTypes.LevelRank);
            GoldManager.Instance.AddGold((int)newRank);
            this.onGetReward?.Invoke(newRank, true);
        }
        else
            this.onGetReward?.Invoke(newRank, false);
    }
}
