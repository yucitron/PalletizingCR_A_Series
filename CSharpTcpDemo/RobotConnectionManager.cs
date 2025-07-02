using CSharpTcpDemo.com.dobot.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpTcpDemo
{
    public  class RobotConnectionManager
    {
        private static readonly object _lock = new object();
        private static RobotConnectionManager _instance = null;

        // Robot bağlantı nesneleri
        private Feedback mFeedback;
        private DobotMove mDobotMove;
        private Dashboard mDashboard;

        // Timer'lar
        private System.Timers.Timer mTimer;
        private System.Timers.Timer mTimerReader;

        // Bağlantı durumu
        public bool IsConnected { get; private set; } = false;

        // Singleton instance
        public static RobotConnectionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new RobotConnectionManager();
                    }
                }
                return _instance;
            }
        }

        private RobotConnectionManager()
        {
            InitializeConnections();
        }

        private void InitializeConnections()
        {
            mFeedback = new Feedback();
            mDobotMove = new DobotMove();
            mDashboard = new Dashboard();
            mTimer = new System.Timers.Timer(300);
            mTimerReader = new System.Timers.Timer(300);
        }

        /// <summary>
        /// Robot'a bağlan
        /// </summary>
        public void Connect()
        {
            if (IsConnected) return;

            string strIp = "192.168.5.1";
            int iPortFeedback = 30004;
            int iPortMove = 30003;
            int iPortDashboard = 29999;

            LogMessage("Connecting...");

            Thread connectionThread = new Thread(() => {
                try
                {
                    if (!mDashboard.Connect(strIp, iPortDashboard))
                    {
                        LogMessage($"Connect {strIp}:{iPortDashboard} Fail!!");
                        return;
                    }

                    if (!mDobotMove.Connect(strIp, iPortMove))
                    {
                        LogMessage($"Connect {strIp}:{iPortMove} Fail!!");
                        return;
                    }

                    if (!mFeedback.Connect(strIp, iPortFeedback))
                    {
                        LogMessage($"Connect {strIp}:{iPortFeedback} Fail!!");
                        return;
                    }

                    mTimerReader.Start();
                    IsConnected = true;
                    LogMessage("Connect Success!!!");
                }
                catch (Exception ex)
                {
                    LogMessage($"Connection Error: {ex.Message}");
                }
            });
            connectionThread.Start();
        }

        /// <summary>
        /// Robot bağlantısını kes
        /// </summary>
        public void Disconnect()
        {
            if (!IsConnected) return;

            try
            {
                mTimerReader?.Stop();
                mTimer?.Stop();

                // Bağlantıları kapat (API'nizde disconnect metodları varsa)
                // mDashboard?.Disconnect();
                // mDobotMove?.Disconnect();
                // mFeedback?.Disconnect();

                IsConnected = false;
                LogMessage("Disconnected from robot");
            }
            catch (Exception ex)
            {
                LogMessage($"Disconnect Error: {ex.Message}");
            }
        }

        // Robot komutları için property'ler
        public Dashboard Dashboard => mDashboard;
        public DobotMove DobotMove => mDobotMove;
        public Feedback Feedback => mFeedback;

        // Log event'i
        public event Action<string> LogReceived;

        private void LogMessage(string message)
        {
            LogReceived?.Invoke($"[{DateTime.Now:HH:mm:ss}] {message}");
        }

        /// <summary>
        /// Uygulama kapanırken resources'ları temizle
        /// </summary>
        public void Cleanup()
        {
            Disconnect();
            mTimer?.Dispose();
            mTimerReader?.Dispose();
        }
    }
}


