using System.Collections;
using TMPro;
using UnityEngine;

public class RoundTimer : MonoBehaviour
{
    private static RoundTimer instance;
    public static RoundTimer Instance { get =>  instance; }

    private RoundManager roundManger;

    [Header("--- UI ---")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private GameObject clockImage;

    [Header("--- Params ---")]
    [SerializeField] private int roundTimeSecond;
    public int LevelTime { get => this.roundTimeSecond; }
    [SerializeField] private int warningSeconds;

    [Header("Shaking clock")]
    [SerializeField] private float shakeDegree;
    [SerializeField] private float shakeDuration;
    [SerializeField] private bool isShaking = false;

    [Header("Count down")]
    private int timeLeft;
    public int TimeLeft { get => this.timeLeft; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        this.roundManger = transform.parent.GetComponent<RoundManager>();
    }

    public void ResetTimer()
    {
        StopAllCoroutines();
        StartCoroutine(this.CountDown(this.roundTimeSecond));
    
        this.clockImage.SetActive(false);
        this.isShaking = false;
    }

    public void PauseCounting()
    {
        StopAllCoroutines();
        SoundManager.Instance.StopSFX();

        this.isShaking = false;
    }

    public void ContinueCounting()
    {
        StartCoroutine(this.CountDown(this.timeLeft));
    }

    private IEnumerator CountDown(int time)
    {
        this.timeLeft = time;
        while(this.timeLeft >= 0f)
        {
            this.UpdateTimerUI(this.timeLeft);
            if(this.timeLeft <= this.warningSeconds && !this.isShaking)
            {
                this.clockImage.SetActive(true);
                StartCoroutine(this.ShakeClock());
            }

            yield return new WaitForSeconds(1f);
            this.timeLeft--;
        }

        this.roundManger.LoseRound();
        this.clockImage.SetActive(false);
    }

    private void UpdateTimerUI(int timeLeft)
    {
        int min = this.timeLeft / 60;
        int sec = this.timeLeft % 60;
        string minText = (min < 10) ? "0" + min.ToString() : min.ToString();
        string secText = (sec < 10) ? "0" + sec.ToString() : sec.ToString();
        this.timerText.text = minText + " : " + secText;
    }

    private IEnumerator ShakeClock()
    {
        SoundManager.Instance.PlaySFX(SoundEffect.ClockTicking);
        this.isShaking = true;
        while(true)
        {
            yield return StartCoroutine(this.RotateClockToAngle(this.shakeDegree));
            yield return StartCoroutine(this.RotateClockToAngle(-this.shakeDegree));
        }
    }

    private IEnumerator RotateClockToAngle(float degree)
    {
        Quaternion startRotation = this.clockImage.transform.rotation;
        Quaternion endRotation = Quaternion.Euler(0, 0, degree) * Quaternion.Euler(0, 0, 0);
        float elapsedTime = 0f;

        while(elapsedTime <this.shakeDuration/2) 
        {
            this.clockImage.transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime/(shakeDuration/2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        this.clockImage.transform.rotation = endRotation;
    }
}
