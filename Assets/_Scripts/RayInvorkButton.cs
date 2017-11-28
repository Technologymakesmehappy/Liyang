using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;

public class RayInvorkButton : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Testa());
    }
    void Update()
    {

    }
    RaycastHit hit;
    IEnumerator Testa()

    {
        while (true)
        {
            Ray ray = new Ray(this.transform.position, this.transform.forward * 10);
            Debug.DrawRay(transform.position, transform.forward * 10, Color.red);
            if (Physics.Raycast(ray, out hit, 10))
            {

                Button button = hit.collider.GetComponent<Button>();
                print("调用Button里面的方法");
                if (button)
                    button.onClick.Invoke();
            }

            if (ButtonFunc.TimeScale&&(Input.GetKeyDown(KeyCode.Alpha1)|| Input.GetKeyDown(KeyCode.Alpha2)|| Input.GetKeyDown(KeyCode.Alpha3)))
            {
                Time.timeScale = 1;
                ButtonFunc.TimeScale = false;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                print("按下了空格键");
                print(Time.timeScale + "Time.TimeScale");
            }
            yield return null;
        }
    }
}

       
