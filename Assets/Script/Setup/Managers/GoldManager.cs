using System;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance { get; private set; }
    public event Action OnGoldChanged;

    public int GoldCount { get; private set; }

    private void Awake()
    {
        this.SetSingleton();
        this.LoadParameters();
    }

    private void SetSingleton()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void LoadParameters()
    {
        Gold data = (Gold)SaveLoad.LoadData(DataTypes.Gold);
        if (data == null)
            this.GoldCount = 0;
        else
            this.GoldCount = data.goldCount;
    }

    public void AddGold(int amount)
    {
        this.GoldCount += amount;
        this.SaveGold();
    }

    public bool UseGold(int amount)
    {
        if (this.GoldCount < amount)
            return false;

        this.GoldCount -= amount;
        this.SaveGold();
        return true;
    }

    private void SaveGold()
    {
        Gold data = new(this.GoldCount);
        SaveLoad.SaveData(data, DataTypes.Gold);
        this.OnGoldChanged?.Invoke();
    }
}
