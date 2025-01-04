using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AutoResizeCamera : MonoBehaviour
{
    public static AutoResizeCamera Instance { get; private set; }

    [SerializeField] private Transform sampleBoard;
    [SerializeField] private RectTransform topBar;
    [SerializeField] private RectTransform bottomBar;

    private Camera cam;
    private float minAspectRatio;
    private float minWidth;
    private float minHeight;

    private float topBarHeightPixels;
    private float bottomBarHeightPixels;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    void Start()
    {
        cam = GetComponent<Camera>();
        this.AdjustCameraToFitBoard(this.sampleBoard);
    }

#if UNITY_EDITOR
    void Update()
    {
        AdjustCamera();
    }
#endif

    public void AdjustCameraToFitBoard(Transform board)
    {
        this.sampleBoard = board;
        this.SetParameters(board);
        this.AdjustCamera();
        BackGroundManager.Instance?.ScaleBackground();
    }

    private void SetParameters(Transform board)
    {
        Bounds boardBounds = Utility.GetBounds(board);
        minWidth = boardBounds.size.x * 1.2f;
        minHeight = boardBounds.size.y * 1.2f;
        this.minAspectRatio = (float)this.minWidth / this.minHeight;
    }

    private void AdjustCamera()
    {
        this.topBarHeightPixels = Utility.GetHeightInPixels(topBar);
        this.bottomBarHeightPixels = Utility.GetHeightInPixels(bottomBar);
        float visibleHeight = Screen.height - topBarHeightPixels - bottomBarHeightPixels;
        float visibleAspectRatio = (float) Screen.width / visibleHeight;

        Rect rect = cam.rect;

        if (visibleAspectRatio > minAspectRatio)
            this.cam.orthographicSize = (float)minHeight * visibleAspectRatio * rect.height / 2 / this.cam.aspect;
        else
            this.cam.orthographicSize = (float)minWidth / 2 / this.cam.aspect;
        cam.rect = rect;
    }
}