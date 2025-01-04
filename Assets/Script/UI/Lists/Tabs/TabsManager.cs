using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabsManager : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] private Sprite defaultImage;
    [SerializeField] private Sprite highlightImage;
    [SerializeField] private GameObject contentHolder;

    [Header("Paramters")]
    private int currentTab;
    private GameObject currentTabObj;
    private List<GameObject> contentsList = new();

    private void Awake()
    {
        for(int i=0; i<this.contentHolder.transform.childCount; i++)
            this.contentsList.Add(this.contentHolder.transform.GetChild(i).gameObject);
    }

    private void Start()
    {
        this.OnClick(transform.GetChild(0).gameObject);
    }

    public void OnClick(GameObject tab)
    {
        Effect.Vibrate();

        this.UnselectCurrentTab();

        tab.GetComponent<Image>().sprite = this.highlightImage;
        this.currentTab = tab.transform.GetSiblingIndex() + 1;
        this.currentTabObj = tab;
        this.contentsList[this.currentTab - 1].SetActive(true);
    }

    private void UnselectCurrentTab()
    {
        if (this.currentTabObj == null)
            return;

        this.currentTabObj.GetComponent<Image>().sprite = this.defaultImage;
        this.contentsList[this.currentTab - 1].SetActive(false);
    }
}
