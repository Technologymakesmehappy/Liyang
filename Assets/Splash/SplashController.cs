using UnityEngine;
using System.Collections;
using UnityEngine.VR;
using UnityEngine.Analytics;
using System.Collections.Generic;
using System;

public class SplashController : MonoBehaviour
{
    public float minimumTimeToShowLogo = 5f;
    public string levelToLoad = "";

    public Animation animation;
    public string introName = "SplashIn";
    public string outroName = "SplashOut";

    IEnumerator Start()
    {




        float minimumTimeEnd = Time.realtimeSinceStartup + minimumTimeToShowLogo;

        // intro

        animation.Play(introName);

        // background load the new scene (but don't activate it yet)

        AsyncOperation o = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(levelToLoad);
        o.allowSceneActivation = false;
        while (o.isDone)
        {
            yield return new WaitForEndOfFrame();
        }

        // delay until minimum time is reached

        if (Time.realtimeSinceStartup < minimumTimeEnd)
        {
            yield return new WaitForSeconds(minimumTimeEnd - Time.realtimeSinceStartup);
        }

        // outro

        AnimationClip clip = animation.GetClip(outroName);
        animation.Play(outroName);
        yield return new WaitForSeconds(clip.length);

        // activate scene

        o.allowSceneActivation = true;
    }
}

	