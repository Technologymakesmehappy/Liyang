using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoScene : MonoBehaviour
{

	void Start ()
    {
        StartCoroutine(Logo());
	}
	
	void Update ()
    {
		
	}
    IEnumerator Logo()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Main");
    }
}
