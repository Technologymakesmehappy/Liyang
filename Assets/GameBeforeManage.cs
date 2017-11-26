using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBeforeManage : MonoBehaviour
{

    public GameObject[] lights;
    public GameObject[] lightSources;
    public GameObject Movecamera;
    private AudioSource audioSource;
    private AudioClip audioClip;

    private GameObject space_feiji;

    private bool Ismove = false;

    public AudioSource BackAudioSource;

    public GameObject lightAll;
    public GameObject LightTwo;
    public GameObject LightThree;
    public GameObject LightFour;


    void Start ()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
        space_feiji = GameObject.Find("space_shatl_Echo_TWO");
        if (space_feiji!=null)
        {
            StartCoroutine(PlayBackAudio());
        }
    }
	
	void Update ()
    {
        if (Ismove)
        {
            space_feiji.GetComponent<Transform>().transform.Translate(Vector3.left * Time.deltaTime*10f);

            Movecamera.GetComponent<Transform>().transform.Translate(Vector3.forward * Time.deltaTime * 10f);
        }
        if (Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown(KeyCode.Alpha1)|| Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4)||Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            print(11);
            StartCoroutine(ChnageScene());
        }
	}

    //开始播放故事背景，播放完成再开始让飞机飞行
    IEnumerator PlayBackAudio()
    {
        yield return new WaitForSeconds(2);
        BackAudioSource.Play();

        yield return new WaitForSeconds(8f);
        StartCoroutine(OpenLight());
        

    }

    //开灯效果
    IEnumerator OpenLight()
    {
        yield return new WaitForSeconds(1f);
        lights[0].gameObject.SetActive(true);
        audioSource.Play();
        lightSources[0].gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        lights[1].gameObject.SetActive(true);
        audioSource.Play();
        lightSources[1].gameObject.SetActive(true);

        lightAll.SetActive(true);
        LightTwo.SetActive(true);
        LightThree.SetActive(true);
        LightFour.SetActive(true);



        //此时灯光亮起之后，飞船开始启动飞出驾驶舱
        yield return new WaitForSeconds(2f);
        StartCoroutine(FeijiBeganMove());
    }


    //飞机开始移动飞行
    IEnumerator FeijiBeganMove()
    {
        yield return new WaitForSeconds(1f);
        Ismove = true;
        audioClip = Resources.Load<AudioClip>("engine_start_002");
        audioSource.clip = audioClip;
        audioSource.Play();

        //飞船出发五秒后，开始播放飞出去的特效声音，然后切换场景
        yield return new WaitForSeconds(4f);
        audioClip = Resources.Load<AudioClip>("engine_flyby_002");
        audioSource.clip = audioClip;
        audioSource.Play();

        yield return new WaitForSeconds(2f);

        Camera.main.GetComponent<Camera>().cullingMask = 0;

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("main");
    }
    IEnumerator ChnageScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("main");
    }
}
