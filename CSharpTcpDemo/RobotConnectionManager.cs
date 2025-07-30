using CSharpTcpDemo.com.dobot.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpTcpDemo
{
    public class RobotConnectionManager
    {
        #region Singleton Pattern Implementation

        // Thread-safe singleton için lock object
        private static readonly object _lock = new object();

        // Tek instance'ı saklar
        private static RobotConnectionManager _instance = null;

        // Singleton instance property - Double-checked locking pattern
        public static RobotConnectionManager Instance
        {
            get
            {
                if (_instance == null) // İlk kontrol (performance için)
                {
                    lock (_lock) // Thread safety için lock
                    {
                        if (_instance == null) // İkinci kontrol (thread safety için)
                            _instance = new RobotConnectionManager();
                    }
                }
                return _instance;
            }
        }

        // Private constructor - dışarıdan instance oluşturulmasını engeller
        private RobotConnectionManager()
        {
            InitializeConnections();
        }

        #endregion

        #region Robot Connection Objects

        // Robot bağlantı nesneleri - private field'lar
        private Feedback mFeedback;
        
        private Dashboard mDashboard;

        // Timer'lar - bağlantı süresince aktif kalacak
        private System.Timers.Timer mTimer;
        private System.Timers.Timer mTimerReader;

        // Bağlantı durumu
        public bool IsConnected { get; private set; } = false;

        // Robot komponentlerine erişim için public property'ler
        public Dashboard Dashboard => mDashboard;
       

        public Feedback Feedback => mFeedback;

        #endregion

        #region Connection Management

        /// <summary>
        /// Robot bağlantı nesnelerini initialize et
        /// </summary>
        private void InitializeConnections()
        {
            mFeedback = new Feedback();
           
            mDashboard = new Dashboard();
            mTimer = new System.Timers.Timer(300);
            mTimerReader = new System.Timers.Timer(300);

            // Timer event'lerini bağla
            SetupTimers();
        }

        /// <summary>
        /// Timer'ları yapılandır
        /// </summary>
        private void SetupTimers()
        {
            mTimerReader.Elapsed += (sender, e) => {
                // Feedback okuma işlemi
                if (IsConnected && mFeedback != null)
                {
                    try
                    {
                        // Feedback verilerini oku
                        // mFeedback.GetFeedbackData(); // API'ye göre düzenleyin
                    }
                    catch (Exception ex)
                    {
                        LogMessage($"Feedback reading error: {ex.Message}");
                    }
                }
            };

            mTimer.Elapsed += (sender, e) => {
                // Diğer periyodik işlemler
                if (IsConnected)
                {
                    // Status kontrolü vb.
                }
            };
        }

        /// <summary>
        /// Robot'a bağlan
        /// </summary>
        public void Connect()
        {
            if (IsConnected)
            {
                LogMessage("Robot already connected!");
                return;
            }

            string strIp = "192.168.5.1";
            int iPortFeedback = 30004;
            int iPortMove = 30003;
            int iPortDashboard = 29999;

            LogMessage("Connecting to robot...");

            Thread connectionThread = new Thread(() => {
                try
                {
                    // Dashboard bağlantısı
                    if (!mDashboard.Connect(strIp, iPortDashboard))
                    {
                        LogMessage($"Dashboard connection failed: {strIp}:{iPortDashboard}");
                        return;
                    }
                    LogMessage($"Dashboard connected: {strIp}:{iPortDashboard}");

                    // Move bağlantısı
                    if (!mDashboard.Connect(strIp, iPortMove))
                    {
                        LogMessage($"Move connection failed: {strIp}:{iPortMove}");
                        return;
                    }
                    LogMessage($"Move connected: {strIp}:{iPortMove}");

                    // Feedback bağlantısı
                    if (!mFeedback.Connect(strIp, iPortFeedback))
                    {
                        LogMessage($"Feedback connection failed: {strIp}:{iPortFeedback}");
                        return;
                    }
                    LogMessage($"Feedback connected: {strIp}:{iPortFeedback}");

                    // Bağlantı başarılı - Timer'ları başlat
                    mTimerReader.Start();
                    mTimer.Start();

                    IsConnected = true;
                    LogMessage("Robot connection successful!");
                }
                catch (Exception ex)
                {
                    LogMessage($"Connection error: {ex.Message}");
                    IsConnected = false;
                }
            });

            connectionThread.IsBackground = true; // Ana thread kapanınca bu da kapansın
            connectionThread.Start();
        }

        /// <summary>
        /// Robot bağlantısını kes
        /// </summary>
        public void Disconnect()
        {
            if (!IsConnected)
            {
                LogMessage("Robot already disconnected!");
                return;
            }

            try
            {
                // Timer'ları durdur
                mTimerReader?.Stop();
                mTimer?.Stop();

                // Bağlantıları kapat (API'nizde disconnect metodları varsa)
                try { mDashboard?.Disconnect(); } catch { }
                try { mDashboard?.Disconnect(); } catch { }
                try { mFeedback?.Disconnect(); } catch { }

                IsConnected = false;
                LogMessage("Robot disconnected successfully!");
            }
            catch (Exception ex)
            {
                LogMessage($"Disconnect error: {ex.Message}");
            }
        }

        /// <summary>
        /// Bağlantı durumunu kontrol et ve gerekirse yeniden bağlan
        /// </summary>
        public void CheckConnection()
        {
            if (!IsConnected)
            {
                LogMessage("Connection lost, attempting to reconnect...");
                Connect();
            }
        }

        #endregion

        #region Logging System

        // Log event'i - tüm formlar bu event'e subscribe olabilir
        public event Action<string> LogReceived;

        private void LogMessage(string message)
        {
            string timeStampedMessage = $"[{DateTime.Now:HH:mm:ss}] {message}";
            LogReceived?.Invoke(timeStampedMessage);

            // Debug için console'a da yazdır
            System.Diagnostics.Debug.WriteLine(timeStampedMessage);
        }

        #endregion

        #region Cleanup and Disposal

        /// <summary>
        /// Uygulama kapanırken resources'ları temizle
        /// </summary>
        public void Cleanup()
        {
            try
            {
                Disconnect();

                // Timer'ları dispose et
                mTimer?.Dispose();
                mTimerReader?.Dispose();

                LogMessage("RobotConnectionManager cleaned up");
            }
            catch (Exception ex)
            {
                LogMessage($"Cleanup error: {ex.Message}");
            }
        }

        // Finalizer - garbage collector tarafından çağrılır
        ~RobotConnectionManager()
        {
            Cleanup();
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Robot komutları için güvenli wrapper
        /// </summary>
        /// <param name="robotCommand">Çalıştırılacak robot komutu</param>
        /// <param name="commandName">Komut adı (log için)</param>
        /// <returns>Komut sonucu</returns>
        public string ExecuteCommand(Func<string> robotCommand, string commandName)
        {
            if (!IsConnected)
            {
                string errorMsg = $"Robot not connected. Cannot execute {commandName}";
                LogMessage(errorMsg);
                return errorMsg;
            }

            try
            {
                LogMessage($"Executing: {commandName}");
                string result = robotCommand();
                LogMessage($"Result: {result}");
                return result;
            }
            catch (Exception ex)
            {
                string errorMsg = $"Command {commandName} failed: {ex.Message}";
                LogMessage(errorMsg);
                return errorMsg;
            }
        }

        /// <summary>
        /// Robot durumunu string olarak getir
        /// </summary>
        public string GetConnectionStatus()
        {
            return IsConnected ? "Connected" : "Disconnected";
        }

        #endregion
    }
}