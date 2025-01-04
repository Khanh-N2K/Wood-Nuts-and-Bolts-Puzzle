using UnityEngine;

public class Config:MonoBehaviour
{
    public static Config Instance { get; private set; }

    [Header("Layers")]
    [SerializeField] private LayerMask boltLayerMask;
    public LayerMask BoltLayerMask { get => boltLayerMask; }

    [SerializeField] private LayerMask wallHoleLayerMask;
    public LayerMask WallHoleLayerMask { get => wallHoleLayerMask; }

    [SerializeField] private LayerMask plateHoleLayerMask;
    public LayerMask PlateHoleLayerMask { get => plateHoleLayerMask; }

    [SerializeField] private LayerMask platesLayerMask;
    public LayerMask PlatesLayerMask { get => platesLayerMask; }

    [SerializeField] private LayerMask ignoreCollisionLayerMask;
    public LayerMask IgnoreCollisionLayerMask { get => ignoreCollisionLayerMask; }

    [Header("Values")]
    [SerializeField] private float holesMinThreshold;
    public float HolesMinThreshold { get => holesMinThreshold; }

    [Header("Holder tag")]
    [SerializeField] private string boardHolderTag;
    [SerializeField] private string wallHolesHolderTag;
    [SerializeField] private string boltsHolderTag;
    [SerializeField] private string platesHolderTag;

    //Holder
    public GameObject boltHolder { get => GameObject.FindWithTag(this.boltsHolderTag); }
    public GameObject wallHoleHolder { get => GameObject.FindWithTag(this.wallHolesHolderTag); }

    // Managers
    public BoardManager boardManager { get => GameObject.FindWithTag(this.boardHolderTag).GetComponent<BoardManager>(); }
    public BoltsManager boltsManager { get => GameObject.FindWithTag(this.boltsHolderTag).GetComponent<BoltsManager>(); }
    public WallHolesManager wallHolesManager { get => GameObject.FindWithTag(this.wallHolesHolderTag).GetComponent<WallHolesManager>(); }
    public PlatesManager platesManager { get => GameObject.FindWithTag(this.platesHolderTag).GetComponent<PlatesManager>(); }
    
    private void Awake()
    {
        this.SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public static bool IsLayerInMask(GameObject gameObject, LayerMask layerMask)
    {
        return (layerMask.value & (1 << gameObject.layer)) != 0;
    }

    public static int GetSingleLayerFromLayerMask(LayerMask layerMask)
    {
        int mask = layerMask.value;
        if (mask == 0) return -1;

        int layer = 0;
        while (mask > 1)
        {
            mask = mask >> 1;
            layer++;
        }
        return layer;
    }
}

public enum LevelStatus
{
    locking,
    unlocking
}