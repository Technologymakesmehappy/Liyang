using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public struct RankScoreRecord
{
    public bool isBreaked;
    public int score;

    public RankScoreRecord(bool _isBreak, int _score)
    {
        isBreaked = _isBreak;
        score = _score;
    }
}
public class RankJudgeRecord : MonoBehaviour 
{
    #region 字段
    #region Public
    //第一次时候的图标
    public GameObject TheOne;

    public GameObject m_PRankData;

    public Transform scalPanel;
    public Transform localPanel;
    #endregion

    private List<RankScoreRecord> previousScores = new List<RankScoreRecord>();
    private List<GameObject> listRankData = new List<GameObject>();
    private int _currentScore;
    #endregion


    #region 方法
    public void ReadRankData(int currentScore)
    {
        _currentScore = currentScore;
        string scoreStr = RankLocalData._instance.ReadData();
        if (scoreStr != null&&scoreStr!="")
        {
            string[] sco = scoreStr.Split(';');
            for (int i = 0; i < sco.Length; i++)
            {
                previousScores.Add(new RankScoreRecord(false, int.Parse(sco[i])));
            }
        }

        JudgeScore(previousScores, currentScore);

        ShowUI();
    }


   public  void ShowUI()
    {
        ShowUI(previousScores, _currentScore);

    }
    public void RemoveUI()
    {
        if(listRankData.Count>0)
            for (int i = 0; i < listRankData.Count; i++)
            {
                Destroy(listRankData[i]);
            }
        listRankData.Clear();

    }

    




    void ShowUI(List<RankScoreRecord> _lists,int _currentScore)
    {
        string saveTo = "";
        if (_lists.Count == 0)
        {
            saveTo = _currentScore.ToString();
          
            ShowLocalData(new List<int> { _currentScore }, _currentScore);
        }else if (_lists.Count > 0)
        {
            List<int> SaveSata = new List<int>();
            for (int i = 0; i < _lists.Count; i++)
            {
                if (_lists[i].score != 0)
                {
                    SaveSata.Add(_lists[i].score);
                }
            }
            SaveSata.Add(_currentScore);

            if (SaveSata.Count > 1)
            {
                int temp = 0;
                for (int i = 0; i < SaveSata.Count - 1; i++)
                {
                    for (int j = i + 1; j < SaveSata.Count; j++)
                    {
                        if (SaveSata[i] < SaveSata[j])
                        {
                            temp = SaveSata[i];
                            SaveSata[i] = SaveSata[j];
                            SaveSata[j] = temp;
                        }
                    }
                }
                for (int i = 0; i < SaveSata.Count; i++)
                {
                    if (i < 5)
                        saveTo += SaveSata[i] + ";";
                }

            }
            else
            {
                saveTo = SaveSata[0] + ";";
            }

            ShowLocalData(SaveSata, _currentScore);
            saveTo = saveTo.Substring(0, saveTo.Length - 1);
        }

        RankLocalData._instance.SaveDt(saveTo);
    }

    void ShowLocalData(List<int> scores,int _CurrentScore)
    {
        for (int i = 0; i < scores.Count; i++)
        {
            if (i < RankGlobalScoreRankUI.CurrentRankNum)
            {
               GameObject go= SelfRankData(i+1, scores[i], localPanel);
                listRankData.Add(go);
                if (i > 0 && i < RankGlobalScoreRankUI.CurrentRankNum)
                    go.transform.localPosition = new Vector3(0, listRankData[i - 1].transform.localPosition.y - 100, 0);
            }
        }


    }

    GameObject SelfRankData(int number,int _score,Transform parent)
    {
        GameObject data = GameObject.Instantiate<GameObject>(m_PRankData);
        data.transform.parent = parent;
        data.transform.localPosition = Vector3.zero;
        data.transform.localRotation = Quaternion.Euler(0, 0, 0);
        data.transform.localScale = new Vector3(1, 1, 1);
        RankData _data = data.GetComponent<RankData>();
        _data.Number = number.ToString();
        _data.ID = "本地";
        _data.Score = _score.ToString() ;   
          
        return data;
    }

    void JudgeScore(List<RankScoreRecord> _lists,int currentScore)
    {
        //First Pepole
        if (_lists.Count == 0)
        {
            if (TheOne)
            {
                TheOne.SetActive(true);
                TheOne.GetComponent<Text>().text = "The One";
            }
        }
        else
        {
            for (int i = previousScores.Count - 1; i >= 0; i--)
            {
                if (!previousScores[i].isBreaked)
                {
                    if (currentScore > previousScores[i].score)
                    {
                        previousScores[i] = new RankScoreRecord(true, previousScores[i].score);
                        if (TheOne)
                        {
                            TheOne.SetActive(true);
                            TheOne.GetComponent<Text>().text = "The One";
                            break;
                        }
                    }

                  }
             }
        }


    }

    #endregion






    #region Unity



    #endregion
}
