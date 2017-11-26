using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonTimeScale : MonoBehaviour
{

    private Ray ray;
    RaycastHit hit;

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, 100))
        {
            Button button = hit.collider.GetComponent<Button>();
            button.GetComponent<RectTransform>().localPosition = new Vector3(1.5f,1.5f,1.5f);
            if (button&&(Input.GetKeyDown(KeyCode.Alpha1)|| Input.GetKeyDown(KeyCode.Alpha2)||Input.GetKeyDown(KeyCode.Alpha3)||Input.GetKeyDown(KeyCode.Alpha4)))
                button.onClick.Invoke();
        }
    }
}
