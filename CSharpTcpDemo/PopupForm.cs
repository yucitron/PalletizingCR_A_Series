using CSharpTcpDemo.com.dobot.api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpTcpDemo
{
    public partial class PopupForm :Form
    {
        private Form1 form1 = new Form1();
        private MainForm mainForm = new MainForm();


        private Feedback mFeedback = new Feedback();
         private DobotMove mDobotMove = new DobotMove();
        private Dashboard mDashboard = new Dashboard();

        //定时获取数据并显示到UI
        private System.Timers.Timer mTimerReader = new System.Timers.Timer(300);



        public PopupForm()
        {
            InitializeComponent();
            SetupForm();

            mFeedback.NetworkErrorEvent += new DobotClient.OnNetworkError(mainForm.OnNetworkErrorEvent_Feedback);
            // mDobotMove.NetworkErrorEvent += new DobotClient.OnNetworkError(this.OnNetworkErrorEvent_DobotMove);
            mDashboard.NetworkErrorEvent += new DobotClient.OnNetworkError(mainForm.OnNetworkErrorEvent_Dashboard);

    
            #region -按钮事件
            BindBtn_MoveEvent(this.btnMinus1, "J1-");
            BindBtn_MoveEvent(this.btnMinus2, "J2-");
            BindBtn_MoveEvent(this.btnMinus3, "J3-");
            BindBtn_MoveEvent(this.btnMinus4, "J4-");
            BindBtn_MoveEvent(this.btnMinus5, "J5-");
            BindBtn_MoveEvent(this.btnMinus6, "J6-");
            #endregion

            #region XYZR+按钮事件
            BindBtn_MoveEvent(this.btnAddX, "X+");
            BindBtn_MoveEvent(this.btnAddY, "Y+");
            BindBtn_MoveEvent(this.btnAddZ, "Z+");
            BindBtn_MoveEvent(this.btnAddRX, "Rx+");
            BindBtn_MoveEvent(this.btnAddRY, "Ry+");
            BindBtn_MoveEvent(this.btnAddRZ, "Rz+");
            #endregion

            #region XYZR-按钮事件
            BindBtn_MoveEvent(this.btnMinusX, "X-");
            BindBtn_MoveEvent(this.btnMinusY, "Y-");
            BindBtn_MoveEvent(this.btnMinusZ, "Z-");
            BindBtn_MoveEvent(this.btnMinusRX, "Rx-");
            BindBtn_MoveEvent(this.btnMinusRY, "Ry-");
            BindBtn_MoveEvent(this.btnMinusRZ, "Rz-");
            #endregion

            //启动定时器
            mTimerReader.Elapsed += new System.Timers.ElapsedEventHandler(mainForm.TimeoutEvent);
            mTimerReader.AutoReset = true;

        }

        private void BindBtn_MoveEvent(Button btn, string strTag)
        {
            btn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMoveJogEvent);
            btn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnStopMoveJogEvent);
            btn.Tag = strTag;
        }

        private void OnMoveJogEvent(object sender, MouseEventArgs e)
        {
            if (sender is Button)
            {
                Button btn = (Button)sender;
                string str = btn.Tag.ToString();
                DoMoveJog(str);
            }
        }
        private void OnStopMoveJogEvent(object sender, MouseEventArgs e)
        {
            if (sender is Button)
            {
                Button btn = (Button)sender;
                mainForm.DoStopMoveJog();
            }
        }

        private void SetupForm()
        {
            // Form properties
            //this.Size = new Size(300, 400);
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
           
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.StartPosition = FormStartPosition.Manual;

            int screenRightEdge = Screen.PrimaryScreen.WorkingArea.Right;
            int formX = screenRightEdge - 500;
            int formY = 100; // dikey konum, ihtiyaca göre ayarlanabilir

            this.Location = new Point(formX, formY);

            
        }

        

        private void Btn1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Button 1 clicked!");
        }

        private void Btn2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Button 2 clicked!");
        }

        private void DoMoveJog(string str)
        {
            form1.PrintLog(string.Format("send to {0}:{1}: MoveJog({2})", mDashboard.IP, mDashboard.Port, str));
            Thread thd = new Thread(() => {
                string ret = mDobotMove.MoveJog(str);
                form1.PrintLog(string.Format("Receive From {0}:{1}: {2}", mDashboard.IP, mDashboard.Port, ret));
            });
            thd.Start();
        }

        
            

        private void PopupForm_Load(object sender, EventArgs e)
        {
           
        }
    }

}

