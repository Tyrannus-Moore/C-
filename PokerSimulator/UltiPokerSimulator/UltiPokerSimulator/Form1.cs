using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace UltiPokerSimulator
{
    public partial class Truck : Form
    {
        public Truck()
        {
            InitializeComponent();
            TextBox.CheckForIllegalCrossThreadCalls = false;
        }

        #region 扑克牌界面
        static Poker[] poker = new Poker[54];
        static Table table = new Table();
        static Dealer dealer = new Dealer();
        static Player p1 = new Player(1);
        static Player p2 = new Player(2);

        private void btBegin_Click(object sender, EventArgs e)
        {
            dealer.InitialPoker(poker);//洗牌
            dealer.Deal(poker, p1, p2);//发牌
            DisplayButton();
            PreConnection();
        }

        //游戏开始按钮变化
        private void DisplayButton()
        {
            btBegin.Visible = false;
            btGo.Visible = true;
            btWaitOp.Visible = true;
            lbProperty.Visible = true;
        }

        //游戏结束按钮变化
        private void HiddeButton()
        {
            btBegin.Visible = true;
            btGo.Visible = false;
            btWaitOp.Visible = false;
            lbProperty.Visible = false;
        }

        //动态调用Resourse资源,隶属于DrawPokers()
        private System.Drawing.Image GetResourceImage(string strImageName)
        {
            object obj = Properties.Resources.ResourceManager.GetObject(strImageName, Properties.Resources.Culture);
            return ((System.Drawing.Image)(obj));
        }

        //记录之前被点中的牌
        static string preclicked = null;
        //记录被点中的那张扑克
        static Poker clickedPoker = new Poker(-1);

        //重置点中状态，隶属于btGo
        private void ResetMagnify()
        {
            preclicked = null;
            clickedPoker.ID = -1;
        }

        //动态画玩桌面上扑克牌后追加的事件,隶属于DrawPokers()
        private void Magnify(object sender, EventArgs e)
        {
            /*放大某张牌之前先缩小之前点的牌*/
            if (preclicked != null && (preclicked != (sender as PictureBox).Name) )//缩小的条件,1.点过某张牌2.点的牌不与之前相同
            {
                for (int i = this.Controls.Count - 1; i > 0; i--)
                {
                    if (this.Controls[i] is PictureBox && this.Controls[i].Name == preclicked)
                    {
                        this.Controls[i].Size = new System.Drawing.Size(72, 95);
                    }
                }
            }

            /*放大/缩小点中的牌*/
            Size norm = new System.Drawing.Size(72, 95);
            Size magn = new System.Drawing.Size(108, 142);
            if (norm == (sender as PictureBox).Size)
            {
                (sender as PictureBox).Size = new System.Drawing.Size(108, 142);
                
                //将点击的图片的名称的数字保存在clickedPoker变量中记录
                int id = int.Parse((sender as PictureBox).Name.Replace("pokers", ""));
                clickedPoker.ID = id;
                preclicked = (sender as PictureBox).Name;
            }
            else
            {
                (sender as PictureBox).Size = new System.Drawing.Size(72, 95);
                clickedPoker.ID = -1;
                preclicked = null;
            }

        }

        //动态画牌
        private void DrawPokers(Table table)
        {
            if (table != null && table.DisplayPoker() != null)
            {
                int[] abstractPoker = table.DisplayPoker();//获得牌号
                PictureBox[] pokers = new PictureBox[table.CountPoker()];
                for (int i = 0; i < pokers.Length; i++)
                {
                    pokers[i] = new PictureBox();
                    pokers[i].Image = GetResourceImage(abstractPoker[i].ToString());//动态加载图片方法
                    pokers[i].InitialImage = null;
                    pokers[i].Location = new System.Drawing.Point(370, 10 + 27 * i);//每个生成的牌向下移动27以便露出大小
                    pokers[i].Name = "pokers" + abstractPoker[i];
                    pokers[i].Size = new System.Drawing.Size(72, 95);
                    pokers[i].SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                    pokers[i].TabIndex = 1;
                    pokers[i].TabStop = false;
                    pokers[i].Click += new EventHandler(Magnify);
                }
                this.Controls.AddRange(pokers);
                //改变图片的cascading顺序
                for (int i = pokers.Length - 1; i >= 0; i--)
                {
                    pokers[i].SendToBack();
                }
            }
        }

        //动态画自己当前手上的牌
        private void DrawPresentPoker(Player p)
        {
            int id=p.ActivatePoker().ID;
            PictureBox present = new PictureBox();
            present.Image = GetResourceImage(id.ToString());//动态加载图片方法
            present.InitialImage = null;
            present.Location = new System.Drawing.Point(530, 360);
            present.Name = "pokers" + id;
            present.Size = new System.Drawing.Size(72, 95);
            present.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            present.TabIndex = 1;
            present.TabStop = false;

            this.Controls.Add(present);
        }

        //动态擦去桌上扑克牌
        private void ErasePokers()
        {
            for (int i = this.Controls.Count - 1; i > 0; i --)
            {
                if (this.Controls[i] is PictureBox)
                    this.Controls.Remove(this.Controls[i]);
            }
        }

        private void WaitOpponent()
        {
            /*收到消息后立马更新桌面牌情况*/
            table = dealer.UpdateObsoleteTable(presentMsg);
            ErasePokers();
            DrawPokers(table);
            DrawPresentPoker(p1);
        }

        static int iniGo = 1;

        private void btGo_Click(object sender, EventArgs e)
        {
            if (p1.CountPoker() != 0)
            {
                if(iniGo!=1)WaitOpponent();
                dealer.Amuse(table, p1, clickedPoker);
                ErasePokers();
                DrawPokers(table);
                DrawPresentPoker(p1);
                lbProperty.Text = p1.CountPoker().ToString();
                ResetMagnify();
            }
            else
            {
                //输了
                this.Close();
            }
            ResetLoop();
            //发送信息-可删
            if (iniSendMsg == 0)
            {
                ServerSendMsg(Encrypt(table.DisplayPoker()));
            }
            else if (iniSendMsg == 1)
            {
                ServerSendMsg(Encrypt(p2.DisplayPoker(),table.ActivatePoker().ID));
                iniSendMsg = 0;
            }
            if (iniGo == 1) { iniGo = 0; btGo.Enabled = false; btWaitOp.Enabled = true; }
            else
            {
                btGo.Enabled = false;
                btWaitOp.Enabled = true;
            }
            timer1.Stop();
            ResetLoop();
            timer2.Start();
            lbWait.Visible = true;
            btWaitOp.Visible = false;
        }

        //计录已经到多少秒
        static int loop = 5;

        //重置计时
        private void ResetLoop()
        {
            loop = 5;
        }

        //剩余选择时间
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (loop != 0)
            {
                loop--;
                lbTimer.Text = loop.ToString();
            }
            else
            {
                loop = 5;
                lbTimer.Text = null;
                btGo_Click(sender, e);
                btGo.Enabled = false;
                timer1.Stop();
            }
        }
        #endregion


        #region 网络部分
        Thread threadWatch = null; //负责监听客户端的线程
        Socket socketWatch = null; //负责监听客户端的套接字

        private void PreConnection()
        {
            //定义一个套接字用于监听客户端发来的信息  包含3个参数(IP4寻址协议,流式连接,TCP协议)
            socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //服务端发送信息 需要1个IP地址和端口号
            IPAddress ipaddress = IPAddress.Parse(txtIP.Text.Trim()); //获取文本框输入的IP地址
            //将IP地址和端口号绑定到网络节点endpoint上 
            IPEndPoint endpoint = new IPEndPoint(ipaddress, int.Parse(txtPORT.Text.Trim())); //获取文本框上输入的端口号
            //监听绑定的网络节点
            socketWatch.Bind(endpoint);
            //将套接字的监听队列长度限制为20
            socketWatch.Listen(20);
            //创建一个监听线程 
            threadWatch = new Thread(WatchConnecting);
            //将窗体线程设置为与后台同步
            threadWatch.IsBackground = true;
            //启动线程
            threadWatch.Start();
            //启动线程后 txtMsg文本框显示相应提示
            txtMsg.AppendText("等待对方连入..." + "\r\n");
        }

        //创建一个负责和客户端通信的套接字 
        Socket socConnection = null;

        /// <summary>
        /// 监听客户端发来的请求
        /// </summary>
        private void WatchConnecting()
        {
            while (true)  //持续不断监听客户端发来的请求
            {
                socConnection = socketWatch.Accept();
                txtMsg.AppendText("客户端连接成功" + "\r\n");
                //创建一个通信线程 
                ParameterizedThreadStart pts = new ParameterizedThreadStart(ServerRecMsg);
                Thread thr = new Thread(pts);
                thr.IsBackground = true;
                //启动线程
                thr.Start(socConnection);
            }
        }

        //记录是否初次发信息
        private int iniSendMsg = 1;

        //对拥有的牌进行初次消息转换以便发送
        private string Encrypt(int[] presentPossession,int table)
        {
            StringBuilder sb=new StringBuilder("");
            string newsb = null;
            if (presentPossession.Length == 0)
            {
                return null;
            }
            else
            {
                for (int i = 0; i < presentPossession.Length; i++)
                {
                    sb.Append(Convert.ToString(presentPossession[i]));
                    sb.Append(",");
                }
                sb.Append(table);
                sb.Append("#");//作用方便取出最后桌上的牌
                newsb = sb.ToString();
                return newsb;
            }
        }

        //非初次发送消息 对旧方法重载
        private string Encrypt(int[] presentPossession)
        {
            StringBuilder sb = new StringBuilder("");
            string newsb = null;
            if (presentPossession.Length == 0)
            {
                return null;
            }
            else
            {
                for (int i = 0; i < presentPossession.Length; i++)
                {
                    sb.Append(Convert.ToString(presentPossession[i]));
                    sb.Append(",");
                }


                newsb = sb.ToString();
                newsb = newsb.Remove(newsb.Length - 1, 1);
                return newsb;
            }
        }


        /// <summary>
        /// 发送信息到客户端的方法
        /// </summary>
        /// <param name="sendMsg">发送的字符串信息</param>
        private void ServerSendMsg(string sendMsg)
        {
            if (sendMsg == null) sendMsg = "-1";
            //将输入的字符串转换成 机器可以识别的字节数组
            byte[] arrSendMsg = Encoding.UTF8.GetBytes(sendMsg);
            //向客户端发送字节数组信息
            socConnection.Send(arrSendMsg);
            //将发送的字符串信息附加到文本框txtMsg上
            txtMsg.AppendText("Moore:" + GetCurrentTime() + "\r\n" + sendMsg + "\r\n");
        }

        /// <summary>
        /// 获取当前系统时间的方法
        /// </summary>
        /// <returns>当前时间</returns>
        private DateTime GetCurrentTime()
        {
            DateTime currentTime = new DateTime();
            currentTime = DateTime.Now;
            return currentTime;
        }

        static string presentMsg = null;
        /// <summary>
        /// 接收客户端发来的信息 
        /// </summary>
        /// <param name="socketClientPara">客户端套接字对象</param>
        private void ServerRecMsg(object socketClientPara)
        {
            Socket socketServer = socketClientPara as Socket;
            while (true)
            {
                //创建一个内存缓冲区 其大小为1024*1024字节  即1M
                byte[] arrServerRecMsg = new byte[1024 * 1024];
                //将接收到的信息存入到内存缓冲区,并返回其字节数组的长度
                int length = socketServer.Receive(arrServerRecMsg);
                //将机器接受到的字节数组转换为人可以读懂的字符串
                string strSRecMsg = Encoding.UTF8.GetString(arrServerRecMsg, 0, length);

                presentMsg = strSRecMsg;//自己加的

                //将发送的字符串信息附加到文本框txtMsg上  
                txtMsg.AppendText("Ego:" + GetCurrentTime() + "\r\n" + strSRecMsg + "\r\n");
                //btWaitOp_Click(null, null);
            }
        }
        #endregion

        private void btWaitOp_Click(object sender, EventArgs e)
        {
            WaitOpponent();
            btGo.Enabled = true;
            btWaitOp.Enabled = false;
            timer1.Start();
        }

        static int opLoop = 10;
        //等待对手出牌的十秒
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (opLoop != 0)
            {
                opLoop--;
            }
            else
            {
                timer2.Stop();
                opLoop = 10;
                lbWait.Visible = false;
                btWaitOp.Visible = true;
            }
        }
    }
}
