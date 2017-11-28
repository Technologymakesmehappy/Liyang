using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testNew : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Time.timeScale = 0;
        StartCoroutine("A");

	}

    IEnumerator A()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                print("W");
            }

            yield return null;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
