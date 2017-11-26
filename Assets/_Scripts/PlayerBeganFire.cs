using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UTJ;

public class PlayerBeganFire : MonoBehaviour {


    public Transform[] m_TLeftAndRight;
    private float Randoms = 0.01f;
    private int m_ICurrentPos;

    private GameObject zhunxing;

    private Transform m_TRayFirePos;
    // Use this for initialization
    //子弹发射频率

    //当游戏进行到后半段，开始慢动作时，飞机发射子弹的速度也需要降低
    private GameObject Movetube;



    private float CanFireTime = 0.1f;
    [HideInInspector]
    private float FireTime = 0f;
	void Start ()
    {
        zhunxing = GameObject.Find("Crosshair");
        Movetube = GameObject.Find("tube_B01(Clone)");
        CanFireTime = 0.1f;

        if (!m_TRayFirePos)
            m_TRayFirePos = Camera.main.transform;
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
                //GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("zidan"), transform.position, transform.rotation) as GameObject;
                //go.GetComponent<Rigidbody>().AddRelativeForce(0,0,2000);




                Ray ray = new Ray(m_TRayFirePos.position, m_TRayFirePos.forward);
                Debug.DrawRay(m_TRayFirePos.position,  m_TRayFirePos.forward * 100,Color.red);
                RaycastHit hit;
                Vector3 spread = new Vector3(Random.Range(-Randoms, Randoms), Random.Range(-Randoms, Randoms),1);
                if (Physics.Raycast(ray, out hit))
                {
    
                    for (int i = 0; i < m_TLeftAndRight.Length; i++)
                    {
                        GameObject go = Instantiate(Resources.Load<GameObject>("zidan"), m_TLeftAndRight[i].position, Quaternion.identity);
                        //go.transform.LookAt(hit.point);
                        go.transform.LookAt(hit.point);
                        Rigidbody g = go.GetComponent<Rigidbody>();

                        if (g)
                        {

                            //  Vector3 spread = Vector3.zero;
                            Vector3 direction = go.transform.forward;
                            g.AddForce(4000 * 3f * direction);
                        }
                    }
              
                }
                else
                {
                    for (int i = 0; i < m_TLeftAndRight.Length; i++)
                    {
                        GameObject go0 = GameObject.Instantiate(Resources.Load<GameObject>("zidan"), m_TLeftAndRight[i].position, transform.rotation) as GameObject;
                        go0.GetComponent<Rigidbody>().AddRelativeForce(4000 * 3f * 2 * spread);
                    }
                 

                }
            }
        }
    }
}

