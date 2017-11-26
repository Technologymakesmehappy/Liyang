using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlGameTipMiss : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(MissTip());
    }

    IEnumerator MissTip()
    {
        yield return new WaitForSeconds(7f);
        this.gameObject.SetActive(false);
    }
}
