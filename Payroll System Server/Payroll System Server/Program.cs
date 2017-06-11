using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Payroll_System_Server
{
    class Program
    {

        static void Main(string[] args)
        {
            System.Threading.Timer timer = new System.Threading.Timer(new System.Threading.TimerCallback(Clock.Payroll),null,0,60*60*1000);
            //实例化Timer类，设置时间间隔为1小时，每小时检查一次是否还有未发的工资，同时检查是否是发薪日
            Server.Start();//启动服务器，监听客户端消息
            
            //以下代码区-------------测试用-------------
            //Console.WriteLine(Clock.GetLastWorkingTime());
            //Console.WriteLine(Clock.GetWeekFirstDayMon());
            //Console.WriteLine(Clock.GetWeekLastDayFri());
            //using (var db = new payrollEntities())
            //{                
            //Console.WriteLine(db.purchaseorder.SqlQuery("select * from purchaseorder where id = 1").Single().id.ToString());
            //}
        }
    }
}