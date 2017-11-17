using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UTJ;

public class PlayerBeganFireNoCollider: MonoBehaviour {

    // Use this for initialization
    //子弹发射频率



    //当游戏进行到后半段，开始慢动作时，飞机发射子弹的速度也需要降低
    private GameObject Movetube;



    private float CanFireTime = 0.1f;
    [HideInInspector]
    private float FireTime = 0f;
	void Start ()
    {
        CanFireTime = 0.1f;

        Movetube = GameObject.Find("tube_B01(Clone)");
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Movetube.GetComponent<Transform>().transform.position.z <= -10200f)
        {
            CanFireTime = 1f;
        }
        else
        {
            CanFireTime = 0.1f;
        }
        if (Player.Instance.CanFire)
        {
            FireTime += Time.deltaTime;
            if (FireTime >= CanFireTime)
            {
                FireTime = 0;
                //模拟玩家发射子弹，GameObject类型的子弹
                GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("zidanNoBoxCollider"), transform.position, transform.rotation) as GameObject;
                go.GetComponent<Rigidbody>().AddRelativeForce(0,0,2000);

            }
        }
    }
}

