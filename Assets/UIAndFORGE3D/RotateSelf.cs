using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSelf : MonoBehaviour
{

	void Start ()
    {
		
	}
	
	void Update ()
    {
        this.transform.Rotate(Vector3.up,Time.deltaTime*20);
	}
}
