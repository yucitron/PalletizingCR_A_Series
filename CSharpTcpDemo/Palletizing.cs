using CSharpTcpDemo.com.dobot.api;
using CSharthiscpDemo.com.dobot.api;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace CSharpTcpDemo
{
    public partial class Palletizing : Form
    {
        private static Palletizing _instance;
        public static Palletizing Instance
        {
            get
            {
                if (_instance == null || _instance.IsDisposed)
                    _instance = new Palletizing();
                return _instance;
            }
        }

        private Feedback mFeedback = new Feedback();
        private Dashboard mDashboard = new Dashboard();
        


        private RobotConnectionManager robotManager;
        //定时获取数据并显示到UI*/




        private System.Timers.Timer mTimer = new System.Timers.Timer(300);

        private Boxes Boxes = new Boxes();
        public List<KutuNesnesi> kutular;
        public int pltHeight = 10;



        private PopupForm popupForm;
        private bool isPopupVisible = false;

        public double x1;
        public double y1;
        public double z1;
        public double rx1;
        public double ry1;
        public double rz1;

        public double x2;
        public double y2;
        public double z2;
        public double rx2;
        public double ry2;
        public double rz2;

        public double x3;
        public double y3;
        public double z3;
        public double rx3;
        public double ry3;
        public double rz3;

        public double Adx;
        public double Ady;
        public double Adz;
        public double Arx;
        public double Ary;
        public double Arz;

        public double Bdx;
        public double Bdy;
        public double Bdz;
        public double Brx;
        public double Bry;
        public double Brz;






        private Form1 parentForm;
        internal class CoordinateData
        {
            public DescartesPoint PickPoint { get; set; }
            public DescartesPoint PalletPt1 { get; set; }
            public DescartesPoint PalletPt2 { get; set; }
            public DescartesPoint PalletPt3 { get; set; }
        }

        public static class GlobalVeri
        {
            private static readonly string ConfigFilePath = "coordinates.json";
            public static List<KutuNesnesi> Kutular = new List<KutuNesnesi>();
            internal static DescartesPoint PickPoint { get; set; }
            internal static DescartesPoint PalletPt1 { get; set; }
            internal static DescartesPoint PalletPt2 { get; set; }
            internal static DescartesPoint PalletPt3 { get; set; }

            // Uygulama başlangıcında çağrılacak
            public static void LoadCoordinates()
            {
                try
                {
                    if (File.Exists(ConfigFilePath))
                    {
                        string jsonString = File.ReadAllText(ConfigFilePath);
                        var data = JsonConvert.DeserializeObject<CoordinateData>(jsonString);
                        if (data != null)
                        {
                            PickPoint = data.PickPoint;
                            PalletPt1 = data.PalletPt1;
                            PalletPt2 = data.PalletPt2;
                            PalletPt3 = data.PalletPt3;
                            Console.WriteLine("Koordinatlar başarıyla yüklendi.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Koordinat dosyası bulunamadı. Varsayılan değerler kullanılacak.");
                        InitializeDefaultValues();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Koordinatlar yüklenirken hata oluştu: {ex.Message}");
                    InitializeDefaultValues();
                }
            }

            // Koordinatları JSON dosyasına kaydet
            public static void SaveCoordinates()
            {
                try
                {
                    var data = new CoordinateData
                    {
                        PickPoint = PickPoint,
                        PalletPt1 = PalletPt1,
                        PalletPt2 = PalletPt2,
                        PalletPt3 = PalletPt3
                    };

                    string jsonString = JsonConvert.SerializeObject(data, Formatting.Indented);
                    File.WriteAllText(ConfigFilePath, jsonString);

                    Console.WriteLine("Koordinatlar başarıyla kaydedildi.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Koordinatlar kaydedilirken hata oluştu: {ex.Message}");
                }
            }

            // Varsayılan değerleri ayarla
            private static void InitializeDefaultValues()
            {
                PickPoint = new DescartesPoint();
                PalletPt1 = new DescartesPoint();
                PalletPt2 = new DescartesPoint();
                PalletPt3 = new DescartesPoint();
            }


            // Tek bir koordinat güncellendiğinde otomatik kaydetme
            public static void UpdatePickPoint(DescartesPoint newPoint)
            {
                PickPoint = newPoint;
                SaveCoordinates();
            }

            public static void UpdatePalletPt1(DescartesPoint newPoint)
            {
                PalletPt1 = newPoint;
                SaveCoordinates();
            }

            public static void UpdatePalletPt2(DescartesPoint newPoint)
            {
                PalletPt2 = newPoint;
                SaveCoordinates();
            }

            public static void UpdatePalletPt3(DescartesPoint newPoint)
            {
                PalletPt3 = newPoint;
                SaveCoordinates();
            }


            // Tek bir koordinat güncellendiğinde otomatik kaydetme

        }



        public Palletizing()
        {
            InitializeComponent();
            UseCoordinates();
            pltHeight = Convert.ToInt16(numpltHeight.Value);
            //////
            InitializeRobotComponents();
        }

        private void InitializeRobotComponents()
        {
            robotManager = RobotConnectionManager.Instance;
            mFeedback = robotManager.Feedback;
            mDashboard = robotManager.Dashboard;
            
        }

        public void PrintLog2(string str)
        {

            if (string.IsNullOrEmpty(str))
            {
                return;
            }
            if (this.rich2TextBoxLog.InvokeRequired)
            {
                this.rich2TextBoxLog.Invoke(new Action<string>(log =>
                {
                    InsertLogToRichBox(this.rich2TextBoxLog, log);
                }), str);
            }
            else
            {
                InsertLogToRichBox(this.rich2TextBoxLog, str);
            }
        }


        private void pltbtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1.Instance.Show();
            Form1.Instance.BringToFront();

        }


        private void boxbtn_Click(object sender, EventArgs e)
        {

            this.Hide();
            Boxes Boxesform = new Boxes();
            Boxesform.Show();

        }
        private void UseCoordinates()
        {
            // Koordinatları okuma
            var pickPoint = GlobalVeri.PickPoint;
            var pallet1 = GlobalVeri.PalletPt1;
            var pallet2 = GlobalVeri.PalletPt2;
            var pallet3 = GlobalVeri.PalletPt3;

            // Koordinat güncelleme örneği
            GlobalVeri.UpdatePickPoint(new DescartesPoint { x = 100, y = 200, z = 300, rx = 300, ry = 300, rz = 300 });
            GlobalVeri.UpdatePalletPt1(new DescartesPoint { x = 100, y = 200, z = 300, rx = 300, ry = 300, rz = 300 });
            GlobalVeri.UpdatePalletPt2(new DescartesPoint { x = 100, y = 200, z = 300, rx = 300, ry = 300, rz = 300 });
            GlobalVeri.UpdatePalletPt3(new DescartesPoint { x = 100, y = 200, z = 300, rx = 300, ry = 300, rz = 300 });
        }

        public void PrintLog(string str)
        {
            if (string.IsNullOrEmpty(str))
                return;

            try
            {
                // Singleton instance kullan
                Form1.Instance.WriteToLog(str);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Log yazma hatası: {ex.Message}");
                // Alternatif: MessageBox ile göster
                // MessageBox.Show($"Log yazma hatası: {ex.Message}");
            }
        }


        public void InsertLogToRichBox(RichTextBox box, string str)
        {
            if (box.GetLineFromCharIndex(box.TextLength) > 300)
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
        }

        public void btnPick_Click(object sender, EventArgs e)
        {

            GlobalVeri.PickPoint = new DescartesPoint()
            {
                x = mFeedback.feedbackData.ToolVectorActual[0],
                y = mFeedback.feedbackData.ToolVectorActual[1],
                z = mFeedback.feedbackData.ToolVectorActual[2],
                rx = mFeedback.feedbackData.ToolVectorActual[3],
                ry = mFeedback.feedbackData.ToolVectorActual[4],
                rz = mFeedback.feedbackData.ToolVectorActual[5]
            };
            PrintLog2($" Picking is: {GlobalVeri.PickPoint} ");
        }

        public void btnMove_Click(object sender, EventArgs e)
        {

            for (double layer = Boxes.BoxHeight; layer < 5; layer++)
            {

                // DEĞIŞTIRILEN SATIR: Doğrudan GlobalVeri'den al
                var kutular = GlobalVeri.Kutular;

                if (kutular == null || kutular.Count == 0) // Koşul düzeltildi
                {
                    MessageBox.Show("Kutu bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    PrintLog("Hata: Kutu listesi boş!");
                    return;
                }
                if (GlobalVeri.PickPoint == null)
                {
                    MessageBox.Show("Önce Pick butonuna basın!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                //PrintLog2($" Toplam {GlobalVeri.PalletPt1} ");
                //PrintLog($" Toplam {kutular.Count} kutu işlenecek.");
                PrintLog2($" Toplam {kutular.Count} kutu işlenecek.");

                foreach (var kutu in kutular)
                {
                    // Örnek koordinat değişikliği
                    kutu.Konum = new Point(kutu.Konum.X , kutu.Konum.Y);


                    DescartesPoint pt1 = new DescartesPoint();
                    Adx = 300;
                    

                    pt1.x = (GlobalVeri.PalletPt1.x + (Adx * ((double)kutu.Merkez.X / Boxes.paletGenislik)));
                    pt1.y = GlobalVeri.PalletPt1.y + (Ady * ((double)kutu.Merkez.Y / Boxes.paletYukseklik));
                    double oran = ((double)kutu.Merkez.X / Boxes.paletGenislik);
                    //pt1.x = kutu.Merkez.X;
                    //pt1.y = kutu.Merkez.Y;
                    pt1.z = 50;
                    pt1.rx = 60;
                    pt1.ry = 80;
                    pt1.rz = 90;

                    // PrintLog($"Kutu ID: {kutu.ID} - Hedef konum: ({pt1.x}, {pt1.y}, {pt1.z})");
                    PrintLog2($"Kutu ID: {kutu.ID} - Hedef konum: ({pt1.x}, {pt1.y}, {pt1.z})");
                    PrintLog2($"PalletX X({GlobalVeri.PalletPt1.x})");
                    PrintLog2($"kutu X({kutu.Merkez.X})");
                    PrintLog2($"Gönderiliyor: MovL({pt1.ToString()})");
                    PrintLog2($"paletYukseklik({Boxes.paletYukseklik})");
                    PrintLog2($"Oran({oran})");

                    PrintLog2($"Adx({Adx})");
                    
                   Thread thd1 = new Thread(() =>
                   {
                       try
                       {
                           string ret = mDashboard.MovL(pt1);
                           PrintLog($"Cevap alındı: {ret}");
                           PrintLog2($"Cevap alındı: {ret}");
                       }
                       catch (Exception ex)
                       {
                           PrintLog($"Hareket hatası: {ex.Message}");
                           PrintLog2($"Hareket hatası: {ex.Message}");
                       }
                   });

                   thd1.Start();
                   Thread.Sleep(500); // 500ms bekleme*/
                    /*
                    Thread thd2 = new Thread(() =>
                    {
                        try
                        {
                            string ret = mDashboard.MovL(GlobalVeri.PickPoint);
                            PrintLog($"Cevap alındı: {ret}");
                            PrintLog2($"Cevap alındı: {ret}");
                        }
                        catch (Exception ex)
                        {
                            PrintLog($"Hareket hatası: {ex.Message}");
                            PrintLog2($"Hareket hatası: {ex.Message}");
                        }
                    });

                    thd2.Start();
                    Thread.Sleep(500); // 500ms bekleme*/

                }

                
                PrintLog("Tüm hareketler tamamlandı.");
            }
        }



        private void mainbtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1.Instance.Show();
            Form1.Instance.BringToFront();
        }

        private void Palletizing_FormClosed(object sender, FormClosedEventArgs e)
        {
            
            this.Hide();

            // Eğer ana form da kapalıysa uygulamayı kapat
            if (Form1.Instance == null || Form1.Instance.IsDisposed ||!Form1.Instance.Visible)
                Application.Exit();
        }

        private void numpltHeight_ValueChanged(object sender, EventArgs e)
        {
            pltHeight = Convert.ToInt16(numpltHeight.Value);
        }

        private void jogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            popupForm = new PopupForm();
            
            popupForm.Show();
             //HidePopup();

        }

        private void Pt1btn_Click(object sender, EventArgs e)
        {
            GlobalVeri.PalletPt1 = new DescartesPoint()
            {/*
                x = mFeedback.feedbackData.ToolVectorActual[0],
                y = mFeedback.feedbackData.ToolVectorActual[1],
                z = mFeedback.feedbackData.ToolVectorActual[2],
                rx = mFeedback.feedbackData.ToolVectorActual[3],
                ry = mFeedback.feedbackData.ToolVectorActual[4],
                rz = mFeedback.feedbackData.ToolVectorActual[5]*/

                x = 56,
                y = 89,
                z = 23,
                rx = 45,
                ry = 56,
                rz = 67
            };

            double x1 = GlobalVeri.PalletPt1.x;
            double y1 = GlobalVeri.PalletPt1.y;
            double z1 = GlobalVeri.PalletPt1.z;
            double rx1 = GlobalVeri.PalletPt1.rx;
            double ry1 = GlobalVeri.PalletPt1.ry;
            double rz1 = GlobalVeri.PalletPt1.rz;
           GlobalVeri.UpdatePalletPt1(GlobalVeri.PalletPt1);
            PrintLog2($"Pallet Point 1 set to: {GlobalVeri.PalletPt1}");


        }

        private void Pt2btn_Click(object sender, EventArgs e)
        {
            GlobalVeri.PalletPt2 = new DescartesPoint()
            {
                x = mFeedback.feedbackData.ToolVectorActual[0],
                y = mFeedback.feedbackData.ToolVectorActual[1],
                z = mFeedback.feedbackData.ToolVectorActual[2],
                rx = mFeedback.feedbackData.ToolVectorActual[3],
                ry = mFeedback.feedbackData.ToolVectorActual[4],
                rz = mFeedback.feedbackData.ToolVectorActual[5]
            };

            double x2 = GlobalVeri.PalletPt2.x;
            double y2 = GlobalVeri.PalletPt2.y;
            double z2 = GlobalVeri.PalletPt2.z;
            double rx2 = GlobalVeri.PalletPt2.rx;
            double ry2 = GlobalVeri.PalletPt2.ry;
            double rz2 = GlobalVeri.PalletPt2.rz;

            double Adx = x1 - x2;
            double Ady = y1 - y2;
            double Adz = z1 - z2;
            double Arx = rx1 - rx2;
            double Ary = ry1 - ry2;
            double Arz = rz1 - rz2;
        }

        private void Pt3btn_Click(object sender, EventArgs e)
        {
            GlobalVeri.PalletPt3 = new DescartesPoint()
            {
                x = mFeedback.feedbackData.ToolVectorActual[0],
                y = mFeedback.feedbackData.ToolVectorActual[1],
                z = mFeedback.feedbackData.ToolVectorActual[2],
                rx = mFeedback.feedbackData.ToolVectorActual[3],
                ry = mFeedback.feedbackData.ToolVectorActual[4],
                rz = mFeedback.feedbackData.ToolVectorActual[5]
            };

            double x3 = GlobalVeri.PalletPt3.x;
            double y3 = GlobalVeri.PalletPt3.y;
            double z3 = GlobalVeri.PalletPt3.z;
            double rx3 = GlobalVeri.PalletPt3.rx;
            double ry3 = GlobalVeri.PalletPt3.ry;
            double rz3 = GlobalVeri.PalletPt3.rz;

            double Bdx = x2 - x3;
            double Bdy = y2 - y3;
            double Bdz = z2 - z3;
            double Brx = rx2 - rx3;
            double Bry = ry2 - ry3;
            double Brz = rz2 - rz3;



        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            rich2TextBoxLog.Text = "";
        }
    }
}
