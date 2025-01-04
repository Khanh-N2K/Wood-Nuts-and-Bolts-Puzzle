using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class NewRecordStamp_UI: MonoBehaviour
{
    [Header("References")]
    private StarRank_Moving_UI starRankUI;
    private Image newRecordStamp;

    [Header("Parameters")]
    [SerializeField] private float recordScaleTime;

    public event Action onDoneAnimation;

    private void Awake()
    {
        this.starRankUI = GetComponent<StarRank_Moving_UI>();
        this.newRecordStamp = transform.Find("Stars").Find("New Record Stamp").GetComponent<Image>();
    }

    public void CheckShowNewRecordMark(bool show)
    {
        this.ShowNewRecordMark(false);

        if (show)
        {
            if (this.IsAllStarsMoving())
                StartCoroutine(this.WaitUntilStarDoneMoving());
            else
                this.ShowNewRecordMark(true);
        }
    }

    private bool IsAllStarsMoving()
    {
        if (this.starRankUI.isStarMoving)
            return true;
        return false;
    }

    private IEnumerator WaitUntilStarDoneMoving()
    {
        while (this.IsAllStarsMoving())
            yield return null;
        this.ShowNewRecordMark(true);
    }

    private void ShowNewRecordMark(bool show)
    {
        if (!show)
            this.newRecordStamp.gameObject.SetActive(false);
        else
        {
            SoundManager.Instance.PlaySFX(SoundEffect.Stamp);

            this.newRecordStamp.gameObject.SetActive(true);
            this.newRecordStamp.transform.localScale = Vector3.one * 1000;

            Color color = this.newRecordStamp.color;
            color.a = 0;
            this.newRecordStamp.color = color;

            Sequence sequence = DOTween.Sequence();
            Tween tween;

            tween = this.newRecordStamp.transform.DOScale(1, this.recordScaleTime);
            sequence.Join(tween);
            tween = this.newRecordStamp.DOFade(1, this.recordScaleTime);
            sequence.Join(tween);

            sequence.AppendCallback(() => this.onDoneAnimation?.Invoke());
            sequence.Play();
        }
    }
}