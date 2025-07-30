using CSharpTcpDemo.com.dobot.api;
using Newtonsoft.Json.Linq;
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


            #region +按钮事件
            BindBtn_MoveEvent(this.btnAdd1, "J1+");
            BindBtn_MoveEvent(this.btnAdd2, "J2+");
            BindBtn_MoveEvent(this.btnAdd3, "J3+");
            BindBtn_MoveEvent(this.btnAdd4, "J4+");
            BindBtn_MoveEvent(this.btnAdd5, "J5+");
            BindBtn_MoveEvent(this.btnAdd6, "J6+");
            #endregion

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
                DoStopMoveJog();
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

        

       

        private void DoMoveJog(string str)
        {
            form1.PrintLog(string.Format("send to {0}:{1}: MoveJog({2})", mDashboard.IP, mDashboard.Port, str));
            Thread thd = new Thread(() => {
                string ret = mDashboard.MoveJog(str);
                form1.PrintLog(string.Format("Receive From {0}:{1}: {2}", mDashboard.IP, mDashboard.Port, ret));
            });
            thd.Start();
        }
        public void DoStopMoveJog()
        {
            form1.PrintLog(string.Format("send to {0}:{1}: MoveJog()", mDashboard.IP, mDashboard.Port));
            Thread thd = new Thread(() => {
                string ret = mDashboard.StopMoveJog();
                form1.PrintLog(string.Format("Receive From {0}:{1}: {2}", mDashboard.IP, mDashboard.Port, ret));
            });
            thd.Start();
        }



        private void PopupForm_Load(object sender, EventArgs e)
        {
           
        }

        private void btnMinus1_Click(object sender, EventArgs e)
        {
             
            MessageBox.Show("Button 1 clicked!");
        
        }
        private void ShowDataResult()
        {
            

            if (null != mFeedback.feedbackData.QActual && mFeedback.feedbackData.QActual.Length >= 6)
            {
                this.labJ1.Text = string.Format("J1:{0:F3}", mFeedback.feedbackData.QActual[0]);
                this.labJ2.Text = string.Format("J2:{0:F3}", mFeedback.feedbackData.QActual[1]);
                this.labJ3.Text = string.Format("J3:{0:F3}", mFeedback.feedbackData.QActual[2]);
                this.labJ4.Text = string.Format("J4:{0:F3}", mFeedback.feedbackData.QActual[3]);
                this.labJ5.Text = string.Format("J5:{0:F3}", mFeedback.feedbackData.QActual[4]);
                this.labJ6.Text = string.Format("J6:{0:F3}", mFeedback.feedbackData.QActual[5]);
            }

            if (null != mFeedback.feedbackData.ToolVectorActual && mFeedback.feedbackData.ToolVectorActual.Length >= 6)
            {
                this.labX.Text = string.Format("X:{0:F3}", mFeedback.feedbackData.ToolVectorActual[0]);
                this.labY.Text = string.Format("Y:{0:F3}", mFeedback.feedbackData.ToolVectorActual[1]);
                this.labZ.Text = string.Format("Z:{0:F3}", mFeedback.feedbackData.ToolVectorActual[2]);
                this.labRx.Text = string.Format("Rx:{0:F3}", mFeedback.feedbackData.ToolVectorActual[3]);
                this.labRy.Text = string.Format("Ry:{0:F3}", mFeedback.feedbackData.ToolVectorActual[4]);
                this.labRz.Text = string.Format("Rz:{0:F3}", mFeedback.feedbackData.ToolVectorActual[5]);
            }

            

            ParseWarn();
        }

        private void ParseWarn()
        {
            if (this.mFeedback.feedbackData.RobotMode != FeedbackData.ROBOT_MODE_ERROR) return;
            string strResult = mDashboard.GetErrorID();
            //strResult=ErrorID,{[[id,...,id], [id], [id], [id], [id], [id], [id]]},GetErrorID()
            if (!strResult.StartsWith("0")) return;

            //截取第一个{}内容
            int iBegPos = strResult.IndexOf('{');
            if (iBegPos < 0) return;
            int iEndPos = strResult.IndexOf('}', iBegPos + 1);
            if (iEndPos <= iBegPos) return;
            strResult = strResult.Substring(iBegPos + 1, iEndPos - iBegPos - 1);
            if (string.IsNullOrEmpty(strResult)) return;

            //剩余7组[]，第1组是控制器报警，其他6组是伺服报警
            StringBuilder sb = new StringBuilder();
            JArray arrWarn = JArray.Parse(strResult);
            for (int i = 0; i < arrWarn.Count; ++i)
            {
                JArray arr = arrWarn[i].ToObject<JArray>();
                for (int j = 0; j < arr.Count; ++j)
                {
                    ErrorInfoBean bean = null;
                    if (0 == i)
                    {//控制器报警
                        bean = ErrorInfoHelper.FindController(arr[j].ToObject<int>());
                    }
                    else
                    {//伺服报警
                        bean = ErrorInfoHelper.FindServo(arr[j].ToObject<int>());
                    }
                    if (null != bean)
                    {
                        sb.Append("ID:" + bean.id + "\r\n");
                        sb.Append("Type:" + bean.Type + "\r\n");
                        sb.Append("Level:" + bean.level + "\r\n");
                        sb.Append("Solution:" + bean.en.solution + "\r\n");
                    }
                }
            }

            if (sb.Length > 0)
            {
                DateTime dt = DateTime.Now;
                string strTime = string.Format("Time Stamp:{0}.{1}.{2} {3}:{4}:{5}", dt.Year,
                    dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
              
            }
            return;
        }
    }

}

