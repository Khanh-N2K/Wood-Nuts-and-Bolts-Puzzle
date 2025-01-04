using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BoltModel : MonoBehaviour
{
    public SpriteRenderer defaultModel { get; private set; }
    private SpriteRenderer fullModel;

    [Header("Parameters")]
    [SerializeField] private float pickTime;        // seconds
    [SerializeField] private float highlightTime;   // seconds

    private Color originalColor;

    private void Awake()
    {
        this.defaultModel = transform.Find("Default").GetComponent<SpriteRenderer>();
        this.fullModel = transform.Find("FullBolt").GetComponent<SpriteRenderer>();
        this.originalColor = this.defaultModel.color;
    }

    public void AnimatePicking(bool picking)
    {
        if (!DOTween.IsTweening(transform.parent))
        {
            this.MoveVertical(picking);
            return;
        }

        StartCoroutine(this.WaitParentMovementFinished(picking));
    }

    private IEnumerator WaitParentMovementFinished(bool picking)
    {
        while(DOTween.IsTweening(transform.parent))
            yield return null;

        this.MoveVertical(picking);
    }

    private void MoveVertical(bool picking)
    {
        transform.DOKill();
        this.defaultModel.DOKill();
        this.defaultModel.transform.DOKill();
        this.fullModel.DOKill();
        this.fullModel.transform.DOKill();

        if (picking)
        {
            transform.DOMoveY(transform.parent.position.y + 0.3f, this.pickTime);
            this.fullModel.transform.DOScaleY(1, this.pickTime);
            this.defaultModel.transform.DOLocalMoveY(0.07f, this.pickTime * 0.1f);
            this.defaultModel.DOFade(0f, this.pickTime * 1.5f);
            this.defaultModel.transform.DOScaleY(0.8f, this.pickTime);
        }
        else
        {
            transform.DOMoveY(transform.parent.position.y, this.pickTime);
            this.fullModel.transform.DOScaleY(0, this.pickTime * 1.5f);
            this.defaultModel.DOFade(1, this.pickTime * 0.5f);
            this.defaultModel.transform.DOScaleY(1f, this.pickTime * 0.1f);
            this.defaultModel.transform.DOLocalMoveY(0, this.pickTime);
        }
    }

    public void Highlight(bool highlight)
    {
        if (highlight)
        {
            this.defaultModel.DOBlendableColor(Color.red, this.highlightTime)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.Linear);
        }
        else
        {
            this.defaultModel.DOKill();
            this.defaultModel.color = this.originalColor;
        }
    }

    public void ChangeSkin(BoltSkin skin)
    {
        this.defaultModel.sprite = skin.defaultSkin;
        this.fullModel.sprite = skin.fullSkin;
    }
}
