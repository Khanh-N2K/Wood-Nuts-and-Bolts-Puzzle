using System.Collections;
using UnityEngine;

public class AutoHidePopup : MonoBehaviour
{
    [SerializeField] protected float showTime;

    protected virtual void OnEnable()
    {
        StartCoroutine(this.HidePopupAfterSeconds(this.showTime));
    }

    private IEnumerator HidePopupAfterSeconds(float showTime)
    {
        yield return new WaitForSeconds(showTime);
        gameObject.SetActive(false);
    }
}
