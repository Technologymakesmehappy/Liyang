using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
public class RankSend : RankSingle<RankSend>
{
    public override void Init()
    {
       
    }
    public void SendScore(float currentScore,bool isShowUI=true)
    {
        if(isShowUI )
        {
            RankManager.instance.Show();
        }else
        {
            RankManager.instance.Hide();
        }
       
        RankRequestRankData._instance.RequestRank(Time.timeSinceLevelLoad, currentScore, "", "-1", "-1", "", "");
        RankManager.instance._JudgeRecord.ReadRankData((int)currentScore);
    }
}

