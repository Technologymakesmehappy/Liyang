using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UTJ;

public class ShowEnemyDeathDisplayScore : MonoBehaviour
{
    public static ShowEnemyDeathDisplayScore instance;
    public GameObject DisplayScoreUI;
    private void Awake()
    {
        instance = this;
    }

    void Start ()
    {
        
	}
	void Update ()
    {
        if(Explosion.Instance.IsShowScoreUI)
        {
            Explosion.Instance.IsShowScoreUI = false;
            SpawnDisplayScore();
        }
	}

    void SpawnDisplayScore()
    {
        GameObject go = Instantiate(DisplayScoreUI);
        go.transform.parent = this.transform;
        go.transform.localPosition = new Vector3(Random.Range(0, 50f), Random.Range(0, 50f), 0);

        Destroy(go.gameObject, 0.5f);
    }
}
