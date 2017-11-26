using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class BecomeRed : MonoBehaviour
{
    public static BecomeRed instance;
    public float m = 1;
    private Color color;
    private bool IsCanRed = false;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {

        color = this.GetComponent<Image>().color;
        color.a = 0f;
    }
    // Update is called once per frame  
    void Update()
    {

        if (IsCanRed)
        {
            color.a = 1f;
            IsCanRed = false;
        }
        color.a -= Time.deltaTime * 2;
        if (color != null && this.GetComponent<Image>().color != null)
        {
            this.GetComponent<Image>().color = color;
        }

    }
    public void Red()
    {
        IsCanRed = true;

    }

}