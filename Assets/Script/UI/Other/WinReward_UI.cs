using TMPro;
using UnityEngine;

public class WinReward_UI : ExternInitialized_GameObject
{
    [Header("References")]
    private StarRank_Moving_UI starRankUI;
    private NewRecordStamp_UI newRecordStamp;
    private GameObject goldObj;
    private TMP_Text goldText;

    [Header("Parameters")]
    private RankStar rank;

    public override void Initialize()
    {
        RewardManager.Instance.onGetReward.AddListener(this.ShowReward);
        this.goldObj = transform.Find("Gold").gameObject;
        this.goldText = this.goldObj.GetComponentInChildren<TMP_Text>();
        this.starRankUI = GetComponent<StarRank_Moving_UI>();
        this.starRankUI.Initialize();

        this.newRecordStamp = GetComponent<NewRecordStamp_UI>();

        this.newRecordStamp.onDoneAnimation += this.ShowGoldReward;
    }

    private void ShowReward(RankStar rank, bool isNewRecord)
    {
        this.rank = rank;
        this.goldObj.SetActive(false);

        this.starRankUI.ShowStars(rank);

        if (!isNewRecord)
            this.newRecordStamp.CheckShowNewRecordMark(false);
        else
            this.newRecordStamp.CheckShowNewRecordMark(true);
    }

    private void ShowGoldReward()
    {
        this.goldObj.SetActive(true);
        this.goldText.text = "+ " + ((int)this.rank).ToString();
    }
}
