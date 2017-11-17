using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UTJ;

public class ManageCloseDoorAudio : MonoBehaviour
{
    //private int time = 0;
    private GameObject CloseDoorAudio;
    private void OnEnable()
    {
        if (FindObjectsOfType<ManageCloseDoorAudio>().Length >= 2)
            Destroy(this);

        CloseDoorAudio = GameObject.Find("CloseDoorAudio");
        Play();
    }
    void Play()
    {
        if (!CloseDoorAudio.GetComponent<AudioSource>().isPlaying)
        {
            CloseDoorAudio.GetComponent<AudioSource>().Play();
        }

    }
    private void OnDisable()
    {
        if (CloseDoorAudio.GetComponent<AudioSource>().isPlaying)
            CloseDoorAudio.GetComponent<AudioSource>().Stop();
    }
    private void Update()
    {
        if (GameManager.Instance.CameraBackageBecomeBlack&& CloseDoorAudio.GetComponent<AudioSource>().isPlaying)
        {
            CloseDoorAudio.GetComponent<AudioSource>().Stop();
        }
    }

}
