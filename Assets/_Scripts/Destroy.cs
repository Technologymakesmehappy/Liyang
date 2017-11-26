using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UTJ;


public class Destroy : MonoBehaviour {

  
	void Start ()
    {
     
        Destroy(this.gameObject,0.5f);
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.name == "zako(Clone)")
        {
            other.gameObject.SetActive(false);
            Destroy(this.gameObject);

            //if (AttackSucceedNumber >= 5)
            /// {
            /// 

          
                GameObject Ex = GameObject.Instantiate(Resources.Load<GameObject>("Ef_ExplosionThree"), other.gameObject.transform.position, other.gameObject.transform.rotation);
                Destroy(Ex, 0.5f);
            




            //AttackSucceedNumber = 0;
            //}
            //else
            //{
            //    GameObject Ex = GameObject.Instantiate(Resources.Load<GameObject>("Ef_ExplosionTwo"), other.gameObject.transform.position, other.gameObject.transform.rotation);
            //    Destroy(Ex, 1.5f);
            //}
            //加分
            //实例化爆炸效果




        }
        //如果打到了大的敌人
        else if (other.gameObject.tag == "Enemy")
        {
            //实例化爆炸效果
            //消失爆炸效果
            //AttackSucceedNumber++;
            //if (AttackSucceedNumber >= 5)
            //{

            other.gameObject.SetActive(false);
            Destroy(this.gameObject);

            //int m = Random.Range(0, 7);
            //if (m > 3)
            //{
            //    //print(m);
            //    GameObject Ex = GameObject.Instantiate(Resources.Load<GameObject>("Ef_Explosion"), other.gameObject.transform.position, other.gameObject.transform.rotation);
            //    Destroy(Ex, 0.5f);
            //}
            //else
            //{
            //    GameObject Ex = GameObject.Instantiate(Resources.Load<GameObject>("Ef_ExplosionTwo"), other.gameObject.transform.position, other.gameObject.transform.rotation);
            //    Destroy(Ex, 0.5f);
            //}
            //AttackSucceedNumber = 0;
            // }
            //else
            //{
            //  GameObject Ex = GameObject.Instantiate(Resources.Load<GameObject>("Ef_ExplosionTwo"), other.gameObject.transform.position, other.gameObject.transform.rotation);
            // Destroy(Ex, 1.5f);
            //}
        }
    }
}
