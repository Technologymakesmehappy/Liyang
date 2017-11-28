using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunc : MonoBehaviour {

    public static bool TimeScale = false;

    public  void Init()
    {
        print("调用Button里面的方法");
        TimeScale = true;
    }
}

