using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // create and show both forms
            Server form1 = new Server();
            Client form2 = new Client();
            form1.Show();
            form2.Show();

            Application.Run();
        }
    }

}
