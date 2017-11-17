using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class RankRequestRankData : RankSingle<RankRequestRankData> 
{
    #region Class

    #endregion


    #region 字段

    private List<RankLeaderBoards> m_PSelfData=new List<RankLeaderBoards> ();
    private List<RankLeaderBoards> m_PRankData = new List<RankLeaderBoards>();
    private float sco;
    #endregion


    #region 方法
    public override void Init() {
        m_PSelfData = new List<RankLeaderBoards>();
        m_PRankData = new List<RankLeaderBoards>();
    }
 





    public void RequestRank(float t,float sco,string ex1, string ex2, string ex3, string ex4, string ex5)
    {
       
        RankManager.instance.StartCoroutine(IRequestRank(t,sco,ex1,ex2,ex3,ex4,ex5));
    }

    IEnumerator IRequestRank(float t, float sco, string ex1, string ex2, string ex3, string ex4, string ex5)
    {
        while (RankManager.instance._login.monitorId.Length == 0)
        {
            Debug.Log("Login is Null ,Login 没有初始化");
            yield return new WaitForSeconds(0.5f);
        }

        //参数说明
        //1  在登陆的返回中会发给客户端，客户端收到会保存起来。在此处上传取可
        //2 游戏版本号
        //3 运行时长
        //4 排行榜值 
        //5 为用到
        //6协助储存信息，不做处理
        //7 关卡id
        //8标识排行榜类型 1 分数 2命中率 3生存时间

        string path = string.Format("http://ucenter.pangaeavr.com:8080/user-center/gameOver?monitorId={0}&version={1}&totalTime={2}&score={3}&behavior=1:20&extend1={4}&extend2={5}&extend3={6}&extend4={7}&extend5={8}"
                , RankManager.instance._login.monitorId
                , RankManager.instance._login.gameVersion
                , t
                , sco
                , ex1
                , ex2
                , ex3
                , ex4
                , ex5);

        this.sco = sco;
        //由Mon调用 RequestServer
        RankManager.instance.StartCoroutine(RequestServer(path));
    }


    IEnumerator RequestServer(string path)
    {
        WWW www = new WWW(path);
        yield return www;

        if (www.error == null)
        {
            m_PRankData = AnalyRankData(www.text, out m_PSelfData);
            RankManager.instance._GlobalScoreRankUI.ShowRankData(m_PRankData, m_PSelfData, sco);

        }
        else
        {
            Debug.Log("请求排行榜数据报错：" + www.error);

        }

    }


    #endregion


    List<RankLeaderBoards> AnalyRankData(string s, out List<RankLeaderBoards> _selfdata)
    {
        RankGlobalData globalData;
        try
        {
            globalData = LitJson.JsonMapper.ToObject<RankGlobalData>(s);
            _selfdata = globalData.leaderBoardsOfSlef;
            return globalData.leaderBoards;
           

        }
        catch (Exception ex)
        {
            Debug.Log(s + "  " + ex.Message);
            _selfdata = null;
            return null;
        }
    

    }

}
