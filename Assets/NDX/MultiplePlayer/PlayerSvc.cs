using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using NDX;

    public delegate void NdxEventHandler(string args);
public class Json
{
    public string Axis;
    public string NUM1;
    public string NUM2;
    public string NUM3;
    public string NUM4;
    public string MaxNUM;
    public string IP;


}
    public class PlayerSvc
    {
        public static PlayerSvc Instance = new PlayerSvc();

        public event NdxEventHandler GamePlay;
        public event NdxEventHandler GameStop;
        private EffectService effectSvc;
        
        public int Init(string appId)
        {
            return PlayerSvcDLL.Init(appId);
        }

        public int CheckApp(string appKey)
        {
            return PlayerSvcDLL.CheckApp(appKey);
        }

        public int StartPlay()
        {
            return PlayerSvcDLL.StartPlay();
        }

        public int StopPlay()
        {
            return PlayerSvcDLL.StopPlay();
        }

        public void Close()
        {
            PlayerSvcDLL.Close();
        }

        public string GetLanguage()
        {
            return Marshal.PtrToStringAnsi(PlayerSvcDLL.GetLanguage());
        }

        public int RunOnNDX()
        {
            return PlayerSvcDLL.RunOnNDX();
        }
        
        public int WaitPlay()
        {
            return PlayerSvcDLL.WaitPlay((a) =>
            {
                if (GamePlay != null)
                {
                    GamePlay(a);
                }
            });
        }
        
        public int WaitStop()
        {
            return PlayerSvcDLL.WaitStop((a) =>
            {
                if (GameStop != null)
                {
                    GameStop(a);
                }
            });
        }
        
        public string EncryptMd5(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            MD5 md1 = MD5.Create();
            byte[] buffer2 = md1.ComputeHash(bytes);
            md1.Clear();
            StringBuilder builder = new StringBuilder();
            foreach (byte num2 in buffer2)
            {
                builder.Append(num2.ToString("X2"));
            }
            return builder.ToString();
        }

        public int SendMotion(float x, float y, float z)
        {
            if (effectSvc != null)
            {
                return effectSvc.SendMotionCmd(new GameMotionCmd
                {
                    x = x,
                    y = y,
                    z = z
                });
            }
            else
            {
                return PlayerSvcDLL.SendMotion(x, y, z, 0, 0, 0, 0, 0, 0, 0);
            }
        }

        public int SendMotionPercent(float x, float y, float z)
        {
            if (effectSvc != null)
            {
                return effectSvc.SendMotionCmd(new GameMotionCmd
                {
                    Type = 1,
                    x = x,
                    y = y,
                    z = z
                });
            }
            else
            {
                return PlayerSvcDLL.SendMotionPercent(x, y, z, 0, 0, 0, 0, 0, 0, 0);
            }
        }
        
        public int SendEffect(int r1, int r2, int r3, int r4, int r5, int r6, int r7, int r8, int r9, int r10, int r11, int r12)
        {
            if (effectSvc != null)
            {
                return effectSvc.SendEffectCmd(new EffectCmd
                {
                    R1 = r1,
                    R2 = r2,
                    R3 = r3,
                    R4 = r4,
                    R5 = r5,
                    R6 = r6,
                    R7 = r7,
                    R8 = r8,
                    R9 = r9,
                    R10 = r10,
                    R11 = r11,
                    R12 = r12
                });
            }
            else
            {
                return PlayerSvcDLL.SendEffect(r1, r2, r3, r4, r5, r6, r7, r8, r9, r10, r11, r12);
            }
        }

        public bool InitMotion()
        {
         
            string path=   UnityEngine.Application.dataPath + "/StreamingAssets/Setting.txt";
         FileStream stream = new FileStream(path, FileMode.Open);

        StreamReader reader = new StreamReader(stream);
       

         Json _json = UnityEngine.JsonUtility.FromJson<Json>( reader.ReadToEnd());
         
         
                try
                {
            string axis = _json.Axis; ;
            string num1 = _json.NUM1; ;
            string num2 = _json.NUM2; ;
            string num3 = _json.NUM3; ;
            string num4 = _json.NUM4; ;
            string maxNum = _json.MaxNUM; ;
            string ip = _json.IP; ;
            
                    effectSvc = new EffectService();
                    effectSvc.SetConfig(new MotionConfig {
                        Axis = int.Parse(axis),
                        NUM1 = int.Parse(num1),
                        NUM2 = int.Parse(num2),
                        NUM3 = int.Parse(num3),
                        NUM4 = int.Parse(num4),
                        MaxNUM = int.Parse(maxNum),
                        IP = ip
                    });
        
              effectSvc.Start();
            return true;
                }catch(Exception ex)
                {
                    return false;
                }       
        }
    }

    /// <summary>
    /// NDX提供的Native DLL方法集合
    /// </summary>
    public class PlayerSvcDLL
    {
        public const string DLLFile = "PlayerSvc";
        [DllImport(DLLFile)]
        public static extern int Init(string appId);
        [DllImport(DLLFile)]
        public static extern int CheckApp(string appKey);
        [DllImport(DLLFile)]
        public static extern int StartPlay();
        [DllImport(DLLFile)]
        public static extern int StopPlay();
        [DllImport(DLLFile)]
        public static extern void Close();
        [DllImport(DLLFile)]
        public static extern IntPtr GetLanguage();
        [DllImport(DLLFile)]
        public static extern int RunOnNDX();
        [DllImport(DLLFile)]
        public static extern int WaitPlay([MarshalAs(UnmanagedType.FunctionPtr)]PlayCallBack fn);
        [DllImport(DLLFile)]
        public static extern int WaitStop([MarshalAs(UnmanagedType.FunctionPtr)]PlayCallBack fn);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void PlayCallBack(string args);

        [DllImport(DLLFile)]
        public static extern int SendMotion(float x, float y, float z, float u, float v, float w, float a, float b, float c, float d);
        [DllImport(DLLFile)]
        public static extern int SendMotionPercent(float x, float y, float z, float u, float v, float w, float a, float b, float c, float d);
        [DllImport(DLLFile)]
        public static extern int SendEffect(int r1, int r2, int r3, int r4, int r5, int r6, int r7, int r8, int r9, int r10, int r11, int r12);

        [DllImport(DLLFile)]
        public static extern IntPtr GetMotionConfig();
    }

