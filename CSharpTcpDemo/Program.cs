using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CSharpTcpDemo.Palletizing;

namespace CSharpTcpDemo
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            try
            {
                GlobalVeri.LoadCoordinates();
                Console.WriteLine("Koordinatlar uygulama başlangıcında yüklendi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Koordinatlar yüklenirken hata oluştu: {ex.Message}",
                               "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Uygulama kapanırken koordinatları kaydetmek için event handler ekle
            Application.ApplicationExit += Application_ApplicationExit;
       
        }
        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            try
            {
                // Uygulama kapanırken koordinatları kaydet
                GlobalVeri.SaveCoordinates();
                Console.WriteLine($"Koordinatlar uygulama kapanırken kaydedildi.{GlobalVeri.PalletPt1}");
            }
            catch (Exception ex)
            {
                // Hata durumunda log'a yazdır (MessageBox kullanmayın çünkü uygulama kapanıyor)
                Console.WriteLine($"Koordinatlar kaydedilirken hata oluştu: {ex.Message}");
            }
        }
        
    }
}
