using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UTJ;

/// <summary>
/// 检测玩家飞船是否碰到了门，如果碰到了门则扣大量的血
/// </summary>
public class CollisionDetection : MonoBehaviour
{

	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag =="wall")
        {
            //如果玩家的飞船碰到了墙，则相当于玩家被攻击一百次，掉100*0.3滴血量
            EnemyBullet.AttackNumber += 100;
            //print(EnemyBullet.AttackNumber);
        }
        
    }
}
