using System;
using System.Collections;
using UnityEngine;

public class HeartBasedBar_DecreasedOverTime : HeartBasedBar
{
    [SerializeField] private float timeToDecrease;

    public event Action onDecreaseUnderMinPoint;

    private void Start()
    {
        StartCoroutine(this.DecreasePointOverTime());
    }

    private IEnumerator DecreasePointOverTime()
    {
        WaitForSeconds waitTime = new WaitForSeconds(timeToDecrease);
        while (true)
        {
            yield return waitTime;
            if (base.CurrentPoint == 0)
                this.onDecreaseUnderMinPoint?.Invoke();
            base.Minus1Point();
        }
    }
}
