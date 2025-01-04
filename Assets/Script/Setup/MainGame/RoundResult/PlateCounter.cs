using UnityEngine;

public class PlateCounter : MonoBehaviour
{
    public static PlateCounter Instance {  get; private set; }

    private RoundManager roundManager;

    [Header("Parameters")]
    [SerializeField] private float delayWinTime;
    public int PlateCount {  get; private set; }

    private void Awake()
    {
        roundManager = transform.parent.GetComponent<RoundManager>();

        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void CountNewPlates()
    {
        this.PlateCount = Config.Instance.platesManager.GetPlatesCount();
    }

    public void UpdatePlateCount(int newCount)
    {
        if(newCount < 0)
        {
            Debug.LogError("Plate count some how < 0???!");
            return;
        }

        this.PlateCount = newCount;

        if(newCount == 0)
        {
            this.roundManager.WinRound();
            return;
        }
    }
}
