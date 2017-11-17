using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlDanyi:MonoBehaviour  {


    private float X;
    private float Y;
    private float Z;
    private float ShootForce = 0;
    private float CurrentX;
    private float ResDanyiPosSpeed = 0.3f;
    private bool isShoot;
    private bool isQuit = false;
    public bool isplayer = false;
    private bool IsShoot
    {
        get { return ShootForce > 1; }

    }
    //public Transform m_pHand;
    private PlayerSvc Svc;
    private Vector3 currentPos = Vector3.zero;


    void Awake()
    {
        //InitiaPlayerSvc
        InitializePlayerSvc();

        //if (!m_pHand)
        //    Debug.LogError("没有头部");
    }
    //private bool IsBegan = false;
    void LateUpdate()
    { 
        if (/*m_pHand&&*/!isQuit)
        {
            if (IsShoot)
            {

                this.SendMotion(-Z + X + ShootForce, Z + X+ ShootForce, Y);

                ShootForce += (0 - ShootForce) / 10f;
            }
            else
            {
                //X = ((m_pHand.localRotation * Vector3.forward).y) * 80;
                //Y = ((m_pHand.localRotation * Vector3.forward).x) * 80;
                //Z = ((m_pHand.localRotation ).z) * 80;
                //print(string.Format("X:{0},Y:{1},Z:{2}", X, Y, Z));
                Y = Input.GetAxis("Horizontal")*30;  //左右移动
                X = -Input.GetAxis("Vertical")*18;   //上下移动
                Z = 0;

                SendMotion(Z+ X,-Z+X , Y);
            }

        }
        if (!isQuit && Input.GetKeyDown(KeyCode.Escape))
          isQuit = true;


        if (isQuit)
        {
            MoveRester();
        }



    }

    void SendMotion() { SendMotion(-Z + X, Z + X, Y); }
    
       
    
    public void SendMotion(float x, float y, float z)
    {
        //说明位置未变
        if ((x == currentPos.x) && (y == currentPos.y) && (z == currentPos.z))
            return;

        x = Mathf.Clamp(x, -30, 80);
        y = Mathf.Clamp(y, -80, 80);
        z = Mathf.Clamp(z, -95, 95);


        int result = Svc.SendMotionPercent(x, y, z);


    }
    /// <summary>
    /// 外界调用座椅震动
    /// </summary>
    public void Fire()
    {
        ShootForce = 35;


    }

    void InitializePlayerSvc()
    {
        //刚刚开始游戏
        if (Svc == null)
            Svc = PlayerSvc.Instance;

        if (Svc == null)
            Debug.LogError("PlayerSvc  is Null");

        if (Svc.InitMotion()) { Debug.Log("Init Ok"); }
        else
            Debug.LogError("Init Error");

    }

    /// <summary>
    /// 复位
    /// </summary>
    /// <returns></returns>
    void MoveRester()
    {
       
          if (X > 0)
            {
                X -= ResDanyiPosSpeed;
                if (X < 0)
                    X = 0;

          }else if (X < 0)
            {
                X += ResDanyiPosSpeed;
                if (X > 0)
                    X = 0;
            }

        if (Y > 0)
        {
            Y -= ResDanyiPosSpeed;
            if (Y < 0)
                Y = 0;

        }
        else if (Y < 0)
        {
            Y += ResDanyiPosSpeed;
            if (Y > 0)
                Y = 0;
        }


        if (Z > 0)
        {
            Z -= ResDanyiPosSpeed;
            if (Z < 0)
                Z = 0;

        }
        else if (Z < 0)
        {
            Z += ResDanyiPosSpeed;
            if (Z > 0)
                Z = 0;
        }
        SendMotion();




        if (X==0&&Y==0&&Z==0)
        StartCoroutine(  Close());
    }


  
    /// <summary>
    /// 复位结束后调用
    /// </summary>
    IEnumerator Close()
    {
        //释放资源
        if (Svc != null)
            Svc.Close();
        yield return new WaitForSecondsRealtime(1);
        print("Quit");
#if !UNITY_EDITOR
        Application.Quit();
#endif
    }
}
