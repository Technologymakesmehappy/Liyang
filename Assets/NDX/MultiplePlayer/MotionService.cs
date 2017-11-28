using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;


    public class MotionService
    {
        int localPort = 8410;
        int remotePort = 7408;
        string remoteIp = "127.0.0.1";
        UdpClient udp = null;
        IPEndPoint endpoint = null;
        bool stopped = false;
        bool reseting = false;
        MotionCmd lastCmd = null;
        EffectCmd lastCmd2 = null;
        int num1, num2, num3, num4;
        uint time = 0;

        public event EventHandler Received;
        public event EventHandler ErrorOccur;
        //日志
        private StreamWriter log = null;
        public int EffectLog { get; set; }
        /// <summary>
        /// 复位的单位距离,默认2
        /// </summary>
        public int ResetUnit { get; set; }
        public void SetLogPath(string logPath)
        {
            if (string.IsNullOrEmpty(logPath) == false && log == null)
            {
                log = new StreamWriter(logPath, true, Encoding.UTF8);
                log.AutoFlush = true;
            }
        }

        internal void OnLog(string msg)
        {
            if (log != null)
            {
                try
                {
                    log.WriteLine(DateTime.Now.ToString("HH:mm:ss fff\t") + "INFO: " + msg);
                }
                catch { }
            }
        }

        internal void OnError(string msg)
        {
            if (log != null)
            {
                try
                {
                    log.WriteLine(DateTime.Now.ToString("HH:mm:ss fff\t") + "ERROR: " + msg);
                }
                catch { }
            }
            ErrorOccur.Invoke(msg, null);
        }

        public MotionService(int port)
        {
            this.localPort = port;
        }

        public int Init(string ip, int port = 7408, int num1 = 256, int num2 = 150, int num3 = 5, int num4 = 10000, int axis = 6)
        {
            this.remoteIp = ip;
            this.remotePort = port;
            this.num1 = num1;
            this.num2 = num2;
            this.num3 = num3;
            this.num4 = num4;
            MotionCmd.RATE = 1.0 * num2 / num1 * num4 / num3;
            if (MotionCmd.RATE < 0)
            {
                return 1;
            }
       
            return 0;
        }

        public int Start()
        {
            if (udp == null)
            {
                try
                {
                UnityEngine.Debug.Log("1");
                    udp = new UdpClient(new IPEndPoint(IPAddress.Any, localPort));
                    endpoint = new IPEndPoint(IPAddress.Parse(remoteIp), remotePort);
                }
                catch (Exception ex)
                {
                    OnLog("Motion service start fail." + ex.Message);
                    return 1;
                }
            }
            /*
            if (recvThreed == null || recvThreed.ThreadState == ThreadState.Aborted || recvThreed.ThreadState == ThreadState.Stopped)
            {
                recvThreed = new Thread(new ThreadStart(RecvThread));
                recvThreed.IsBackground = true;
                recvThreed.Start();
            }
            */
            stopped = false;
            OnLog("Motion service started.");
            return 0;
        }

        void RecvThread()
        {
            while (stopped == false)
            {
                try
                {
                    byte[] buf = udp.Receive(ref endpoint);
                    if (buf.Length >= 0 && stopped == false)
                    {
                        Received.Invoke(ByteArrayToHexStringNoBlank(buf), null);
                    }
                }
                catch
                {

                }
                Thread.Sleep(100);
            }
        }

        public int SendHex(string hexData)
        {
            try
            {
                byte[] bs = HexStringToByteArray(hexData);
                udp.Send(bs, bs.Length, endpoint);
                return 0;
            }
            catch
            {
                return 1;
            }
        }

        public int Send(byte[] bs)
        {
            udp.Send(bs, bs.Length, endpoint);
            return 0;
        }

        public int Send(MotionCmd cmd)
        {
            lastCmd = cmd;
            if (EffectLog == 1)
            {
                
            }
            if(lastCmd2 != null) //获取最后一条特效指令
            {
                cmd.BaseDout = lastCmd2.Dout;
            }
            return Send(cmd.ToBytes());
        }

        public int Send(EffectCmd cmd)
        {
            lastCmd2 = cmd;
            if (EffectLog == 1)
            {
               
            }
            return Send(cmd.ToBytes());
        }
        
        public int MoveAbsThree(uint time, int x, int y, int z)
        {
            return Send(MotionCmd.MoveThree(time, x, y, z));
        }

        public int MoveAbsSix(uint time, int x, int y, int z, int u, int v, int w)
        {
            return Send(MotionCmd.MoveSix(time, x, y, z, u, v, w));
        }
        
        public int EffectReset()
        {
            if(lastCmd2 != null)
            {
                lastCmd2.Dout = 0;
            }
            lastCmd2 = null;
            SendHex("55AA000012010002FFFFFFFF000100010000");
            return 0;
        }

        public int MoveReset()
        {
            if (lastCmd == null)
            {
                SendHex("55aa000013010002ffff00000000000100000000000000000000000000000000000000000000000000000000000000000000000012345678abcd");
                SendHex("55aa000012010002ffffffff000000010000");
                return 0;
            }
            OnLog("start reset...");
            int per = ResetUnit;
            if (per <= 0)
            {
                per = 2;
            }
            if (reseting)
                return 0;

            reseting = true;
            while (lastCmd.X != 0 || lastCmd.Y != 0 || lastCmd.Z != 0 || lastCmd.U != 0 || lastCmd.V != 0 || lastCmd.W != 0 || lastCmd.A != 0 || lastCmd.B != 0 || lastCmd.C != 0 || lastCmd.D != 0)
            {
                time += 50;
                lastCmd.Time = time;
                if (lastCmd.X != 0)
                {
                    if (lastCmd.X > 0)
                    {
                        lastCmd.X -= per; if (lastCmd.X < 0) lastCmd.X = 0;
                    }
                    else
                    {
                        lastCmd.X += per; if (lastCmd.X > 0) lastCmd.X = 0;
                    }
                }
                if (lastCmd.Y != 0)
                {
                    if (lastCmd.Y > 0)
                    {
                        lastCmd.Y -= per; if (lastCmd.Y < 0) lastCmd.Y = 0;
                    }
                    else
                    {
                        lastCmd.Y += per; if (lastCmd.Y > 0) lastCmd.Y = 0;
                    }
                }
                if (lastCmd.Z != 0)
                {
                    if (lastCmd.Z > 0)
                    {
                        lastCmd.Z -= per; if (lastCmd.Z < 0) lastCmd.Z = 0;
                    }
                    else
                    {
                        lastCmd.Z += per; if (lastCmd.Z > 0) lastCmd.Z = 0;
                    }
                }
                if (lastCmd.U != 0)
                {
                    if (lastCmd.U > 0)
                    {
                        lastCmd.U -= per; if (lastCmd.U < 0) lastCmd.U = 0;
                    }
                    else
                    {
                        lastCmd.U += per; if (lastCmd.U > 0) lastCmd.U = 0;
                    }
                }
                if (lastCmd.V != 0)
                {
                    if (lastCmd.V > 0)
                    {
                        lastCmd.V -= per; if (lastCmd.V < 0) lastCmd.V = 0;
                    }
                    else
                    {
                        lastCmd.V += per; if (lastCmd.V > 0) lastCmd.V = 0;
                    }
                }
                if (lastCmd.W != 0)
                {
                    if (lastCmd.W > 0)
                    {
                        lastCmd.W -= per; if (lastCmd.W < 0) lastCmd.W = 0;
                    }
                    else
                    {
                        lastCmd.W += per; if (lastCmd.W > 0) lastCmd.W = 0;
                    }
                }
                if (lastCmd.A != 0)
                {
                    if (lastCmd.A > 0)
                    {
                        lastCmd.A -= per; if (lastCmd.A < 0) lastCmd.A = 0;
                    }
                    else
                    {
                        lastCmd.A += per; if (lastCmd.A > 0) lastCmd.A = 0;
                    }
                }
                if (lastCmd.B != 0)
                {
                    if (lastCmd.B > 0)
                    {
                        lastCmd.B -= per; if (lastCmd.B < 0) lastCmd.B = 0;
                    }
                    else
                    {
                        lastCmd.B += per; if (lastCmd.B > 0) lastCmd.B = 0;
                    }
                }
                if (lastCmd.C != 0)
                {
                    if (lastCmd.C > 0)
                    {
                        lastCmd.C -= per; if (lastCmd.C < 0) lastCmd.C = 0;
                    }
                    else
                    {
                        lastCmd.C += per; if (lastCmd.C > 0) lastCmd.C = 0;
                    }
                }
                if (lastCmd.D != 0)
                {
                    if (lastCmd.D > 0)
                    {
                        lastCmd.D -= per; if (lastCmd.D < 0) lastCmd.D = 0;
                    }
                    else
                    {
                        lastCmd.D += per; if (lastCmd.D > 0) lastCmd.D = 0;
                    }
                }
                lastCmd.BaseDout = 0;
                Send(lastCmd);
                Thread.Sleep(50);
            }
            Send(lastCmd);//发多一次0点位置
            Thread.Sleep(100);
            lastCmd = null;
            reseting = false;
            OnLog("reset completed.");
            return 0;
        }

        public int Stop()
        {
            stopped = true;
          //  udp.Close();
            //udp = null;
            OnLog("Motion service stopped.");
            if (log != null)
            {
                log.Close();
            }
        if (PlayerSvc.Instance != null)
            PlayerSvc.Instance.Close();
            return 0;
        }

        byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }

        /// <summary>
        /// 字节数组转换成十六进制字符串(不带空格)
        /// </summary>
        /// <param name="data">要转换的字节数组</param>
        /// <returns>转换后的字符串</returns>
        string ByteArrayToHexStringNoBlank(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
            return sb.ToString().ToUpper();
        }
    }

    public class MotionCmd
    {
        public ushort ConfirmCode = 0x55aa;
        public ushort PassCode = 0;
        public ushort FunctionCode = 0x1301;
        public ushort ObjectChannel = 0;
        public ushort WhoAccept = 0xffff;
        public ushort WhoReply = 0;
        public uint Line = 1;
        public uint Time = 0;
        public int X = 0;
        public int Y = 0;
        public int Z = 0;
        public int U = 0;
        public int V = 0;
        public int W = 0;
        public int A = 0;
        public int B = 0;
        public int C = 0;
        public int D = 0;
        public ushort BaseDout = 0;
        public uint DAC = 0x5678abcd;
        private byte[] bytes = null;
        public static double RATE = 1.0;
        public MotionCmd()
        {
        }

        public static MotionCmd MoveThree(uint time, int x, int y, int z)
        {
            return new MotionCmd { ObjectChannel = 0, Time = time, X = x, Y = y, Z = z };
        }

        public static MotionCmd MoveFour(uint time, int x, int y, int z, int u)
        {
            return new MotionCmd { ObjectChannel = 1, Time = time, X = x, Y = y, Z = z, U = u };
        }

        public static MotionCmd MoveSix(uint time, int x, int y, int z, int u, int v, int w)
        {
            return new MotionCmd { ObjectChannel = 1, Time = time, X = x, Y = y, Z = z, U = u, V = v, W = w };
        }

        public static MotionCmd MoveTen(uint time, int x, int y, int z, int u, int v, int w, int a, int b, int c, int d)
        {
            return new MotionCmd { ObjectChannel = 2, Time = time, X = x, Y = y, Z = z, U = u, V = v, W = w, A = a, B = b, C = c, D = d };
        }

        public int ConvertValue(int value)
        {
            return Convert.ToInt32(Math.Floor(value * RATE));
        }

        public byte[] ToBytes()
        {
            byte[] bs = null;
            if (ObjectChannel == 0)
            {
                bs = new byte[38];
            }
            else
            {
                if (ObjectChannel == 1)
                {
                    bs = new byte[50];
                }
                else
                {
                    bs = new byte[66];
                }
            }
            int idx = 0;
            BitConverter.GetBytes(ConfirmCode).Reverse().ToArray().CopyTo(bs, idx);
            idx += 2;
            BitConverter.GetBytes(PassCode).Reverse().ToArray().CopyTo(bs, idx);
            idx += 2;
            BitConverter.GetBytes(FunctionCode).Reverse().ToArray().CopyTo(bs, idx);
            idx += 2;
            BitConverter.GetBytes(ObjectChannel).Reverse().ToArray().CopyTo(bs, idx);
            idx += 2;
            BitConverter.GetBytes(WhoAccept).Reverse().ToArray().CopyTo(bs, idx);
            idx += 2;
            BitConverter.GetBytes(WhoReply).Reverse().ToArray().CopyTo(bs, idx);
            idx += 2;
            BitConverter.GetBytes(Line).Reverse().ToArray().CopyTo(bs, idx);
            idx += 4;
            BitConverter.GetBytes(Time).Reverse().ToArray().CopyTo(bs, idx);
            idx += 4;
            BitConverter.GetBytes(ConvertValue(X)).Reverse().ToArray().CopyTo(bs, idx);
            idx += 4;
            BitConverter.GetBytes(ConvertValue(Y)).Reverse().ToArray().CopyTo(bs, idx);
            idx += 4;
            BitConverter.GetBytes(ConvertValue(Z)).Reverse().ToArray().CopyTo(bs, idx);
            idx += 4;
            if (ObjectChannel > 0)
            {
                BitConverter.GetBytes(ConvertValue(U)).Reverse().ToArray().CopyTo(bs, idx);
                idx += 4;
                BitConverter.GetBytes(ConvertValue(V)).Reverse().ToArray().CopyTo(bs, idx);
                idx += 4;
                BitConverter.GetBytes(ConvertValue(W)).Reverse().ToArray().CopyTo(bs, idx);
                idx += 4;
            }
            if (ObjectChannel > 1)
            {
                BitConverter.GetBytes(ConvertValue(A)).Reverse().ToArray().CopyTo(bs, idx);
                idx += 4;
                BitConverter.GetBytes(ConvertValue(B)).Reverse().ToArray().CopyTo(bs, idx);
                idx += 4;
                BitConverter.GetBytes(ConvertValue(C)).Reverse().ToArray().CopyTo(bs, idx);
                idx += 4;
                BitConverter.GetBytes(ConvertValue(D)).Reverse().ToArray().CopyTo(bs, idx);
                idx += 4;
            }
            BitConverter.GetBytes(BaseDout).Reverse().ToArray().CopyTo(bs, idx);
            idx += 2;
            BitConverter.GetBytes(DAC).Reverse().ToArray().CopyTo(bs, idx);

            bytes = bs;
            return bytes;
        }
    }

    public class EffectCmd
    {
        private string Prefix = "55AA000012010002FFFFFFFF00010001";
        public double Time;
        public int R1 = 0;
        public int R2 = 0;
        public int R3 = 0;
        public int R4 = 0;
        public int R5 = 0;
        public int R6 = 0;
        public int R7 = 0;
        public int R8 = 0;
        public int R9 = 0;
        public int R10 = 0;
        public int R11 = 0;
        public int R12 = 0;

        public ushort Dout = 0;

        public byte[] ToBytes()
        {
            string hexStr = Prefix;
            string rstr = "0000" + R12 + R11 + R10 + R9 + R8 + R7 + R6 + R5 + R4 + R3 + R2 + R1;
            rstr = rstr.Replace("-1", "0");
            string tx = C2to16(rstr);
            rstr = null;
            hexStr += tx;
            Dout = BitConverter.ToUInt16(HexStringToByteArray(tx).Reverse().ToArray(), 0);
            tx = null;
            return HexStringToByteArray(hexStr);
        }

        string C2to16(string code2)
        {
            string code16 = "";
            for (int i = 0; i < code2.Length; i += 4)
            {
                int b2 = Convert.ToInt32(code2.Substring(i, 4), 2);
                string b16 = string.Format("{0:X}", b2);
                code16 += b16;
            }
            return code16;
        }

        byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }
    }

