using System;
using System.Windows.Forms;


namespace Payroll_System_Client
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
            Application.Run(new Login());//打开登录窗口
            //Application.Run(new Admin());
            //Application.Run(new Employee());
        }
    }
}
