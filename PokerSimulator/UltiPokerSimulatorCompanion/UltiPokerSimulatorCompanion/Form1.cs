using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace UltiPokerSimulatorCompanion
{
    public partial class Truck : Form
    {
        public Truck()
        {
            InitializeComponent();
            TextBox.CheckForIllegalCrossThreadCalls = false;
        }

        #region 扑克牌界面
        static Poker[] poker = new Poker[27];
        static Table table = new Table();
        static Dealer dealer = new Dealer();
        static Player p2 = new Player(2);

        private void btBegin_Click(object sender, EventArgs e)
        {
            PreConnection();//先要获得消息
            DisplayButton();
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
            if (preclicked != null && (preclicked != (sender as PictureBox).Name))//缩小的条件,1.点过某张牌2.点的牌不与之前相同
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
            int id = p.ActivatePoker().ID;
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
            for (int i = this.Controls.Count - 1; i > 0; i--)
            {
                if (this.Controls[i] is PictureBox)
                    this.Controls.Remove(this.Controls[i]);
            }
        }

        ////对传输过来的流信息进行第一次翻译
        private Poker[] TranslatePoker(string presentMsg, int invalid)
        {
            Poker[] realsuit = new Poker[27];
            string[] strArray = presentMsg.Split(',');
            if (presentMsg != null)
            {
                for (int i = 0; i < 27; i++)
                {
                    realsuit[i] = new Poker(int.Parse(strArray[i]));//写法特殊注意！！
                }
            }
            //把最后那个密码加到桌上
            int index1 = presentMsg.LastIndexOf(",");
            int index2 = presentMsg.LastIndexOf("#");
            int length = index2 - index1-1;
            string value = presentMsg.Substring(index1+1, length);
            Poker p = new Poker(int.Parse(value));
            table.GainPoker(p);

            return realsuit;
        }

        private void WaitOpponent()
        {
            /*收到消息后立马更新桌面牌情况*/
            table = dealer.UpdateObsoleteTable(presentMsg);
            ErasePokers();
            DrawPokers(table);
            DrawPresentPoker(p2);
        }

        static int iniGo = 1;

        private void btGo_Click(object sender, EventArgs e)
        {
            if (iniGo == 0) { btGo.Enabled = false; btWaitOp.Enabled = true; }

            //模拟第一次刚接收到信息测试用-可删
            if (iniReceiveMsg == 1)
            {
                poker = TranslatePoker(presentMsg,1);
                dealer.Deal(poker, p2);//发牌
                iniReceiveMsg = 0;

                ErasePokers();
                DrawPokers(table);
                DrawPresentPoker(p2);
                lbProperty.Text = p2.CountPoker().ToString();
                ResetMagnify();
                iniGo = 0;
            }
            else
            {
                if (p2.CountPoker() != 0)
                {
                    //if (iniGo != 1) btWaitOp_Click(sender, e);
                    dealer.Amuse(table, p2, clickedPoker);
                    ErasePokers();
                    DrawPokers(table);
                    DrawPresentPoker(p2);
                    lbProperty.Text = p2.CountPoker().ToString();
                    ResetMagnify();
                    iniGo = 0;
                }
                else
                {
                    //输了
                    this.Close();
                }
            }
            ResetLoop();
            //发送信息-可删
            ClientSendMsg(Encrypt(table.DisplayPoker()));

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

        //创建 1个客户端套接字 和1个负责监听服务端请求的线程  
        Socket socketClient = null;
        Thread threadClient = null;

        private void PreConnection()
        {
            //定义一个套字节监听  包含3个参数(IP4寻址协议,流式连接,TCP协议)
            socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //需要获取文本框中的IP地址
            IPAddress ipaddress = IPAddress.Parse(txtIP.Text.Trim());
            //将获取的ip地址和端口号绑定到网络节点endpoint上
            IPEndPoint endpoint = new IPEndPoint(ipaddress, int.Parse(txtPort.Text.Trim()));
            //这里客户端套接字连接到网络节点(服务端)用的方法是Connect 而不是Bind
            socketClient.Connect(endpoint);
            //创建一个线程 用于监听服务端发来的消息
            threadClient = new Thread(RecMsg);
            //将窗体线程设置为与后台同步
            threadClient.IsBackground = true;
            //启动线程
            threadClient.Start();

        }

        static string presentMsg = null;

        /// <summary>
        /// 接收服务端发来信息的方法
        /// </summary>
        private void RecMsg()
        {
            while (true) //持续监听服务端发来的消息
            {
                //定义一个1M的内存缓冲区 用于临时性存储接收到的信息
                byte[] arrRecMsg = new byte[1024 * 1024];
                //将客户端套接字接收到的数据存入内存缓冲区, 并获取其长度
                int length = socketClient.Receive(arrRecMsg);
                //将套接字获取到的字节数组转换为人可以看懂的字符串
                string strRecMsg = Encoding.UTF8.GetString(arrRecMsg, 0, length);

                presentMsg = strRecMsg;//自己加的

                //将发送的信息追加到聊天内容文本框中
                txtMsg.AppendText("Moore:" + GetCurrentTime() + "\r\n" + strRecMsg + "\r\n");
                //btWaitOp_Click(null, null);
            }
        }

        //记录是否为初次接收信息
        static private int iniReceiveMsg = 1;

        //对拥有的牌进行消息转换以便发送
        private string Encrypt(int[] presentPossession)
        {
            StringBuilder sb = new StringBuilder("");
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
                return sb.ToString();
            }
        }

        /// <summary>
        /// 发送字符串信息到服务端的方法
        /// </summary>
        /// <param name="sendMsg">发送的字符串信息</param>
        private void ClientSendMsg(string sendMsg)
        {
            if (sendMsg == null) sendMsg = "-1";
            //将输入的内容字符串转换为机器可以识别的字节数组
            byte[] arrClientSendMsg = Encoding.UTF8.GetBytes(sendMsg);
            //调用客户端套接字发送字节数组
            socketClient.Send(arrClientSendMsg);
            //将发送的信息追加到聊天内容文本框中
            txtMsg.AppendText("Ego:" + GetCurrentTime() + "\r\n" + sendMsg + "\r\n");
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
