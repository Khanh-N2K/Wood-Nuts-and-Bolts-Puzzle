using UnityEngine;

public class BackGroundManager : MonoBehaviour
{
    public static BackGroundManager Instance { get; private set; }

    [SerializeField] private BackGroundListSO backGroundList;
    [SerializeField] private RectTransform topBar;
    [SerializeField] private RectTransform bottomBar;

    private SpriteRenderer spriteRenderer;

    private Camera cam;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        cam = Camera.main;
        this.SetInitBackground();
    }

    private void SetInitBackground()
    {
        BackGround data = (BackGround) SaveLoad.LoadData(DataTypes.BackGround);
        if(data == null)
        {
            this.SetBackGround(this.backGroundList.backGroundList[0]);
            return;
        }

        this.SetBackGround(this.backGroundList.backGroundList[data.index]);
    }

    public void SetBackGround(Sprite newBackGround)
    {
        this.spriteRenderer.sprite = newBackGround;
        this.ScaleBackground();
    }

#if UNITY_EDITOR
    private void LateUpdate()
    {
        this.ScaleBackground();
    }
#endif

    public void ScaleBackground()
    {
        float topBarHeightPixels = Utility.GetHeightInPixels(topBar);
        float bottomBarHeightPixels = Utility.GetHeightInPixels(bottomBar);
        float visibleHeightPixel = Screen.height - topBarHeightPixels - bottomBarHeightPixels;
        float visibleAspectRatio = (float)Screen.width / visibleHeightPixel;

        float visibleWidth = 2 * cam.orthographicSize * cam.aspect;
        float visibleHeight = 2 * cam.orthographicSize * cam.aspect / visibleAspectRatio;

        // Vertical offset
        float offsetRatio = (float)(bottomBarHeightPixels - topBarHeightPixels) / Screen.height * 0.5f;
        transform.localPosition = new Vector3(0f, offsetRatio * 2 * cam.orthographicSize, 0f);

        transform.localScale = Vector3.one;

        Bounds backgroundBound = Utility.GetBounds(transform);
        float width = backgroundBound.size.x;
        float height = backgroundBound.size.y;
        float backgroundAspectRatio = (float) width / height;

        float scale;
        if(backgroundAspectRatio > visibleAspectRatio)
        {
            // Fit height
            scale = (float)visibleHeight / height;
        }
        else
        {
            // Fit width
            scale = (float)visibleWidth / width;
        }

        transform.localScale = new Vector3(scale, scale, 1f);
    }
}
