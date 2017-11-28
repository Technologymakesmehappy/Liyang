using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UTJ;

public class ManageUILast : MonoBehaviour
{
    public Text ScoreLast;
    public Text KillEnemy;
    public Text SurplusEnergy;
    public Text sss;


    private void Awake()
    {
        
    }
    void Start ()
    {
       
    }
	
	
	void Update ()
    {
        //Score.text =  /*"当前得分：" +*/ Explosion.Instance.PlayerAttackEnemyNumber.ToString();
        ScoreLast.text = (Explosion.Instance.PlayerAttackEnemyNumber).ToString();
        KillEnemy.text = ((Explosion.Instance.PlayerAttackEnemyNumber) * 0.01F).ToString();
        SurplusEnergy.text = (SystemManager.Instance.NowBloodVolume).ToString();
    }

   
    
}
