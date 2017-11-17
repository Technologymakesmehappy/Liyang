using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RankGlobalData
{
        public byte status;
        public string monitorId;
        public int authType;
        public string authValue;
        public string errMsg;
        public string url;
        public List<RankLeaderBoards> leaderBoards = new List<RankLeaderBoards>();
        public List<RankLeaderBoards> leaderBoardsOfSlef = new List<RankLeaderBoards>();   
}
