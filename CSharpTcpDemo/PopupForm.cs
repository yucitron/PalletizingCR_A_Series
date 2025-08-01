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
    public partial class PopupForm : Form
    {
        // RobotConnectionManager singleton'dan referansları al
        private RobotConnectionManager robotManager;
        public Feedback mFeedback => robotManager?.Feedback;
        public Dashboard mDashboard => robotManager?.Dashboard;

        // Popup için kendi timer'ı
        private System.Timers.Timer mTimerReader = new System.Timers.Timer(300);

        public PopupForm()
        {
            InitializeComponent();

            // Singleton instance'ı al
            robotManager = RobotConnectionManager.Instance;

            // Log event'ine subscribe ol
            robotManager.LogReceived += OnLogReceived;

            SetupForm();
            SetupButtons();
            SetupTimer();
        }

        private void SetupButtons()
        {
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
        }

        private void SetupTimer()
        {
            mTimerReader.Elapsed += new System.Timers.ElapsedEventHandler(TimeoutEvent);
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
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.StartPosition = FormStartPosition.Manual;

            int screenRightEdge = Screen.PrimaryScreen.WorkingArea.Right;
            int formX = screenRightEdge - 500;
            int formY = 100;

            this.Location = new Point(formX, formY);
        }

        private void DoMoveJog(string str)
        {
            // RobotConnectionManager'ın ExecuteCommand metodunu kullan
            robotManager?.ExecuteCommand(() => {
                return mDashboard?.MoveJog(str) ?? "Dashboard not available";
            }, $"MoveJog({str})");
        }

        public void DoStopMoveJog()
        {
            // RobotConnectionManager'ın ExecuteCommand metodunu kullan
            robotManager?.ExecuteCommand(() => {
                return mDashboard?.StopMoveJog() ?? "Dashboard not available";
            }, "StopMoveJog()");
        }

        private void TimeoutEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!robotManager.IsConnected || mFeedback == null || !mFeedback.DataHasRead)
            {
                return;
            }

            // UI güncellemesi için Invoke kullan
            if ( this.InvokeRequired)
            {
                
                this.Invoke(new Action(() => {
                    ShowDataResult();
                }));
            }
            else
            {
                ShowDataResult();
            }

        }

        private void PopupForm_Load(object sender, EventArgs e)
        {
            // Bağlantı kontrolü
            if (!robotManager.IsConnected)
            {
                // Otomatik bağlanmayı dene
                robotManager.Connect();
            }

            // Timer'ı başlat
            mTimerReader.Start();
        }

        private void PopupForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Timer'ı durdur
            mTimerReader?.Stop();
            mTimerReader?.Close();

            // Log event'ından unsubscribe ol
            if (robotManager != null)
            {
                robotManager.LogReceived -= OnLogReceived;
            }
        }

        private void ShowDataResult()
        {
            // Form dispose kontrolü
            if (this.IsDisposed || this.Disposing)
                return;

            if (mFeedback?.feedbackData == null) return;

            try
            {
                // Joint pozisyonları
                if (null != mFeedback.feedbackData.QActual && mFeedback.feedbackData.QActual.Length >= 6)
                {
                    if (!this.IsDisposed)
                    {
                        this.labJ1.Text = string.Format("J1:{0:F3}", mFeedback.feedbackData.QActual[0]);
                        this.labJ2.Text = string.Format("J2:{0:F3}", mFeedback.feedbackData.QActual[1]);
                        this.labJ3.Text = string.Format("J3:{0:F3}", mFeedback.feedbackData.QActual[2]);
                        this.labJ4.Text = string.Format("J4:{0:F3}", mFeedback.feedbackData.QActual[3]);
                        this.labJ5.Text = string.Format("J5:{0:F3}", mFeedback.feedbackData.QActual[4]);
                        this.labJ6.Text = string.Format("J6:{0:F3}", mFeedback.feedbackData.QActual[5]);
                    }
                }

                // Kartezyen pozisyonları
                if (null != mFeedback.feedbackData.ToolVectorActual && mFeedback.feedbackData.ToolVectorActual.Length >= 6)
                {
                    if (!this.IsDisposed)
                    {
                        this.labX.Text = string.Format("X:{0:F3}", mFeedback.feedbackData.ToolVectorActual[0]);
                        this.labY.Text = string.Format("Y:{0:F3}", mFeedback.feedbackData.ToolVectorActual[1]);
                        this.labZ.Text = string.Format("Z:{0:F3}", mFeedback.feedbackData.ToolVectorActual[2]);
                        this.labRx.Text = string.Format("Rx:{0:F3}", mFeedback.feedbackData.ToolVectorActual[3]);
                        this.labRy.Text = string.Format("Ry:{0:F3}", mFeedback.feedbackData.ToolVectorActual[4]);
                        this.labRz.Text = string.Format("Rz:{0:F3}", mFeedback.feedbackData.ToolVectorActual[5]);
                    }
                }

                // Hata kontrolü
                if (!this.IsDisposed)
                {
                    ParseWarn();
                }
            }
            catch (ObjectDisposedException)
            {
                // Form dispose edilmişse timer'ı durdur
                mTimerReader?.Stop();
            }
            catch (Exception ex)
            {
                // Hata durumunda log'a yaz
                System.Diagnostics.Debug.WriteLine($"ShowDataResult error: {ex.Message}");
            }
        
        }

        private void ParseWarn()
        {
            if (mFeedback?.feedbackData == null || mDashboard == null) return;
            if (this.mFeedback.feedbackData.RobotMode != FeedbackData.ROBOT_MODE_ERROR) return;

            string strResult = mDashboard.GetErrorID();
            if (!strResult.StartsWith("0")) return;

            // Error parsing işlemi (orijinal koddan)
            int iBegPos = strResult.IndexOf('{');
            if (iBegPos < 0) return;
            int iEndPos = strResult.IndexOf('}', iBegPos + 1);
            if (iEndPos <= iBegPos) return;
            strResult = strResult.Substring(iBegPos + 1, iEndPos - iBegPos - 1);
            if (string.IsNullOrEmpty(strResult)) return;

            StringBuilder sb = new StringBuilder();
            JArray arrWarn = JArray.Parse(strResult);
            for (int i = 0; i < arrWarn.Count; ++i)
            {
                JArray arr = arrWarn[i].ToObject<JArray>();
                for (int j = 0; j < arr.Count; ++j)
                {
                    ErrorInfoBean bean = null;
                    if (0 == i)
                    {
                        bean = ErrorInfoHelper.FindController(arr[j].ToObject<int>());
                    }
                    else
                    {
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

                // Error'u popup olarak göster
                this.BeginInvoke(new Action(() => {
                    MessageBox.Show(this, strTime + "\r\n" + sb.ToString(),
                                  "Robot Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }));
            }

        

            if (sb.Length > 0)
            {
                DateTime dt = DateTime.Now;
                string strTime = string.Format("Time Stamp:{0}.{1}.{2} {3}:{4}:{5}", dt.Year,
                    dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);

                // Error'u popup olarak göster
                this.BeginInvoke(new Action(() => {
                    MessageBox.Show(this, strTime + "\r\n" + sb.ToString(),
                                  "Robot Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }));
            }
        }

        // Log mesajlarını almak için event handler
        private void OnLogReceived(string logMessage)
        {
            // İsterseniz popup'ta da log gösterebilirsiniz
            // Veya sadece debug için kullanabilirsiniz
            System.Diagnostics.Debug.WriteLine($"PopupForm Log: {logMessage}");
        }

        // Connection status'u kontrol etmek için yardımcı metod
        private void UpdateConnectionStatus()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(UpdateConnectionStatus));
                return;
            }

            // Bağlantı durumunu UI'da göstermek istiyorsanız
            string status = robotManager?.GetConnectionStatus() ?? "Unknown";
            this.Text = $"Robot Control - {status}";
        }

        // Timer ile periyodik bağlantı kontrolü
        private void CheckConnectionStatus()
        {
            robotManager?.CheckConnection();
            UpdateConnectionStatus();
        }

        private void PopupForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            mTimerReader?.Stop();
            mTimerReader?.Close();

            // Log event'ından unsubscribe ol
            if (robotManager != null)
            {
                robotManager.LogReceived -= OnLogReceived;
            }
        }
    }
}