using System.Collections.Generic;

[System.Serializable]
public class LevelRank
{
    public List<RankStar> rankList = new();

    public void UpdateRank(int level, RankStar rank)
    {
        if(rankList.Count >= level)
            this.rankList[level - 1] = rank;
        else
        {
            for(int i=this.rankList.Count; i<level - 1; i++)
                this.rankList.Add(RankStar.zero);
            this.rankList.Add(rank);
        }
    }

    public RankStar GetRank(int level)
    {
        if (rankList.Count < level)
            return RankStar.zero;

        return rankList[level - 1];
    }
}

public enum RankStar
{
    zero = 0,
    one = 1,
    two = 2,
    three = 3,
}