using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UTJ;

public class BeganGameAudioControl : MonoBehaviour
{
    private AudioSource AudioOneQifeiQianJingBao;
    private AudioSource AudioOneQifeiQiandaojishiyinxiao;
    private AudioSource AudioOneQifeiQianhuxi;
    private AudioSource AudioOneQifeiQiandaojishi123;

    private bool ISAudioOneQifeiQiandaojishiyinxiao = false;

    public bool IsBenganIenumber = true;

    public bool IsEnableQifeiQiandaojishiyinxiao = false;

    public static BeganGameAudioControl instance;


    //游戏灯光控制  开始时亮度较低
    private GameObject Light;
    public bool IslightBecome = false;
    private void Awake()
    {
        instance = this;
    }

    private AudioClip clip;
    private AudioClip clip1;
    private AudioClip clip2;

    public bool IsCanBegan = false;//管理整個游戲的開始，在聲音播放完成之後



    public float WaitAudioReady = 0;
    void Start ()
    {
        //开始就去播放倒计时
        StartCoroutine(PlayDaoJIshi());


        //灯光
        Light = GameObject.Find("Directional light");

        AudioOneQifeiQianJingBao = GameObject.Find("AudioOneQifeiQianJingBao").GetComponent<AudioSource>();
        AudioOneQifeiQiandaojishiyinxiao = GameObject.Find("AudioOneQifeiQiandaojishiyinxiao").GetComponent<AudioSource>();
        AudioOneQifeiQiandaojishi123 = GameObject.Find("AudioOneQifeiQiandaojishi123").GetComponent<AudioSource>();
    }

   
	
	void Update ()
    {

        //灯光控制

        if (IslightBecome)
        {
            Light.GetComponent<Light>().intensity = 1.5f;
        }
        else
        {
            Light.GetComponent<Light>().intensity = 1f;
        }


        if (IsEnableQifeiQiandaojishiyinxiao)
        {
            AudioOneQifeiQiandaojishiyinxiao.enabled = false;
        }
        else
        {
            AudioOneQifeiQiandaojishiyinxiao.enabled = true;
        }

        #region 飞船起飞前的几秒警报控制，和游戏前背景音效同时存在   由于现在只要呼吸的声音，这一个先关闭
        if (AudioControl.IsPlayBeganAudioClip == false)//此时点击了按钮开始准备游戏，开始播放倒计时
        {
            AudioOneQifeiQianJingBao.Stop();
            ISAudioOneQifeiQiandaojishiyinxiao = true;
        }
        if (AudioControl.IsPlayBeganAudioClip && !AudioOneQifeiQianJingBao.isPlaying)
        {
            AudioOneQifeiQianJingBao.Play();
        }
        #endregion


        if (ISAudioOneQifeiQiandaojishiyinxiao&& AudioOneQifeiQiandaojishiyinxiao.isPlaying==false)
        {
            
            AudioOneQifeiQiandaojishiyinxiao.Play();
            ISAudioOneQifeiQiandaojishiyinxiao = false;
          
        }


        WaitAudioReady += Time.deltaTime;
        if (WaitAudioReady >= 1F)
        {
            if ( !ReplayManager.IsAutoAttack && IsBenganIenumber && (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.JoystickButton0)))
            {
                SystemManager.Instance.IsCanPlayDaojishi = false;
                IsBenganIenumber = false;         //游戏重置时将Bool变为true
                //StartCoroutine(PlayDaoJIshi());

                print("进去协程里面去播放倒计时的声音");
            }
        }
    }
    IEnumerator PlayDaoJIshi()
    {
        yield return new WaitForSeconds(5f);
        clip = Resources.Load<AudioClip>("daosjishiThree");
        AudioOneQifeiQiandaojishi123.clip = clip;
        AudioOneQifeiQiandaojishi123.Play();

        yield return new WaitForSeconds(1.5f);
        AudioOneQifeiQiandaojishi123.Stop();
        clip1 = Resources.Load<AudioClip>("daosjishiTwo");
        AudioOneQifeiQiandaojishi123.clip = clip1;
        AudioOneQifeiQiandaojishi123.Play();

        yield return new WaitForSeconds(1.5f);
        AudioOneQifeiQiandaojishi123.Stop();
        clip2 = Resources.Load<AudioClip>("daosjishiOne");
        AudioOneQifeiQiandaojishi123.clip = clip2;
        AudioOneQifeiQiandaojishi123.Play();

        yield return new WaitForSeconds(1f);
        AudioOneQifeiQiandaojishi123.Stop();
        clip2 = Resources.Load<AudioClip>("planeflewAudio");
        AudioOneQifeiQiandaojishi123.clip = clip2;
        AudioOneQifeiQiandaojishi123.Play();


       
        IslightBecome = true;//灯光亮度控制
        

        IsEnableQifeiQiandaojishiyinxiao = true;  //游戏重置的时候开启


        IsCanBegan = true;//注意在这里的很多变量都需要在游戏重置时 重新赋值     //暂时没用
        //SystemManager.Instance.IsBegan = true;
        print("IsCanBeganIsCanBeganIsCanBeganIsCanBeganIsCanBegan"+ IsCanBegan);
    }

}
