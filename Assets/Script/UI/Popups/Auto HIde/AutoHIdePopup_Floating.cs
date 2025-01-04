using DG.Tweening;
using UnityEngine;

public class AutoHIdePopup_Floating : AutoHidePopup
{
    [Header("Parameters")]
    [SerializeField] private float floatHeight;
    [SerializeField] private Ease easeType;

    private Vector3 localOriginalPos;

    private void Start()
    {
        this.localOriginalPos = transform.localPosition;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        transform.DOLocalMoveY(this.floatHeight, base.showTime)
            .SetEase(this.easeType);
    }

    private void OnDisable()
    {
        transform.localPosition = localOriginalPos;
    }
}
