using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _NewGame : MonoBehaviour
{
    public GameObject cc;
	void Start ()
    {
       
        GameObject.Find("Canvas").transform.FindChild("Image").gameObject.SetActive(true);
    }
	
	void Update ()
    {
		
	}
    private void Awake()
    {
        StartCoroutine(waitNewGame(3f));
    }
    IEnumerator waitNewGame(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("main");
    }

  
}
