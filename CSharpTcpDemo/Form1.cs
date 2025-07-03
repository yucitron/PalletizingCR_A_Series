using CSharpTcpDemo.com.dobot.api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CSharthiscpDemo.com.dobot.api;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CSharpTcpDemo
{
    public partial class Form1 : Form
    {
        public Feedback mFeedback = new Feedback();
         public DobotMove mDobotMove = new DobotMove();
        public Dashboard mDashboard = new Dashboard();

        //定时获取数据并显示到UI
        private System.Timers.Timer mTimer = new System.Timers.Timer(300);
        private System.Timers.Timer mTimerReader = new System.Timers.Timer(300);


        //MainFrom AÇMAK

        private MainForm mainForm = new MainForm();
        private Palletizing palletform = new Palletizing();

        private static Form1 _instance;

        public RobotConnectionManager robotManager;

        public static Form1 Instance
        {
            get
            {
                if (_instance == null || _instance.IsDisposed)
                {
                    _instance = new Form1();
                }
                return _instance;
            }
        }

        public Form1()
        {
            InitializeComponent();
            _instance = this;
            robotManager = RobotConnectionManager.Instance;

            robotManager.LogReceived += WriteToLog;
        }

        public void WriteToLog(string message)
        {
            if (string.IsNullOrEmpty(message)) return;

            if (richTextBoxLog.InvokeRequired)
            {
                richTextBoxLog.Invoke(new Action<string>(WriteToLog), message);
            }
            else
            {
                InsertLogToRichBox(richTextBoxLog, message);
            }
        }

        private void InsertLogToRichBox(RichTextBox box, string str)
        {
            if (box.GetLineFromCharIndex(box.TextLength) > 100)
            {
                box.Text = str + "\r\n";
            }
            else
            {
                box.Text += str + "\r\n";
            }
            box.SelectionStart = box.Text.Length;
            box.ScrollToCaret();
        }

        





        private void btnConnect_Click(object sender, EventArgs e)
        {
            robotManager.Connect();
            

        }
        public void Connect2()
        {
            string strIp = "192.168.5.1";
            int iPortFeedback = 30004;
            int iPortMove = 30003;
            int iPortDashboard = 29999;

            PrintLog("Connecting...");
            Thread thd = new Thread(() => {
                if (!mDashboard.Connect(strIp, iPortDashboard))
                {
                    PrintLog(string.Format("Connect {0}:{1} Fail!!", strIp, iPortDashboard));
                    return;
                }
                if (!mDobotMove.Connect(strIp, iPortMove))
                {
                    PrintLog(string.Format("Connect {0}:{1} Fail!!", strIp, iPortMove));
                    return;
                }
                if (!mFeedback.Connect(strIp, iPortFeedback))
                {
                    PrintLog(string.Format("Connect {0}:{1} Fail!!", strIp, iPortFeedback));
                    return;
                }

               
                mTimerReader.Start();

                PrintLog("Connect Success!!!");


                //  EnableWindow();
               // this.btnConnect.Text = "Disconnect";

            });
            thd.Start();
        }


        private void enbbtn_Click(object sender, EventArgs e)
        {
            if (!robotManager.IsConnected)
            {
                WriteToLog("Robot bağlı değil!");
                return;
            }

            bool bEnable = this.enbbtn.Text.Equals("Enable");
            var dashboard = robotManager.Dashboard;

            WriteToLog($"send to {dashboard.IP}:{dashboard.Port}: {(bEnable ? "EnableRobot" : "DisableRobot")}()");

            Thread thd = new Thread(() => {
                string ret = bEnable ? dashboard.EnableRobot() : dashboard.DisableRobot();
                bool bOk = ret.StartsWith("0");

                this.enbbtn.Invoke(new Action(() => {
                    if (bOk)
                    {
                        this.enbbtn.Text = bEnable ? "Disable" : "Enable";
                    }
                }));

                WriteToLog($"Receive From {dashboard.IP}:{dashboard.Port}: {ret}");
            });
            thd.Start();
        }

        private void rstbtn_Click(object sender, EventArgs e)
        {
            if (!robotManager.IsConnected)
            {
                WriteToLog("Robot bağlı değil!");
                return;
            }

            var dashboard = robotManager.Dashboard;
            WriteToLog($"send to {dashboard.IP}:{dashboard.Port}: ResetRobot()");

            Thread thd = new Thread(() => {
                string ret = dashboard.ResetRobot();
                WriteToLog($"Receive From {dashboard.IP}:{dashboard.Port}: {ret}");
            });
            thd.Start();
        }

        private void clrErrbtn_Click(object sender, EventArgs e)
        {
            if (!robotManager.IsConnected)
            {
                WriteToLog("Robot bağlı değil!");
                return;
            }

            var dashboard = robotManager.Dashboard;
            WriteToLog($"send to {dashboard.IP}:{dashboard.Port}: ClearError()");

            Thread thd = new Thread(() => {
                string ret = dashboard.ClearError();
                WriteToLog($"Receive From {dashboard.IP}:{dashboard.Port}: {ret}");
            });
            thd.Start();
        }

        /*public void InsertLogToRichBox(RichTextBox box, string str)
        {
            if (box.GetLineFromCharIndex(box.TextLength) > 100)
            {
                box.Text = (str += "\r\n");
            }
            else
            {
                box.Text += (str + "\r\n");
            }
            box.Focus();
            box.Select(box.TextLength, 0);
            box.ScrollToCaret();
        }*/

        public void PrintLog(string str)
        {
            WriteToLog(str);
        }
        public void PrintErrorInfo(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return;
            }
            if (this.richTextBoxErrInfo.InvokeRequired)
            {
                this.richTextBoxErrInfo.Invoke(new Action<string>(log => {
                    InsertLogToRichBox(this.richTextBoxErrInfo, log);
                }), str);
            }
            else
            {
                InsertLogToRichBox(this.richTextBoxErrInfo, str);
            }
        }

        public void pltbtn_Click(object sender, EventArgs e)
        {
            if (palletform == null || palletform.IsDisposed)
            {
                palletform = new Palletizing();
            }

            this.Hide();
            palletform.Show();
        }

        public void boxbtn_Click(object sender, EventArgs e)
        {
            Boxes boxesForm = new Boxes();
            this.Hide();
            boxesForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (palletform != null && !palletform.IsDisposed)
            {
                palletform.btnMove_Click(sender, e);
            }
        }

        private void mainbtn_Click(object sender, EventArgs e)
        {
            if (mainForm == null || mainForm.IsDisposed)
            {
                mainForm = new MainForm();
            }

            this.Hide();
            mainForm.Show();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            robotManager.LogReceived -= WriteToLog;
            base.OnFormClosed(e);
        }
                   
    }
}
