using UnityEngine;
using System.Collections;

public class RankLeaderBoards
{

 
        public string monitorId;
        public int gameId;
        public string number;
        public string shortName;
        public double score;
        public string extend1;
        public string extend2;
        public string extend3;
        public string extend4;
        public string extend5;
        public string pkid;
        public RankLeaderBoards(string mId, int gId, string n, string s, double sco, string e1, string e2, string e3, string e4, string e5, string p)
        {
            this.monitorId = mId;
            this.gameId = gId;
            this.number = n;
            this.shortName = s;
            this.score = sco;
            extend1 = e1;
            extend2 = e2;
            extend3 = e3;
            extend4 = e4;
            extend5 = e5;
            this.pkid = p;
        }
    public RankLeaderBoards()
    {

    }

}
