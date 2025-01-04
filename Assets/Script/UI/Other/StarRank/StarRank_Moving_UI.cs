using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarRank_Moving_UI : StarRank_UI
{
    [Header("Root Canvas")]
    private RectTransform rootCanvas;

    [Header("Parameters")]
    [SerializeField] private float moveTime;

    [Header("State")]
    public bool isStarMoving = false;
    private Queue<Image> pendingQueue = new();

    public override void Initialize()
    {
        base.Initialize();

        this.rootCanvas = Utility.GetRootCanvas(transform);
    }

    private Vector3 GetStartPosition(RectTransform starShowingPosition)
    {
        Vector3[] starCorners = new Vector3[4];
        starShowingPosition.GetWorldCorners(starCorners);
        Vector3 starTopCenter = (starCorners[1] + starCorners[2]) / 2;

        Vector3[] canvasCorners = new Vector3[4];
        this.rootCanvas.GetWorldCorners(canvasCorners);
        Vector3 canvasTopCenter = (canvasCorners[1] + canvasCorners[2]) / 2;

        float offset = starShowingPosition.rect.height / 2;
        Vector3 newPosition = new Vector3(starTopCenter.x, canvasTopCenter.y + offset, starTopCenter.z);

        return newPosition;
    }

    protected override void DisplayStar(Image star, bool show)
    {
        base.DisplayStar(star, false);
        if (!show)
            return;

        this.pendingQueue.Enqueue(star);

        if (this.isStarMoving)
            return;

        this.isStarMoving = true;
        if (PopupManager.Instance.isCurrentPopupMoving)
            StartCoroutine(this.WaitUntilWinScreenDoneMoving(star));
        else
            this.MoveStar(pendingQueue.Dequeue());
    }

    private IEnumerator WaitUntilWinScreenDoneMoving(Image star)
    {
        while(PopupManager.Instance.isCurrentPopupMoving)
            yield return null;
        this.MoveStar(this.pendingQueue.Dequeue());
    }

    private void MoveStar(Image star)
    {
        GameObject starClone = Instantiate(star.gameObject, base.starsHolder.transform);
        base.DisplayStar(starClone.GetComponent<Image>(), true);
        starClone.transform.position = this.GetStartPosition(star.rectTransform);

        Tween currentTween = starClone.transform.DOMove(star.transform.position, this.moveTime);
        currentTween.OnComplete(() =>
            {
                base.DisplayStar(star, true);
                Destroy(starClone);

                if (this.pendingQueue.Count == 0)
                    this.isStarMoving = false;
                else
                    MoveStar(this.pendingQueue.Dequeue());
            });
    }
}
