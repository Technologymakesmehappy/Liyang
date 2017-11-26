using UnityEngine;
using System.Collections;
using LitJson;

using System.Collections.Generic;
using System;

public class RankLogin
{

    #region class
   

    #endregion

    #region 字段
    private string macStr;
    private string ip;
    private int gameId;
    private bool isLoad = false;
    private Coroutine _cououtine;

    #region Public
    public string monitorId = "";
    public string gameVersion = "1.0.0";
    #endregion
    #endregion


    public void Init()
    {
        //用一个mo来启动 LoginLeaderBoardServer
        gameId = RankManager.instance.Id;
        if (monitorId.Length == 0)
        {
            //_cououtine= RankManager.instance.StartCoroutine(LoginLeaderBoardServer());
        }
        else
        {
            if (_cououtine != null)
            {
                RankManager.instance.StopCoroutine(_cououtine);
                _cououtine = null;
            }
        }
     
    }

    //IEnumerator LoginLeaderBoardServer()
    //{
      

    //    WWW getIP = new WWW("http://lovattostudio.com/Utils/GetIP.php");
    //    yield return getIP;

    //    if (getIP.error != null && !string.IsNullOrEmpty(getIP.error))
    //        Debug.LogError("GetIP Error:" + getIP.error);

    //    ip = getIP.text;

    //    macStr = GetIMEI();

    //    var www = new WWW(string.Format("http://ucenter.pangaeavr.com:8080/user-center/login?version={0}&gameId={1}&ip={2}&mac={3}", gameVersion, gameId, ip, macStr));

    //    yield return www;
    //    if (www.error != null && !string.IsNullOrEmpty(www.error))
    //        Debug.LogError("LoginLeaderBoardServer Error:" + www.error);

    //    try
    //    {
    //        var globalData = JsonMapper.ToObject<RankGlobalData>(www.text);
    //        Debug.Log("MonitorId = " + globalData.monitorId);
    //        monitorId = globalData.monitorId;
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.LogError(e.Message);
    //    }

      
      
      
    //}

    private string GetIMEI()
    {
        string imei = SystemInfo.deviceUniqueIdentifier;
        //try
        //{

        //    if (Application.platform == RuntimePlatform.Android)
        //    {
        //        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        //        AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject>("currentActivity");
        //        AndroidJavaClass contextClass = new AndroidJavaClass("android.content.Context");
        //        string TELEPHONY_SERVICE = contextClass.GetStatic<string>("TELEPHONY_SERVICE");
        //        AndroidJavaObject telephonyService = activity.Call<AndroidJavaObject>("getSystemService", TELEPHONY_SERVICE);
        //        bool noPermission = false;

        //        try
        //        {
        //            if (telephonyService.Call<string>("getDeviceId").Trim() != "" && telephonyService.Call<string>("getDeviceId").Trim().Length > 0)
        //                imei = telephonyService.Call<string>("getDeviceId");
        //        }
        //        catch (System.Exception e)
        //        {
        //            noPermission = true;
        //        }
        //    }

        //    Debug.Log("IMEI: " + imei);
        //}
        //catch (System.Exception exc)
        //{
        //    Debug.Log(exc.ToString());
        //}
        if(imei.Length>50)
        {
            imei = imei.Substring(0, 50);
        }

        return imei;
    }


}
