using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UTJ;

public enum ButtonStat
{
   
    LocalPanel,
    GlobalPanelNext,
    GlobalPanelPrevious,


}

public class RankManager : MonoBehaviour 
{

   
    [Header("游戏ID，必填项")]
    public int Id =21;
    [Header("---------------------------------------")]
    //主要是记录按钮。。。。未起到管理作用
    public static RankManager instance;
    public Button LocalRank;
    public Button GlobalRank;
    public Button Next;
    public Button Previous;
    [Header("返回和退出按钮，自己定义")]

    public Button Again;
    public Button Back;



    private bool isNext;
    [HideInInspector]
    public RankGlobalScoreRankUI _GlobalScoreRankUI;
    [HideInInspector]
    public RankJudgeRecord _JudgeRecord;

    [HideInInspector]
    public RankLogin _login;
    void Awake()
    {
        instance = this;
        if (LocalRank)
            LocalRank.onClick.AddListener(BLocakRank);
        if (GlobalRank)
            GlobalRank.onClick.AddListener(BGlobalRank);
        if (Next)
            Next.onClick.AddListener(BNext);
        if (Previous)
            Previous.onClick.AddListener(BPrevious);
        if (Again)
            Again.onClick.AddListener(BAgain);
        if (Back)
            Back.onClick.AddListener(BBack);
        isNext = false;
        ChangeButtonStat(ButtonStat.LocalPanel);
        if (!_GlobalScoreRankUI)
            _GlobalScoreRankUI = this.GetComponentInChildren<RankGlobalScoreRankUI>();
        if (!_JudgeRecord)
            _JudgeRecord = this.GetComponentInChildren<RankJudgeRecord>();
        if (_login==null)
            _login = new RankLogin();

        if (_login != null)
            _login.Init();


        Show();
        Hide();
    }


    public void Show()
    {
        this.transform.GetChild(0).gameObject.SetActive(true);
    }

   public  void Hide()
    {

            this.transform.GetChild(0).gameObject.SetActive(false);
        
    }
    void BLocakRank()
    {
        ChangeButtonStat(ButtonStat.LocalPanel);
        _GlobalScoreRankUI.RemoveUI();
        _JudgeRecord.ShowUI();
    }

    void BGlobalRank()
    {
        ChangeButtonStat(ButtonStat.GlobalPanelPrevious);
        _JudgeRecord.RemoveUI();
        _GlobalScoreRankUI.ShowUI();
    }
    void BNext()
    {
        ChangeButtonStat(ButtonStat.GlobalPanelNext);
        Next.gameObject.SetActive(false);
        Previous.gameObject.SetActive(true);
        _GlobalScoreRankUI.Next();
    }
    void BPrevious()
    {
        ChangeButtonStat(ButtonStat.GlobalPanelPrevious);
        Previous.gameObject.SetActive(false);
        Next.gameObject.SetActive(true);

        _GlobalScoreRankUI.previous();
    }
   public  void BAgain()
    {
        Show();
        ShowScore();
    }
    void BBack()
    {
        Hide();
    }
    public void ShowScore()
    {
        List<RankScoreRecord> previousScores = new List<RankScoreRecord>();
        string scoreStr = RankLocalData._instance.ReadData();
        if (scoreStr != null && scoreStr != "")
        {
            string[] sco = scoreStr.Split(';');
            for (int i = 0; i < sco.Length; i++)
            {
                previousScores.Add(new RankScoreRecord(false, int.Parse(sco[i])));
            }
        }

        if (previousScores.Count > 0)
        {
            RankSend._instance.SendScore(previousScores[0].score);
        }
        else
        {
            RankSend._instance.SendScore(0);
        }
    }
    //全球的事件调用
    void HaveRank()
    {
        isNext = true;
    }


    void ChangeButtonStat(ButtonStat _stat)
    {
        switch (_stat)
        {
         
            case ButtonStat.LocalPanel:
                LocalRank.gameObject.SetActive(false);
                GlobalRank.gameObject.SetActive(true);
                Next.gameObject.SetActive(false);
                Previous.gameObject.SetActive(false);
                break;
            case ButtonStat.GlobalPanelNext:
                LocalRank.gameObject.SetActive(true);
                GlobalRank.gameObject.SetActive(false);
                if (!isNext)
                    break;
                Next.gameObject.SetActive(true);
                Previous.gameObject.SetActive(false);
                break;
            case ButtonStat.GlobalPanelPrevious:
                LocalRank.gameObject.SetActive(true);
                GlobalRank.gameObject.SetActive(false);
                if (!isNext)
                    break;
                Next.gameObject.SetActive(false);
                Previous.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }


}
