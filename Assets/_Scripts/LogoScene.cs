using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LogoScene : MonoBehaviour
{
	void Start ()
    {
        
	}
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)|| Input.GetKeyDown(KeyCode.Alpha2)|| Input.GetKeyDown(KeyCode.Alpha3)|| Input.GetKeyDown(KeyCode.Alpha4)|| Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            //SceneManager.LoadScene("Main");
            SceneManager.LoadScene("ChooseLevel");
        }
	}
    
}
