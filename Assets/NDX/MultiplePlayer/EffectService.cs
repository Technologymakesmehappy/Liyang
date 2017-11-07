using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

using System.Xml;

namespace NDX
{
    public class MotionConfig
    {
        public int Axis { get; set; }
        public string IP { get; set; }
        public int MaxNUM { get; set; }
        public int NUM1 { get; set; }
        public int NUM2 { get; set; }
        public int NUM3 { get; set; }
        public int NUM4 { get; set; }
    }
    /// <summary>
    /// 动作特效服务
    /// </summary>
    public class EffectService
    {
        MotionService svc = null;
        MotionConfig cfg = null;

        public void SetConfig(MotionConfig cfg)
        {
            this.cfg = cfg;
        }
        public EffectService()
        {
        }

        public void Start()
        {
            if (svc == null)
            {
                svc = new MotionService(8410);
                svc.SetLogPath(new FileInfo("effect.log").FullName);
                svc.EffectLog = 1;
                svc.ResetUnit = 2;
                string mip = cfg.IP;
                svc.Init(mip, 7408, cfg.NUM1, cfg.NUM2, cfg.NUM3, cfg.NUM4, cfg.Axis);
            }
            svc.Start();
        }

        public void Stop()
        {
            svc.Stop();
        }
        
        public int SendMotionCmd(GameMotionCmd cmd)
        {
            if(cmd.Type == 1 && cfg.MaxNUM > 0)
            {
                cmd.x = cfg.MaxNUM * 1.0f * cmd.x / 100;
                cmd.y = cfg.MaxNUM * 1.0f * cmd.y / 100;
                cmd.z = cfg.MaxNUM * 1.0f * cmd.z / 100;
                cmd.u = cfg.MaxNUM * 1.0f * cmd.u / 100;
                cmd.v = cfg.MaxNUM * 1.0f * cmd.v / 100;
                cmd.w = cfg.MaxNUM * 1.0f * cmd.w / 100;
                cmd.a = cfg.MaxNUM * 1.0f * cmd.a / 100;
                cmd.b = cfg.MaxNUM * 1.0f * cmd.b / 100;
                cmd.c = cfg.MaxNUM * 1.0f * cmd.c / 100;
                cmd.d = cfg.MaxNUM * 1.0f * cmd.d / 100;
            }
            MotionCmd mcmd = cmd.ToMotionCmd(cfg.Axis);
            int result = svc.Send(mcmd);
            return result;
        }

        public int SendEffectCmd(EffectCmd cmd)
        {
            int result = svc.Send(cmd);
            return result;
        }
    }
    
    public class GameMotionCmd
    {
        public int Type = 0;//0,data,1:percent
        public float x = 0f;
        public float y = 0f;
        public float z = 0f;
        public float u = 0f;
        public float v = 0f;
        public float w = 0f;
        public float a = 0f;
        public float b = 0f;
        public float c = 0f;
        public float d = 0f;

        public MotionCmd ToMotionCmd(int axis)
        {
            switch (axis)
            {
                case 3: return MotionCmd.MoveThree(0, Convert.ToInt32(Math.Round(x)), Convert.ToInt32(Math.Round(y)), Convert.ToInt32(Math.Round(z)));
                case 4: return MotionCmd.MoveFour(0, Convert.ToInt32(Math.Round(x)), Convert.ToInt32(Math.Round(y)), Convert.ToInt32(Math.Round(z)), Convert.ToInt32(Math.Round(u)));
                case 6: return MotionCmd.MoveSix(0, Convert.ToInt32(Math.Round(x)), Convert.ToInt32(Math.Round(y)), Convert.ToInt32(Math.Round(z)), Convert.ToInt32(Math.Round(u)), Convert.ToInt32(Math.Round(v)), Convert.ToInt32(Math.Round(w)));
                case 10: return MotionCmd.MoveTen(0, Convert.ToInt32(Math.Round(x)), Convert.ToInt32(Math.Round(y)), Convert.ToInt32(Math.Round(z)), Convert.ToInt32(Math.Round(u)), Convert.ToInt32(Math.Round(v)), Convert.ToInt32(Math.Round(w)), Convert.ToInt32(Math.Round(a)), Convert.ToInt32(Math.Round(b)), Convert.ToInt32(Math.Round(c)), Convert.ToInt32(Math.Round(d)));
            }
            return null;
        }
    }
}
