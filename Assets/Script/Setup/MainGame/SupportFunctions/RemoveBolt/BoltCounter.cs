using UnityEngine;

public static class BoltCounter
{
    public static int BoltCount { get; private set; }

    public static CustomEvent onBoltCountChanged = new();

    public static void CountBolts()
    {
        BoltCount = Config.Instance.boltsManager.boltsList.Count;
        onBoltCountChanged?.Invoke();
    }

    public static void UpdateCount(int amount)
    {
        BoltCount += amount;
        onBoltCountChanged?.Invoke();

        if (BoltCount < 0)
            Debug.LogError("Bolt count < 0???!");
    }
}
