using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollPanelUI : MonoBehaviour, IEndDragHandler
{
    [Header("References")]
    [SerializeField] private RectTransform contentRect;
    [SerializeField] private TMP_Text pageName;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private GameObject defaultPage;

    [Header("Parameters")]
    [SerializeField] private float pageWidth;
    [SerializeField] private float swipeTime;
    
    private int currentPage;
    private int actualPage { get => this.currentPage_actualPage_dictionary[currentPage]; }

    private int maxPage = 0;
    private Vector3 pageStep;
    private Vector3 targetPos;
    private float dragThreshold;
    private Dictionary<int, int> currentPage_actualPage_dictionary = new();

    private void Awake()
    {
        this.pageStep = new Vector3(this.pageWidth, 0f, 0f);
        this.maxPage = this.contentRect.transform.childCount;

        int inactivePage = 0;
        int decreasedCurrentPageAmount = 0;
        int loopCurrentPage = 1;
        int actualPage = this.defaultPage.transform.GetSiblingIndex() + 1;
        for(int pageIndex=0; pageIndex<this.contentRect.childCount; pageIndex++)
        {
            if (!contentRect.GetChild(pageIndex).gameObject.activeSelf)
            {
                inactivePage++;
                if (currentPage > pageIndex + 1) { }
                    decreasedCurrentPageAmount++;
            }
            else
            {
                if (actualPage == pageIndex + 1)
                    this.currentPage = loopCurrentPage;
                currentPage_actualPage_dictionary[loopCurrentPage] = pageIndex + 1;
                loopCurrentPage++;
            }
        }
        this.maxPage -= inactivePage;

        this.contentRect.localPosition = new Vector3((this.maxPage + 1 - 2 * this.currentPage) * this.pageWidth / 2
                                                        , this.contentRect.localPosition.y,
                                                        this.contentRect.localPosition.z);
        this.targetPos = this.contentRect.localPosition;
        this.dragThreshold = Screen.width / 15;

        this.pageName.text = this.contentRect.transform.GetChild(this.actualPage - 1).name;
    }

    private void Start()
    {
        this.SetButtonsInteractable();
    }

    public void RightPage()
    {
        if (this.currentPage >= this.maxPage)
            return;

        this.currentPage++;
        this.targetPos -= this.pageStep;
        this.MovePage();
    }

    public void LeftPage() 
    {
        if (this.currentPage <= 1)
            return;

        this.currentPage--;
        this.targetPos += this.pageStep;
        this.MovePage();
    }

    private void MovePage()
    {
        Effect.Vibrate();
        this.contentRect.DOLocalMove(this.targetPos, this.swipeTime);
        this.pageName.text = this.contentRect.transform.GetChild(this.actualPage - 1).name;
        this.SetButtonsInteractable();
    }

    private void SetButtonsInteractable()
    {
        if(this.currentPage == 1)
            this.leftButton.interactable = false;
        else
            this.leftButton.interactable = true;

        if(this.currentPage == this.maxPage)
            this.rightButton.interactable = false;
        else
            this.rightButton.interactable = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(Mathf.Abs(eventData.position.x - eventData.pressPosition.x) > this.dragThreshold)
        {
            if (eventData.position.x > eventData.pressPosition.x)
                this.LeftPage();
            else
                this.RightPage();
        }
        else
            this.MovePage();
    }
}
