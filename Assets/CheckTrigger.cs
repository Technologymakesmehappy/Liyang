using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTrigger : MonoBehaviour
{

    float distance = 10;
    public GameObject Cube;
    public GameObject Sphere;
	void Start ()
    {
		
	}
	
	
	void Update ()
    {
        distance = Vector3.Distance(Cube.transform.position,Sphere.transform.position);
        if (distance<=0.1f)
        {
            print("你碰到我了");
        }

    }
    
}
