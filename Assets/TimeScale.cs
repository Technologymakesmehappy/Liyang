using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScale : MonoBehaviour
{

	void Start ()
    {
		
	}

    private float time = 0; 
	void Update ()
    {
        time += Time.deltaTime;

    }
}
