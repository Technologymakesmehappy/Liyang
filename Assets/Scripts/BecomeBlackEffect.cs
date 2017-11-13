using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BecomeBlackEffect : MonoBehaviour
{
    private float a ;
    private Color OldColor;
	void Start ()
    {
        OldColor = this.GetComponent<Image>().color;
        //this.GetComponent<Image>().color.a += Time.deltaTime;
	}
	
	void Update ()
    {
        a += Time.deltaTime;
        a = Mathf.Clamp(a,0,1);
        this.GetComponent<Image>().color = new  Color(0,0,0,Mathf.Lerp(0,a,2f));
	}
    private void OnDisable()
    {
        this.GetComponent<Image>().color = OldColor;
    }
}
