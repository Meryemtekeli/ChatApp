using System;
using System.Windows.Forms;
using System.Threading;

namespace ChatClient
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 2 client penceresi
            Form1 client1 = new Form1();
            Form1 client2 = new Form1();

            client1.AutoConnect("Kullanýcý1");
            client2.AutoConnect("Kullanýcý2");

            Thread t1 = new Thread(() => Application.Run(client1));
            Thread t2 = new Thread(() => Application.Run(client2));

            t1.SetApartmentState(ApartmentState.STA);
            t2.SetApartmentState(ApartmentState.STA);

            t1.Start();
            t2.Start();
        }
    }
}
