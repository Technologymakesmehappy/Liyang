using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlDanyi:MonoBehaviour  {


    private float X;
    private float Y;
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
    public Transform m_pHand;
    private PlayerSvc Svc;
    private Vector3 currentPos = Vector3.zero;


    void Awake()
    {
        //InitiaPlayerSvc
        InitializePlayerSvc();

        if (!m_pHand)
            Debug.LogError("没有头部");
    }
    void LateUpdate()
    {
        if (m_pHand&&!isQuit)
        {
            if (IsShoot)
            {
                this.SendMotion(X + ShootForce, Y, -Y);

                ShootForce += (0 - ShootForce) / 10f;
            }
            else
            {
                X = ((m_pHand.localRotation * Vector3.forward).y) * 60;
                Y = ((m_pHand.localRotation * Vector3.forward).x) * 60;
                SendMotion(X, Y, -Y);
            }

        }
        if (!isQuit && Input.GetKeyDown(KeyCode.Escape))
          isQuit = true;


        if (isQuit)
        {
            MoveRester();
        }



    }
    public void SendMotion(float x, float y, float z)
    {
        //说明位置未变
        if ((x == currentPos.x) && (y == currentPos.y) && (z == currentPos.z))
            return;

        x = Mathf.Clamp(x, -70, 70);
        y = Mathf.Clamp(y, -60, 60);
        z = Mathf.Clamp(z, -60, 60);


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

        SendMotion(X, Y, -Y);

          
        
        if(X==0&&Y==0)
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
