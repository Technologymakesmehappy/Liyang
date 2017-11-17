using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RankGlobalScoreRankUI : MonoBehaviour 
{


    #region 字段
    public Transform RankPanel;
    public Transform SelfPanel;
    

    public GameObject m_PRankData;
    public GameObject[] m_PStar;
    [Header("未进入排行榜")]
    public GameObject Info;

  


    //排行榜的数目
    private const int RankNum = 10;
    //每行排行榜的数目
    public  const int CurrentRankNum = 5;

 
    private   List<GameObject> listRankData;

    private List<RankLeaderBoards> rankdata=new List<RankLeaderBoards> ();
    private List<RankLeaderBoards> _selfdatas = new List<RankLeaderBoards>();
    private float sco;
    private bool isInto;

  
    #endregion




    #region 方法

    void Initialzie()
    {
        Info.SetActive(false);
        listRankData = new List<GameObject>();
        if (m_PStar.Length > 0)
            for (int i = 0; i < m_PStar.Length; i++)
                m_PStar[i].SetActive(false);
        isInto = false;
    }


    public void ShowUI()
    {
        Initialzie();
        if (rankdata.Count == 0 || _selfdatas.Count == 0)
            return;

        RankLeaderBoards selfdata = ReturnCurrentLeader(_selfdatas, sco);

        RankData(rankdata);
     
        if (selfdata==null|| int.Parse(selfdata.number) > 10)
        {
            Info.SetActive(true);
            isInto = false;
        }

        else if(selfdata!=null)
        {
            RankData(selfdata);
            isInto = true;
        }
    
        

        //放到初始化里面




        if(listRankData.Count>=5)
            for (int i = 5; i < (!isInto? listRankData.Count :listRankData.Count-1); i++)
            {
                listRankData[i].SetActive(false);
            }
      

    }
    public void RemoveUI()
    {
        if(listRankData.Count>0)
            for (int i = 0; i < listRankData.Count; i++)
            {
                Destroy(listRankData[i]);

            }
        listRankData.Clear();
        Info.gameObject.SetActive(false);

    }
    public void Next()
    {
        if (listRankData.Count > 5)
        {
            for (int i = 0; i < (!isInto ? listRankData.Count : listRankData.Count - 1); i++)
            {
                if (i < 5)
                    listRankData[i].SetActive(true);
                else
                    listRankData[i].SetActive(false);

            }
        }else if (listRankData.Count > 0)
        {
            for (int i = 0; i < (!isInto ? listRankData.Count : listRankData.Count - 1); i++)
            {               
                    listRankData[i].SetActive(true);
             
            }
        }
      
    }
    public void previous()
    {
        if (listRankData.Count < 6)
            return;

        for (int i = 0; i < (!isInto ? listRankData.Count : listRankData.Count - 1); i++)
        {
            if (i < 5)
                listRankData[i].SetActive(false);
            else
                listRankData[i].SetActive(true);
        }



    }



    public void ShowRankData(List<RankLeaderBoards> rankdata,List<RankLeaderBoards> _selfdatas,float sco)
    {

      this.rankdata = new List<RankLeaderBoards>();
        this.  _selfdatas = new List<RankLeaderBoards>();
        this.rankdata = rankdata;
        this._selfdatas = _selfdatas;
        this.sco = sco;
        Info.gameObject.SetActive(false);
        if (rankdata.Count > 5)
            transform.parent.SendMessage("HaveRank", SendMessageOptions.DontRequireReceiver);

    }


    GameObject  SelfRankData(RankLeaderBoards datas,Transform parent)
    {
        GameObject data = GameObject.Instantiate<GameObject>(m_PRankData);
        data.transform.parent = parent;
        data.transform.localPosition = Vector3.zero;
        data.transform.localRotation = Quaternion.Euler(0, 0, 0);
        data.transform.localScale = new Vector3(1, 1, 1);
        RankData _data = data.GetComponent<RankData>();
        _data.Number = datas.number;
        _data.ID = datas.shortName;
        _data.Score = datas.score.ToString();
        if(parent==SelfPanel)
        _data.SetColor(Color.green);
        if (int.Parse( _data.Number) <= 3)
        StartCoroutine( RankStar(int.Parse(_data.Number),data.transform.Find("StarParent")));
        return data;
    }
    void RankData(List<RankLeaderBoards> datas)
    {
        for (int i = 0; i < datas.Count; i++)
        {
            if (i < RankNum)
            {
               GameObject go = SelfRankData(datas[i], RankPanel);
                listRankData.Add(go);
                if (i > 0 && i < CurrentRankNum)
                    go.transform.localPosition = new Vector3(0, listRankData[i-1].transform.localPosition.y - 100, 0);
                else if (i > CurrentRankNum && i <= RankNum)
                    go.transform.localPosition = new Vector3(0, listRankData[i-1 - CurrentRankNum].transform.localPosition.y - 100);

                go.name = i.ToString();
            }

        }    
    }
    void RankData(RankLeaderBoards datas)
    {
     GameObject go=  SelfRankData(datas, SelfPanel);
        go.name = "Scal";
        listRankData.Add(go);
    }


    IEnumerator RankStar(int number,Transform parent)
    {

        if (m_PStar.Length > 0)
        {
            GameObject data = GameObject.Instantiate<GameObject>(m_PStar[number-1]);
            data.SetActive(true);
            data.transform.parent = parent;
            data.transform.localPosition = Vector3.zero;
            data.transform.localRotation = Quaternion.Euler(0, 0, 0);
            yield break;
            /* Animation anima = data.GetComponent<Animation>();
             if (anima == null)
             {
                 anima = data.AddComponent<Animation>();
                 Debug.LogError("播放片段为Null");
                yield break;
             }


             switch(number-1)
             {
                 case 0:
                     anima.Play("one");
                     break;
                 case 1:

                     anima.Play("two");
                     break;
                 case 2:

                     anima.CrossFade("three");
                     break;
             }*/
        }


    }





    RankLeaderBoards ReturnCurrentLeader(List<RankLeaderBoards> selfdata,float sco)
    {
        for (int i = 0; i < selfdata.Count; i++)
        {
            // if (selfdata[i].monitorId == Login._instance.monitorId && selfdata[i].score == sco)
            //   return selfdata[i];
            if (selfdata[i].monitorId == "1" && selfdata[i].score == sco)
                return selfdata[i];
        }
        return null;
    }
    #endregion


    #region Unity

    #endregion
}
