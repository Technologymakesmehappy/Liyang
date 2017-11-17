using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UTJ;

public class RankManage : MonoBehaviour
{

    public static RankManage instance;
    private GameObject rank;
    private void OnEnable()
    {
        StartCoroutine(Rank());
    }
    private void Awake()
    {
        instance = this;
    }
  
	
	void Update ()
    {
        
	}
    IEnumerator Rank()
    {
        yield return new WaitForSeconds(1f);
        RankSend._instance.SendScore(Explosion.Instance.PlayerAttackEnemyNumber);
    }

}
