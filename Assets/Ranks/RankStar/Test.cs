using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

    // Use this for initialization
    public RankGlobalScoreRankUI ui;
    void Start ()
    {
        RankSend._instance.SendScore(2000);
       /* LeaderBoards data = new LeaderBoards();
        data.score = 100;
        data.shortName = "test";
        data.number = "1s";
        ui.RankData(data);*/

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
