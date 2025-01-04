using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get ; private set; }

    [Header("References")]
    [SerializeField] private Image blur;
    [SerializeField] private GameObject confirmPaymentBoxPrefab;
    [SerializeField] private GameObject confirmBoxPrefab;
    [SerializeField] private GameObject SpawnedPopupHolder;
    [SerializeField] private GameObject informPopupPrefab;

    [SerializeField] private Transform startShowPos;
    [SerializeField] private Transform endShowPos;
    [SerializeField] private Transform showPos;
    private RoundTimer timer;

    [Header("Parameters")]
    private float blurAlpha = 101f;
    private float popupMoveTime = 0.5f;

    private GameObject currentPopup;
    public bool isCurrentPopupMoving { get => DOTween.IsTweening(currentPopup.transform); }

    private Stack<GameObject> popupStack = new();
    private Stack<GameObject> confirmPaymentBoxStack = new();
    private Stack<GameObject> confirmBoxStack = new();
    private Stack<GameObject> informStack = new();

    private void Awake()
    {
        this.SetSingleton();
        this.LoadReferences();
    }

    private void SetSingleton()
    {
        if(PopupManager.Instance != null)
            Destroy(this);
        else
            Instance = this;
    }

    private void LoadReferences()
    {
        this.timer = transform.parent.GetComponentInChildren<RoundTimer>();
    }

    #region === POPUP MENU ===

    public void OpenNewPopup(GameObject popup)
    {
        this.PlayButtonEffects();
        Effect.PlayPopupSfx();
        InputManager.Instance?.AllowPickBolt(false);

        if(this.currentPopup != null)
        {
            this.ShowPopup(this.currentPopup, false);
            this.popupStack.Push(this.currentPopup);
        }

        this.currentPopup = popup;
        this.ShowPopup(this.currentPopup, true);
    }

    private void ShowPopup(GameObject popup, bool show)
    {
        if(show)
        {
            popup.transform.position = this.startShowPos.position;
            popup.SetActive(true);
            popup.transform.DOKill();
            popup.transform.DOMove(this.showPos.position, this.popupMoveTime);

            this.blur.gameObject.SetActive(true);
            this.blur.DOKill();
            this.blur.DOFade((float) this.blurAlpha / 255, this.popupMoveTime);
        } 
        else
        {
            popup.transform.DOKill();
            popup.transform.DOMove(this.endShowPos.position, this.popupMoveTime)
                .OnComplete(() =>
                {
                    popup.SetActive(false);
                });

            this.blur.DOKill();
            this.blur.DOFade(0, this.popupMoveTime)
                .OnComplete(() =>
                {
                    this.blur.gameObject.SetActive(false);
                });
        }
    }

    public void OpenLastPopup()
    {
        if (this.currentPopup != null)
        {
            this.PlayButtonEffects();
            this.ShowPopup(this.currentPopup, false);
        }

        if(this.popupStack.Count>0)
        {
            this.PlayButtonEffects();

            this.currentPopup = this.popupStack.Pop();
            this.ShowPopup(this.currentPopup, true);
        }
        else
        {
            this.currentPopup = null;
            InputManager.Instance?.AllowPickBolt(true);
        }
    }

    public void CloseAllPopups()
    {
        this.PlayButtonEffects();
        InputManager.Instance?.AllowPickBolt(true);

        this.ShowPopup(this.currentPopup, false);
        this.currentPopup = null;
        this.popupStack.Clear();
    }

    private void PlayButtonEffects()
    {
        Effect.Vibrate();
        Effect.PlayButtonSfx();
    }

    public void ShowPaymentConfirmBox(string title, float cost, UnityAction yesAction)
    {
        GameObject confirmPaymentObject;
        if(this.confirmPaymentBoxStack.Count >0)
            confirmPaymentObject = this.confirmPaymentBoxStack.Pop();
        else
            confirmPaymentObject = Instantiate(this.confirmPaymentBoxPrefab.gameObject, this.SpawnedPopupHolder.transform);

        List<UnityAction> yesActionList = new()
        {
            () => this.confirmPaymentBoxStack.Push(this.currentPopup),
            yesAction
        };
        List<UnityAction> noActionList = new()
        {
            () => this.confirmPaymentBoxStack.Push(this.currentPopup),
            () => this.OpenLastPopup()
        };
        ConfirmPaymentBox confirmPayment = confirmPaymentObject.GetComponent<ConfirmPaymentBox>();
        confirmPayment.Show(title, cost, yesActionList, noActionList);

        this.OpenNewPopup(confirmPaymentObject);
    }
    
    public void ShowConfirmBox(string title, string message, UnityAction yesAction)
    {
        GameObject confirmBoxObject;
        if(this.confirmBoxStack.Count >0)
            confirmBoxObject = this.confirmBoxStack.Pop();
        else
            confirmBoxObject = Instantiate(this.confirmBoxPrefab.gameObject, this.SpawnedPopupHolder.transform);

        List<UnityAction> yesActionList = new()
        {
            () => this.confirmBoxStack.Push(this.currentPopup),
            yesAction
        };
        List<UnityAction> noActionList = new()
        {
            () => this.confirmBoxStack.Push(this.currentPopup),
            () => this.OpenLastPopup()
        };
        ConfirmBox confirmBox = confirmBoxObject.GetComponent<ConfirmBox>();
        confirmBox.Show(title, message, yesActionList, noActionList);

        this.OpenNewPopup(confirmBoxObject);
    }

    public void ShowInformBox(string messageTitle, string messageBody)
    {
        GameObject informObject;
        if (this.informStack.Count > 0)
            informObject = this.informStack.Pop();
        else
            informObject = Instantiate(this.informPopupPrefab.gameObject, this.SpawnedPopupHolder.transform);

        InformBox informBox = informObject.GetComponent<InformBox>();
        informBox.Show(messageTitle, messageBody, TurnOffInformBox);

        this.OpenNewPopup(informObject);
    }

    private void TurnOffInformBox()
    {
        this.informStack.Push(this.currentPopup);
        this.OpenLastPopup();
    }

    #endregion

    #region === GAME ROUND FUNCTIONS ===

    public void GenerateNextRound()
    {
        StartCoroutine(RoundManager.Instance.PrepareNextRound());
    }

    public void GenerateLastRound()
    {
        StartCoroutine(RoundManager.Instance.PrepareCurrentRound());
    }

    public void GenerateChoosedLevel(int index)
    {
        StartCoroutine(RoundManager.Instance.PrepareNewRound(index));
    }

    public void PauseGame()
    {
        this.timer.PauseCounting();
    }

    public void UnPauseGame()
    {
        this.timer.ContinueCounting();
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    #endregion
}
