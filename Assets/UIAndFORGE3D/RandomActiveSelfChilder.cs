using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomActiveSelfChilder : MonoBehaviour
{

    public GameObject[] holoInterfaces;

    void Start ()
    {
        StartIenumber();
    }
	
	void Update ()
    {
		
	}

    void StartIenumber()
    {
        StartCoroutine(ShowUI());
    }

    //用来显示高大上的UI
    IEnumerator ShowUI()
    {
        yield return new WaitForSeconds(1f);
        holoInterfaces[0].SetActive(true);
        yield return new WaitForSeconds(5f);
        holoInterfaces[0].SetActive(false);
        holoInterfaces[2].SetActive(true);
        yield return new WaitForSeconds(4f);
        holoInterfaces[2].SetActive(false);
        holoInterfaces[3].SetActive(true);

        yield return new WaitForSeconds(5f);
        holoInterfaces[3].SetActive(false);
        holoInterfaces[4].SetActive(true);

        yield return new WaitForSeconds(4f);
        holoInterfaces[4].SetActive(false);
        holoInterfaces[5].SetActive(true);

        yield return new WaitForSeconds(4f);
        holoInterfaces[5].SetActive(false);
        StartIenumber();
    }
}
