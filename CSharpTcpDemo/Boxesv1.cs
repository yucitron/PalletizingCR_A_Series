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
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Button = System.Windows.Forms.Button;
using ComboBox = System.Windows.Forms.ComboBox;

// Yönelim enum'u
public enum KutuYonelimi
{
    Kuzey = 0,      // ↑
    KuzeyDogu = 1,  // ↗
    Dogu = 2,       // →
    GuneyDogu = 3,  // ↘
    Guney = 4,      // ↓
    GuneyBati = 5,  // ↙
    Bati = 6,       // ←
    KuzeyBati = 7   // ↖
}

namespace CSharpTcpDemo
{
    public partial class Boxes : Form
    {
        private static bool ilkAcilis = false;
        private Feedback mFeedback = new Feedback();
        private Dashboard mDashboard = new Dashboard();
        private System.Timers.Timer mTimer = new System.Timers.Timer(300);
        private MainForm mainForm = new MainForm();

        public int BoxLength;
        public int BoxWidth;
        public int BoxHeight;

        private Panel paletPanel;
        private KutuNesnesi secilenKutu = null;
        public List<KutuNesnesi> kutular;
        private int kutuSayaci;
        private Point baslangicKonum;
        private bool suruklemeModu = false;

        // Palet boyutları
        public int paletGenislik = 400;
        public int paletYukseklik = 400;
        public int kutuBosluk = 5;
        private bool yatayYerlestirme = true;
        private bool otomatikYerlestirme = false;

        // Yönelim kontrolleri
        
        private ComboBox cmbKutuYonelimi;
        private Button btnYonelimUygula;
        private Panel yonelimPanel;

        // Diğer kontroller - bunları mevcut form'unuzdan tanımlamanız gerekiyor

        public Boxes()
        {
            InitializeComponent();

            // Kontrolleri başlat
            KontrolleriBaslat();

            kutular = Palletizing.GlobalVeri.Kutular;
            kutuSayaci = kutular.Count;
            BoxHeight = Parse2Int(heighttextbox.Text);
            FormOlustur();
        }

        private void KontrolleriBaslat()
        {
            // Ana layout
            /*
            mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));

            // Sol panel (palet için)
            solPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            // Kontrol paneli
            kontrolPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.LightGray
            };

            // Yerleşim paneli
            yerlesimPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 2,
                RowCount = 2,
                Height = 60
            };
            
            // Temel kontroller
            txtKutuBilgileri = new RichTextBox
            {
                Dock = DockStyle.Bottom,
                Height = 300,
                ReadOnly = true,
                Font = new Font("Courier New", 8)
            };

            cmbYerlesimYonu = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            chkOtomatikYerlestirme = new CheckBox
            {
                Text = "Otomatik Yerleştirme",
                Dock = DockStyle.Top,
                Height = 25
            };

            
          

            lblBaslik = new Label
            {
                Text = "Kutu Kontrolleri",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Ana panele text box'ı ekle
            kontrolPanel.Controls.Add(txtKutuBilgileri);
        }*/
        }
        private void heighttextbox_Click(object sender, EventArgs e)
        {
            heighttextbox.Text = "";
        }

        public int Parse2Int(string str)
        {
            int iValue = 0;
            try
            {
                iValue = int.Parse(str);
            }
            catch
            {
                MessageBox.Show("integer deger giriniz");
            }
            return iValue;
        }

        private void boxsavebtn_Click(object sender, EventArgs e)
        {
            FormAcilisindaVeriYukle();
        }

        // Yönelim kontrollerini oluştur
        private void YonelimKontrolleriniOlustur()
        {
            // Yönelim paneli
          

            Label lblYonelimBaslik = new Label
            {
                Text = "Kutu Yönelim Ayarları",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(200, 20)
            };

            Label lblSecilenKutu = new Label
            {
                Text = "Seçilen Kutu:",
                Location = new Point(10, 35),
                Size = new Size(80, 20)
            };

            Label lblSecilenKutuID = new Label
            {
                Text = "Hiçbiri",
                Name = "lblSecilenKutuID",
                Location = new Point(95, 35),
                Size = new Size(50, 20),
                ForeColor = Color.Blue
            };

            Label lblYonelim = new Label
            {
                Text = "Yönelim:",
                Location = new Point(10, 60),
                Size = new Size(60, 20)
            };

            cmbKutuYonelimi = new ComboBox
            {
                Location = new Point(75, 58),
                Size = new Size(120, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Yönelim seçeneklerini ekle
            cmbKutuYonelimi.Items.AddRange(new object[]
            {
                "Kuzey (↑)",
                "Kuzey-Doğu (↗)",
                "Doğu (→)",
                "Güney-Doğu (↘)",
                "Güney (↓)",
                "Güney-Batı (↙)",
                "Batı (←)",
                "Kuzey-Batı (↖)"
            });

            btnYonelimUygula = new Button
            {
                Text = "Yönelimi Uygula",
                Location = new Point(200, 58),
                Size = new Size(100, 25),
                Enabled = false
            };

            btnYonelimUygula.Click += BtnYonelimUygula_Click;

            // ID ile yönelim ayarlama
            Label lblIDYonelim = new Label
            {
                Text = "ID ile Yönelim:",
                Location = new Point(10, 90),
                Size = new Size(90, 20)
            };

            NumericUpDown numKutuID = new NumericUpDown
            {
                Location = new Point(105, 88),
                Size = new Size(50, 25),
                Minimum = 1,
                Maximum = 1000
            };

            ComboBox cmbIDYonelim = new ComboBox
            {
                Location = new Point(160, 88),
                Size = new Size(120, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbIDYonelim.Items.AddRange(cmbKutuYonelimi.Items.Cast<object>().ToArray());

            Button btnIDYonelimUygula = new Button
            {
                Text = "Tümüne Uygula",
                Location = new Point(285, 88),
                Size = new Size(80, 25)
            };

            btnIDYonelimUygula.Click += (s, e) =>
            {
                KutuYonelimi yeniYonelim = (KutuYonelimi)cmbIDYonelim.SelectedIndex;

                // Tüm kutulara aynı yönelimi uygula
                foreach (var kutu in kutular)
                {
                    kutu.Yonelimi = yeniYonelim;
                }

                // Paneli zorla yenile
                paletPanel.Invalidate();
                paletPanel.Update();

                KutuBilgileriniGuncelle();

                // Seçilen kutu varsa onun da yönelim combo box'ını güncelle
                if (secilenKutu != null && cmbKutuYonelimi != null)
                {
                    cmbKutuYonelimi.SelectedIndex = (int)yeniYonelim;
                }

                
            
             else if (kutular.Count == 0)
            {
                MessageBox.Show("Henüz kutu bulunmuyor. Önce kutu ekleyin.");
            }
            else
            {
                MessageBox.Show("Lütfen bir yönelim seçin.");
            }
            };

            // Kontrolleri panele ekle
            yonelimPanel.Controls.AddRange(new Control[]
            {
                lblYonelimBaslik, lblSecilenKutu, lblSecilenKutuID, lblYonelim,
                cmbKutuYonelimi, btnYonelimUygula,
                cmbIDYonelim,    btnIDYonelimUygula
            });
        }

        private void BtnYonelimUygula_Click(object sender, EventArgs e)
        {
            if (secilenKutu != null && cmbKutuYonelimi.SelectedIndex >= 0)
            {
                // Eski yönelimi kaydet
                KutuYonelimi eskiYonelim = secilenKutu.Yonelimi;

                // Yeni yönelimi ata
                secilenKutu.Yonelimi = (KutuYonelimi)cmbKutuYonelimi.SelectedIndex;

                // Paneli zorla yenile
                paletPanel.Invalidate();
                paletPanel.Update();

                // Kutu bilgilerini güncelle
                KutuBilgileriniGuncelle();

                // Debug için log yaz
                

                MessageBox.Show($"Kutu {secilenKutu.ID} için yönelim ayarlandı: {GetYonelimMetni(secilenKutu.Yonelimi)}");
            }
            else
            {
                MessageBox.Show("Lütfen önce bir kutu seçin ve yönelim belirleyin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Seçilen kutu değiştiğinde yönelim kontrollerini güncelle
        private void SecilenKutuDegisti()
        {
            var lblSecilenKutuID = yonelimPanel?.Controls["lblSecilenKutuID"] as Label;

            if (secilenKutu != null)
            {
                if (lblSecilenKutuID != null)
                    lblSecilenKutuID.Text = $"ID: {secilenKutu.ID}";

                // Mevcut yönelimi combo box'ta seç
                if (cmbKutuYonelimi != null)
                {
                    cmbKutuYonelimi.SelectedIndex = (int)secilenKutu.Yonelimi;
                }

                if (btnYonelimUygula != null)
                {
                    btnYonelimUygula.Enabled = true;
                }

                // Debug için log
                
            }
            else
            {
                if (lblSecilenKutuID != null)
                    lblSecilenKutuID.Text = "Hiçbiri";

                if (cmbKutuYonelimi != null)
                {
                    cmbKutuYonelimi.SelectedIndex = -1;
                }

                if (btnYonelimUygula != null)
                {
                    btnYonelimUygula.Enabled = false;
                }
            }

            // Paneli yenile
            paletPanel?.Invalidate();
        }

        public void FormAcilisindaVeriYukle()
        {
            if (KutuVeriYoneticisi.OtomatikKayitVarMi())
            {
                var sonuc = MessageBox.Show("Daha önce kaydedilmiş kutu verileri bulundu. Yüklemek ister misiniz?",
                                          "Kayıtlı Veri Bulundu",
                                          MessageBoxButtons.YesNo,
                                          MessageBoxIcon.Question);

                if (sonuc == DialogResult.Yes)
                {
                    var yuklenmisnkutular = KutuVeriYoneticisi.KutulariYukle();
                    if (yuklenmisnkutular.Count > 0)
                    {
                        kutular = yuklenmisnkutular;
                        kutuSayaci = kutular.Max(k => k.ID);

                        // EKLENEN SATIR: GlobalVeri'yi güncelle
                        Palletizing.GlobalVeri.Kutular = kutular;

                        KutuBilgileriniGuncelle();
                        paletPanel?.Invalidate();
                    }
                }
            }
        }

        public void FormKapanirkenVeriKaydet()
        {
            if (kutular.Count > 0)
            {
                var sonuc = MessageBox.Show("Kutu verilerini kaydetmek ister misiniz?",
                                          "Veri Kaydetme",
                                          MessageBoxButtons.YesNoCancel,
                                          MessageBoxIcon.Question);

                if (sonuc == DialogResult.Yes)
                {
                    KutuVeriYoneticisi.KutulariKaydet(kutular);
                }
                else if (sonuc == DialogResult.Cancel)
                {
                    return;
                }
            }
        }

        public void KutuBilgileriniGuncelle()
        {
            if (txtKutuBilgileri == null) return;

            txtKutuBilgileri.Clear();
            txtKutuBilgileri.SelectionFont = new Font("Arial", 10, FontStyle.Bold);
            txtKutuBilgileri.SelectionColor = Color.DarkBlue;
            txtKutuBilgileri.AppendText("KUTU KOORDİNATLARI VE BİLGİLERİ\n");
            txtKutuBilgileri.AppendText("============================================================\n\n");

            foreach (KutuNesnesi kutu in kutular)
            {
                txtKutuBilgileri.SelectionFont = new Font("Arial", 9, FontStyle.Bold);
                txtKutuBilgileri.SelectionColor = Color.DarkRed;
                txtKutuBilgileri.AppendText($"KUTU ID: {kutu.ID}\n");

                txtKutuBilgileri.SelectionFont = new Font("Arial", 8, FontStyle.Regular);
                txtKutuBilgileri.SelectionColor = Color.Black;
                txtKutuBilgileri.AppendText($"  • Merkez Koordinatı: ({kutu.Merkez.X}, {kutu.Merkez.Y})\n");
                txtKutuBilgileri.AppendText($"  • Boyutlar: {kutu.Genislik} x {kutu.Yukseklik} piksel\n");

                // Yönelim bilgisi
                txtKutuBilgileri.SelectionColor = Color.Purple;
                string yonelimMetni = GetYonelimMetni(kutu.Yonelimi);
                txtKutuBilgileri.AppendText($"  • Yönelim: {yonelimMetni}\n");

                txtKutuBilgileri.SelectionColor = kutu.YatayMi ? Color.Green : Color.Orange;
                txtKutuBilgileri.AppendText($"  • Boyut Yönelimi: {(kutu.YatayMi ? "YATAY" : "DİKEY")}\n");

                txtKutuBilgileri.SelectionColor = Color.Black;
                txtKutuBilgileri.AppendText($"  • Döndürülmüş: {(kutu.DondurmeModu ? "Evet" : "Hayır")}\n");
                txtKutuBilgileri.AppendText("------------------------------------------------------------\n");
            }

            // Özet bilgileri
            txtKutuBilgileri.SelectionFont = new Font("Arial", 9, FontStyle.Bold);
            txtKutuBilgileri.SelectionColor = Color.DarkBlue;
            txtKutuBilgileri.AppendText("\nÖZET BİLGİLER:\n");
            txtKutuBilgileri.AppendText("============================================================\n");

            txtKutuBilgileri.SelectionFont = new Font("Arial", 8, FontStyle.Regular);
            txtKutuBilgileri.SelectionColor = Color.Black;

            int yatayKutuSayisi = kutular.Count(k => k.YatayMi);
            int dikeyKutuSayisi = kutular.Count - yatayKutuSayisi;

            txtKutuBilgileri.AppendText($"• Toplam Kutu Sayısı: {kutular.Count}\n");
            txtKutuBilgileri.AppendText($"• Yatay Kutular: {yatayKutuSayisi}\n");
            txtKutuBilgileri.AppendText($"• Dikey Kutular: {dikeyKutuSayisi}\n");

            // Yönelim dağılımı
            txtKutuBilgileri.AppendText("\n• Yönelim Dağılımı:\n");
            var yonelimGruplari = kutular.GroupBy(k => k.Yonelimi);
            foreach (var grup in yonelimGruplari)
            {
                string yonelimAdi = GetYonelimMetni(grup.Key);
                txtKutuBilgileri.AppendText($"  - {yonelimAdi}: {grup.Count()} kutu\n");
            }

            if (kutular.Count > 0)
            {
                txtKutuBilgileri.AppendText("\n");
                txtKutuBilgileri.SelectionColor = Color.DarkGreen;
                txtKutuBilgileri.AppendText("KOORDİNAT LİSTESİ (CSV Formatında):\n");
                txtKutuBilgileri.SelectionColor = Color.Black;
                txtKutuBilgileri.AppendText("ID,X,Y,Genislik,Yukseklik,BoyutYonelimi,Yonelim\n");

                foreach (KutuNesnesi kutu in kutular)
                {
                    string yonelimKisaltma = GetYonelimKisaltma(kutu.Yonelimi);
                    string csvSatir = $"{kutu.ID},{kutu.Merkez.X},{kutu.Merkez.Y},{kutu.Genislik},{kutu.Yukseklik},{(kutu.YatayMi ? "Yatay" : "Dikey")},{yonelimKisaltma}\n";
                    txtKutuBilgileri.AppendText(csvSatir);
                }
            }

            txtKutuBilgileri.SelectionStart = 0;
            txtKutuBilgileri.ScrollToCaret();
        }

        private string GetYonelimMetni(KutuYonelimi yonelim)
        {
            switch (yonelim)
            {
                case KutuYonelimi.Kuzey: return "Kuzey (↑)";
                case KutuYonelimi.KuzeyDogu: return "Kuzey-Doğu (↗)";
                case KutuYonelimi.Dogu: return "Doğu (→)";
                case KutuYonelimi.GuneyDogu: return "Güney-Doğu (↘)";
                case KutuYonelimi.Guney: return "Güney (↓)";
                case KutuYonelimi.GuneyBati: return "Güney-Batı (↙)";
                case KutuYonelimi.Bati: return "Batı (←)";
                case KutuYonelimi.KuzeyBati: return "Kuzey-Batı (↖)";
                default: return "Bilinmeyen";
            }
        }

        private string GetYonelimKisaltma(KutuYonelimi yonelim)
        {
            switch (yonelim)
            {
                case KutuYonelimi.Kuzey: return "N";
                case KutuYonelimi.KuzeyDogu: return "NE";
                case KutuYonelimi.Dogu: return "E";
                case KutuYonelimi.GuneyDogu: return "SE";
                case KutuYonelimi.Guney: return "S"; 
                case KutuYonelimi.GuneyBati: return "SW";
                case KutuYonelimi.Bati: return "W";
                case KutuYonelimi.KuzeyBati: return "NW";
                default: return "?";
            }
        }

        private void KutulariYerlestir()
        {
            if (kutular.Count == 0) return;

            int x = 5;
            int y = 5;
            int satirYukseklik = 0;
            int sutunGenislik = 0;

            if (yatayYerlestirme)
            {
                foreach (KutuNesnesi kutu in kutular)
                {
                    if (x + kutu.Genislik > paletPanel.Width - 10)
                    {
                        x = 10;
                        y += satirYukseklik + kutuBosluk;
                        satirYukseklik = 0;
                    }

                    kutu.Konum = new Point(x, y);
                    x += kutu.Genislik + kutuBosluk;
                    satirYukseklik = Math.Max(satirYukseklik, kutu.Yukseklik);
                }
            }
            else
            {
                foreach (KutuNesnesi kutu in kutular)
                {
                    if (y + kutu.Yukseklik > paletPanel.Height - 10)
                    {
                        y = 10;
                        x += sutunGenislik + kutuBosluk;
                        sutunGenislik = 0;
                    }

                    kutu.Konum = new Point(x, y);
                    y += kutu.Yukseklik + kutuBosluk;
                    sutunGenislik = Math.Max(sutunGenislik, kutu.Genislik);
                }
            }

            txtKutuBilgileri.Clear();
            KutuBilgileriniGuncelle();
            paletPanel.Invalidate();
        }

        private void FormOlustur()
        {
            paletPanel = new Panel
            {
                Width = paletGenislik,
                Height = paletYukseklik,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(20, 20)
            };
            paletPanel.Paint += PaletPanel_Paint;

            // Yönelim kontrollerini oluştur
            YonelimKontrolleriniOlustur();

            TableLayoutPanel boyutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 2,
                RowCount = 3,
                Height = 100,
                Margin = new Padding(0, 10, 0, 0)
            };
            boyutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            boyutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));

            Label lblKutuBosluk = new Label { Text = "Kutu Boşluğu (px):", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight };
            NumericUpDown numKutuBosluk = new NumericUpDown { Minimum = 0, Maximum = 50, Value = kutuBosluk, Dock = DockStyle.Fill };
            numKutuBosluk.ValueChanged += (s, e) =>
            {
                kutuBosluk = (int)numKutuBosluk.Value;
                KutulariYerlestir();
            };

            Label lblYerlesimYonu = new Label { Text = "Yerleşim Yönü:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight };

            cmbYerlesimYonu.Items.AddRange(new object[] { "Yatay (Soldan Sağa)", "Dikey (Yukarıdan Aşağıya)" });
            cmbYerlesimYonu.SelectedIndex = 0;
            cmbYerlesimYonu.SelectedIndexChanged += (s, e) =>
            {
                yatayYerlestirme = cmbYerlesimYonu.SelectedIndex == 0;
                KutulariYerlestir();
            };

            yerlesimPanel.Controls.Add(lblKutuBosluk, 0, 0);
            yerlesimPanel.Controls.Add(numKutuBosluk, 1, 0);
            yerlesimPanel.Controls.Add(lblYerlesimYonu, 0, 1);
            yerlesimPanel.Controls.Add(cmbYerlesimYonu, 1, 1);

            chkOtomatikYerlestirme.CheckedChanged += (s, e) =>
            {
                otomatikYerlestirme = chkOtomatikYerlestirme.Checked;
                if (otomatikYerlestirme)
                    KutulariYerlestir();
            };

            btnYerlestir.Click += (s, e) =>
            {
                KutulariYerlestir();
                KutuBilgileriniGuncelle();
            };

            btnEkle.Click += (sender, e) =>
            {
                int genislik = (int)numGenislik.Value;
                int yukseklik = (int)numYukseklik.Value;
                Color renk = Color.Red;

                KutuEkle(genislik, yukseklik, renk);
            };

            btnDondur.Click += (sender, e) =>
            {
                if (secilenKutu != null)
                {
                    secilenKutu.Dondur();
                    if (otomatikYerlestirme)
                        KutulariYerlestir();
                    else
                        paletPanel.Invalidate();
                }
            };

            btnSil.Click += (sender, e) =>
            {
                if (secilenKutu != null)
                {
                    kutular.Remove(secilenKutu);
                    secilenKutu = null;

                    // EKLENEN SATIR: GlobalVeri'yi güncelle
                    Palletizing.GlobalVeri.Kutular = kutular;

                    SecilenKutuDegisti();
                    KutuBilgileriniGuncelle();
                    paletPanel.Invalidate();
                }
            };

            btnTemizle.Click += (s, e) =>
            {
                if (MessageBox.Show("Tüm kutuları silmek istediğinizden emin misiniz?",
                    "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    kutular.Clear();
                    secilenKutu = null;
                    kutuSayaci = 0;

                    // EKLENEN SATIR: GlobalVeri'yi güncelle
                    Palletizing.GlobalVeri.Kutular = kutular;

                    SecilenKutuDegisti();
                    KutuBilgileriniGuncelle();
                    paletPanel.Invalidate();
                }
            };

            btnPaletGuncelle.Click += (sender, e) =>
            {
                paletGenislik = (int)numPaletGenislik.Value;
                paletYukseklik = (int)numPaletYukseklik.Value;
                paletPanel.Size = new Size(paletGenislik, paletYukseklik);
                KutuBilgileriniGuncelle();
                FormKapanirkenVeriKaydet();
                paletPanel.Invalidate();
            };

            // Kontrol paneline yönelim panelini ekle
           // kontrolPanel.Controls.Add(yonelimPanel);
            kontrolPanel.Controls.Add(btnPaletGuncelle);
            kontrolPanel.Controls.Add(btnSil);
            kontrolPanel.Controls.Add(btnDondur);
            kontrolPanel.Controls.Add(btnEkle);
            kontrolPanel.Controls.Add(btnYerlestir);
            kontrolPanel.Controls.Add(lblBaslik);
            kontrolPanel.Controls.Add(btnTemizle);

            solPanel.Controls.Add(paletPanel);
            mainLayout.Controls.Add(solPanel, 0, 0);
            mainLayout.Controls.Add(kontrolPanel, 1, 0);
            this.Controls.Add(mainLayout);

            paletPanel.MouseDown += PaletPanel_MouseDown;
            paletPanel.MouseMove += PaletPanel_MouseMove;
            paletPanel.MouseUp += PaletPanel_MouseUp;
        }

        private void KutuEkle(int genislik, int yukseklik, Color renk)
        {
            
            if (genislik > paletPanel.Width || yukseklik > paletPanel.Height)
            {
                MessageBox.Show("Kutu boyutları palet boyutlarından büyük olamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            KutuNesnesi yeniKutu = new KutuNesnesi
            {
                ID = ++kutuSayaci,
                Konum = new Point(0, 0),
                Genislik = genislik,
                Yukseklik = yukseklik,
                Renk = renk,
                Yonelimi = KutuYonelimi.Kuzey
            };

            kutular.Add(yeniKutu);

            // EKLENEN SATIR: GlobalVeri'yi güncelle
            Palletizing.GlobalVeri.Kutular = kutular;

            try
            {
                Form1.Instance.WriteToLog($"Yeni kutu eklendi: ID={yeniKutu.ID}, Boyut={genislik}x{yukseklik}");
                Form1.Instance.WriteToLog($"Toplam kutu sayısı: {kutular.Count}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Log hatası: {ex.Message}");
            }

            secilenKutu = yeniKutu;
            SecilenKutuDegisti();
            KutuBilgileriniGuncelle();
            paletPanel.Invalidate();
        
        }

        private void PaletPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            foreach (KutuNesnesi kutu in kutular)
            {
                kutu.Ciz(g, kutu == secilenKutu);
            }
        }

        private void PaletPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                KutuNesnesi tiklananKutu = null;

                for (int i = kutular.Count - 1; i >= 0; i--)
                {
                    if (kutular[i].IceriyorMu(e.Location))
                    {
                        tiklananKutu = kutular[i];
                        break;
                    }
                }

                if (tiklananKutu != null)
                {
                    secilenKutu = tiklananKutu;
                    baslangicKonum = e.Location;
                    suruklemeModu = true;

                    kutular.Remove(secilenKutu);
                    kutular.Add(secilenKutu);
                }
                else
                {
                    secilenKutu = null;
                }

                SecilenKutuDegisti();
                paletPanel.Invalidate();
            }
        }

        private void PaletPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (suruklemeModu && secilenKutu != null)
            {
                int dx = e.X - baslangicKonum.X;
                int dy = e.Y - baslangicKonum.Y;

                Point yeniKonum = new Point(
                    secilenKutu.Konum.X + dx,
                    secilenKutu.Konum.Y + dy
                );

                if (yeniKonum.X >= 0 &&
                    yeniKonum.Y >= 0 &&
                    yeniKonum.X + secilenKutu.Genislik <= paletPanel.Width &&
                    yeniKonum.Y + secilenKutu.Yukseklik <= paletPanel.Height)
                {
                    secilenKutu.Konum = yeniKonum;
                }

                baslangicKonum = e.Location;
                paletPanel.Invalidate();
            }
        }

        private void PaletPanel_MouseUp(object sender, MouseEventArgs e)
        {
            suruklemeModu = false;
        }

        private void Boxes_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormKapanirkenVeriKaydet();
            Application.Exit();
        }

        private void mainbtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1.Instance.Show();
            Form1.Instance.BringToFront();
        }

        private void pltbtn_Click(object sender, EventArgs e)
        {
            try
            {
                Form1.Instance.WriteToLog($"Palletizing'e geçiliyor. Mevcut kutu sayısı: {kutular.Count}");
                foreach (var kutu in kutular)
                {
                    Form1.Instance.WriteToLog($"Kutu ID: {kutu.ID}, Konum: ({kutu.Konum.X}, {kutu.Konum.Y}), Yönelim: {kutu.Yonelimi}");
                }
            }
            catch { }
            this.Hide();
            Palletizing.Instance.Show();
            Palletizing.Instance.BringToFront();
        }

        
    }
}

public class KutuVeriYoneticisi
{
    private static readonly string DOSYA_YOLU = Path.Combine(Application.StartupPath, "kutu_verileri.json");

    public static void KutulariKaydet(List<KutuNesnesi> kutular)
    {
        try
        {
            var kutuVerileri = kutular.Select(k => new KutuVerisi
            {
                ID = k.ID,
                KonumX = k.Konum.X,
                KonumY = k.Konum.Y,
                Genislik = k.Genislik,
                Yukseklik = k.Yukseklik,
                RenkArgb = k.Renk.ToArgb(),
                DondurmeModu = k.DondurmeModu,
                Yonelimi = k.Yonelimi
            }).ToList();

            string json = JsonConvert.SerializeObject(kutuVerileri, Formatting.Indented);
            File.WriteAllText(DOSYA_YOLU, json);

            MessageBox.Show($"Kutu verileri başarıyla kaydedildi!\nDosya: {DOSYA_YOLU}",
                          "Kayıt Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Kutu verileri kaydedilirken hata oluştu: {ex.Message}",
                          "Kayıt Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public static List<KutuNesnesi> KutulariYukle()
    {
        try
        {
            if (!File.Exists(DOSYA_YOLU))
                return new List<KutuNesnesi>();

            string json = File.ReadAllText(DOSYA_YOLU);
            var kutuVerileri = JsonConvert.DeserializeObject<List<KutuVerisi>>(json);

            if (kutuVerileri == null)
                return new List<KutuNesnesi>();

            var kutular = kutuVerileri.Select(kv => new KutuNesnesi
            {
                ID = kv.ID,
                Konum = new Point(kv.KonumX, kv.KonumY),
                Genislik = kv.Genislik,
                Yukseklik = kv.Yukseklik,
                Renk = Color.FromArgb(kv.RenkArgb),
                DondurmeModu = kv.DondurmeModu,
                Yonelimi = kv.Yonelimi
            }).ToList();

            MessageBox.Show($"{kutular.Count} kutu verisi başarıyla yüklendi!",
                          "Yükleme Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

            return kutular;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Kutu verileri yüklenirken hata oluştu: {ex.Message}",
                          "Yükleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return new List<KutuNesnesi>();
        }
    }

    public static bool OtomatikKayitVarMi()
    {
        return File.Exists(DOSYA_YOLU);
    }
}

// Güncellenmiş serileştirme sınıfı
public class KutuVerisi
{
    public int ID { get; set; }
    public int KonumX { get; set; }
    public int KonumY { get; set; }
    public int Genislik { get; set; }
    public int Yukseklik { get; set; }
    public int RenkArgb { get; set; }
    public bool DondurmeModu { get; set; }
    public KutuYonelimi Yonelimi { get; set; } = KutuYonelimi.Kuzey;
}

// Güncellenmiş kutu nesnesi sınıfı
public class KutuNesnesi
{
    public Point Konum { get; set; }
    public int Genislik { get; set; }
    public int Yukseklik { get; set; }
    public Color Renk { get; set; }
    public bool DondurmeModu { get; set; } = false;
    public int ID { get; set; }
    public KutuYonelimi Yonelimi { get; set; } = KutuYonelimi.Kuzey;

    // Boyut yönelimi (genişlik >= yükseklik ise yatay)
    public bool YatayMi => Genislik >= Yukseklik;

    public Point Merkez => new Point(Konum.X + Genislik / 2, Konum.Y + Yukseklik / 2);

    public void Ciz(Graphics g, bool secili)
    {
        // Kutu içini doldur
        using (SolidBrush brush = new SolidBrush(Renk))
        {
            g.FillRectangle(brush, Konum.X, Konum.Y, Genislik, Yukseklik);
        }

        // Seçili ise çerçeveyi kalın çiz
        using (Pen pen = new Pen(secili ? Color.Blue : Color.Black, secili ? 3 : 1))
        {
            g.DrawRectangle(pen, Konum.X, Konum.Y, Genislik, Yukseklik);
        }

        // Boyut bilgilerini çiz
        using (Font font = new Font("Arial", 8))
        using (SolidBrush textBrush = new SolidBrush(Color.Black))
        using (StringFormat format = new StringFormat())
        {
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            string boyutText = $"{Genislik}x{Yukseklik}";
            Rectangle textRect = new Rectangle(Konum.X, Konum.Y + Yukseklik / 2 - 10, Genislik, 20);
            g.DrawString(boyutText, font, textBrush, textRect, format);
        }

        // ID'yi çiz (önce)
        using (Font idFont = new Font("Arial", 7, FontStyle.Bold))
        using (SolidBrush idBrush = new SolidBrush(Color.White))
        using (SolidBrush idBackBrush = new SolidBrush(Color.Black))
        {
            string idText = $"ID:{ID}";
            SizeF idSize = g.MeasureString(idText, idFont);
            Rectangle idRect = new Rectangle(Konum.X + 2, Konum.Y + 2, (int)idSize.Width + 2, (int)idSize.Height);

            g.FillRectangle(idBackBrush, idRect);
            g.DrawString(idText, idFont, idBrush, Konum.X + 3, Konum.Y + 3);
        }

        // Yönelim okunu çiz (sonra, daha görünür olsun)
        CizYonelimOku(g);
    }

    private void CizYonelimOku(Graphics g)
    {
        Point merkez = new Point(Konum.X + Genislik / 2, Konum.Y + Yukseklik / 2);
        int okBoyutu = Math.Min(Genislik, Yukseklik) / 3;

        if (okBoyutu < 15) okBoyutu = 15; // Minimum ok boyutu artırıldı

        // Ok çizimi - daha kalın ve belirgin
        using (Pen okPen = new Pen(Color.Red, 3)) // Kırmızı ve kalın
        {
            okPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

            Point hedef = YonelimHedefiHesapla(merkez, okBoyutu);
            g.DrawLine(okPen, merkez, hedef);
        }

        // Yönelim kısaltmasını büyük ve belirgin yaz
        using (Font yonelimFont = new Font("Arial", 8, FontStyle.Bold))
        using (SolidBrush yonelimBrush = new SolidBrush(Color.Red))
        using (SolidBrush yonelimBackBrush = new SolidBrush(Color.White))
        {
            string yonelimKisaltma = GetYonelimKisaltma();
            SizeF textSize = g.MeasureString(yonelimKisaltma, yonelimFont);
            Point textKonum = new Point(Konum.X + Genislik - (int)textSize.Width - 5,
                                       Konum.Y + Yukseklik - (int)textSize.Height - 5);

            // Arka plan beyaz kutu
            Rectangle backRect = new Rectangle(textKonum.X - 2, textKonum.Y - 1,
                                             (int)textSize.Width + 4, (int)textSize.Height + 2);
            g.FillRectangle(yonelimBackBrush, backRect);
            g.DrawRectangle(new Pen(Color.Black, 1), backRect);

            // Metni çiz
            g.DrawString(yonelimKisaltma, yonelimFont, yonelimBrush, textKonum);
        }
    }

    private Point YonelimHedefiHesapla(Point merkez, int uzunluk)
    {
        double aci = (int)Yonelimi * 45.0; // Her yönelim 45 derece
        double radyan = aci * Math.PI / 180.0;

        int deltaX = (int)(uzunluk * Math.Sin(radyan));
        int deltaY = (int)(-uzunluk * Math.Cos(radyan)); // Y eksenini ters çevir

        return new Point(merkez.X + deltaX, merkez.Y + deltaY);
    }

    private string GetYonelimKisaltma()
    {
        switch (Yonelimi)
        {
            case KutuYonelimi.Kuzey: return "N";
            case KutuYonelimi.KuzeyDogu: return "NE";
            case KutuYonelimi.Dogu: return "E";
            case KutuYonelimi.GuneyDogu: return "SE";
            case KutuYonelimi.Guney: return "S";
            case KutuYonelimi.GuneyBati: return "SW";
            case KutuYonelimi.Bati: return "W";
            case KutuYonelimi.KuzeyBati: return "NW";
            default: return "?";
        }
    }

    public void Dondur()
    {
        int temp = Genislik;
        Genislik = Yukseklik;
        Yukseklik = temp;
        DondurmeModu = !DondurmeModu;
    }

    public bool IceriyorMu(Point konum)
    {
        return konum.X >= Konum.X && konum.X <= Konum.X + Genislik &&
               konum.Y >= Konum.Y && konum.Y <= Konum.Y + Yukseklik;
    }
}