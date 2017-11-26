using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeResourcesControl : MonoBehaviour
{

    public static GameTimeResourcesControl _instance;

    public bool IsResurceEnd = false;//判断是否加载完毕资源

    public float currentTime = 0;//在这里将是否加载完成的时间管理起来，在游戏重置时将此时间归零

    private void Awake()
    {
        _instance = this;
    }

    
	
	void Start ()
    {
		
	}
	
	void Update ()
    {
        currentTime += Time.deltaTime;
        if (currentTime>=0f)
        {
            IsResurceEnd = true;
        }
    }
}
