using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour
{
    private AudioSource AS;
    private AudioClip AC;
    public static bool IsPlayBeganAudioClip = true;

    void Start()
    {
        AS = this.GetComponent<AudioSource>();
        AC = Resources.Load<AudioClip>("BeganBGM");
    }

    void Update()
    {
        if (IsPlayBeganAudioClip && !AS.isPlaying)
        {
            StartCoroutine(PlayAudio());

        }
        if (IsPlayBeganAudioClip == false)
        {
            AS.clip = AC;
            AS.Stop();

        }
    }
    IEnumerator PlayAudio()
    {
        yield return new WaitForSeconds(2f);
        AS.clip = AC;
        AS.Play();
    }
}
