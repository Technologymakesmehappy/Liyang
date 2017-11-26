using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UTJ;

/// <summary>
/// 检测玩家飞船是否碰到了门，如果碰到了门则扣大量的血
/// </summary>
public class CollisionDetection : MonoBehaviour
{
    private AudioSource PlayDoorCollisionAudio;
	void Start ()
    {
        PlayDoorCollisionAudio = GameObject.Find("PlayDoorCollidionAudio").GetComponent<AudioSource>();
    }
	
	void Update ()
    {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag =="wall")
        {
            //如果玩家的飞船碰到了墙，则相当于玩家被攻击一百次，掉50*0.3滴血量
            //EnemyBullet.AttackNumber += 50*0.3f;

            //如果玩家的飞船碰到了墙，则相当于玩家被攻击一百次，掉20滴血量
            EnemyBullet.AttackNumber += 10;
            //当玩家碰到了门时，会触发音效和粒子特效
            //播放声音
            PlayDoorCollisionAudio.Play();
            //实例化玩家碰撞到门的粒子效果

            //视野变红的效果
            BecomeRed.instance.Red();
        }
    }

}
