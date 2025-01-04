using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartBasedBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    [Header("Parameteres")]
    [SerializeField] private int currentHeart;

    private List<GameObject> heartList = new();

    // Events

    // Read-only properties
    public int CurrentPoint { get => this.currentHeart; }

    private void Awake()
    {
        foreach(Transform child in transform)
        {
            this.heartList.Add(child.gameObject);
        }

        this.UpdateHearthAmount(this.currentHeart);
    }

    private void UpdateHearthAmount(int amount)
    {
        amount = (int)Mathf.Clamp(amount, 0f, this.heartList.Count);

        if (currentHeart > amount)
        {
            transform.DOShakePosition(1f, 20f, 10, 45);
        }

        this.currentHeart = amount;

        for (int i = 0; i < amount; i++)
        {
            this.heartList[i].GetComponent<Image>().sprite = fullHeart;
        }

        for (int i = amount; i < this.heartList.Count; i++)
        {
            this.heartList[i].GetComponent<Image>().sprite = this.emptyHeart;
        }
    }

    public void Minus1Point()
    {
        this.UpdateHearthAmount(this.currentHeart - 1);
    }

    public void RestoreFull()
    {
        this.UpdateHearthAmount(this.heartList.Count);
    }
}
