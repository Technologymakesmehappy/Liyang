using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UTJ;

public class FirePointMuzzleFlash : MonoBehaviour
{


    //void Update ()
    //   {

    //       if (Input.GetKey(KeyCode.Alpha1))
    //       {
    //           //已经发射了子弹，可以发射枪口火焰效果
    //           GameObject go = Instantiate(Resources.Load<GameObject>("GreenLightningMuzzleFlash"),this.transform.position,this.transform.rotation);
    //           Destroy(go,0.1f);
    //       }

    //}

    //当游戏进行到后半段，开始慢动作时，飞机发射子弹的速度也需要降低
    private GameObject Movetube;



    private float CanFireTime = 0.1f;
    [HideInInspector]
    private float FireTime = 0f;
    void Start()
    {
        Movetube = GameObject.Find("tube_B01(Clone)");
        CanFireTime = 0.1f;
    }

    // Update is called once per frame
    void Update()
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
                //已经发射了子弹，可以发射枪口火焰效果
                //GameObject go = Instantiate(Resources.Load<GameObject>("GreenLightningMuzzleFlash"), this.transform.position, this.transform.rotation);
                //Destroy(go, 0.1f);



               



            }
        }
    }
}

