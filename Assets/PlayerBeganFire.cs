using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UTJ;

public class PlayerBeganFire : MonoBehaviour {

    // Use this for initialization
    //子弹发射频率
    public float CanFireTime = 0.1f;
    public float FireTime = 0f;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Player.Instance.CanFire)
        {
            FireTime += Time.deltaTime;
            if (FireTime>= CanFireTime)
            {
                //模拟玩家发射子弹，GameObject类型的子弹
                GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("zidan"), transform.position, transform.rotation) as GameObject;
                go.GetComponent<Rigidbody>().AddForce(0, 0, 2000);
                FireTime = 0;
            }
           
        }
    }
}
