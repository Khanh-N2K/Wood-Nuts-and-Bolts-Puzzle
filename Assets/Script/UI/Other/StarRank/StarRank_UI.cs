using UnityEngine;
using UnityEngine.UI;

public class StarRank_UI : ExternInitialized_GameObject
{
    [Header("Stars")]
    [SerializeField] private Sprite yellowStar;
    [SerializeField] private Sprite starBorder;

    [Header("References")]
    protected GameObject starsHolder;
    protected Image firstStar;
    protected Image secondStar;
    protected Image thirdStar;

    public override void Initialize()
    {
        this.starsHolder = transform.Find("Stars").gameObject;
        this.firstStar = this.starsHolder.transform.GetChild(0).GetComponent<Image>();
        this.secondStar = this.starsHolder.transform.GetChild(1).GetComponent<Image>();
        this.thirdStar = this.starsHolder.transform.GetChild(2).GetComponent<Image>();
    }

    public void ShowStars(RankStar rank)
    {
        switch (rank)
        {
            case RankStar.zero:
                this.DisplayStar(this.firstStar, false);
                this.DisplayStar(this.secondStar, false);
                this.DisplayStar(this.thirdStar, false);
                break;
            case RankStar.one:
                this.DisplayStar(this.firstStar, true);
                this.DisplayStar(this.secondStar, false);
                this.DisplayStar(this.thirdStar, false);
                break;
            case RankStar.two:
                this.DisplayStar(this.firstStar, true);
                this.DisplayStar(this.secondStar, true);
                this.DisplayStar(this.thirdStar, false);
                break;
            case RankStar.three:
                this.DisplayStar(this.firstStar, true);
                this.DisplayStar(this.secondStar, true);
                this.DisplayStar(this.thirdStar, true);
                break;
            default:
                break;
        }
    }

    protected virtual void DisplayStar(Image star, bool show)
    {
        if (show)
            star.sprite = this.yellowStar;
        else
            star.sprite = this.starBorder;
    }
}
